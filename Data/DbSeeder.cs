using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.InteropServices;
using TestMakerFreeWebApp.Data.Models;

namespace TestMakerFreeWebApp.Data
{
    public class DbSeeder
    {
        #region public Methods
        public static void Seed(ApplicationDbContext dbContext)
        {
            // Create default users (if there are no users)
            if (!dbContext.Users.Any()) CreateUsers(dbContext);

            // Create default quizzes (if there are no quizzes)
            if (!dbContext.Quizzes.Any()) CreateQuizzes(dbContext);
        }
        #endregion

        #region generating Methods
        private static void CreateUsers(ApplicationDbContext dbContext)
        {
            // local Variables
            DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            // create an "Admin" user account(if it does not already exist)
            var user_Admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate,
            };

            // Insert user admin into the database
            dbContext.Users.Add(user_Admin);

            #if DEBUG
            // Create sample accounts of registered users
            var user_Ryan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Ryan",
                Email = "ryan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate,
            };

            // Create sample accounts of registered users
            var user_Solice = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Solice",
                Email = "solice@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate,
            };

            // Create sample accounts of registered users
            var user_Vodan = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Vodan",
                Email = "vodan@testmakerfree.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate,
            };

            // Insert sample users in the database
            dbContext.Users.AddRange(user_Ryan, user_Solice, user_Vodan);
            #endif

            dbContext.SaveChanges();
        }

        private static void CreateQuizzes(ApplicationDbContext dbContext)
        {
            // local Variables
            DateTime createdDate = new DateTime(2017, 08, 08, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            // get user admin, because we will use him as default author
            var authorId = dbContext.Users
                .Where(u => u.UserName == "Admin")
                .FirstOrDefault()
                .Id;

            #if DEBUG
            // Create 47 sample quizes with automatically generated data
            var num = 47;
            for(int i = 1; i <= num; i++)
            {
                CreateSampleQuiz(
                    dbContext,
                    i,
                    authorId,
                    num - 1,
                    3,
                    3,
                    3,
                    createdDate.AddDays(-num));
            }
            #endif

            // Create 3 more quizzes with better descriptive data
            EntityEntry<Quiz> e1 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Are you on the light side or the dark side of power",
                Description = "Personalities test based on Star Wars",
                Text = @"You must choose wisely, young padawan",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate,
            });

            EntityEntry<Quiz> e2 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Generation X, Y or Z?",
                Description = "Find out which decade you best fit into",
                Text = @"Do you feel comfortable in your generation?" + 
                        "Here are some questions to find out!",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate,
            });

            EntityEntry<Quiz> e3 = dbContext.Quizzes.Add(new Quiz()
            {
                UserId = authorId,
                Title = "Which character from Shingeki No Kyojin are yo??",
                Description = "Personalities test based on Shingeki No Kyojin",
                Text = @"Discover the real you with this personality test!",
                ViewCount = 5203,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate,
            });

            dbContext.SaveChanges();
        }
        #endregion

        #region supporting methods
        private static void CreateSampleQuiz(
            ApplicationDbContext dbContext,
            int num,
            string authorId,
            int viewCount,
            int numberOfQuestions,
            int numberOfAnswersPerQuestion,
            int numberOfResults,
            DateTime createdDate)
        {
            var quiz = new Quiz()
            {
                UserId = authorId,
                Title = String.Format("Quiz title {0}", num),
                Description = String.Format("This is sample quiz description {0}", num),
                Text = "This is sample quiz created by DbSeeder",
                ViewCount = viewCount,
                CreatedDate = createdDate,
                LastModifiedDate = createdDate,
            };
            dbContext.Quizzes.Add(quiz);
            dbContext.SaveChanges();

            for (int i = 0; i < numberOfQuestions; i++)
            {
                var question = new Question()
                {
                    QuizId = quiz.Id,
                    Text = "This is sample question created by DbSeeder",
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate,
                };
                dbContext.Questions.Add(question);
                dbContext.SaveChanges();

                for(int j = 0; j < numberOfAnswersPerQuestion; j++)
                {
                    var e2 = dbContext.Answers.Add(new Answer()
                    {
                        QuestionId = question.Id,
                        Text = "This is sample answer created by DbSeeder",
                        Value = j,
                        CreatedDate = createdDate,
                        LastModifiedDate = createdDate,
                    });
                }
            }

            for(int i = 0; i < numberOfResults; i++) 
            {
                dbContext.Results.Add(new Result()
                {
                    QuizId = quiz.Id,
                    Text = "This is sample result created by DbSeeder",
                    MinValue = 0,
                    MaxValue = numberOfAnswersPerQuestion * 2,
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate,
                });
            }
            dbContext.SaveChanges();
        }
        #endregion

    }
}
