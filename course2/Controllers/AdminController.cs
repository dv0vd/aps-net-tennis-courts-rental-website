using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using course2.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Collections;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;

namespace course2.Controllers
{
    public class AdminController : Controller
    {
        Context db = new Context();
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Exit()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Clients()
        {
            ArrayList clients = new ArrayList();
            foreach (var c in db.Clients)
            {
                if (c.Name != "---УДАЛЁН---")
                    clients.Add(c);
            }
            ViewBag.Clients = clients;
            return View("Clients");
        }
        public ActionResult Coaches()
        {
            ArrayList coaches = new ArrayList();
            foreach (var c in db.Coaches)
            {
                if (c.Name != "---УДАЛЁН---")
                    coaches.Add(c);
            }
            ViewBag.Coaches = coaches;
            return View("Coaches");
        }
        public ActionResult AddCoach(string Name)
        {
            bool check = false;
            Coach c = new Coach { Name = Name, Working = true };
            foreach (var v in db.Coaches)
            {
                if (v.Name == c.Name)
                {
                    check = true;
                    break;
                }
            }
            if (!check)
                db.Coaches.Add(c);
            db.SaveChanges();
            ArrayList coaches = new ArrayList();
            foreach (var g in db.Coaches)
            {
                if (g.Name != "---УДАЛЁН---")
                    coaches.Add(g);
            }
            ViewBag.Coaches = coaches;
            return View("Coaches");
        }
        public ActionResult RemoveCoach(int id)
        {
            foreach (var b in db.Coaches)
            {
                if (b.Id == id)
                {
                    db.Coaches.Remove(b);
                    break;
                }
            }
            db.SaveChanges();
            ArrayList coaches = new ArrayList();
            foreach (var b in db.Coaches)
            {
                if (b.Name != "---УДАЛЁН---")
                    coaches.Add(b);
            }
            ViewBag.Coaches = coaches;
            return View("Coaches");
        }
        public ActionResult RemoveCoaches()
        {
            foreach (var b in db.Coaches)
            {
                db.Coaches.Remove(b);
                break;
            }
            db.SaveChanges();
            ViewBag.Coaches = db.Coaches;
            return View("Coaches");
        }
        public ActionResult HideCoach(int id)
        {
            ArrayList coaches = new ArrayList();
            foreach (var g in db.Coaches)
            {
                if (g.Id == id)
                {
                    g.Working = false;
                }
                if (g.Name != "---УДАЛЁН---")
                    coaches.Add(g);
            }
            ViewBag.Coaches = coaches;
            db.SaveChanges();
            return View("Coaches");
        }
        public ActionResult ShowCoach(int id)
        {
            ArrayList coaches = new ArrayList();
            foreach (var g in db.Coaches)
            {
                if (g.Id == id)
                {
                    g.Working = true;
                }
                if (g.Name != "---УДАЛЁН---")
                    coaches.Add(g);
            }
            ViewBag.Coaches = coaches;
            db.SaveChanges();
            return View("Coaches");
        }
        public ActionResult Purchases()
        {
            ViewBag.Purchases = db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd);
            return View("Purchases");
        }
        public ActionResult RemoveClient(int id)
        {
            foreach (var b in db.Clients)
            {
                if (b.Id == id)
                {
                    db.Clients.Remove(b);
                    break;
                }
            }
            db.SaveChanges();
            ArrayList clients = new ArrayList();
            foreach (var b in db.Clients)
            {
                if (b.Name != "---УДАЛЁН---")
                    clients.Add(b);
            }
            ViewBag.Clients = clients;
            return View("Clients");
        }
        public ActionResult RemoveClients()
        {
            foreach (var b in db.Clients)
            {
                db.Clients.Remove(b);
            }
            db.SaveChanges();
            ViewBag.Clients = db.Clients;
            return View("Clients");
        }
        public ActionResult RemovePurchases()
        {
            foreach (var b in db.Purchases)
            {
                db.Purchases.Remove(b);
            }
            db.SaveChanges();
            ViewBag.Purchases = db.Purchases;
            return View("Purchases");
        }
        public ActionResult RemovePurchase(int id)
        {
            foreach (var b in db.Purchases)
            {
                if (b.Id == id)
                {
                    db.Purchases.Remove(b);
                    break;
                }
            }
            db.SaveChanges();
            ViewBag.Purchases = db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd);
            return View("Purchases");
        }
        public ActionResult ClearAll()
        {
            foreach (var g in db.Clients)
            {
                db.Clients.Remove(g);
            }
            foreach (var g in db.Coaches)
            {
                db.Coaches.Remove(g);
            }
            foreach (var g in db.Purchases)
            {
                db.Purchases.Remove(g);
            }
            db.SaveChanges();
            return View("Index");
        }
        public ActionResult SearchClientPhoneNameSurname(string Name, string Surname, string Phone)
        {
            ArrayList clients = new ArrayList();
            foreach (var v in db.Clients)
            {
                if ((v.Name == Name) && (v.Surname == Surname) && (v.Phone == Phone))
                {
                    clients.Add(v);
                }
            }
            ViewBag.Clients = clients;
            return View("SearchClient");
        }
        public ActionResult SearchClientName(string Name)
        {
            ArrayList clients = new ArrayList();
            foreach (var v in db.Clients)
            {
                if (v.Name == Name)
                {
                    clients.Add(v);
                }
            }
            ViewBag.Clients = clients;
            return View("SearchClient");
        }
        public ActionResult SearchClientSurname(string Surname)
        {
            ArrayList clients = new ArrayList();
            foreach (var v in db.Clients)
            {
                if (v.Surname == Surname)
                {
                    clients.Add(v);
                }
            }
            ViewBag.Clients = clients;
            return View("SearchClient");
        }
        public ActionResult SearchClientNameSurname(string Name, string Surname)
        {
            ArrayList clients = new ArrayList();
            foreach (var v in db.Clients)
            {
                if ((v.Name == Name) && (v.Surname == Surname))
                {
                    clients.Add(v);
                }
            }
            ViewBag.Clients = clients;
            return View("SearchClient");
        }
        public ActionResult SearchClientPhone(string Phone)
        {
            ArrayList clients = new ArrayList();
            foreach (var v in db.Clients)
            {
                if (v.Phone == Phone)
                {
                    clients.Add(v);
                }
            }
            ViewBag.Clients = clients;
            return View("SearchClient");
        }
        public ActionResult SearchPurchaseNameSurname(string Name, string Surname)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if ((v.Client.Name == Name) && (v.Client.Surname == Surname))
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult SearchPurchaseCoachName(string Name)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if (v.Coach.Name == Name)
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult SearchPurchaseDate(string Date)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if (v.Date == Date)
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult SearchPurchaseTime(string Time)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if (v.TimeStart.Period + " - " + v.TimeEnd.Period == Time)
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult SearchPurchaseDateTime(string Date, string Time)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if ((v.TimeStart.Period + " " + v.TimeEnd.Period == Time) && (v.Date == Date))
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult SearchPurchaseCardNumber(string CardNUmber)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if (v.CardNumber == CardNUmber)
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult SearchPurchaseNameSurnameDate(string Name, string Surname, string Date)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if ((v.Client.Name == Name) && (v.Client.Surname == Surname) && (v.Date == Date))
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult SearchPurchaseCoachNameDate(string Name, string Date)
        {
            ArrayList purchases = new ArrayList();
            foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
            {
                if ((v.Coach.Name == Name) && (v.Date == Date))
                {
                    purchases.Add(v);
                }
            }
            ViewBag.Purchases = purchases;
            return View("SearchPurchase");
        }
        public ActionResult Download()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Корты");
                worksheet.Cell("A1").Value = "Id";
                worksheet.Cell("B1").Value = "Клиент";
                worksheet.Cell("C1").Value = "Тренер";
                worksheet.Cell("D1").Value = "Дата";
                worksheet.Cell("E1").Value = "Время";
                worksheet.Cell("F1").Value = "Спортивное оборудование";
                worksheet.Cell("G1").Value = "Номер карты";
                worksheet.Cell("H1").Value = "Срок действия карты";
                worksheet.Cell("I1").Value = "Проверочный код карты";
                worksheet.Cell("J1").Value = "Сумма, руб.";
                worksheet.Row(1).Style.Font.Bold = true;
                int i = 2;
                foreach (var v in db.Purchases.Include(p => p.Coach).Include(p => p.Client).Include(p => p.TimeStart).Include(p => p.TimeEnd))
                {
                    worksheet.Cell(i,1).Value = v.Id;
                    worksheet.Cell(i, 2).Value = v.Client.Name+" "+v.Client.Surname;
                    worksheet.Cell(i, 3).Value = v.Coach.Name;
                    worksheet.Cell(i, 4).Value = v.Date;
                    worksheet.Cell(i, 5).Value = v.TimeStart.Period+" - "+v.TimeStart.Period;
                    worksheet.Cell(i, 6).Value = v.SportsEquipment;
                    worksheet.Cell(i, 7).Value = v.CardNumber;
                    worksheet.Cell(i, 8).Value = v.CardDate;
                    worksheet.Cell(i, 9).Value = v.CardCVC;
                    worksheet.Cell(i, 10).Value = v.Sum;
                    i++;
                }
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = "Корты.xlsx"
                    };
                }
            }
        }
    }
}
