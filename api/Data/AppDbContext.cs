using Microsoft.EntityFrameworkCore;
using Pulse.Api.Model;

namespace Pulse.Api.Data
{
  public class AppDbContext:DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    
    }
    public DbSet<User> Users {  get; set; }
    public DbSet<Quiz> Quizzes {  get; set; }
    public DbSet<Question> Questions {  get; set; }
    public DbSet<PlayerAnswer> PlayerAnswers {  get; set; }
    public DbSet<Player> Players {  get; set; }
    public DbSet<GameSession> GameSessions {  get; set; }
    public DbSet<AnswerOption> AnswerOptions {  get; set; }
  }
}