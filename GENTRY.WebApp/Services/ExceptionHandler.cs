using AutoMapper;
using Microsoft.Extensions.Logging;
using GENTRY.WebApp.Controllers;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public ExceptionHandler(ILoggerFactory loggerFactory, IMapper mapper)
        {
            logger = loggerFactory.CreateLogger<BaseController>();
            this.mapper = mapper;
        }

        public void RaiseException(Exception ex, string customMessage = "")
        {
            logger.LogError(ex, customMessage);
        }
    }
}
