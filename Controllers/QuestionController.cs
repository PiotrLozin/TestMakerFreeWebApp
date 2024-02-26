using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Data.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        #region Private fields

        private ApplicationDbContext DbContext;

        #endregion

        #region Constructors

        public QuestionController(ApplicationDbContext context)
        {
            // Create ApplicationDbContext using incjected dependencies
            DbContext = context;
        }

        #endregion

        #region Methods adapting to the REST convention
        /// <summary>
        /// Retrieves the question with the specified {id}
        /// </summary>
        /// <param name="id">identifier of the existing question</param>
        /// <returns>question with {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = 
                DbContext.Questions.Where(i => i.Id == id).FirstOrDefault();

            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Didn't find question with id {0}", id)
                });
            }

            return new JsonResult(
                question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }

        /// <summary>
        /// Adds a new question to the database
        /// </summary>
        /// <param name="model">QuestionViewModel object with data to be inserted</param>
        [HttpPut]
        public IActionResult Put([FromBody]QuestionViewModel model)
        {
            // Returns general http code 500 (Server Error),
            // if the data sent by the customer is incorrect
            if (model == null) return new StatusCodeResult(500);

            // Replicates ViewModel as Model
            var question = model.Adapt<Question>();

            // Overwrite properties,
            // wchich should be set up by server
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;

            // Add new question
            DbContext.Questions.Add(question);
            // Save changes in database
            DbContext.SaveChanges();

            // Return new added question to the client
            return new JsonResult(question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }

        /// <summary>
        /// Modifies the question with the specified {id}
        /// </summary>
        /// <param name="model">QuestionViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {
            // Returns general status code HTTP 500 (server error),
            // if the data sent by the customer is incorrect
            if (model == null) return new StatusCodeResult(500);

            // Download question to edition
            var question = DbContext.Questions.Where(q => q.Id == model.Id).FirstOrDefault();

            // Handle requests asking for irrelevant questions
            if (question == null) return NotFound(new
            {
                Error = String.Format("Didn't find question with id {0}", model.Id)
            });

            // Handle the update (without mapping the objects)
            // by manually rewriting the properties received in the client task
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;

            // Properties set by the server
            question.LastModifiedDate = DateTime.Now;

            // Save changes in database
            DbContext.SaveChanges();

            // Returns updated question to the client
            return new JsonResult(question.Adapt<QuestionViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }

        /// <summary>
        /// Deletes the question with the specified {id} from the database
        /// </summary>
        /// <param name="id">identification of the existing question</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Download question from the database
            var question = DbContext.Questions.Where(i => i.Id == id).FirstOrDefault();

            // Handle requests asking for irrelevant questions
            if (question == null) return NotFound(new
            {
                Error = String.Format("Couldn't find question with id {0}", id)
            });

            // Delete question from database
            DbContext.Questions.Remove(question);
            // Save changes to database
            DbContext.SaveChanges();

            // Return code status HTTP 204
            return new NoContentResult();
        }
        #endregion

        #region Routing methods based on atributes
        //GET api/guestion/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = 
                DbContext.Questions.Where(q => q.QuizId == quizId).ToArray();

            //Pass the results in JSON format
            return new JsonResult(
                questions.Adapt<QuestionViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }
        #endregion
    }
}
