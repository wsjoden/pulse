namespace Pulse.Api.Model
{
  public class AnswerOption
  {
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string? Text { get; set; }
    public Boolean IsCorrect { get; set; }
    public int Order { get; set; }
  }
}