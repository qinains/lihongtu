<%@ Page Language="C#" %>
<%@ OutputCache Duration="3600" VaryByParam="none" VaryByCustom="css" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    void Page_Load()
    {
        if (Request.Browser.SupportsCss)
            pnlCss.Visible = true;
        else
            pnlNotCss.Visible = true;
        
    } 
    
      
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        
        <asp:Panel
           ID="pnlCss"
           Visible ="false"
           runat ="server">
            <span style ="font-weight:bold">Hello!</span>
          
           </asp:Panel>  
            
          <asp:Panel
                 ID="pnlNotCss"
                 Visible ="false"
                 runat ="server">
                <b>Hello!</b>       
          </asp:Panel>    
           
            
    </div>
    </form>
</body>
</html>
