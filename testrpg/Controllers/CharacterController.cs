
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
        [HttpGet("{id}")]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetsingleCharacter(int id) 
        { 
            return Ok(await _characterService.GetCharacterById(id)); 
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
        {
            return Ok(await _characterService.DeleteCharacter(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Addcharacter(AddCharacterDto newCharacter) 
        {
            return Ok (await _characterService.AddCharacter(newCharacter));
        }
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Updatecharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if(response.Data != null)
            {
                return NotFound();
            }
            return Ok(await _characterService.UpdateCharacter(updatedCharacter));
        }
    }
}
