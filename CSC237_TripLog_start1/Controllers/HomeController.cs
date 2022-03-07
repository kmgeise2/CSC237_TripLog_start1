using CSC237_TripLog_start1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CSC237_TripLog_start1.Controllers
{
    public class HomeController : Controller
    {
        private TripLogContext context { get; set; }
        public HomeController(TripLogContext ctx) => context = ctx;

        public ViewResult Index()
        {
            // Change the OrderBy to StartDate
            var trips = context.Trips.OrderBy(t => t.Destination).ToList();
            return View(trips);
        }

    }
}
