using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Data.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        #region Private fields

        private ApplicationDbContext DbContext;

        #endregion

        #region Constructors

        public AnswerController(ApplicationDbContext context)
        {
            // Create ApplicationDbContext, using dependencies injection
            DbContext = context;
        }

        #endregion

        #region Methods adapting to the REST convention
        /// <summary>
        /// Retrieves the response with the specified {id}
        /// </summary>
        /// <param name="id">identifier of the existing response</param>
        /// <returns>the answer of {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var answer = DbContext.Answers.Where(i => i.Id == id).FirstOrDefault();

            // Handle requests asking for not existing answer
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Couldn't find answer with id {0}", id)
                });
            }

            return new JsonResult(
                answer.Adapt<AnswerViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }

        /// <summary>
        /// Adds a new answer to the database
        /// </summary>
        /// <param name="model">AnswerViewModel object with data to be inserted</param>
        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            // Returns general status code HTTP 500 (ServerError),
            // if data from the client is incorrect
            if (model == null) return new StatusCodeResult(500);

            // Replaces a ViewModel with a model
            var answer = model.Adapt<Answer>();

            // Replace properties, which should be set only by server
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;

            // Add new answer
            DbContext.Answers.Add(answer);
            // Save changes to database
            DbContext.SaveChanges();

            // Return new created answer to the client
            return new JsonResult(answer.Adapt<AnswerViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting= Formatting.Indented
                });
        }

        /// <summary>
        /// Modifies the response with the specified {id}
        /// </summary>
        /// <param name="model">AnswerViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {
            // Returns general status code HTTP 500 (Server Error),
            // if data from the client is incorrect
            if (model == null) return new StatusCodeResult(500);

            // Download answers to modify
            var answer = 
                DbContext.Answers.Where(q => q.Id == model.Id).FirstOrDefault();

            // Handle requests asking for non existing answers
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Couldnt find answer with id {0}", model.Id)
                });
            }

            // Handle the update (without mapping the objects)
            // by manually rewriting the properties received in the client task
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;

            // Properties set by the server
            answer.LastModifiedDate = DateTime.Now;

            // Save changes in database
            DbContext.SaveChanges();

            // Return updated answer to the client
            return new JsonResult(answer.Adapt<AnswerViewModel>(),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });

        }

        /// <summary>
        /// Deletes the response with the specified {id} from the database
        /// </summary>
        /// <param name="id">identification of the existing response</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Download asnwer from database
            var answer = 
                DbContext.Answers.Where(i => i.Id == id).FirstOrDefault();

            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Couldn't find answer with id {0}", id)
                });
            }

            // Delete answer from database
            DbContext.Answers.Remove(answer);
            // Save changes to Database
            DbContext.SaveChanges();

            // Return status code HTTP 204
            return new NoContentResult();
        }
        #endregion

        #region Routing methods based on atributes
        // GET api/answer/all
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var answers = 
                DbContext.Answers.Where(q => q.QuestionId == questionId).ToArray();

            //Pass the results in JSON format
            return new JsonResult(
                answers.Adapt<AnswerViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }
        #endregion
    }
}
