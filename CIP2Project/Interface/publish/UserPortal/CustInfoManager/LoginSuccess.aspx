<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_LoginSuccess, App_Web_loginsuccess.aspx.8268bb4f" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>注册成功</title>
   
    <link href="CSS/global.css" type="text/css" rel="stylesheet"/>
    <link href="CSS/reg.css" type="text/css" rel="stylesheet"/>  
   
     <link rel="stylesheet" href="../css/base.css" />
   
  <style type="text/css">
     .Yiyou_header,.content{width:980px;position:relative;margin:0 auto 10px} 
    .foot{width:980px;margin:10px auto;text-align:center;color:#78797a;line-height:23px}
    .foot a{color:#acadaf;text-decoration: none;}
    .foot a:hover,.foot a:visited{color:#acadaf;text-decoration: none;}
    
    </style> 
  
  
   
   <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script> 
    
   
</head>
<body>

    <asp:Panel ID="PanelForYiYouHeader" runat="server">
         <div class="Yiyou_header">
            <img src="../images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="../images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
        </div>
     </asp:Panel>

     <asp:Panel ID="PanelForYiGouHeader" runat="server">
         <div class="Yiyou_header">
            <img src="../images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="../images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
         </div>  
     </asp:Panel>
     
        <div style=" width:925px; margin:0 auto; border:1px solid #8fc4e0; padding:200px 0 0 0; background:url(images/finish.jpg) center 60px no-repeat; text-align:center; font-family:'宋体'; color:#000;">
          <div><a href="<%=ReturnUrl%>"><img src="images/bt_back.jpg" width="161" height="36" /></a></div>
         
          <div style=" background: #f8f8f8;">
            <div style="font-size:18px; line-height:120px;">为了您的账户安全，防止遗忘密码，建议您立即验证邮箱或手机！</div>
            <div style=" margin:0 0 50px 0;">
              <ul style=" padding:0 0 0 170px;">
                <li style=" float:left; padding:0 10px;"><a href="../verifyEmail.aspx?id=5&SPID=<%=SPID%>&SPTokenRequest=<%=newSPTokenRequest%>"><img src="images/bt_yzMail.jpg" width="139" height="38" /></a></li>
                <li style=" float:left; padding:0 10px;"><a href="../mobileInfo.aspx?id=4&SPID=<%=SPID%>&SPTokenRequest=<%=newSPTokenRequest%>"><img src="images/bt_yzMobile.jpg" width="139" height="38" /></a></li>
                <li style=" float:left; padding:0 10px;"><a href="../CreateBesttoneAccount.aspx?SPTokenRequest=<%=newSPTokenRequest%>"><img src="images/bt_acc.jpg" width="212" height="38" /></a></li>
              </ul>
            </div>
          </div>
        </div>
  <asp:Panel ID="PanelForYiYouFooter" runat="server">
        <script type="text/javascript" src="../JS/register_foot.js"></script>
    </asp:Panel>

    <asp:Panel ID="PanelForYiGouFooter" runat="server">
         <div class="foot">
            <p>中国电信集团：<a href="">189邮箱</a> | <a href="">天翼宽带</a> | <a href="">号百商旅</a> | <a href="">号百导航</a> | <a href="">天翼手机网</a> | <a href="">翼支付</a> | <a href="">爱音乐</a> | <a href="">天翼视讯</a> | <a href="">协同通讯</a> | <a href="">物联网</a> | <a href="">天翼空间</a> | <a href="">天翼阅读</a> | <a href="">爱游戏</a> | <a href="">爱动漫</a></p>
            违法和不良信息举报 <a href="">service@118114.cn</a>   copyright© 2007-2011 号百商旅电子商务有限公司版权所有<br />   
            增值电信业务经营许可证：沪B2-20110026　沪ICP备11017770号  上海工商标识编号：20110624111724289
        </div>  
    </asp:Panel>
</body>
</html>
