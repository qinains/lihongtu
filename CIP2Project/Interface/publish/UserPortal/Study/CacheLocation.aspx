<%@ Page Language="C#" %>
<%@ OutputCache Duration="3600" VaryByParam="none" Location="Client" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    void Page_Load()
    {
        Random rnd = new Random();
        lblRandom.Text = rnd.Next(10).ToString();
    } 
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label
            ID="lblRandom"
            runat = "server" />
                 
    </div>
   
    <a href ="CacheLocation.aspx">Request Page</a>
    
    </form>
</body>
</html>
