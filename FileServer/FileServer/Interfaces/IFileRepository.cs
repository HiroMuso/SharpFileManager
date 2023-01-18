using System.Collections.Generic;
using System.Threading.Tasks;
using FileServer.Models.File;

namespace FileServer.Interfaces
{
    public interface IFileRepository
    {
        Task<string> GetDirectoryName(string username);

    }
}
