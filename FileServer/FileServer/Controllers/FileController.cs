using FileServer.Interfaces;
using FileServer.Models;
using FileServer.Models.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FileServer.Cache;

namespace FileServer.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly UserManager<IdentityUser> _user_manager;
        private readonly IFileRepository _file_repository;
        private readonly ILogger<FileController> _logger;
        private IWebHostEnvironment _environment;
        private string www_path;

        public FileController(ILogger<FileController> logger, IWebHostEnvironment environment, IFileRepository file_repository, UserManager<IdentityUser> user_manager)
        {
            _logger = logger;
            _environment = environment;
            _file_repository = file_repository;
            _user_manager = user_manager;

            if (string.IsNullOrWhiteSpace(_environment.WebRootPath)) _environment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            www_path = _environment.WebRootPath;
        }

        // Класс для получения данных пользователя, cash этих данных хранится в словаре cash_user
        private async Task<string> DirectoryInfo()
        {
            if (!User.Identity.IsAuthenticated) return null;

            string username = HttpContext.User.Identity.Name;
            string directory_name = Data.cash_user.Where(x => x.Key == username).SingleOrDefault().Value;
            if (directory_name == null)
            {
                directory_name = await _file_repository.GetDirectoryName(username);
                if (directory_name == null) return null;
                Data.cash_user.Add(username, directory_name);
            }
            // Если по какой-то причине директория была удалена, то создадим её
            string full_save_path_directory = Path.Combine(www_path, directory_name);
            if (!Directory.Exists(full_save_path_directory)) Directory.CreateDirectory(full_save_path_directory);
            // Если словарь имеет размер 20, то мы очищаем его для того, чтобы не засорять память
            if (Data.cash_user.Count == 20) Data.cash_user.Clear();
            return full_save_path_directory;
        }


        [HttpPost("files/load/")]
        [RequestSizeLimit(100_000_000)]
        public async Task<string> FileLoadAsync([FromBody] FileLoadModel file_model)
        {
            if (file_model == null) return null;
            if (file_model.buffer_file == null) return null;

            _logger.LogInformation($"Название: {file_model.name_file}. Формат: {file_model.format_file}.");

            string full_save_path_directory = await DirectoryInfo();
            string full_save_path_file = null;
            if (file_model.name_file == null)
            {
                string file_name = Guid.NewGuid().ToString();
                full_save_path_file = Path.Combine(full_save_path_directory, file_name) + file_model.format_file;
            }
            else full_save_path_file = Path.Combine(full_save_path_directory, file_model.name_file) + file_model.format_file;
            using (FileStream file_stream = new FileStream(full_save_path_file, FileMode.Create))
                file_stream.Write(file_model.buffer_file, 0, file_model.buffer_file.Length);
            _logger.LogInformation($"Файл: {file_model.name_file}{file_model.format_file} успешно был сохранён на сервер.");
            return $"Файл {file_model.name_file}, сохранён по пути: {full_save_path_file}.";
        }


        [HttpGet("files/info")]
        public async Task<FileInfoModel> FullDirectoryInfoAsync()
        {
            string full_path_directory = await DirectoryInfo();
            if (full_path_directory == null) return null;

            var directory_info = new DirectoryInfo(full_path_directory);

            FileInfoModel file_info_model = new FileInfoModel();
            FileInfo[] files_directory = directory_info.GetFiles();
            List<FileInfoSerialize> file_info = new List<FileInfoSerialize>();

            try
            {
                for (int i = 0; i < files_directory.Length; i++)
                {
                    FileInfoSerialize file_info_serializ = new FileInfoSerialize()
                    {
                        name = files_directory[i].Name,
                        full_name = files_directory[i].FullName,
                        creation_time = files_directory[i].CreationTime,
                        extension = files_directory[i].Extension,
                        length = files_directory[i].Length
                    };
                    file_info.Add(file_info_serializ);
                }
                file_info_model.file_info = file_info;
            }
            catch (Exception) { _logger.LogError("Произошла ошибка при отправки информации об директории."); return null; }
            _logger.LogInformation($"Пользователю успешно была отправлена информация об директории файлов.");
            return file_info_model;
        }


        [HttpGet("files/download/{name_file}")]
        public async Task<FileDownalodModel> FileDownloadAsync(string name_file)
        {
            string download_path_file = await DirectoryInfo();
            if (download_path_file == null) return null;

            string full_path_file = null;
            string full_name_file = null;
            try
            {
                var directory_info = new DirectoryInfo(download_path_file);
                var file_info = directory_info.GetFiles(name_file + ".*");
                full_path_file = file_info[0].FullName;
                full_name_file = file_info[0].Name;
            }
            catch { _logger.LogError($"Файл {name_file} не был найден."); return null; }

            if (full_name_file == null || full_path_file == null) return null;

            byte[] buffer_file = System.IO.File.ReadAllBytes(full_path_file);

            FileDownalodModel file_downalod_model = new FileDownalodModel()
            { full_name = full_name_file, buffer_file = buffer_file };
            _logger.LogInformation($"Файл по пути: {full_path_file}, успешно был отправлен пользователю.");
            return file_downalod_model;

        }
    }
}
