using RestClientApi.Cache;
using RestClientApi.RestClient;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace RestClientApi.UserInterface
{
    public class ConsoleRender
    {
        private static bool is_exit_programm = false;
        private static bool is_exit_service = false;

        public static RestFiles files;
        public static RestAccount account;
        public static RestRequest request;
        public static void Init(string title, int height, int width, ConsoleColor background_color, ConsoleColor foreground_color)
        {
            Console.Title = title;
            Console.WindowHeight = height;
            Console.WindowWidth = width;
            Console.BackgroundColor = background_color;
            Console.ForegroundColor = foreground_color;

            string address = ConfigurationManager.AppSettings.Get("ip");
            int port = int.Parse(ConfigurationManager.AppSettings.Get("port"));
            bool ssl = bool.Parse(ConfigurationManager.AppSettings.Get("ssl"));

            files = new RestFiles(address, port, ssl);
            account = new RestAccount(address, port, ssl);
            request = new RestRequest();
        }

        public static async Task StartAsync()
        {
            bool load_cache = CacheSave.LoadCacheLocal();

            if (load_cache)
            {
                is_exit_service = false;
                await InfiniteLoopServiceAsync();
                is_exit_programm = false;
                await InfiniteLoopMenuAsync();
            }
            else
            {
                await InfiniteLoopMenuAsync();

            }
        }

        public static async Task InfiniteLoopMenuAsync()
        {
            while (!is_exit_programm)
            {
                int index_menu = MenuPage();
                if (index_menu == 1) await RegistrationPageAsync();
                else if (index_menu == 2)
                {
                    bool is_login = await LoginPageAsync();
                    if (is_login == true)
                        Exit();
                }
                else if (index_menu == 3) Exit();
            }
        }

        public static async Task InfiniteLoopServiceAsync()
        {
            while (!is_exit_service)
            {
                int index_menu = ServicePage();
                if (index_menu == 1)
                {
                    Console.Write("Придумайте название: ");
                    string name_file = Console.ReadLine();
                    Console.Write("Укажите полный путь файла: ");
                    string path_file = Console.ReadLine();
                    string message_server = await files.SendFileBinaryAsync(path_file, name_file);

                    if (message_server == null) Console.WriteLine("Вы ввели некорретные данные, либо отправили пустой файл.");
                    else Console.WriteLine(message_server);
                }
                else if (index_menu == 2)
                {
                }
                else if (index_menu == 3)
                {

                }
                else if (index_menu == 4)
                {
                    CacheSave.ExitAccount();
                    ExitService();
                }
                else if (index_menu == 5)
                {
                    ExitService();
                }
            }
        }

        public static void Exit() => is_exit_programm = true;

        public static void ExitService() => is_exit_service = true;

        public static int MenuPage()
        {
            Console.WriteLine();
            Console.WriteLine("Сервис для хранения/выгрузки файлов с сервера.");
            Console.WriteLine("Чтобы продолжить сперва авторизуйтесь:\n");
            Console.WriteLine("1.Регистрация.");
            Console.WriteLine("2.Авторизация.");
            Console.WriteLine("3.Выход.");
            return Input(InputPage.menu);
        }

        public static int ServicePage()
        {
            Console.WriteLine();
            Console.WriteLine($"Ваш аккаунт: {account.cache_account.username}. Пользуйтесь сервисом на здоровье.");
            Console.WriteLine("1.Загрузить файл.");
            Console.WriteLine("2.Выгрузить файл.");
            Console.WriteLine("3.Просмотр удаленной директории.");
            Console.WriteLine("4.Выйти из аккаунта.");
            Console.WriteLine("5.Выйти.");
            return Input(InputPage.service);
        }

        public static async Task<bool> RegistrationPageAsync()
        {
            Console.WriteLine();
            Console.WriteLine("Для регистрации заполните все поля:");
            Console.Write("Нейминг: ");
            string username = Console.ReadLine();
            Console.Write("Емаил: ");
            string email = Console.ReadLine();
            Console.Write("Пароль: ");
            string password = Console.ReadLine();
            Console.Write("Подтвердите пароль: ");
            string confirm_password = Console.ReadLine();

            bool is_registration = await account.Registration(username, email, password, confirm_password);

            if (is_registration) return true;

            return false;
        }

        public static async Task<bool> LoginPageAsync()
        {
            Console.WriteLine();
            Console.WriteLine("Для регистрации заполните все поля:");
            Console.Write("Нейминг: ");
            string username = Console.ReadLine();
            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            bool is_login = await account.Login(username, password);

            if (is_login) return true;

            return false;
        }


        public static int Input(InputPage input_page)
        {
            try
            {
                Console.WriteLine();
                Console.Write("Ввод: ");
                int index_page = int.Parse(Console.ReadLine());

                if (input_page == InputPage.menu)
                    if (!(index_page >= 1 && index_page <= 3)) return Input(input_page);
                else if (input_page == InputPage.service)
                    if (!(index_page >= 1 && index_page <= 5)) return Input(input_page);

                return index_page;
            }
            catch (Exception) { Input(input_page); }
            return 0;
        }

    }
}

public enum InputPage
{
    menu,
    service,
}
