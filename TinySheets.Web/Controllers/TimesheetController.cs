using System;
using System.Linq;
using System.Web.Mvc;
using TinySheets.Entities;
using TinySheets.Persistence;

namespace TinySheets.Web.Controllers
{
    public class TimesheetController : Controller
    {
        readonly IRepository<TimeEntry> _timeEntries;

        public TimesheetController(IRepository<TimeEntry> timeEntries)
        {
            if (timeEntries == null) throw new ArgumentNullException("timeEntries");
            _timeEntries = timeEntries;
        }

        public ActionResult Index()
        {
            var timeEntries = _timeEntries.Items.ToArray();
            return View(timeEntries);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddEntry(string description, double hours)
        {
            var timeEntry = new TimeEntry(description, hours);
            _timeEntries.Add(timeEntry);
            return RedirectToAction("Index");
        }
    }
}