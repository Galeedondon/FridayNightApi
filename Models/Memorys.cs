namespace FrightNight.Models
{
    public class Memorys
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PicKey { get; set; }
        public DateTime MemoryDate { get; set; }
        public string Tags { get; set; }
        public string Locate { get; set; }
        public string CreateAt { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UpdateAt { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? DeleteAt { get; set; }
        public DateTime? DeleteDate { get; set; }  
    }
}