using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using testrpg.Data;
using testrpg.Dtos.Weapon;

namespace testrpg.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context, IHttpContextAccessor httpcontextaccessor, IMapper mapper)
        {
            _context = context;
            _httpcontextaccessor = httpcontextaccessor;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponse<GetCharacterDto> serviceresponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == int
                    .Parse(_httpcontextaccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

                if (character == null)
                {
                    serviceresponse.Success = false;
                    serviceresponse.Message = "perso introuvable";
                    return serviceresponse;
                    
                }

                Weapon weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                _context.Weapons.Add(weapon);
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
