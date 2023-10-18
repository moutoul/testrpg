using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testrpg.Dtos.Fights;
using testrpg.Services.FightService;

namespace testrpg.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightservice;

        public FightController(IFightService fightservice)
        {
            _fightservice = fightservice;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
        {
            return Ok(await _fightservice.WeaponAttack(request));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
        {
            return Ok(await _fightservice.SkillAttack(request));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto request)
        {
            return Ok(await _fightservice.Fight(request));
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<HighscoreDto>>>> GetHighscore()
        {
            return Ok(await _fightservice.GetHighscore());
        }
    }
}
