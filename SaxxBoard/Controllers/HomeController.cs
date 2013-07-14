using System.IO;
using System.Net;
using System.Net.Http;
using Raven.Client;
using SaxxBoard.ViewModels.Home;
using SaxxBoard.Widgets;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SaxxBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentStore _db;
        private readonly WidgetCollection _widgets;

        public HomeController(WidgetCollection widgets, IDocumentStore db)
        {
            _widgets = widgets;
            _db = db;
        }

        public ActionResult Index()
        {
            using (var dbSession = _db.OpenSession())
            {
                var viewModel = new IndexViewModel();
                return View(viewModel);
            }
        }

    }
}