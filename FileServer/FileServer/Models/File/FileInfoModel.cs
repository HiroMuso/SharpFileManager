using System;
using System.Collections.Generic;

namespace FileServer.Models
{
    public class FileInfoModel
    {
        public List<FileInfoSerialize> file_info { get; set; }
    }

    [Serializable]
    public class FileInfoSerialize
    {
        public string name { get; set; }
        public string full_name { get; set; }
        public DateTime creation_time { get; set; }
        public string extension { get; set; }

        public long length { get; set; }
    }
}
