<%@ Page Language="C#" %>
<%@ OutputCache Duration="3600" VaryByControl="dropCategories" %>
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
    
        <asp:DropDownList 
             ID="dropCategories"
              DataSourceID ="srcCategories"
              DataTextField = "Name"
              DataValueField = "Id"
              runat = "server" />
        
        <asp:Button
            ID="btnSelect"
            Text = "Select"          
            runat = "server" />
              
        <br />    
         
        <asp:GridView
            ID = "grdMovies"
            DataSourceID = "srcMovies"
            GridLines = "none"
            runat = "server"
          />   
           
         <asp:SqlDataSource 
            ID = "srcCategories"
            ConnectionString = "<%$ ConnectionStrings:Movies %>"
            SelectCommand ="Select Id,Name from MovieCategories"
            runat ="server" />
           
          <asp:SqlDataSource
           ID = "srcMovies"
           ConnectionString =  "<%$ ConnectionStrings:Movies %>"
           SelectCommand = "Select Title,Director from Movies where CategoryId=@CategroyId" 
           runat ="server">
                <SelectParameters>
                    <asp:ControlParameter 
                         Name ="CategoryId"
                         ControlID = "dropCategories" />                  
                </SelectParameters>
           </asp:SqlDataSource> 
                 
          
            
    </div>
    </form>
</body>
</html>
