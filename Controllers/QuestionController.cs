using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        #region Methods adapting to the REST convention
        /// <summary>
        /// Retrieves the question with the specified {id}
        /// </summary>
        /// <param name="id">identifier of the existing question</param>
        /// <returns>question with {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Content("Not yet implemented");
        }

        /// <summary>
        /// Adds a new question to the database
        /// </summary>
        /// <param name="model">QuestionViewModel object with data to be inserted</param>
        [HttpPut]
        public IActionResult Put(QuestionViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Modifies the question with the specified {id}
        /// </summary>
        /// <param name="model">QuestionViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post(QuestionViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the question with the specified {id} from the database
        /// </summary>
        /// <param name="id">identification of the existing question</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Routing methods based on atributes
        //GET api/guestion/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var sampleQuestions = new List<QuestionViewModel>();

            //Add first sample question
            sampleQuestions.Add(new QuestionViewModel()
            {
                Id = 1,
                QuizId = quizId,
                Text = "What do you value most in your life?",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
            });

            //Add more sample question
            for (int i = 2; i <= 5; i++)
            {
                sampleQuestions.Add(new QuestionViewModel()
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
                sampleQuestions,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }
        #endregion
    }
}
