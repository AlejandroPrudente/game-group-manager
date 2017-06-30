using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameGroupManager.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{
		public ActionResult Index()	// Index(string name, int number) to get parameters name and number passed in the query string
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}