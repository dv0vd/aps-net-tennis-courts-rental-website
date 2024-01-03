using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using course2.Models;
using System.Collections;
using System.Web.Security;

namespace course2.Controllers
{ 
    public class HomeController : Controller
    {
        Context db = new Context();
        const int courtsCount = 4;
        const int PriceWithoutCoach = 300;
        const int PriceWithCoach = 1000;
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult CheckingLogin(string Password)
        {
            if(FormsAuthentication.Authenticate("admin", Password))
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return RedirectToAction("Index", "Admin");
            }
            return View("ErrorLogin");
        }
        public ActionResult Date()
        {
           ArrayList coaches = new ArrayList();
            foreach (var v in db.Coaches)
            {
                if ((v.Working) && (v.Name!="---УДАЛЁН---"))
                    coaches.Add(v);
            }
            ViewBag.Coaches = coaches;
            return View("Date");
        }
        public ActionResult Time(string Date, string Coach)
        {
            if (Date == null)
                return View("Index");
            ArrayList periods = new ArrayList();
            ViewBag.Date = Date;
            ViewBag.Coach = Coach;
            if (Coach == "не нужны")
            {
                periods = checkingWithoutCoach(Date);
            }
            else
            {
                periods = checkingWithoutCoach(Date);
                periods = checkingWithCoach(Date, FindCoach(Coach), periods);
            }
            ArrayList periodsEnd = checkingHours(periods);
            ViewBag.Periods = periodsEnd;
            return View("Time");
        }
        public ActionResult OrderDetails(string Time, string Date, string Coach)
        {
            if (Date == null) 
                return View("Index");
            ViewBag.Time = Time;
            ViewBag.Date = Date;
            ViewBag.Coach = Coach;
            int timeStartId =0, timeEndId=0;
            string timeStart = "", timeEnd = "";
            for(int i=0; i<5; i++)
            {
                timeStart += Time[i];
                timeEnd += Time[i + 8];
            }
            foreach(Time t in db.Times)
            {
                if(timeStart==t.Period)
                {
                    timeStartId = t.Id;
                    break;
                }
            }
            foreach (Time t in db.Times)
            {
                if (timeEnd == t.Period)
                {
                    timeEndId = t.Id;
                    break;
                }
            }
            ViewBag.TimeStartId = timeStartId;
            ViewBag.TimeEndId = timeEndId;
            return View("OrderDetails");
        }
        public ActionResult Payment(string Name, string Surname, string Phone, string CardNumber, string CardDate, string CardCVC, int? TimeStartId, int? TimeEndId, string Coach, string Date, string Time, bool SportsEquipment = false)
        {
           
            if (Name == null)
                return View("Index");
            int clientId = 0, count = 0;
            foreach (var v in db.Clients)
            {
                count++;
                if ((v.Name == Name) && (v.Surname == Surname) && (v.Phone == Phone))
                {
                    clientId = v.Id;
                    break;
                }
            }
            if (clientId == 0)
            {
                Client c = new Client{ Name = Name, Surname = Surname, Phone = Phone };
                db.Clients.Add(c);
                db.SaveChanges();
                clientId = c.Id;
            }
            string sportsequipment;
            if (SportsEquipment)
            {
                sportsequipment = "да";
            }
            else
            {
                sportsequipment = "нет";
            }
            int coachid = 0, price;
            if (Coach != "не нужны")
            {
                price = PriceWithCoach;
                coachid = FindCoach(Coach);
            }
            else
            {
                price = PriceWithoutCoach;
            }
            int sum = price * (TimeEndId.Value - TimeStartId.Value);
            Purchase p = new Purchase { ClientId = clientId, CoachId = coachid, Date = Date, SportsEquipment = sportsequipment, CardNumber = CardNumber, CardCVC = CardCVC, CardDate = CardDate, Sum = sum, TimeStartId = TimeStartId.Value, TimeEndId = TimeEndId.Value };
            ArrayList periods = new ArrayList(), periodsEnd = new ArrayList();
            if (Coach == "не нужны")
            {
                periods = checkingWithoutCoach(Date);
            }
            else
            {
                periods = checkingWithoutCoach(Date);
                periods = checkingWithCoach(Date, coachid, periods);
            }
            periodsEnd = checkingHours(periods);
            for (int i = 0; i < periodsEnd.Count; i++)
            {
                if (Time == (string)periodsEnd[i])
                {
                    string d = DateTime.Now.ToString("dd/MM/yyyy | HH:mm");
                    string s = sum.ToString();
                    db.Purchases.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("OK", new { name = Name, surname = Surname, phone = Phone, date = Date, time = Time, sportsEquipment = SportsEquipment, coach = Coach, today =  d, sum = s});
                }
            }
            return RedirectToAction("Error", new { name = Name});
        }
        public ActionResult Error(string Name)
        {
            if (Name == null)
                return View("Index");
            else
            {
                ArrayList coaches = new ArrayList();
                foreach (var v in db.Coaches)
                {
                    if (v.Working)
                        coaches.Add(v);
                }
                ViewBag.Coaches = coaches;
                return View("Error");
            }
        }
        public ActionResult OK(string Name, string Surname, string Phone, string Date, string Time, bool SportsEquipment, string Coach, string Today, string Sum)
        {
            if (Name == null)
                return View("Index");
            else
            {
                ViewBag.Name = Name;
                ViewBag.Surname = Surname;
                ViewBag.Phone = Phone;
                ViewBag.Date = Date;
                ViewBag.Time = Time;
                ViewBag.Today = Today;
                ViewBag.Sum = Sum;
                string sportsequipment;
                if (SportsEquipment)
                {
                    sportsequipment = "да";
                }
                else
                {
                    sportsequipment = "нет";
                }
                ViewBag.SportsEquipment = sportsequipment;
                ViewBag.Coach = Coach;
                return View("OK");
            }
        }
        private ArrayList checkingHours(ArrayList periods)
        {
            ArrayList array = new ArrayList();
            for(int j=0; j<db.Times.Count(); j++)
            {
                for (int i = 0; i < periods.Count - j - 1; i++)
                {
                    string temp1 = "", temp2 = "";
                    if ((int)periods[i + j] == (int)periods[i] + j)
                    {
                        temp1 = db.Times.Find((int)periods[i]).Period;
                        temp2 = db.Times.Find((int)periods[i] + j + 1).Period;
                    }
                    if ((temp1 != "") && (temp2 != ""))
                        array.Add(temp1 + " - " + temp2);
                }
            }
            return array;
        }
        private ArrayList checkingWithoutCoach(string date)
        {
            List<Purchase> purchases = new List<Purchase>();
            foreach(Purchase p in db.Purchases)
            {
                purchases.Add(p);
            }
            List<Time> times = new List<Time>();
            foreach (Time t in db.Times)
            {
                times.Add(t);
            }
            ArrayList array = new ArrayList();
            for (int i = 0; i < db.Times.Count(); i++)
            {
                int count = 0;
                for (int j = 0; j < db.Purchases.Count(); j++)
                {
                    if (count == courtsCount)
                        break;
                    if ((date == purchases[j].Date) && ((times[i].Id >= purchases[j].TimeStartId) && (times[i].Id < purchases[j].TimeEndId)))
                    {
                        count++;
                    }
                }
                if (count < courtsCount) 
                {
                    array.Add(times[i].Id);
                }
            }
            return array;
        }
        private ArrayList checkingWithCoach(string date, int coachid, ArrayList per)
        {
            ArrayList array = new ArrayList();
            for (int i=0; i<per.Count; i++)
            {
                bool check = false;
                foreach(Purchase p in db.Purchases)
                {
                    if((p.Date==date) && (p.CoachId==coachid))
                    {
                        check = true;
                        if(((int)per[i]<p.TimeStartId) || ((int)per[i] >= p.TimeEndId))
                        {
                            array.Add(per[i]);
                        }
                    }
                }
                if(!check)
                    array.Add(per[i]);
            }
            return array;
        }
        private int FindCoach(string coach)
        {
            int coachid = 0;
            foreach (Coach c in db.Coaches)
            {
                if (coach == c.Name)
                {
                    coachid = c.Id;
                }
            }
            return coachid;
        }
    }
}