namespace Pulse.Api.Model
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string? Text { get; set; }
        public int Order { get; set; }
        public int TimeLimit { get; set; }
        public List<AnswerOption> AnswerOptions { get; set; } = [];
        public Quiz Quiz { get; set; } = null!;
    }
}
