using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        // GET api/answer/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var sampleAnswers = new List<ResultViewModel>();

            // Add first sample result
            sampleAnswers.Add(new ResultViewModel
            {
                Id = 1,
                QuizId = quizId,
                Text = "What do you value most in your life",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
            });

            // Add more sample results
            for (int i = 2; i <= 5; i++)
            {
                sampleAnswers.Add(new ResultViewModel
                {
                    Id = i,
                    QuizId = quizId,
                    Text = String.Format("Sample question {0}", i),
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                });
            }

            //Pass the results in JSON format
            return new JsonResult(
                sampleAnswers,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }
    }
}
