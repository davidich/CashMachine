using System.Web.Mvc;

namespace CashMachine.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}