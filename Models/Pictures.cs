namespace FrightNight.Models
{
    public class Pictures
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
        public string CreateAt { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UpdateAt { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? DeleteAt { get; set; }
        public DateTime? DeleteDate { get; set; }  
    }
}