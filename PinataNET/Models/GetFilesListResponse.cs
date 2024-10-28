using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinataNET.Models
{
    public class GetFilesListResponse
    {
        public FileList Data { get; set; }
    }

    public class FileList
    {
        public List<FileItem> Files { get; set; }
        public string NextPageToken { get; set; }
    }

    public class FileItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cid { get; set; }
        public int Size { get; set; }
        public int NumberOfFiles { get; set; }
        public string CreatedAt { get; set; }
        public string MimeType { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public Dictionary<string, object> KeyValues { get; set; }
    }
}
