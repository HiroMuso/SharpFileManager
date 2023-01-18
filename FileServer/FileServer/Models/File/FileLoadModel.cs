using System.ComponentModel.DataAnnotations;

namespace FileServer.Models
{
    public class FileLoadModel
    {
        [Required(ErrorMessage = "Введите имя файла.")]
        public string name_file { get; set; }

        [Required(ErrorMessage = "Введите формат файла.")]
        public string format_file { get; set; }

        [Required(ErrorMessage = "Заполните буфер файла.")]
        public byte[] buffer_file { get; set; }
    }
}
