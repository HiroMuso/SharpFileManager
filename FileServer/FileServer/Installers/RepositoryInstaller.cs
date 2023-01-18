using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileServer.Repositories;
using FileServer.Interfaces;

namespace FileServer.Installers
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountRepository, AccountRepositories>();
            services.AddScoped<IFileRepository, FileRepositories>();
        }
    }
}
