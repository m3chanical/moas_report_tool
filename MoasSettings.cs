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
        public int Year { get; set; }
        public int Position { get; set; }
        public string ScaName { get; set; }
        public string ModernName { get; set; }
        public string EmailAddress { get; set; }
        public int MembershipNumber { get; set; }
        public bool WarrantCopy { get; set; }
        public DateTime MembershipExpiration { get; set; }
        public List<KeyValuePair<string, bool>> SharePermission { get; set; }
        public string SeneschalScaName { get; set; }
        public string SeneschalEmail { get; set; }
        public bool UpdateContactInformation { get; set; }
        public bool HaveDeputy { get; set; }
        public string NeedRecognition { get; set; }
        public string NeedFromKingdom { get; set; }
        public string GroupGoals { get; set; }
        public string WorkshopsOccurred { get; set; }
        public string ASEvents { get; set; }
        public string CollegiaEvents { get; set; }
        public string MiscASActivities { get; set; }
    }
}
