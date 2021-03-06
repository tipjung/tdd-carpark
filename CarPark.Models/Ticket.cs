﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string PlateNo { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }

        public decimal? ParkingFee
        {
            get
            {
                //decimal fee = 0m;
                //TimeSpan ts = DateOut - DateIn;
                //if (ts.TotalMinutes > 15)
                //{
                //    fee = 50;
                //    double minutes = ts.TotalMinutes - 180;
                //    while (minutes > 15)
                //    {
                //        fee += 30;
                //        minutes -= 60;
                //    }
                //}
                //return fee;

                if (DateOut == null)
                {
                    return null;
                }
                if (DateOut.Value < DateIn)
                {
                    throw new Exception("Invalid date");
                }

                TimeSpan ts = DateOut.Value - DateIn;
                ts = ts.Add(TimeSpan.FromMinutes(-15));
                if (ts.TotalMinutes <= 0)
                    return 0m;
                else if (ts.TotalHours <= 3.0)
                    return 50m;
                else
                    return 50m + ((decimal)Math.Ceiling(ts.TotalHours - 3) * 30m);
            }
        }
    }
}
