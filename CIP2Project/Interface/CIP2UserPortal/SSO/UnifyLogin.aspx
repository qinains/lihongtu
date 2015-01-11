<%Response.AddHeader("P3P", "CP=CAO PSA OUR");%>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnifyLogin.aspx.cs" Inherits="SSO_UnifyLogin" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="css/global_unify.css" rel="stylesheet" type="text/css"/>
    <title>无标题页</title>
        <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
        <script language="javascript" type="text/javascript" src="../js/jquery.inputHint.js"></script> 
        <script language="javascript" type="text/javascript" src="../js/tab_yiyou.js"></script>
    
        <style type="text/css">
	            .loginTabShow,.loginTabHide{position:absolute;top:23;left:0;opacity:1;filter:alpha(opacity=100);z-index:2}
	            .loginTabHide{opacity:0;filter:alpha(opacity=0);z-index:1}
        </style>   
    
</head>
<body>
 <div style=" width:310px; background:#f2f2f2; border: 1px solid #dcdcdc;">
    <div class="tabs">
      <div class="tab_on" style="width:50%; float:left; line-height:40px; background:url(images/icon_tianyi.gif) 0 center no-repeat; text-indent:50px; font-size:14px;"><span onclick="tabconAlt(0,'E',8);" id="tabE0" >天翼帐号</span></div>
      <div class="tab_off" style="width:50%; float:left; line-height:40px; text-indent:50px; font-size:14px;"><span onclick="tabconAlt(1,'E',8);" id="tabE1" >普通帐号</span>
      </div>
    </div>
      <div style="padding:0 30px;"   id="conE0" class="loginTabShow"> 
        <iframe frameborder="0"  style="filter:chroma (color=#ffffff)"  height="310px" width="100%" 
          id="udbLoginFrame" src="<%=Login189Url %>"  scrolling="no">
        </iframe>
    </div>
   
                  <div id="conE1" class="loginTabHide">
                    <div id="errorHint" runat="server"></div>
                        <form id="form1" runat="server" onsubmit="return checkLogin();">
                            <label for="username"><span>登录名：</span><asp:TextBox ID="username" CssClass="input" runat="server"></asp:TextBox> </label>
                            <label for="password"><span>密码：</span><asp:TextBox ID="password" CssClass="input" runat="server" TextMode="Password"></asp:TextBox><a href="../FindPwdSelect.aspx?SPID=<%=SPID%>" target="_blank">忘记密码？</a></label>
                            <label for="checkCode"><span>验证码：</span><input type="text" value="" id="checkCode" name="checkCode" class="input" maxlength="4" /><img width="62" height="30" src="../ValidateToken.aspx" id="IMG2"><a href="javascript:RefreshCode()">看不清？换一张</a></label>
                          <%--  <label for="autoLogin"><input type="checkbox" class="checkbox" id="autoLogin" hidefocus="true" />下次自动登录</label>--%>
                            <asp:Button ID="login" runat="server"   /> 
                            <input type="hidden" value ="" id="AuthenType" name="AuthenType" />
                        </form>
                        <p>没有号百通行证？<asp:HyperLink ID="linkU1" runat="server">马上注册一个</asp:HyperLink></p>
                 </div>
  
    
  </div>
</body>
</html>
