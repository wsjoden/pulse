using Microsoft.EntityFrameworkCore;
using Pulse.Api.Data;
using Pulse.Api.Model;

namespace Pulse.Api.Endpoints
{
    public static class QuizEndpoints
    {
        public static RouteGroupBuilder MapQuizEndpoints(this RouteGroupBuilder group)
        {
            // Quizzes
            group.MapGet(
                "/",
                async (AppDbContext context) =>
                {
                    var quizzes = await context
                        .Quizzes.Include(q => q.Questions)
                        .ThenInclude(q => q.AnswerOptions)
                        .ToListAsync();
                    return Results.Ok(quizzes);
                }
            );

            group.MapGet(
                "/{id:int}",
                async (AppDbContext context, int id) =>
                {
                    var quiz = await context
                        .Quizzes.Include(q => q.Questions)
                        .ThenInclude(q => q.AnswerOptions)
                        .FirstOrDefaultAsync(q => q.Id == id);

                    if (quiz is null)
                        return Results.NotFound();

                    return Results.Ok(quiz);
                }
            );

            group.MapPost(
                "/",
                async (AppDbContext context, Quiz newQuiz) =>
                {
                    if (newQuiz is null)
                        return Results.BadRequest();

                    context.Quizzes.Add(newQuiz);
                    await context.SaveChangesAsync();

                    return Results.Created($"/api/quiz/{newQuiz.Id}", newQuiz);
                }
            );

            group.MapDelete(
                "/{id:int}",
                async (AppDbContext context, int id) =>
                {
                    try
                    {
                        var quiz = context.Quizzes.Attach(new Quiz { Id = id });
                        quiz.State = EntityState.Deleted;
                        await context.SaveChangesAsync();
                        return Results.NoContent();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Results.NotFound();
                    }
                }
            );

            // Questions
            group.MapPost(
                "/questions/{id:int}",
                async (AppDbContext context, int id, Question newQuestion) =>
                {
                    if (newQuestion is null)
                        return Results.BadRequest();

                    newQuestion.QuizId = id;
                    context.Questions.Add(newQuestion);
                    await context.SaveChangesAsync();
                    return Results.Created($"/api/quiz/{id}/{newQuestion.Id}", newQuestion);
                }
            );

            group.MapPut(
                "/questions/{questionId:int}",
                async (AppDbContext context, int questionId, Question updatedQuestion) =>
                {
                    var question = await context.Questions.FindAsync(questionId);
                    if (question is null)
                        return Results.NotFound();

                    question.Text = updatedQuestion.Text;
                    question.Order = updatedQuestion.Order;
                    question.TimeLimit = updatedQuestion.TimeLimit;

                    await context.SaveChangesAsync();
                    return Results.Ok(question);
                }
            );

            group.MapDelete(
                "/questions/{questionId:int}",
                async (AppDbContext context, int questionId) =>
                {
                    try
                    {
                        var question = context.Questions.Attach(new Question { Id = questionId });
                        question.State = EntityState.Deleted;
                        await context.SaveChangesAsync();
                        return Results.NoContent();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Results.NotFound();
                    }
                }
            );
            // AnswerOptions
            group.MapPost(
                "/questions/{questionId:int}/options",
                async (AppDbContext context, int questionId, AnswerOption newOption) =>
                {
                    if (newOption is null)
                        return Results.BadRequest();

                    var questionExists = await context.Questions.AnyAsync(q => q.Id == questionId);
                    if (!questionExists)
                        return Results.NotFound();

                    newOption.QuestionId = questionId;
                    context.AnswerOptions.Add(newOption);
                    await context.SaveChangesAsync();

                    return Results.Created(
                        $"/api/quiz/questions/{questionId}/options/{newOption.Id}",
                        newOption
                    );
                }
            );

            group.MapPut(
                "/options/{id:int}",
                async (AppDbContext context, int id, AnswerOption updatedOption) =>
                {
                    var option = await context.AnswerOptions.FindAsync(id);
                    if (option is null)
                        return Results.NotFound();

                    option.Text = updatedOption.Text;
                    option.IsCorrect = updatedOption.IsCorrect;
                    option.Order = updatedOption.Order;

                    await context.SaveChangesAsync();
                    return Results.Ok(option);
                }
            );

            group.MapDelete(
                "/options/{id:int}",
                async (AppDbContext context, int id) =>
                {
                    try
                    {
                        var option = context.AnswerOptions.Attach(new AnswerOption { Id = id });
                        option.State = EntityState.Deleted;
                        await context.SaveChangesAsync();
                        return Results.NoContent();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Results.NotFound();
                    }
                }
            );
            return group;
        }
    }
}
