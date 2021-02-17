using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Common.Constants
{
    public static class LeadType
    {
        public const string Sales = "SL";
        public const string AfterSales = "ASL";
    };

    public static class LeadStatus
    {
        public const string SalesNew = "S-N1";
        public const string SalesAccepted = "S-A2";
        public const string SalesCustomerContacted = "S-CC3";
        public const string SalesTestDriveBooked = "S-TDB4";
        public const string TestDriveDone = "S-TDD5";
        public const string SalesSuccess = "S-S6";
        public const string AfterSalesNew = "AS-N8";
        public const string AfterSalesAccepted = "AS-A9";
        public const string AfterSalesCustomerContacted = "AS-CC10";
        public const string AfterSalesServiceBooked = "AS-SB11";
        public const string AfterSalesServiceDone = "AS-SD12";
        public const string AfterSalesSuccess = "AS-S13";
        public const string AfterSalesSalesLost = "S-SL14";
        public const string AfterSalesLost = "AS-ASL15";
    }

    public static class Roles
    {
        public const string Sales = "S";
        public const string AfterSales = "AS";
        public const string Admin = "A";
        public const string Dealer = "D";
    }
}
