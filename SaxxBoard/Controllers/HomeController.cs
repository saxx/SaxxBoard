using SaxxBoard.Models;
using SaxxBoard.ViewModels.Home;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SaxxBoard.Controllers
{
    public class HomeController : Controller
    {
        private readonly Db _db;
        private readonly WidgetCollection _widgets;

        public HomeController(WidgetCollection widgets, Db db)
        {
            _widgets = widgets;
            _db = db;
        }

        public ActionResult Index()
        {
            // just try if the DB works here. This makes it easier to find problems, because otherwise the exceptions will be thrown in some async threads
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                _db.DataPoints.Count();
            }
            catch (Exception ex)
            {
                throw new System.ApplicationException("Database seems not to work: " + ex.Message, ex);
            }

            var viewModel = new IndexViewModel
                {
                    CurrentWidgets = _widgets
                };
            return View(viewModel);
        }

    }
}