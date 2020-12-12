using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moasreport
{
    class MoasSettings
    {
        public int GroupName;
        public int Quarter;
        public int Year;
        public int Position;
        public string ScaName;
        public string ModernName;
        public string EmailAddress;
        public int MembershipNumber;
        public bool WarrantCopy;
        public DateTime MembershipExpiration;
        public List<KeyValuePair<string, bool>> SharePermission;
        public string SeneschalScaName;
        public string SeneschalEmail;
        public bool UpdateContactInformation;
        public bool HaveDeputy;
        public string NeedRecognition;
        public string NeedFromKingdom;
        public string GroupGoals;
        public string WorkshopsOccurred;
        public string ASEvents;
        public string CollegiaEvents;
        public string MiscASActivities;
    }
}
