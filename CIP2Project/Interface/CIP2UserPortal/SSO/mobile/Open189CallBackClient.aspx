<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Open189CallBackClient.aspx.cs" Inherits="SSO_mobile_Open189CallBackWap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
 <script type="text/javascript" >
   
        function sendToPhone(action, requestData) {
	        try {
		        if (/(iPhone|iPad|iPod)/i.test(navigator.userAgent)){
			        sendToIos(action, requestData);
		        }else{
			        sendToAndroid(action, requestData)
		        }
	        } catch (e) {
		        //console.log(e.stack);
	        }
        }
        function sendToIos(action, requestData){
            window.location="action="+action+"&requestData="+encodeURI("{'custId':'<%=CustID %>','welcomeName':'<%=welcomeName %>','userName':'<%=UserName %>','realName':'<%=RealName %>','certificateCode':'<%=CertificateCode %>','sex':'<%=Sex %>','email':'<%=Email %> ','phone':'<%=Phone %>'}");   
        }
        function sendToAndroid(action, requestData){
	        window.android.execute(action, requestData);
        }  
  
         sendToPhone('yg_login_success',encodeURI("{'custId':'<%=CustID %>','welcomeName':'<%=welcomeName %>','userName':'<%=UserName %>','realName':'<%=RealName %>','certificateCode':'<%=CertificateCode %>','sex':'<%=Sex %>','email':'<%=Email %> ','phone':'<%=Phone %>'}"));   
  
    </script>  
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
