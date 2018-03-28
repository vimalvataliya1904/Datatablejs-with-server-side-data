using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vimal_DataTable.Models;

namespace Vimal_DataTable.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetGridData()
        {
            string mode = Convert.ToString(Request.Form["mode"]);
            GridData og = new GridData(mode);
            return og.JsonData;
        }
    }
}