namespace DataTransfer.Api.Models
{
    public class UserQuestionPostBodyAfterAndBefore
    {
        public Guid UserId { get; set; }

        public Guid QuestionId { get; set; }
    }

    public class UserQuestionPostBody
    {
        public UserQuestionPostBodyAfterAndBefore? After { get; set; }

        public UserQuestionPostBodyAfterAndBefore? Before { get; set; }
    }
}
