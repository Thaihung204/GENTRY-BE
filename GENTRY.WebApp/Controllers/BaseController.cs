    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using GENTRY.WebApp.Models;
    using GENTRY.WebApp.Services.Interfaces;

    namespace GENTRY.WebApp.Controllers
    {
        public class BaseController : Controller
        {
            public readonly IExceptionHandler exceptionHandler;

            public BaseController(IExceptionHandler exceptionHandler)
            {
                this.exceptionHandler = exceptionHandler;
            }
        }
    }
