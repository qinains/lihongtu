using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule.Young.Entity
{
    public class ClubMember
    {
        private string member_id;

        public string MEMBER_ID
        {
            get { return member_id; }
            set { member_id = value; }
        }

        private string cust_id;

        public string CUST_ID
        {
            get { return cust_id; }
            set { cust_id = value; }
        }

        private string member_code;
        public string MEMBER_CODE
        {
            get { return member_code; }
            set { member_code = value; }
        }

        private string member_name;
        public string MEMBER_NAME
        {
            get { return member_name; }
            set { member_name = value; }
        }

        private string membership_level;
        public string MEMBERSHIP_LEVEL
        {
            get { return membership_level; }
            set { membership_level = value; }
        }

        private string access_date;
        public string ASSESS_DATE
        {
            get { return access_date; }
            set { access_date = value; }
        }

        private string eff_date;
        public string EFF_DATE
        {
            get { return eff_date; }
            set { eff_date = value; }
        }

        private string exp_date;
        public string EXP_DATE
        {
            get { return exp_date; }
            set { exp_date = value; }
        }

        private string status_cd;
        public string STATUS_CD
        {
            get { return status_cd; }
            set { status_cd = value; }
        }

    }
}
