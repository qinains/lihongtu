<%@ page language="C#" autoeventwireup="true" inherits="ChangePayPWD, App_Web_changepaypwd.aspx.cdcab7d2" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/CommonHead.ascx" TagName="CommonHead" TagPrefix="uc1" %>
<%@ Register Src="UserCtrl/CommonFoot.ascx" TagName="CommonFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                  
 
                    
                    //设置头和尾是否显示
                    var isNeed = $("#hdHeadFoot").val();
                    if(isNeed=="0"){
                        $("#divHead,#divFoot").css("display","none");
                    }else{
                        $("#divHead,#divFoot").css("display","block");
                    }        
        }); 
    </script>

</head>
<body>
    <uc1:CommonHead ID="CommonHead1" runat="server"></uc1:CommonHead>
    
    
    <% 
        if (success.Equals("0"))
        {
    %>
             <div style="width:400px;height:150px;position:absolute;left:50%;margin-left:-205px;border:5px solid #39c;border-radius:5px">
	            <h3 style="padding-left:5px;height:30px;color:#fff;font:14px/30px Microsoft Yahei;background:#2d94c8">修改密码</h3>
	            <p style="font:14px Microsoft Yahei;margin:30px auto 10px;text-align:center">修改密码成功。</p>
                <a href="<%=_ReturnUrl %>" target="_top" style="display:block;margin:0 auto;width:124px"><img src="images/finishReturn.gif" width="106" height="26" alt=""></a>
            </div>
    <%
        } 
        else
        {
    %>
  
             <div class="content">
                <h2>
                    修改支付密码</h2>
                <form id="form1" runat="Server">
                    <label for="password">
                        <span>旧支付密码：</span><input type="password" id="oldPassWord" name="oldPassWord" class="input" /><em>*</em><p
                            id="hintPassword">
                            请填写您的旧支付密码</p>
                    </label>
                    <label for="password">
                        <span>设置新支付密码：</span><input type="password" id="newPassWord" name="newPassWord" class="input" /><em>*</em><p
                            id="hintPassword1">
                            只能由数字组成，且不能含空格</p>
                    </label>
                    <label for="password2">
                        <span>确认新支付密码：</span><input type="password" id="confirmPassWord" name="confirmPassWord" class="input" /><em>*</em><p
                            id="hintPassword2">
                            请重复输入密码</p>
                    </label>
                    <asp:Button ID="Modify" runat="server" Text="确认" OnClick="Modify_Click" />
                    <asp:HiddenField ID="hdHeadFoot" runat="server" />
                </form>
            </div>
  
                
    <%
        }
            
    %>
    
 
    
    
    
    
    <uc2:CommonFoot ID="CommonFoot1" runat="server"></uc2:CommonFoot>
</body>
</html>
