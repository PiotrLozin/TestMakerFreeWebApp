using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        #region Methods adapting to the REST convention
        /// <summary>
        /// Retrieves the response with the specified {id}
        /// </summary>
        /// <param name="id">identifier of the existing response</param>
        /// <returns>the answer of {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("Not yet implemented");
        }

        /// <summary>
        /// Adds a new answer to the database
        /// </summary>
        /// <param name="model">AnswerViewModel object with data to be inserted</param>
        [HttpPut]
        public IActionResult Put(AnswerViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Modifies the response with the specified {id}
        /// </summary>
        /// <param name="model">AnswerViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post(AnswerViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the response with the specified {id} from the database
        /// </summary>
        /// <param name="id">identification of the existing response</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Routing methods based on atributes
        // GET api/answer/all
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var sampleAnswers = new List<AnswerViewModel>();

            // Add first sample answer
            sampleAnswers.Add(new AnswerViewModel
            {
                Id = 1,
                QuestionId = questionId,
                Text = "Friends and family",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
            });

            // Add more sample answers
            for (int i = 2; i <= 5; i++)
            {
                sampleAnswers.Add(new AnswerViewModel
                {
                    Id = i,
                    QuestionId = questionId,
                    Text = String.Format("Sample answer {0}", i),
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
