using System.ComponentModel.DataAnnotations;

namespace FileServer.Models.File
{
    public class FileUserInformationModel
    {
        [Key]
        public string Username { get; set; }
        public string DirectoryName { get; set; }
    }

}

