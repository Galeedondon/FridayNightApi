namespace FrightNight.Models
{
    public class Comments
    {
        public int Id { get; set; }
        public int? ReplyId { get; set; }
        public int MemoryId { get; set; }
        public string Comment  { get; set; }
        public string CreateAt { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UpdateAt { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? DeleteAt { get; set; }
        public DateTime? DeleteDate { get; set; }  
    }
}