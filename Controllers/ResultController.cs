using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        #region Methods adapting to the REST convention
        /// <summary>
        /// Retrieves the result with the specified {id}
        /// </summary>
        /// <param name="id">identifier of the existing result</param>
        /// <returns>the answer of {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("Not yet implemented");
        }

        /// <summary>
        /// Adds a new result to the database
        /// </summary>
        /// <param name="model">ResultViewModel object with data to be inserted</param>
        [HttpPut]
        public IActionResult Put(ResultViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Modifies the result with the specified {id}
        /// </summary>
        /// <param name="model">ResultViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post(ResultViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the result with the specified {id} from the database
        /// </summary>
        /// <param name="id">identification of the existing result</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Routing methods based on atributes
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
        #endregion
    }
}
