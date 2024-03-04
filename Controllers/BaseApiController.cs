using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TestMakerFreeWebApp.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace TestMakerFreeWebApp.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        #region Constructor

        public BaseApiController(ApplicationDbContext context) 
        {
            // Create ApplicationDbContext using dependency injection
            DbContext = context;

            // Create singe object JsonSerializerSetting,
            // which can be used multiple times
            JsonSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
        }

        #endregion

        #region Shared properties

        protected ApplicationDbContext DbContext { get; private set; }

        protected JsonSerializerSettings JsonSettings { get; private set; }

        #endregion
    }
}
