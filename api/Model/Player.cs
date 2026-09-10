namespace Pulse.Api.Model
{
  public class Player {
    public int Id {  get; set; }
    public int GameSessionId {  get; set; }
    public GameSession GameSession {  get; set; } = null!;
    public string Nickname {  get; set; } = string.Empty;
    public string? ConnectionId {  get; set; }
    public List<PlayerAnswer> Answers { get; set; } = [];
  }
}