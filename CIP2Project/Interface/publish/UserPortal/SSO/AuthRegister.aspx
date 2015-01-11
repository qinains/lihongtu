<%@ page language="C#" autoeventwireup="true" inherits="SSO_AuthRegister, App_Web_authregister.aspx.27254924" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    <style type="text/css">
        .authBox{width:600px;padding:20px 30px 20px 20px;background:#fff;color:#555}
        .authBox a{color:#618ADE}
        .authBox p{margin-bottom:5px}
        .authBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1;margin-bottom:30px}
        .authBox h2 span{color:#555;margin-right:5px}
        .authBox form{margin-bottom:30px}
        .authBox label{display:block;height:35px;margin-bottom:15px}
        .authBox label span{display:block;float:left;margin-right:10px;width:80px;text-align:right;font-size:14px;line-height:30px}
        .authBox label em{line-height:30px;color:red;margin-left:5px;margin-right:10px;float:left;display:inline}
        .authBox label p{line-height:30px;float:left;margin-left:5px}
        .authBox label p.hintCorrect{padding-left:20px;color:#390;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px 5px}
        .authBox label p.hintError{padding-left:20px;color:#C30;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px -21px}
        .authBox label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
        input#checkCode{width:50px;margin-right:5px}
        #sendCode{float:left;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer}
        .authBox label .checkbox{margin-left:90px;margin-right:10px}
        .authBox label a{line-height:30px;margin-left:5px}
        #auth{width:164px;height:38px;margin-left:90px;border:none;background:url(images/spriteLoginRegister.png) no-repeat 0 -360px;cursor:pointer;display:block;text-indent:-9999px}
        #successNote{margin-left:90px;margin-top:10px;padding-left:20px;line-height:30px;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px 5px;display:none}
    </style>
</head>
<body>
<div class="authBox">
	<h2><span>新用户绑定</span>号百通行证</h2>
	<span id="errorHint"  runat="server"></span>
    <form id="form1" runat="Server">
        <label for="mobile"><span>手机号：</span><asp:TextBox  id="mobile" CssClass="input" runat="server"></asp:TextBox><em>*</em><p id="hintMobile">请输入11位手机号码</p></label>
        <label for="checkCode"><span>验证码：</span><asp:TextBox id="checkCode" CssClass="input" runat="server"></asp:TextBox><span id="sendCode">发送验证码</span><em>*</em><p id="hintCode">请输入收到的验证码</p></label>
        <label for="password"><span>设置密码：</span><asp:TextBox ID="password" CssClass="input" runat="server" TextMode="Password"></asp:TextBox><em>*</em><p id="hintPassword">6-12位,不能含空格</p></label>
        <label for="password2"><span>确认密码：</span><asp:TextBox ID="password2" CssClass="input" runat="server" TextMode="Password"></asp:TextBox><em>*</em><p id="hintPassword2">请重复输入密码</p></label>
        <asp:Button ID="register" runat="server" Text="注册" OnClick="register_Click" />
        <p id="successNote">账号绑定成功，您还可以通过绑定的手机号进行登录。3秒后跳转至首页。</p>
    </form>
</div>
</body>
</html>
