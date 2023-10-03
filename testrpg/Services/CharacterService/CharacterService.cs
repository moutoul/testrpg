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
            characters.Add(_mapper.Map<Character>(newCharacter));
            serviceresponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceresponse;
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
    }
}
