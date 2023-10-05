using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testrpg.Data;
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

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
           
            var serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceresponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceresponse;
        }
           

  

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();
            serviceresponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
           
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceresponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceresponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceresponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id.Equals(updatedCharacter.Id));


                character.Name = updatedCharacter.Name;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Hitpoints = updatedCharacter.Hitpoints;
                character.Class = updatedCharacter.Class;
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

       public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {

                Character character = characters.First(c => c.Id.Equals(id));
                characters.Remove(character);
                serviceresponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

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
