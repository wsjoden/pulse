namespace Pulse.Api.Model
{
  public class PlayerAnswer
  {
    public int Id {  get; set;  }
    public int PlayerId {  get; set;  }
    public int QuestionId {  get; set;  }
    public int OptionId {  get; set;  }
    public DateTime AnswerTime {  get; set;  }
    public Boolean IsCorrect {  get; set; }
    public int PointsAwarded {  get; set; }
  }
}