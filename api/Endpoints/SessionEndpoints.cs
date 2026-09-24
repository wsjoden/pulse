using Microsoft.EntityFrameworkCore;
using Pulse.Api.Data;
using Pulse.Api.Model;

namespace Pulse.Api.Endpoints
{
    public record CreateSessionRequest(int QuizId);

    public record JoinSessionRequest(string Nickname);

    public static class SessionEndpoints
    {
        public static RouteGroupBuilder MapSessionEndpoints(this RouteGroupBuilder group)
        {
            group.MapPost(
                "/",
                async (AppDbContext context, CreateSessionRequest req) =>
                {
                    var quiz = await context.Quizzes.FindAsync(req.QuizId);
                    if (quiz is null)
                        return Results.NotFound();

                    var session = new GameSession
                    {
                        QuizId = quiz.Id,
                        Joincode = GenerateJoinCode(),
                        CurrentQuestionIndex = 0,
                        Status = GameStatus.Lobby,
                    };

                    context.GameSessions.Add(session);
                    await context.SaveChangesAsync();

                    return Results.Created($"/api/session/{session.Id}", session);
                }
            );

            group.MapPost(
                "/{joinCode}/join",
                async (AppDbContext context, string joinCode, JoinSessionRequest request) =>
                {
                    var session = await context.GameSessions.FirstOrDefaultAsync(s =>
                        s.Joincode == joinCode
                    );

                    if (session is null)
                        return Results.NotFound();

                    var player = new Player
                    {
                        GameSessionId = session.Id,
                        Nickname = request.Nickname,
                    };

                    context.Players.Add(player);
                    await context.SaveChangesAsync();

                    return Results.Created(
                        $"/api/session/{session.Id}/players/{player.Id}",
                        player
                    );
                }
            );

            group.MapPost(
                "/{id:int}/next",
                async (AppDbContext context, int id) =>
                {
                    var session = await context.GameSessions.FindAsync(id);
                    if (session is null)
                        return Results.NotFound();

                    session.CurrentQuestionIndex++;
                    await context.SaveChangesAsync();

                    return Results.Ok(session);
                }
            );

            group.MapGet(
                "/{id:int}/current-question",
                async (AppDbContext context, int id) =>
                {
                    var session = await context
                        .GameSessions.Include(s => s.Quiz)
                        .ThenInclude(q => q.Questions)
                        .ThenInclude(a => a.AnswerOptions)
                        .FirstOrDefaultAsync(s => s.Id == id);

                    if (session is null)
                        return Results.NotFound();

                    var questions = session.Quiz.Questions.OrderBy(q => q.Order).ToList();

                    if (session.CurrentQuestionIndex >= questions.Count)
                        return Results.NotFound("No more questions");

                    var question = questions[session.CurrentQuestionIndex];
                    return Results.Ok(question);
                }
            );
            return group;
        }

        private static string GenerateJoinCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = Random.Shared;
            return new string(
                Enumerable.Range(0, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray()
            );
        }
    }
}
