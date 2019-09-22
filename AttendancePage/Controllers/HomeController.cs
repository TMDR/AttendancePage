using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AttendancePage.Models;
using System.Collections.Generic;
using System.Collections;
using System;

namespace AttendancePage.Controllers
{
    public class HomeController : Controller
    {
        public static string admin0 = "admin0";
        public static string admin1 = "admin1";
        public static string LocalDate = DateTime.Now.Year.ToString() + "-" + ((DateTime.Now.Month >= 10) ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString());
        public static bool openAdmin = false;
        public static bool openUser = false;
        public static string UserName = string.Empty;
        public static List<DateTime11> seeTime = new List<DateTime11>();
        public static string SeePage = "See";
        public static SortedSet<int> seeYear = new SortedSet<int>();
        public static SortedSet<int> seeMonth = new SortedSet<int>();
        public static SortedSet<int> seeDay = new SortedSet<int>();
        public static string[] aWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        public static bool openSee = false;
        private dbcontext db = new dbcontext();
        public static List<string> workers = new List<string>();
        public static List<string> workersToDeny = new List<string>();
        public static Dictionary<string, int> Duration = new Dictionary<string, int>();
        public static List<workerPerMonth> workerPerMonths = new List<workerPerMonth>();
        public static float rate = 0;

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult See()
        {
            if (HomeController.seeDay != null && HomeController.seeDay.Any())
                HomeController.seeDay.Clear();
            foreach (DateTime11 dt in db.DateTime11)
            {
                HomeController.seeDay.Add(dt.Date.Value.Day);
            }
            if (HomeController.openSee)
            {
                HomeController.openSee = false;
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public ActionResult CalculateTotalDuration()
        {
            int duration; HomeController.Duration.Clear();
            foreach (Worker worker in db.Worker)
            {
                duration = 0;
                foreach (DateTime11 date in HomeController.seeTime.Where(e => e.Email.Equals(worker.Email)))
                {
                    if (date.TimeOut.HasValue)
                    {
                        if (date.TimeOut.Value.Subtract(date.TimeIn.Value).Minutes <= 30)
                        {
                            duration += date.TimeOut.Value.Subtract(date.TimeIn.Value).Hours;
                        }
                        else
                        {
                            duration += date.TimeOut.Value.Subtract(date.TimeIn.Value).Hours + 1;
                        }
                    }
                }
                HomeController.Duration.Add(worker.Email, duration);
            }
            return Json(new { status = true });
        }

        [HttpPost]
        public IActionResult SeeRedirect()
        {
            HomeController.openSee = true;
            return Json(new { status = true });
        }




        [HttpGet]
        public IActionResult About()
        {
            if (HomeController.openAdmin)
            {
                HomeController.openAdmin = false;
                return View();
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }

        [HttpPost]

        public ActionResult FalseUser()
        {
            openUser = false;
            return Json(new { status = false });
        }

        [HttpPost]

        public ActionResult allowUser()
        {
            openUser = true;
            return Json(new { status = false });
        }

       [HttpPost]

        public ActionResult FalseAdmin()
        {
            openAdmin = false;
            return Json(new { status = false });
        }

        [HttpPost]

        public ActionResult ContactRedirect(string UserName)
        {
            HomeController.UserName = UserName;
            return Json(new { status = true});
        }

        [HttpPost]
        public ActionResult submit(string email, string password, float Rate)
        {
            Worker worker = new Worker(); worker.Email = email; worker.Psswrd = password; worker.RatePerHour = Rate;
            db.Worker.Add(worker);
            db.SaveChanges();
            return Json(new { status = true });
        }
        public IActionResult Contact()
        {
            HomeController.seeTime.Clear();
            foreach (DateTime11 dt in db.DateTime11.Where(e => e.Email == HomeController.UserName))
            {
                HomeController.seeTime.Add(dt);
            }
            HomeController.seeTime.Sort(delegate (DateTime11 c1, DateTime11 c2) { return c1.Date.Value.CompareTo(c2.Date.Value); });
            if (HomeController.seeDay != null && HomeController.seeDay.Any())
                HomeController.seeDay.Clear();
            if (HomeController.seeMonth != null && HomeController.seeMonth.Any())
                HomeController.seeMonth.Clear();
            if (HomeController.seeYear != null && HomeController.seeYear.Any())
                HomeController.seeYear.Clear();
            foreach (DateTime11 dt in db.DateTime11)
            {
                HomeController.seeYear.Add(dt.Date.Value.Year);
                HomeController.seeMonth.Add(dt.Date.Value.Month);
                HomeController.seeDay.Add(dt.Date.Value.Day);
            }
            HomeController.workerPerMonths.Clear(); int duration;
            foreach (int year in HomeController.seeYear)
            {
                foreach (int month in HomeController.seeMonth)
                {
                    duration = 0;
                    if (db.DateTime11.Where(e => e.Email.Equals(HomeController.UserName) && e.Date.Value.Year.Equals(year) && e.Date.Value.Month.Equals(month)).Any())
                    {
                        foreach (DateTime11 date in db.DateTime11.Where(e => e.Email.Equals(HomeController.UserName) && e.Date.Value.Year.Equals(year) && e.Date.Value.Month.Equals(month)))
                        {
                            if (date.TimeOut.HasValue)
                            {
                                if (date.TimeOut.Value.Subtract(date.TimeIn.Value).Minutes < 30)
                                {
                                    duration += date.TimeOut.Value.Subtract(date.TimeIn.Value).Hours;
                                }
                                else
                                {
                                    duration += date.TimeOut.Value.Subtract(date.TimeIn.Value).Hours + 1;
                                }
                            }
                        }
                        HomeController.workerPerMonths.Add(new workerPerMonth(year, month, duration));
                    }
                }
            }
            foreach (Worker worker in db.Worker)
            {
                if (worker.Email.Equals(HomeController.UserName) && !worker.Email.Equals("admin0"))
                {
                    HomeController.rate = (float)worker.RatePerHour;
                }
            }
            if (HomeController.openUser)
            {
                HomeController.openUser = false;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult remove(string email, string password)
        {
            Worker worker = db.Worker.Find(email);
            db.Worker.Remove(worker);
            db.SaveChanges();
            return Json(new { status = true });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public ActionResult Validate(string email, string password)
        {
            Worker _admin = db.Worker.Where(e => e.Email.Equals(email)).FirstOrDefault();
            if (_admin != null)
            {
                if (_admin.Psswrd.Equals(password))
                {
                    if (email == "admin0" || email == "admin1")
                        HomeController.openAdmin = true;
                    else
                        HomeController.openUser = true;
                    return Json(new { status = true, message = "login successful" });
                }
                else
                    return Json(new { status = false, message = "Invalid password" });
            }
            else
            {
                return Json(new { status = false, message = "invalid email" });
            }
        }

        [HttpPost]
        public ActionResult adminOrNot(string email)
        {

            if (email.Equals(HomeController.admin1)) 
                return Json(new { status = false ,message=email});
            return Json(new { status = true, message = email });
        }


        [HttpPost]

        public ActionResult TimeInExceptionBreak(string inputBreak)
        {
            foreach (string s in HomeController.workers)
            {
                if (!HomeController.workersToDeny.Contains(s))
                {
                    DateTime11 date = new DateTime11();
                    date.Email = s;
                    date.Date = DateTime.Today;
                    date.Id = new Random().Next();
                    date.TimeIn = DateTime.Now.TimeOfDay;
                    if (db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).Any())
                    {
                        List<DateTime11> listd = db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).ToList();
                        if (!listd.OrderBy(e => e.TimeIn).Last().TimeOut.HasValue)
                        {
                            if (inputBreak != null)
                            {
                                db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).OrderBy(e => e.TimeIn).Last().TimeOut = TimeSpan.Parse(inputBreak);
                            }
                            else
                            {
                                db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).OrderBy(e => e.TimeIn).Last().TimeOut = date.TimeIn;
                            }
                            db.DateTime11.Update(db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).OrderBy(e => e.TimeIn).Last());
                            db.SaveChanges();
                        }
                    }
                    db.DateTime11.Add(date);
                    db.SaveChanges();
                }

                db.SaveChanges();
            }
            db.SaveChanges();
            return Json(new { status = true });
        }

        [HttpPost]
        public ActionResult TimeIn(string[] list)
        {
            HomeController.workers.Clear();
            HomeController.workersToDeny.Clear();
            foreach (string s in list)
            {
                DateTime11 date = new DateTime11();
                date.Email = s;
                date.Date = DateTime.Today;
                date.Id = new Random().Next();
                date.TimeIn = DateTime.Now.TimeOfDay;
                if (db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).Any())
                {
                    List<DateTime11> listd = db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).ToList();
                    if (!listd.OrderBy(e => e.TimeIn).Last().TimeOut.HasValue)
                    {
                        foreach (string r in list)
                        {
                            HomeController.workers.Add(r);
                        }
                        db.SaveChanges();
                        return Json(new { status = false });
                    }
                    else
                    {
                        HomeController.workersToDeny.Add(s);
                    }
                }
                HomeController.workersToDeny.Add(s);
                db.DateTime11.Add(date);
                db.SaveChanges();
            }
            db.SaveChanges();
            return Json(new { status = true });
        }
        [HttpPost]
        public ActionResult TimeOut(string[] list)
        {
            foreach (string s in list)
            {
                DateTime11 date = db.DateTime11.Where(e => e.Email.Equals(s) && e.Date.Equals(DateTime.Today)).OrderBy(e => e.TimeIn).Last();
                if (date == null)
                {
                    continue;
                }
                date.TimeOut = DateTime.Now.TimeOfDay;
                db.DateTime11.Update(date);
                db.SaveChanges();
            }
            return Json(new { status = true });
        }
        [HttpPost]
        public ActionResult checkItOut(string email)
        {
            if (db.DateTime11.Where(e => e.Email == email && e.Date.Value.Year.Equals(DateTime.Today.Year) && e.Date.Value.Month.Equals(DateTime.Today.Month) && e.Date.Value.Day.Equals(DateTime.Today.Day)).Any())
                return Json(new { status = true });
            return Json(new { status = false });
        }
        [HttpPost]
        public ActionResult seeItLocal()
        {
            if (HomeController.seeTime != null && HomeController.seeTime.Any())
                HomeController.seeTime.Clear();
            foreach (DateTime11 dt in db.DateTime11.Where(e => e.Date.Value.Year.Equals(DateTime.Today.Year) && e.Date.Value.Month.Equals(DateTime.Today.Month)))
            {
                HomeController.seeTime.Add(dt);
            }
            HomeController.seeTime.Sort(delegate (DateTime11 c1, DateTime11 c2) { return c1.Date.Value.CompareTo(c2.Date.Value); });
            return Json(new { status = true });
        }
        [HttpPost]
        public ActionResult seeItGiven(string Date)
        {
            if (HomeController.seeTime != null && HomeController.seeTime.Any())
                HomeController.seeTime.Clear();
            if (Date == null)
            {
                return Json(new { status = false });
            }
            string year = Date.Substring(0, Date.IndexOf("-"));
            string month = Date.Substring(Date.IndexOf("-") + 1, Date.Length - year.Length - 1);
            List<DateTime11> list = db.DateTime11.ToList();
            foreach (DateTime11 dt in list)
            {
                if (dt.Date.Value.Year == int.Parse(year) && dt.Date.Value.Month == int.Parse(month))
                {
                    HomeController.seeTime.Add(dt);
                }
            }
            HomeController.seeTime.Sort(delegate (DateTime11 c1, DateTime11 c2) { return c1.Date.Value.CompareTo(c2.Date.Value); });
            return Json(new { status = true });
        }
        [HttpPost]
        public ActionResult checkadmin(string email)
        {
            if (email == admin0 || email == admin1)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }

        }
    }
}
