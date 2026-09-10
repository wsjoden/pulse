namespace Pulse.Api.Model
{
  public class Quiz
  {
    public int Id { get; set; }
    public string? Title {  get; set; }
    public int HostId {  get; set;  }
    public User Host {  get; set; } = null!;
    public DateTime CreatedAt {  get; set;  }
    public List<Question> Questions { get; set; } = new();
  }
}