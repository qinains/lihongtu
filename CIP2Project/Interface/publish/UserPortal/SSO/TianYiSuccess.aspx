<%@ page language="C#" autoeventwireup="true" inherits="SSO_TianYiSuccess, App_Web_tianyisuccess.aspx.27254924" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
   
    <script type="text/javascript">
    
  	        if(window.top != window) {  
		        window.top.location.href="QuickLogin1.aspx?LoginTicket=<%=ticket%>&ReturnUrl=<%=HttpUtility.UrlEncode(ReturnUrl) %>";  
	        }else{
		        window.location.href="QuickLogin1.aspx?LoginTicket=<%=ticket%>&ReturnUrl=<%=HttpUtility.UrlEncode(ReturnUrl) %>";
	        }   
          
    </script>
    
</head>
<body>

</body>
</html>
