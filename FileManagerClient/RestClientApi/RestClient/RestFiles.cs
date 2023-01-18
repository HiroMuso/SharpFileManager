using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestClientApi.Models.FileModel;

namespace RestClientApi.RestClient
{
    internal class RestFiles
    {
        private static string server = "http://localhost:5001/files/";
        private static string root_load = "load";
        private static string root_info = "info";
        private static string root_download = "download/";

        public static async Task<string> SendFileBinaryAsync(string path_file, string name_file)
        {
            try
            {
                if (!File.Exists(path_file)) return $"Файл, котоырй вы хотели отправить на сервер не существует у вас на компьютере.";
                byte[] buffer_file = File.ReadAllBytes(path_file);
                string format_file = path_file.Substring(path_file.IndexOf("."));
                FileLoadModel file_model = new FileLoadModel() { name_file = name_file, format_file = format_file, buffer_file = buffer_file };
                var json_person = JsonConvert.SerializeObject(file_model);
                var person_string_content = new StringContent(json_person, Encoding.UTF8, "application/json");
                Uri addres_web_server = new Uri(server + root_load);
                using (HttpClient rest_client = new HttpClient())
                {
                    rest_client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", RestAccount.cache_account.token);
                    var message_server = await rest_client.PostAsync(addres_web_server, person_string_content);
                    if (message_server.StatusCode == HttpStatusCode.Unauthorized)
                        return "Сперва авторизуйтесь для того, чтобы использовать данную функцию.";
                    string result = await message_server.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception ex) { return $"Произошла непредвиденная ошибка при отправки файла на сервер: {ex.Message}."; }
        }


        public static async Task<FileInfoModel> DirectoryInfo()
        {
            try
            {
                Uri uri = new Uri(server + root_info);
                using (HttpClient rest_client = new HttpClient())
                {
                    rest_client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", RestAccount.cache_account.token);
                    var message_server = await rest_client.GetAsync(uri);
                    if (message_server.StatusCode == HttpStatusCode.Unauthorized)
                        Console.WriteLine("Сперва авторизуйтесь для того, чтобы использовать данную функцию.");
                    string result = await message_server.Content.ReadAsStringAsync();
                    FileInfoModel file_info_model = JsonConvert.DeserializeObject<FileInfoModel>(result);
                    return file_info_model;
                }
            }
            catch (Exception) { Console.WriteLine("Произошла непредвиденная ошибка при просмотре директории."); return null; }
        }

        public static async Task<string> DownloadFile(string name_file, string path_save_file)
        {
            try
            {
                if (!Directory.Exists(path_save_file)) return "Введите корректный путь сохранения файла.";
                Uri uri = new Uri(server + root_download + name_file);
                using (HttpClient rest_client = new HttpClient())
                {
                    rest_client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", RestAccount.cache_account.token);
                    var message_server = await rest_client.GetAsync(uri);
                    if (message_server.StatusCode == HttpStatusCode.Unauthorized)
                        return "Сперва авторизуйтесь для того, чтобы использовать данную функцию.";
                    string result = await message_server.Content.ReadAsStringAsync();
                    FileDownalodModel file_download_model = JsonConvert.DeserializeObject<FileDownalodModel>(result);
                    if (file_download_model == null) return "Такого файла не существует.";

                    string full_path_save_file = Path.Combine(path_save_file, file_download_model.full_name);
                    using (FileStream file_stream = new FileStream(full_path_save_file, FileMode.Create, FileAccess.Write))
                        file_stream.Write(file_download_model.buffer_file, 0, file_download_model.buffer_file.Length);

                    return $"Файл успешно сохранён по пути: {full_path_save_file}.";
                }
            }
            catch (Exception) { return "Произошла непредвиденная ошибка при скачивании файла с сервера."; }
        }
    }
}
