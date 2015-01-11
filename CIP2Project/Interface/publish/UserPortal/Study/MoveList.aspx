<%@ Page Language="C#" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>
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
               DataSourceID ="srcMovies"
               runat = "server" />
              
           <asp:SqlDataSource  
                ID ="srcMovies"
                 ConnectionString = "<%$ ConnectionStrings:Movies %>"   
                 SelectCommand = "select title,director from movies"
                 runat ="server" />
                
               
            <br />
           <a href ="AddMovie.aspx">Add Movie</a>   
             
                    
    </div>
    </form>
</body>
</html>
