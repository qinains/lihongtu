using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Linkage.BestTone.Interface.Rule;
public partial class test2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void  btnSubmit_Click(object sender, EventArgs e)
    {
        string ErrMsg = "";
        //SetMail.InsertEmailSendHistory("10", "2", "3", "4", "5",0, "6", "7");
        //return;
        string EMailMessage = txtMessage.Text.Trim();
        string EMailAddress = txtAddress.Text.Trim();
        string SubjectName = txtSubject.Text.Trim();
        int Result = -1;

        string EmailModelPath = txtEmailModelPath.Text.Trim();
        //Result = SetMail.EmailSend(EMailMessage, EMailAddress, EmailModelPath, SubjectName, out ErrMsg);
        Result = SetMail.InsertEmailSendMassage("10", "2", EMailMessage, "11", 0, EMailAddress,
            DateTime.Now, "11", SubjectName, 0, out ErrMsg);
        Response.Write("Result-" + Result.ToString() + "ErrMsg-" + ErrMsg);
    }
}
