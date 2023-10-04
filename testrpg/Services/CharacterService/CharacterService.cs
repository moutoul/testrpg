using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
           
            var serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id)+1;
            characters.Add(character);
            serviceresponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceresponse;
        }

        public Task<ServiceResponse<Character>> DeleteCharacter(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceresponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceresponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceresponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceresponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdatedCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceresponse = new ServiceResponse<GetCharacterDto>();
            Character character = characters.FirstOrDefault(c => c.Id.Equals(updatedCharacter.Id));
            character.Name = updatedCharacter.Name;
            character.Strength = updatedCharacter.Strength;
            character.Defense = updatedCharacter.Defense;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Hitpoints = updatedCharacter.Hitpoints;
            character.Class = updatedCharacter.Class;
            serviceresponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceresponse;
        }
    }
}
