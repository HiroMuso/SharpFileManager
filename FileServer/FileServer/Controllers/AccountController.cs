using FileServer.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileServer.Interfaces;

namespace FileServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<IdentityUser> _user_manager;
        private readonly SignInManager<IdentityUser> _sign_in_manager;
        public AccountController(IAccountRepository accountRepository,
            UserManager<IdentityUser> user_manager, SignInManager<IdentityUser> signin_manager)
        {
            _accountRepository = accountRepository;
            _user_manager = user_manager;
            _sign_in_manager = signin_manager;
        }


        [HttpPost("account/registration")]
        public async Task<IActionResult> SignUpAccount([FromBody] SignUpModel sign_up_model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _accountRepository.SingUpAccount(sign_up_model);

            if (result.Succeeded) { return Ok(result.Succeeded); }

            return Unauthorized(result);
        }


        [HttpPost("account/login")]
        public async Task<IActionResult> LoginAccount([FromBody] SignInModel sign_in_model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _accountRepository.LoginAccount(sign_in_model);

            if (result != null) return Ok(result);

            return Unauthorized();
        }
    }
}
