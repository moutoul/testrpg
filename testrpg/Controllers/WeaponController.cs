using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testrpg.Dtos.Weapon;
using testrpg.Services.WeaponService;

namespace testrpg.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponservice;

        public WeaponController(IWeaponService weaponservice) 
        {
            _weaponservice = weaponservice;
        }

        [HttpPost]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await  _weaponservice.AddWeapon(newWeapon));
        }
    }
}
