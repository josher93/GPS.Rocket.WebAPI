﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.SVC.Models
{
    public class OneSignalFilter
    {
        public string field { get; set; }
        public string key { get; set; }
        public string relation { get; set; }
        public string value { get; set; }
    }
}
