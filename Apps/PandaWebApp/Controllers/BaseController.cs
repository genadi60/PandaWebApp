namespace PandaWebApp.Controllers
{
    using Data;
    using SIS.MvcFramework;

    public class BaseController : Controller
    {
        public BaseController()
        {
            this.Db = new PandaDbContext();
        }

        protected PandaDbContext Db { get; }
    }
}
