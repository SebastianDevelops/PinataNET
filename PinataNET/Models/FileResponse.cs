namespace PinataNET.Models
{
    public class FileResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Cid { get; set; }
        public string CreatedAt { get; set; }
        public int Size { get; set; }
        public int NumberOfFiles { get; set; }
        public string MimeType { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public bool IsDuplicate { get; set; }
    }
}
