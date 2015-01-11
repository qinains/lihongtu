<%@ page language="C#" autoeventwireup="true" inherits="Certification_CertificationMobile, App_Web_certificationmobile.aspx.daf067bf" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
<script language="javascript" type="text/javascript" src="../JS/CertificationJs.js"></script>
    <title>设置认证手机</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table align="center">
    <tr>
    <td>请输入需要认证的手机：</td>
    <td><input type="text" id="txtMobile" onblur="MobileYanZhengShow();" />&nbsp;&nbsp;<input type="text" id="txtMobileYanZhen" style="display:none;width:63px;"  />
    </td>
    </tr>
    <tr>
    <td style="text-align:right;">验证码：</td>
    <td><input type="text" id="txtYanZhen" /></td>
    </tr>
    <tr>
    <td colspan="2" style="text-align:center;"><input type="button" value="确定" id="BtnOk" onclick="SubMobile();" /></td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
