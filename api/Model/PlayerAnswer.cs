namespace Pulse.Api.Model
{
    public class PlayerAnswer
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public int OptionId { get; set; }
        public DateTime AnswerTime { get; set; }
        public Boolean IsCorrect { get; set; }
        public int PointsAwarded { get; set; }
    }
}
