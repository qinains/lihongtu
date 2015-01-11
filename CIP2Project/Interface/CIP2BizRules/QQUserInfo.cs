using System;
using System.Collections.Generic;
using System.Text;

namespace Linkage.BestTone.Interface.Rule
{



    public class Data
    {
        QQUserInfo userinfo;
        public QQUserInfo UserInfo
        {
            get { return userinfo; }
            set { userinfo = value; }
        }
    }

    public class QQUserInfo
    {
        string ret;

        public string Ret
        {
            get { return ret; }
            set { ret = value; }
        }
        string msg;

        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }
        string nick;

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }
        
        string sex;

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        string openid;
        public string Openid
        {
            get {return openid; }
            set {openid = value;}
        }

        string province_code;
        public string Province_code
        {
            get { return province_code; }
            set {province_code = value;}
        }
        
        
    }
}
