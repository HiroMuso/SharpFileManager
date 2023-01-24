using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestClientApi.RestClient
{
    public class RestRequest
    {
        private object post_data = null;
        public enum TypeRequest
        {
            GET,
            POST,
        }

        public enum TypeMethodDownloadFile
        {
            HTTP_CLIENT_STREAM,
            HTTP_CLIENT_BYTE_BUFFER,
            WEB_CLIENT,
        }

        public async Task<string> ReadRequest(string address, TypeRequest type_request = TypeRequest.GET)
        {
            if (type_request == TypeRequest.POST)
            {
                var is_data_responce = post_data == null ? false : true;
                if (is_data_responce)
                {
                    var json_person = JsonConvert.SerializeObject(post_data, Formatting.Indented);
                    var person_srtring_content = new StringContent(json_person, Encoding.UTF8, "application/json");
                    post_data = null;

                    Uri uri = new Uri(address);
                    using (HttpClient client_request = new HttpClient())
                    {
                        try
                        {
                            var responce = await client_request.PostAsync(uri, person_srtring_content);
                            string result = await responce.Content.ReadAsStringAsync();
                            return result;
                        }
                        catch (Exception) { return null; };
                    }
                }
                else return null;
            }
            else if (type_request == TypeRequest.GET)
            {
                try
                {
                    Uri uri = new Uri(address);
                    using (HttpClient client_request = new HttpClient())
                    {
                        var responce = await client_request.GetAsync(uri);
                        string result = await responce.Content.ReadAsStringAsync();
                        return result;
                    }
                }
                catch (Exception) { return null; }
            }
            return null;
        }


        public async Task<string> DownloadFile(string address, string path_save_file, string name_file, TypeMethodDownloadFile type_method_download_file = TypeMethodDownloadFile.HTTP_CLIENT_STREAM, string expansion_file = null)
        {
            bool address_validate = address.IndexOf("/") == -1 ? false : true;
            if (address_validate)
            {
                try
                {
                    string full_name_file = null;
                    if (expansion_file == null)
                        full_name_file = name_file + address.Substring(address.LastIndexOf("."));
                    else
                        full_name_file = name_file + expansion_file;

                    path_save_file += "\\" + full_name_file;
                    Uri uri = new Uri(address);
                    if (type_method_download_file == TypeMethodDownloadFile.HTTP_CLIENT_STREAM)
                    {
                        Console.WriteLine("Адрес скачиваемого файла: " + uri + ", файл будет сохранён по пути: " + path_save_file);
                        using (var http_client = new HttpClient())
                        {
                            var stream = await http_client.GetStreamAsync(uri).ConfigureAwait(false);
                            using (var memory_stream = new MemoryStream())
                            {
                                stream.CopyTo(memory_stream);
                                byte[] buffer_file = memory_stream.GetBuffer();
                                using (FileStream file_stream = new FileStream(path_save_file, FileMode.OpenOrCreate, FileAccess.Write))
                                    file_stream.Write(buffer_file, 0, buffer_file.Length);
                                return "Файл сохранён.";
                            }
                        }
                    }
                    else if (type_method_download_file == TypeMethodDownloadFile.HTTP_CLIENT_BYTE_BUFFER)
                    {
                        Console.WriteLine("Адрес скачиваемого файла: " + uri + ", файл будет сохранён по пути: " + path_save_file);
                        using (HttpClient client_request = new HttpClient())
                        {
                            try
                            {
                                byte[] buffer_file = await client_request.GetByteArrayAsync(uri);
                                File.WriteAllBytes(path_save_file, buffer_file);
                                return "Файл сохранён.";
                            }
                            catch (Exception) { return null; }
                        }
                    }
                    else if (type_method_download_file == TypeMethodDownloadFile.WEB_CLIENT)
                    {
                        Console.WriteLine("Адрес скачиваемого файла: " + uri + ", файл будет сохранён по пути: " + path_save_file);
                        using (WebClient web_client = new WebClient())
                        {
                            web_client.DownloadFileCompleted += (s, e) => Console.WriteLine("File download completed.");
                            web_client.DownloadProgressChanged += (s, e) => Console.WriteLine($"Uploaded  {e.ProgressPercentage}%");
                            await web_client.DownloadFileTaskAsync(uri, path_save_file);
                            return "Файл сохранён.";
                        }
                    }
                    return "Произошла непредвиденная ошибка при скачивании файла с сервера."; ;
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            else { return "Путь к файлу должен быть правильным."; }
            return "Путь к файлу должен быть правильным.";
        }

    }
}
