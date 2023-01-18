using System.IO;
using Newtonsoft.Json;
using System;
using RestClientApi.RestClient;

namespace RestClientApi.Cache
{
    public static class CacheSave
    {
        public static void SaveCacheLocal(this CacheAccount account)
        {
            try
            {
                if (!Directory.Exists(CacheOther.name_directory)) Directory.CreateDirectory(CacheOther.name_directory);

                string full_path = Path.Combine(CacheOther.name_directory, CacheOther.name_file);
                Console.WriteLine($"Начался процесс сохранения кеша на локальный диск по пути: {full_path}. Не удаляйте данный файл.");
                string json_file = JsonConvert.SerializeObject(account);
                File.WriteAllText(full_path, json_file);
                Console.WriteLine($"Кеш успешно сохранился в файл.");
            }
            catch (System.Exception ex) { Console.WriteLine($"Произошла ошибка при сохранении кеша на локальный диск: {ex.Message}"); }
        }

        public static bool LoadCacheLocal()
        {
            try
            {
                if (!Directory.Exists(CacheOther.name_directory)) return false;

                string full_path = Path.Combine(CacheOther.name_directory, CacheOther.name_file);
                if (!File.Exists(full_path)) return false;
                Console.WriteLine($"Начался процесс выгрузки кеша из локального диска по пути: {full_path}. Не удаляйте данный файл.");
                string json_file = File.ReadAllText(full_path);
                RestAccount.cache_account = JsonConvert.DeserializeObject<CacheAccount>(json_file);
                Console.WriteLine($"Кеш успешно выгрузился из файла.");
                return true;
            }
            catch (System.Exception ex) { Console.WriteLine($"Произошла ошибка при выгрузки кеша из локального диска: {ex.Message}"); return false; }
        }

        public static void ExitAccount()
        {
            RestAccount.cache_account = null;
            if (Directory.Exists(CacheOther.name_directory)) Directory.Delete(CacheOther.name_directory, true);
            Console.Write("Вы успешно вышли из аккаунта.");
        }
    }
}
