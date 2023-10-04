
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using testrpg.Services.CharacterService;

namespace testrpg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
     

       private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
           _characterService = characterService;
        }

        [HttpGet("GetAll")]

        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get() {
            return Ok(await _characterService.GetAllCharacters());
        }
        [HttpGet("GetOne")]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetsingleCharacter(int id) 
        { 
            return Ok(await _characterService.GetCharacterById(id)); 
        }
        
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Addcharacter(AddCharacterDto newCharacter) 
        {
            return Ok (await _characterService.AddCharacter(newCharacter));
        }
    }
}
