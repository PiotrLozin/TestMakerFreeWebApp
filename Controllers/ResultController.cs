using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Data.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : BaseApiController
    {

        #region Constructor

        public ResultController(ApplicationDbContext context)
            :   base(context) { }

        #endregion

        #region Methods adapting to the REST convention
        /// <summary>
        /// Retrieves the result with the specified {id}
        /// </summary>
        /// <param name="id">identifier of the existing result</param>
        /// <returns>the answer of {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = DbContext.Results.Where(i => i.Id == id).FirstOrDefault();

            // Handle requests asking for non-existing result
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Couldn't find answer with id {0}", id)
                });
            }

            return new JsonResult(
                result.Adapt<ResultViewModel>(),
                JsonSettings);
        }

    /// <summary>
    /// Adds a new result to the database
    /// </summary>
    /// <param name="model">ResultViewModel object with data to be inserted</param>
    [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {
            // Returns general status code HTTP 500 (server error),
            // if the data sent by the customer is incorrect
            if (model == null) return new StatusCodeResult(500);

            // Replicates ViewModel as Model
            var result = model.Adapt<Result>();

            // Overwrite properties,
            // wchich should be set up by server
            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;

            // Add new result
            DbContext.Results.Add(result);
            // Save changes in database
            DbContext.SaveChanges();

            // Return new added result to the client
            return new JsonResult(result.Adapt<ResultViewModel>(),
                JsonSettings);
        }

        /// <summary>
        /// Modifies the result with the specified {id}
        /// </summary>
        /// <param name="model">ResultViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {
            // Returns general status code HTTP 500 (server error),
            // if the data sent by the customer is incorrect
            if (model == null) return new StatusCodeResult(500);

            // Download question to edition
            var result = DbContext.Results.Where(q => q.Id == model.Id).FirstOrDefault();

            // Handle requests asking for irrelevant questions
            if (result == null) return NotFound(new
            {
                Error = String.Format("Didn't find result with id {0}", model.Id)
            });

            // Handle the update (without mapping the objects)
            // by manually rewriting the properties received in the client task
            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;

            // Properties set by the server
            result.LastModifiedDate = DateTime.Now;

            // Save changes in database
            DbContext.SaveChanges();

            // Returns updated result to the client
            return new JsonResult(result.Adapt<QuestionViewModel>(),
                JsonSettings);
        }

        /// <summary>
        /// Deletes the result with the specified {id} from the database
        /// </summary>
        /// <param name="id">identification of the existing result</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Download result from the database
            var result = DbContext.Results.Where(i => i.Id == id).FirstOrDefault();

            // Handle requests asking for irrelevant results
            if (result == null) return NotFound(new
            {
                Error = String.Format("Couldn't find question with id {0}", id)
            });

            // Delete result from database
            DbContext.Results.Remove(result);
            // Save changes to database
            DbContext.SaveChanges();

            // Return code status HTTP 204
            return new NoContentResult();
        }
        #endregion

        #region Routing methods based on atributes
        // GET api/answer/all
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var results = DbContext.Results
                .Where(q => q.QuizId == quizId)
                .ToArray();

            //Pass the results in JSON format
            return new JsonResult(
                results.Adapt<ResultViewModel[]>(),
                JsonSettings);
        }
        #endregion
    }
}
