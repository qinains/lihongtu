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

public partial class GenerateBtCer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string SPID = txtSPID.Text.Trim();
        string FilePath = txtFilePath.Text.Trim();
        string UserName = txtUserName.Text.Trim();
        string Password = txtPassword.Text.Trim();
        string ErrMsg = "";
        CerFileOP cfObj = new CerFileOP();
        byte[] btCer = cfObj.ReadFile( FilePath);
       
        int Result = cfObj.InsertCerFileByte(SPID, ddlCerType.SelectedValue, FilePath, UserName, Password, btCer, out ErrMsg);

        Response.Write(Result.ToString() + "," + ErrMsg);



    }
}
