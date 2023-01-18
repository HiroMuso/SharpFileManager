using FileServer.Interfaces;
using FileServer.DatabaseContext;
using FileServer.Models.File;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace FileServer.Repositories
{
    public class FileRepositories : IFileRepository
    {
        private readonly ILogger<FileRepositories> _logger;
        private readonly FileContext _context;
        private IWebHostEnvironment _environment;
        private string www_path;

        public FileRepositories(FileContext context, IWebHostEnvironment environment, ILogger<FileRepositories> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
            if (string.IsNullOrWhiteSpace(_environment.WebRootPath)) _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            www_path = _environment.WebRootPath;
        }


        public async Task<string> GetDirectoryName(string username)
        {
            // Проверяем, существует ли такой пользователь вообще?
            var result = await _context.DirectoryInfo
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Username == username);

            if (result == null)
            {
                // Так как пользователь не пользовался сервисом, то мы создаём
                // для него директорию
                result = new FileUserInformationModel()
                {
                    Username = username,
                    DirectoryName = Guid.NewGuid().ToString(),
                };
                await StartCreateDirectories(result);
            }

            return result.DirectoryName;
        }



        public async Task StartCreateDirectories(FileUserInformationModel model)
        {
            string full_save_path_directory = Path.Combine(www_path, model.DirectoryName);
            if (!Directory.Exists(full_save_path_directory))
                Directory.CreateDirectory(full_save_path_directory);

            await _context.DirectoryInfo.AddAsync(model);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Пользователь {model.Username} успешно создал стартовую директорию по пути {full_save_path_directory}.");
        }
    }
}
