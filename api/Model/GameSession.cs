using Microsoft.AspNetCore.SignalR;

namespace Pulse.Api.Model
 {
  public class GameSession
  {
    public int Id {  get; set; }
    public int QuizId {  get; set; }
    public string? Joincode {  get; set; }
    public GameStatus? Status { get; set; }
    public Quiz Quiz {  get; set; } = null!;
    public int CurrentQuestionIndex { get; set; }
    public List<Player> Players { get; set; } = [];
    }
  
  }
  