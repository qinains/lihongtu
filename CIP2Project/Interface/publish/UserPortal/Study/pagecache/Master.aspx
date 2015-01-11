<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:GridView 
                     ID="grdMovies"
                      DataSourceID="srcMovies"
                      AutoGenerateColumns="false" 
                      ShowHeader="false"
                      GridLines ="None"
                      runat="server">
                     <Columns>
                        <asp:HyperLinkField
                         DataTextField ="Title"
                         DataNavigateUrlFields="Id"
                         DataNavigateUrlFormatString="~/Detail.aspx?id={0}"  />
                     </Columns> 
            </asp:GridView>
           
          <asp:SqlDataSource
                     ID="srcMovies"
                      ConnectionString ="<%$ ConnectionStrings:Movies %>"
                      SelectCommand ="select Id,Title From Movies"
                      runat ="server"/> 
                       
                        
                           
    </div>
    </form>
</body>
</html>
