﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientDeviceID { get; set; }
    }
}