using System;
using System.IO;
using System.Text;

namespace RestClientApi.Models.FileModel
{
    public class FileInfoModel
    {
        public FileInfoSerialize[] file_info { get; set; }

        public override string ToString()
        {
            if (this == null) return null;

            StringBuilder string_builder = new StringBuilder();
            int counter = 1;
            foreach (var file in file_info)
            {
                string_builder.AppendLine($"{counter}. Название файла: {file.name}. Полная информация об этом файле:");
                string_builder.AppendLine($"\tFullName: {file.full_name}");
                string_builder.AppendLine($"\tCreationTime: {file.creation_time}");
                string_builder.AppendLine($"\tExtension: {file.extension}");
                string_builder.AppendLine($"\tLength: {file.length / 1024} кб");
                counter++;
            }
            return string_builder.ToString();
        }
    }

    public class FileInfoSerialize
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public DateTime creation_time { get; set; }
        public string extension { get; set; }

        public long length { get; set; }

    }
}
