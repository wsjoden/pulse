namespace Pulse.Api.Model
{
  public class Question
  {
    public int Id {  get; set;  }
    public int QuizId {  get; set;  }
    public string? text {  get; set;  }
    public int order {  get; set;  }
    public int timeLimit {  get; set;  }
  }
}