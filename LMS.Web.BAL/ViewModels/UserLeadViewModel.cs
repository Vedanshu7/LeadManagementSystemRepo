﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Web.BAL.ViewModels
{
    public class UserLeadViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ModelName { get; set; }
        public string AssignedUserName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerContactNumber { get; set; }
        public int LeadStatusId { get; set; }
        public string LeadStatus { get; set; }
        public string Comments { get; set; }
    }
}