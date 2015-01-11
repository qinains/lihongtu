<%@ Page Language="C#" %>
<%@ OutputCache Duration="3600" VaryByParam="id" %>
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
        <%= DateTime.Now.ToString("T") %>
        <hr />
        <asp:DetailsView
                  id="dtlMovie"
                   DataSourceID="srcMovies"
                   runat ="server" />
        
        
        <asp:SqlDataSource
                 ID="srcMovies"
                 ConnectionString ="<%$ ConnectionStrings:Movies %>"
                 SelectCommand ="select * from movies where id=@id"
                 runat="server">
                <SelectParameters>
                    <asp:QueryStringParameter Name="Id" Type="Int32" QueryStringField="Id" />
                </SelectParameters>        
                
         </asp:SqlDataSource>                 


    </div>
    </form>
</body>
</html>
