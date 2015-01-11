<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LowSintRegisterMSuccess.aspx.cs" Inherits="CustInfoManager_M_LowSintRegisterMSuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>注册成功 </title>
    <link rel="stylesheet" type="text/css" href="css2014/global.css"/>  
    <link rel="stylesheet" type="text/css" href="css2014/lihongtu.css"/>
    
</head>
<body>
  <form id="form1" runat="server">
        <div class="top">
          <a href="#" class="back"></a>
          <p>注册成功</p>
        </div>
        <div class="wrap" style="text-align:center; font-size:14px; background:#fafafa;">
        <div style="margin:30px 0 0 0; font-size:20px; color:#ff6c00; line-height:25px;">恭喜您注册成功!</div>
        <div style="margin:15px 0 0 0; font-size:15px;">请妥善保存您的登录密码</div>
        <div style="margin:20px 0 0 0; font-size:15px;"><a href="http://m.114yg.cn" class="bt_bb">去首页逛逛</a></div>
        <div style=" margin:20px 0 0 0; padding:20px 0; border-top:1px solid #ececec; color:#808080;">为了您的账户安全，防止遗忘密码，建议您立即验证邮箱或手机！</div>
        <div style="margin:20px 0 0 0; font-size:15px;"><a href="AuthenEmailM.aspx?SPID=<%=SPID%>&SPTokenRequest=<%=newSPTokenRequest%>" class="bt_bh">通过邮箱验证</a></div>
        <div style="margin:20px 0 0 0; font-size:15px; padding:0 0 20px 0;"><a href="AuthenMobileM.aspx?SPID=<%=SPID%>&SPTokenRequest=<%=newSPTokenRequest%>" class="bt_bh">通过手机验证</a></div>
        <div style="padding:20px 0 0 0; font-size:15px; border-top:1px solid #ececec;"><a href="#" class="bt_bh">开通号码百事通账户</a></div>
        </div> 
                <asp:HiddenField ID="HiddenField_SPID" runat="server" />
                <asp:HiddenField ID="HiddenField_CUSTID" runat="server" />
                <asp:HiddenField ID="HiddenField_URL" runat="server" />        
        </form>
</body>
</html>
