using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using testrpg.Data;
using testrpg.Dtos.Skills;
using testrpg.Models;

namespace testrpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character() {Id = 1,Name = "Sam"}
        };

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextaccessor;

        public CharacterService(IMapper mapper, DataContext context,IHttpContextAccessor contextaccessor)
        {
            _mapper = mapper;
            _context = context;
            _contextaccessor = contextaccessor;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
           
            var serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceresponse.Data = await _context.Characters.Where(c => c.User!.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceresponse;
        }

        private int GetUserId() => int.Parse(_contextaccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

  

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Where(c=>c.User!.Id == GetUserId()).ToListAsync();
            serviceresponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
           
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceresponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceresponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceresponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                
                var character = await _context.Characters.Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id.Equals(updatedCharacter.Id));

                if (character.User.Id == GetUserId())
                {

                    character.Name = updatedCharacter.Name;
                    character.Strength = updatedCharacter.Strength;
                    character.Defense = updatedCharacter.Defense;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Hitpoints = updatedCharacter.Hitpoints;
                    character.Class = updatedCharacter.Class;

                    await _context.SaveChangesAsync();

                    serviceresponse.Data = _mapper.Map<GetCharacterDto>(character);
                }

                else
                {
                    serviceresponse.Success = false;
                    serviceresponse.Message = " perso non trouvé :O ";
                }
               
            }

            catch (Exception ex)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = ex.Message;
            }
                
            return serviceresponse;
        }

       public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {

                Character character = await _context.Characters.FirstAsync(c => c.Id.Equals(id)&& c.User!.Id == GetUserId());
                if (character == null || character.User!.Id != GetUserId())
                    throw new Exception($"aucun perso avec l id '{id}' trouvé ");
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                serviceresponse.Data = _context.Characters.Where(c => c.User.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            }

              


            catch (Exception ex)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = ex.Message;

            }
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkills(AddCharacterSkillDto newCharacterSkill)
        {
            var serviceresponse = new ServiceResponse<GetCharacterDto>();
            try 
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c =>c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());

                if (character == null)
                {
                    serviceresponse.Success = false;
                    serviceresponse.Message = "aucun perso correspondant trouvé";
                    return serviceresponse;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill == null)
                {
                    serviceresponse.Success = false;
                    serviceresponse.Message = "Competence introuvable";
                    return serviceresponse;

                }
                character.Skills.Add(skill);
                await _context.SaveChangesAsync();
                serviceresponse.Data = _mapper.Map<GetCharacterDto>(character);
              
            }
            catch (Exception ex)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = ex.Message;
            }
            return serviceresponse;
        }
    }
}
