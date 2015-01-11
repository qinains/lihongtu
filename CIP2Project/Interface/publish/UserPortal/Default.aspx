<%@ page language="C#" autoeventwireup="true" inherits="_Default, App_Web_default.aspx.cdcab7d2" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

</head>
<body>

  <form id="Form1" runat="server">
<asp:DropDownList id="DropDownList1" runat="Server">
<asp:ListItem Value="hidden" Text="hide" />
	<asp:ListItem Value="show" Text="show" />
	
</asp:DropDownList>
<asp:TextBox id="txt1" runat="server" />
</form>
<script language="C#" runat="server">
      void Page_Load(Object sender, EventArgs e)
      {
         if(!IsPostBack)
         {
            DropDownList1.Attributes.Add("onchange","f_show(this)");
         }
      }
       
</script>
<script language="javascript">
function f_show(obj)
{
    if (obj.value=="hidden")
    {
        document.all.txt1.style.display ="none";
    }
    else
    {
       document.all.txt1.style.display="";
    } 
}
</script>

</body>
</html>
