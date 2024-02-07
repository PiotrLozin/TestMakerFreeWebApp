using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFreeWebApp.ViewModels;
using TestMakerFreeWebApp.Data;
using Mapster;

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

            //Return results in JSON format
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
        public IActionResult Put(QuizViewModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Modifies the quiz with the specified {id}
        /// </summary>
        /// <param name="model">QuizViewModel object with data to be updated</param>
        [HttpPost]
        public IActionResult Post(QuizViewModel model)
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
