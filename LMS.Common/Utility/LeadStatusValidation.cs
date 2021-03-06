using System.Collections.Generic;

namespace LMS.Common.Utility
{
    public static class LeadStatusValidation
    {
        private static IDictionary<string, int> leadStatus = new Dictionary<string, int>();

        static LeadStatusValidation()
        {
            leadStatus[Constants.LeadStatus.SalesNew] = 1;
            leadStatus[Constants.LeadStatus.SalesAccepted] = 2;
            leadStatus[Constants.LeadStatus.SalesCustomerContacted] = 3;
            leadStatus[Constants.LeadStatus.SalesTestDriveBooked] = 4;
            leadStatus[Constants.LeadStatus.SalesTestDriveDone] = 5;
            leadStatus[Constants.LeadStatus.SalesSuccess] = 6;
            leadStatus[Constants.LeadStatus.SalesSalesLost] = 7;
            leadStatus[Constants.LeadStatus.AfterSalesNew] = 8;
            leadStatus[Constants.LeadStatus.AfterSalesAccepted] = 9;
            leadStatus[Constants.LeadStatus.AfterSalesCustomerContacted] = 10;
            leadStatus[Constants.LeadStatus.AfterSalesServiceBooked] = 11;
            leadStatus[Constants.LeadStatus.AfterSalesServiceDone] = 12;
            leadStatus[Constants.LeadStatus.AfterSalesSuccess] = 13;
            leadStatus[Constants.LeadStatus.AfterSalesLost] = 15;
        }

        public static bool IsPrevious(string previousState, string currentState)
        {
            if (leadStatus[previousState] < leadStatus[currentState])
            {
                return true;
            }

            return false;
        }
    }
}
