using Newtonsoft.Json;
using RestClientApi.Cache;
using RestClientApi.Models.AccountModel;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestClientApi.RestClient
{
    public class RestAccount
    {
        private string server = null;
        private string root_registration = "registration";
        private string root_login = "login";

        public CacheAccount cache_account;

        public RestAccount(string address, int port, bool ssl)
        {
            if (ssl == true) server = $"https://{address}:{port}/account/";
            else server = $"http://{address}:{port}/account/";
        }

        public async Task<bool> Registration(string username, string email, string password, string confirm_password)
        {
            try
            {
                Uri addres_web_server = new Uri(server + root_registration);
                using (HttpClient rest_client = new HttpClient())
                {
                    RegistrationModel registration_model = new RegistrationModel()
                    {
                        Username = username,
                        Email = email,
                        Password = password,
                        ConfirmPassword = confirm_password
                    };
                    var json_person = JsonConvert.SerializeObject(registration_model);
                    var person_string_content = new StringContent(json_person, Encoding.UTF8, "application/json");

                    var message_server = await rest_client.PostAsync(addres_web_server, person_string_content);
                    string result = await message_server.Content.ReadAsStringAsync();
                    if (message_server.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var error_message = JsonConvert.DeserializeObject<ErrorRegistrationModel>(result);
                        Console.WriteLine(error_message.ToString());
                        return false;
                    } else if (message_server.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var error_message = JsonConvert.DeserializeObject<ErrorRegistrationBadRequestModel>(result);
                        Console.WriteLine(error_message.ToString());
                        return false;
                    }
                    Console.WriteLine("Вы успешно создали аккаунт. Авторизуйтесь для того, чтобы войти в систему.");
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine($"Не удалось зайти в аккаунт из-за ошибки: {ex.Message}."); return false; }       
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                Uri addres_web_server = new Uri(server + root_login);
                using (HttpClient rest_client = new HttpClient())
                {
                    LoginModel login_model = new LoginModel()
                    {
                        username = username,
                        password = password,
                    };
                    var json_person = JsonConvert.SerializeObject(login_model);
                    var person_string_content = new StringContent(json_person, Encoding.UTF8, "application/json");

                    var message_server = await rest_client.PostAsync(addres_web_server, person_string_content);
                    string result = await message_server.Content.ReadAsStringAsync();
                    if (message_server.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("Такого аккаунта не существует, либо вы ввели некорректный пароль.");
                        return false;
                    } else if (message_server.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var error_message = JsonConvert.DeserializeObject<ErrorLoginBadRequestModel>(result);
                        Console.WriteLine(error_message.ToString());
                        return false;
                    }
                    cache_account = JsonConvert.DeserializeObject<CacheAccount>(result);
                    cache_account.SaveCacheLocal();
                    Console.WriteLine("Вы успешно вошли в систему.");
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine($"Не удалось зайти в аккаунт из-за ошибки: {ex.Message}."); return false; } 
        }
    }
}
