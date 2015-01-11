<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePayPWD2.aspx.cs" Inherits="ChangePayPWD2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改密码</title>
    <link rel="stylesheet" type="text/css" href="http://static.118114.cn/css/base.css" />
    <style type="text/css">
    .content{width:920px;position:relative;margin:0 auto;padding:0 5px}
    .content h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1;margin-bottom:30px}
    .content h2 span{color:#555;margin-right:5px}
    .content form{margin-bottom:30px}
    .content label{display:block;height:35px;margin-bottom:15px}
    .content label span{display:block;float:left;margin-right:10px;width:120px;text-align:right;font-size:14px;line-height:30px}
    .content label em{line-height:30px;color:red;margin-left:5px;margin-right:10px;float:left;display:inline}
    .content label p{line-height:30px;float:left}
    .content label p.hintCorrect{padding-left:20px;color:#390;background:url(images/bg_sprite.png) no-repeat -206px 5px}
    .content label p.hintError{padding-left:20px;color:#C30;background:url(images/bg_sprite.png) no-repeat -206px -21px}
    .content label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
    input#checkCode{width:50px;margin-right:5px}
    #sendCode{float:left;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer}
    .submit{width:108px;height:38px;margin-left:130px;border:none;background:url(images/bg_sprite.png) no-repeat 0 -76px;cursor:pointer;display:block;text-indent:-9999px}
     #Modify{width:108px;height:38px;margin-left:130px;border:none;background:url(images/bg_sprite.png) no-repeat 0 -76px;cursor:pointer;display:block;text-indent:-9999px}
    .foot{width:980px;margin:10px auto;text-align:center;color:#78797a;line-height:23px}
    .foot a{color:#acadaf}
    .head{width:980px;margin:0 auto;clear:both;font-family:Microsoft Yahei,simhei;color:#007bc1;font-size:24px}
    .head p{margin-top:12px;margin-left:20px;padding-left:20px;padding-top:8px;border-left:1px solid #c2c2c2;height:30px}
    .tabA{width:980px;margin:0 auto;clear:both;border-bottom:1px solid #8fc4e0}
    .tabA li{float:left;margin-top:16px;padding-left:10px;clear:none;width:162px;height:32px;line-height:32px;background:#f6f6f6;font-weight:700;font-size:14px;color:#007bc1; cursor:pointer;text-align:center;margin-left:20px;border-radius:10px 10px 0 0;}
    .tabA li.vazn{border:1px solid #8fc4e0;border-width:1px 1px 0 1px;background:#fff}
    html body{background:#fff} 
    </style>

    <script language="javascript" type="text/javascript" src="ModelJS/jquery-1.3.1.js"></script>

    <script type="text/javascript">
           $(document).ready(function(){
                    $("#confirmPassWord").blur(function(){
                        if($("#newPassWord").attr("value") != $("#confirmPassWord").attr("value")){
                            $("#newPassWord").attr("value","");
                            $("#newPassWord").attr("value","");
                            $("#hintPassword2").html("密码不一致");
                            $("#hintPassword2").attr("class","hintError");
                        }else{
                            $("#hintPassword2").html("");
                        }
                    });   
                   
                  var tel_reg = /^\d{6}$/; 
                  $("#newPassWord").blur(function(){
                        if(!tel_reg.test($("#newPassWord").attr("value")))
                        {
                            $("#hintPassword1").html("请输入正确的密码(6位数字)");
                            $("#hintPassword1").attr("class","hintError");
                        }else{
                                $("#hintPassword1").html("输入正确");
                                $("#hintPassword1").attr("class","hintCorrect"); 
                        }
                  });
                  
 
  
        }); 
    </script>

</head>
<body>
 
 <asp:Panel ID="header" runat="server">

<div class="head clearfix"><div class="fl"><img src="images/logoct.gif" alt="" /><img style="padding-left:20px" src="images/logo_besttone.gif"  alt=""/></div><p class="fl">号码百事通账户</p></div>
</asp:Panel>



<% 
    if (success.Equals("0"))
    {
%>
     <div style="width:400px;height:150px;position:absolute;left:50%;margin-left:-205px;border:5px solid #39c;border-radius:5px;background: #fff;
">
	    <h3 style="padding-left:5px;height:30px;color:#fff;font:14px/30px Microsoft Yahei;background:#2d94c8">支付密码修改</h3>
	    <p style="font:14px Microsoft Yahei;margin:30px auto 10px;text-align:center">支付密码修改成功!</p>
        <a href="<%=ReturnUrl%>"  target="_top" style="display:block;margin:0 auto;width:124px"><img src="images/finishReturn.jpg" width="106" height="26" alt=""></a>
    </div>

<%
    } 
    else
    {
%>


<% 
    if (IsBesttoneAccountBindV5Result != 0)
    {
        Response.Write("IsBesttoneAccountBindV5Result=" + IsBesttoneAccountBindV5Result); 
%>

        <div style="width:390px;padding:30px 10px 30px 140px;margin:50px auto;background:url(images/bg_noAccount.png) no-repeat 0 50%">
	        <h3 style="margin:10px auto;font:bold 18px Microsoft Yahei;color:#039">抱歉！<span>您的号码百事通账户尚未开通。</span></h3>
            <a href="CreateBesttoneAccount.aspx?SPTokenRequest=<%=SPTokenRequest%>" style="color:#39c;font:bold 14px Microsoft Yahei" target="_blank">马上开通>></a>
        </div>
<%
    } 
    else
    {
%>

        <div class="content">
            <h2>修改支付密码</h2>
            <form id="form1"  runat="Server">
                <label for="password"><span>旧支付密码：</span><input type="password" id="oldPassWord" name="oldPassWord" class="input"/><em>*</em><p id="hintPassword">请填写您的旧支付密码</p></label>
                <label for="password"><span>设置新支付密码：</span><input type="password" id="newPassWord" name="newPassWord" class="input"/><em>*</em><p id="hintPassword1">只能由数字组成，且不能含空格</p></label>
                <label for="password2"><span>确认新支付密码：</span><input type="password" id="confirmPassWord" name="confirmPassWord" class="input"/><em>*</em><p id="hintPassword2">请重复输入密码</p></label>
            
                
                <asp:Button ID="Modify" runat="server" Text="确认" OnClick="Modify_Click" />
                
            </form>
        </div>
<%
    }
%>


<%
    
    }
    
%>



<asp:panel ID="footer" runat="server" > 
<div class="foot">
<p>中国电信集团：<a href="">189邮箱</a> | <a href="">天翼宽带</a> | <a href="">号百商旅</a> | <a href="">号百导航</a> | <a href="">天翼手机网</a> | <a href="">翼支付</a> | <a href="">爱音乐</a> | <a href="">天翼视讯</a> | <a href="">协同通讯</a> | <a href="">物联网</a> | <a href="">天翼空间</a> | <a href="">天翼阅读</a> | <a href="">爱游戏</a> | <a href="">爱动漫</a></p>
违法和不良信息举报 <a href="">service@118114.cn</a>   copyright© 2007-2011 号百商旅电子商务有限公司版权所有<br />   
增值电信业务经营许可证：沪B2-20110026　沪ICP备11017770号  上海工商标识编号：20110624111724289
</div>

</asp:panel>
</body>
</html>
