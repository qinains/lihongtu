<%@ page language="C#" autoeventwireup="true" inherits="Registration, App_Web_kpte7rfs" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    
    <script language="javascript" type="text/javascript" src="ModelJS/jquery-1.3.1.js"></script>   
    <script language="javascript" type="text/javascript" src="JS/UserRegistry/Registration.js"></script>
    <script language="javascript" type="text/javascript" src="JS/UserRegistry/jquery.inputHint.js"></script>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
        <style type="text/css">
        .registerBox{width:600px;padding:20px 30px 20px 20px;background:#fff;color:#555}
        .registerBox a{color:#618ADE}
        .registerBox p{margin-bottom:5px}
        .registerBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1;margin-bottom:30px}
        .registerBox h2 span{color:#555;margin-right:5px}
        .registerBox form{margin-bottom:30px}
        .registerBox label{display:block;height:35px;margin-bottom:15px}
        .registerBox label span{display:block;float:left;margin-right:10px;width:80px;text-align:right;font-size:14px;line-height:30px}
        .registerBox label em{line-height:30px;color:red;margin-left:5px;margin-right:10px;float:left;display:inline}
        .registerBox label p{line-height:30px;float:left}
        .registerBox label p.hintCorrect{padding-left:20px;color:#390;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px 5px}
        .registerBox label p.hintError{padding-left:20px;color:#C30;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px -21px}
        .registerBox label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
        input#checkCode{width:50px;margin-right:5px}
        #sendCode{float:left;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer;margin-right:0}
        .registerBox label .checkbox{margin-left:90px;margin-right:10px}
        .registerBox label a{line-height:30px;margin-left:5px}
        #register{width:108px;height:38px;margin-left:90px;border:none;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat 0 -38px;cursor:pointer;display:block;text-indent:-9999px}
        </style>     
</head>
<body>
   <form id="form1" runat="server">
        <div class="registerBox">
	        <h2><span>欢迎成为</span>号码百事通会员!</h2>
          
    	        <label for="mobile"><span>手机号：</span><asp:TextBox ID="mobile" CssClass="input" runat="server" maxlength="11"   /><input id="phonestate" type="hidden" name="phonestate" value="" /><em>*</em><p id="hintMobile" runat="Server">请输入11位手机号码</p></label>
                <label for="checkCode"><span>验证码：</span><asp:TextBox ID="checkCode" CssClass="input"  runat="server" /><span id="sendCode" onclick="SendPhoneAuth()">发送验证码</span><em>*</em><p id="hintCode" runat="Server">请输入收到的验证码</p></label>
                <label for="password"><span>设置密码：</span><asp:TextBox ID="password" CssClass="input" runat="server" TextMode="Password" /><em>*</em><p id="hintPassword" runat="Server">6-12位，可以由英文、数字或符号组成，不能含空格</p></label>
                <label for="password2"><span>确认密码：</span><asp:TextBox ID="password2" CssClass="input" runat="server" TextMode="Password" /><em>*</em><p id="hintPassword2" runat="Server">请重复输入密码</p></label>
                <label for="acceptDeal"><input type="checkbox" class="checkbox" id="acceptDeal" hidefocus="true" />我接受<a href="javascript:void(0)">《号百通行证用户服务协议》</a>
                </label>
                <label for="registBesttoneAccount"><input type="checkbox" class="checkbox" id="registBesttoneAccount" hidefocus="true" />开通号码百事通账户</label><br/>
                <asp:Button ID="register" runat="server"  OnClientClick="return CheckInput()&&IsPhone();" OnClick="register_Click"/>
                <input type="hidden" id="hid_openAccount" name="hid_openAccount" value="0" />
                <asp:HiddenField ID="HiddenField_SPID" runat="server" />
            
                <asp:HiddenField ID="HiddenField_URL" runat="server" />
            
        </div>
        
    </form>
    
   
</body>
</html>
