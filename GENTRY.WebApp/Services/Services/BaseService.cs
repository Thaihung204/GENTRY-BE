using GENTRY.WebApp.Models;
using GENTRY.WebApp.Services.Interfaces;

namespace GENTRY.WebApp.Services.Services
{
    public class BaseService
    {
        protected readonly IRepository Repo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private GENTRYContext? _context;

        protected GENTRYContext GENTRYContext
        {
            get
            {
                if (_context == null)
                {
                    _context = _httpContextAccessor.HttpContext?.Items["GENTRYContext"] as GENTRYContext ?? new GENTRYContext();
                }
                return _context;
            }
        }

        protected Guid UserId => GENTRYContext.UserId;
        protected Guid AdminId => GENTRYContext.AdminId;

        public BaseService(IRepository repo)
        {
            this.Repo = repo;
        }

        public BaseService(IRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            this.Repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

    }
}
