namespace DataTransfer.Api.Models
{
    public class UserQuestion
    {
        public Guid? UserId { get; set; }

        public Guid? QuestionId { get; set; }

        public double? Rank { get; set; }

        public DateTime? LastUpdated { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? TestId { get; set; }
    }
}
