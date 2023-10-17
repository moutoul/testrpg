using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testrpg.Data;
using testrpg.Dtos.User;

namespace testrpg.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authrepo;
        public AuthController(IAuthRepository authrepo)
        {
            _authrepo = authrepo;
        }
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register (UserRegisterDTO request)
        {
            var response = await _authrepo.Register(
                new User { Username = request.UserName }, request.Password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDTO request)
        {
            var response = await _authrepo.Login(request.UserName,request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }



            return Ok(response);
        }
    }
}
