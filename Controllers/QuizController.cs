using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        //GET api/quiz/latest
        [HttpGet("Latest/{num?}")]
        public IActionResult Latest(int num = 10)
        {
            var sampleQuizzes = new List<QuizViewModel>();

            //Add first sample quiz
            sampleQuizzes.Add(new QuizViewModel()
            {
                Id = 1,
                Title = "Which character from Shingeki No Kyojin are you?",
                Description = "Personality test based on anime",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            //Add more sample quizzes
            for (int i = 2; i <= num; i++)
            {
                sampleQuizzes.Add(new QuizViewModel()
                {
                    Id = i,
                    Title = String.Format("Sample Quiz {0}", i),
                    Description = "This is a sample quiz",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            //Pass the results in JSON format
            return new JsonResult(
                sampleQuizzes,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }

        /// <summary>
        /// GET: api/quiz/ByTitle
        /// Downloads {num} quizes sorted by title from A to Z
        /// </summary>
        /// <param name="num">number of quizes to download</param>
        /// <returns>{num} quizes sorted by title</returns>
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTittle(int num = 10)
        {
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
            
            return new JsonResult(
                sampleQuizzes.OrderBy(t => t.Title),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        }

        /// <summary>
        /// GET: api/quiz/mostViewed
        /// Downloads {num} random quizes
        /// </summary>
        /// <param name="num">number of quizes to download</param>
        /// <returns>{num} random quizes</returns>
        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;

            return new JsonResult(
                sampleQuizzes.OrderBy(t => Guid.NewGuid()),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        }
    }
}
