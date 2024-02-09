using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;
using TestMakerFreeWebApp.Data;
using Mapster;
using TestMakerFreeWebApp.Data.Models;
using System.Data;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        #region private fields
        private ApplicationDbContext DbContext;
        #endregion

        #region Constructor
        public QuizController(ApplicationDbContext context)
        {
            // Create applicationDbContext by injecting dependencies
            DbContext = context;
        }
        #endregion

        #region Methods adapting to the REST convention
        /// <summary>
        /// GET: api/quiz/{}id
        /// </summary>
        /// <param name="id">Identifier of the existing quiz</param>
        /// <returns>Quiz with provided {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = DbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();

            // Handle requests asking for non-existent quizzes
            if (quiz == null) 
            {
                return NotFound(new
                {
                    Error = String.Format("Unable to find quiz with ID {0}", id)
                }); ;
            }

            // Return results in JSON format
            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }

        /// <summary>
        /// Adds a new quiz to the database
        /// </summary>
        /// <param name="model">QuizViewModel object with data to be inserted</param>
        [HttpPut]
        public IActionResult Put([FromBody]QuizViewModel model)
        {
            // Returns status code HTTP 500 (Server Error)
            // if transmitted data are improperly
            if (model == null) return new StatusCodeResult(500);

            // Support for insertion without object mapping
            var quiz = new Quiz();

            // Properties uploaded from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            // Properties handled by server
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;

            // Right now, autor will be assigned to Admin
            // because login function is not implemented yet
            quiz.UserId = DbContext.Users.Where(u => u.UserName == "Admin").
                FirstOrDefault().Id;

            // Add new quiz
            DbContext.Quizzes.Add(quiz);
            // Save changes in DataBase
            DbContext.SaveChanges();

            // Return the newly created quiz to the client
            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting= Formatting.Indented,
                });

        }

        /// <summary>
        /// Modifies the quiz with the specified {id}
        /// </summary>
        /// <param name="model">QuizViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post([FromBody]QuizViewModel model)
        {
            // Returns status code HTTP 500 (Server Error)
            // if transmitted data are improperly
            if (model == null) return new StatusCodeResult(500);

            // Upload quiz to modify
            var quiz = DbContext.Quizzes.Where(q => q.Id == model.Id).FirstOrDefault();

            // Handle requests for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Cannot found quiz with ID {0}", model.Id)
                });
            }

            // Properties uploaded from the request
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;

            // Properties handled by server
            quiz.LastModifiedDate = DateTime.Now;

            // Save changes to DataBase
            DbContext.SaveChanges();

            // Return changed quiz to the client
            return new JsonResult(
                quiz.Adapt<QuizViewModel>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }

        /// <summary>
        /// Deletes the response with the specified {id} from the database
        /// </summary>
        /// <param name="id">identification of the existing response</param>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Upload quiz do modify
            var quiz = DbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();

            // Handle request for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Cannot found quiz with ID {0}", id)
                });
            }

            // Delete quiz from DataBase
            DbContext.Quizzes.Remove(quiz);
            // Save changes in DataBase
            DbContext.SaveChanges();

            // Return HTTP code status 204
            return new NoContentResult();
        }
        #endregion

        #region Routing methods based on atributes
        /// <summary>
        /// GET api/quiz/latest
        /// Get {num} first quizes
        /// </summary>
        /// <param name="id">Number of quizes to GET</param>
        /// <returns>{num} newest quiz</returns>
        [HttpGet("Latest/{num?}")]
        public IActionResult Latest(int num = 10)
        {
            var latest = DbContext.Quizzes
                .OrderByDescending(q => q.CreatedDate)
                .Take(num)
                .ToArray();

            //Pass the results in JSON format
            return new JsonResult(
                latest.Adapt<QuizViewModel[]>(),
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
            var byTitle = DbContext.Quizzes
                .OrderBy(q => q.Title)
                .Take(num)
                .ToArray();
            
            return new JsonResult(
                byTitle.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
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
            var random = DbContext.Quizzes
                .OrderBy(q => Guid.NewGuid())
                .Take(num)
                .ToArray();

            return new JsonResult(
                random.Adapt<QuizViewModel[]>(),
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                });
        }
        #endregion
    }
}
