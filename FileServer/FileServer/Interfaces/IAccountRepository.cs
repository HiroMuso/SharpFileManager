using FileServer.Models.Account;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FileServer.Interfaces
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SingUpAccount(SignUpModel sign_up_model);
        Task<AuthorizationModel> LoginAccount(SignInModel sign_in_model);
    }
}
