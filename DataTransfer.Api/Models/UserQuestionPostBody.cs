namespace DataTransfer.Api.Models
{
    public class UserQuestionPostBody
    {
        public Guid UserId { get; set; }

        public Guid QuestionId { get; set; }
    }
}
