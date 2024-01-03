using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course2.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CoachId { get; set; }
        public string Date { get; set; }
        public string SportsEquipment { get; set; }
        public string CardCVC { get; set; }
        public string CardNumber { get; set; }
        public string CardDate { get; set; }
        public int TimeStartId { get; set; }
        public int TimeEndId { get; set; }
        public int Sum { get; set; }
        public Client Client { get; set; }
        public Coach Coach { get; set; }
        public Time TimeStart { get; set; }
        public Time TimeEnd { get; set; }
    }
}