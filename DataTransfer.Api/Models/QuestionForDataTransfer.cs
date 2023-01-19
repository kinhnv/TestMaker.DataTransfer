namespace DataTransfer.Api.Models
{
    public class QuestionForDataTransfer
    {
        public Guid QuestionId { get; set; }

        public int Type { get; set; }

        public string? Media { get; set; }

        public string? ContentAsJson { get; set; }

        public Guid SectionId { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
