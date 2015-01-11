<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_SetAuthenEmail, App_Web_setauthenemail.aspx.8268bb4f" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css">
    <script type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/jquery-latest.pack.js"></script>
    <style>
        .findPasswordBox
        {
            width: 600px;
            padding: 20px 30px 20px 20px;
            background: #fff;
            color: #555;
        }
        .findPasswordBox a
        {
            color: #618ADE;
        }
        .findPasswordBox p
        {
            margin-bottom: 5px;
        }
        .findPasswordBox h2
        {
            font: 18px/36px Microsoft Yahei,simhei;
            color: #007bc1;
            margin-bottom: 30px;
        }
        .findPasswordBox h2 span
        {
            color: #555;
            margin-right: 5px;
        }
        .findPasswordBox form
        {
            margin-bottom: 30px;
        }
        .findPasswordBox label
        {
            display: block;
            height: 35px;
            margin-bottom: 15px;
        }
        .findPasswordBox label span
        {
            display: block;
            float: left;
            margin-right: 10px;
            width: 80px;
            text-align: right;
            font-size: 14px;
            line-height: 30px;
        }
        .findPasswordBox label em
        {
            line-height: 30px;
            color: red;
            margin-left: 5px;
            margin-right: 10px;
            float: left;
            display: inline;
        }
        .findPasswordBox label p
        {
            line-height: 30px;
            float: left;
        }
        .findPasswordBox label p.hintCorrect
        {
            padding-left: 20px;
            color: #390;
            background: url(images/spriteLoginRegister.png) no-repeat -206px 5px;
        }
        .findPasswordBox label p.hintError
        {
            padding-left: 20px;
            color: #C30;
            background: url(images/spriteLoginRegister.png) no-repeat -206px -21px;
        }
        .findPasswordBox label .input
        {
            display: block;
            float: left;
            width: 180px;
            height: 28px;
            padding: 0 5px;
            border: 1px solid #ceceb1;
            line-height: 30px;
            font-size: 14px;
        }
        input#checkCode
        {
            width: 50px;
            margin-right: 5px;
        }
        #sendCode
        {
            float: left;
            width: 123px;
            height: 28px;
            line-height: 28px;
            font-size: 14px;
            text-align: center;
            color: #2a609c;
            border: 1px solid #95c7e2;
            background: #d3effe;
            cursor: pointer;
        }
        .findPasswordBox label .checkbox
        {
            margin-left: 90px;
            margin-right: 10px;
        }
        .findPasswordBox label a
        {
            line-height: 30px;
            margin-left: 5px;
        }
        #btnSave
        {
            width: 164px;
            height: 38px;
            margin-left: 90px;
            border: none;
            background: url(images/spriteLoginRegister.png) no-repeat 0 -76px;
            cursor: pointer;
            display: block;
            text-indent: -9999px;
        }
        #successNote
        {
            margin-left: 90px;
            margin-top: 10px;
            padding-left: 20px;
            line-height: 30px;
            background: url(images/spriteLoginRegister.png) no-repeat -206px 5px;
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="findPasswordBox">
            <label for="Email">
                <span>邮箱：</span><input type="text" id="txtEmail" class="input" onblur="doCheckEmail()" /><em>*</em><p
                    id="hintEmail">
                    请输入邮箱，如 user@example.com</p>
            </label>
            <label for="checkCode">
                <span>验证码：</span><input type="text" id="checkCode" class="input" maxlength="4" onblur="doCheckEmail()" /><img
                    width="62" height="30" src="../ValidateToken.aspx"
                    id="IMG2"><a href="javascript:RefreshViewCode()">看不清？换一张</a><p id="hintCode">
                    </p>
            </label>
            <input type="button" value="下一步" id="btnSave" onclick="submitFun()" />
            <p id="successNote">
                密码已发送到您的邮箱，请<a href="javascript:void(0)" id="jumpLink" target="_blank">登录邮箱</a>查收</p>
        </div>

        <script type="text/javascript">
            //刷新验证码
            RefreshViewCode = function(){
                document.all.IMG2.src = '../ValidateToken.aspx?yymm='+Math.random();
            }
            //邮箱验证
            doCheckEmail = function(){
                var email = $("#txtEmail").val();
                if(email == ""){
                    $("#hintEmail").text("邮箱不能为空，请输入");
		            $("#hintEmail").attr("class","hintError");
		            return false;
                }
                var regEmail=/^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$/;;		//邮箱验证
	            if(!regEmail.test(email)){	//邮箱号格式不正确
		            $("#hintEmail").text("您输入的邮箱不正确，请重新输入");		
		            $("#hintEmail").attr("class","hintError");
		            return false;
	            }else{
		            $("#hintEmail").text("邮箱验证通过");	
		            $("#hintEmail").attr("class","hintCorrect");
		            $("jumpLink").attr("href","http://mail."+email.substring(email.indexOf("@")+1));
	            }
            }
            
            submitFun = function(){
                if(doCheckEmail()==false){
                    return false;
                }
                
                var checkCode = $("#checkCode").val();
                if(checkCode==""){
                    $("#hintCode").text("请输入验证码");
		            $("#hintCode").attr("class","hintError");
		            return false;
                }else{
                    $("#hintCode").text("");
		            $("#hintCode").attr("class","hintCorrect");
                }
                
                var email = $("#txtEmail").val();
                var custid = $("#hdCustID").val();
                var returnUrl = $("#hdReturnUrl").val();
                var time = new Date();
                $.ajax({
                    type:"post",
                    url:"../HttpHandler/FindPwdByEmailHandler.ashx",
                    dataType:"json",
                    data:{CustID:custid,Email:email,Code:checkCode,Type:"AuthenEmail",ReturnUrl:returnUrl,time:time.getTime()},
                    beforeSend: function(XMLHttpRequest) {
                        
                    },
                    complete: function(XMLHttpRequest, textStatus) {
                        
                    },
                    success:function(data,textStatus){
                        $.each(data,function(index,item){
                            if(item["result"]=="true"){
                                window.location.href='SendMailSuccess.aspx?ReturnUrl='+encodeURI(returnUrl);
                            }else if(item["result"]=="CodeError"){
                                $("#hintCode").text("验证码错误");
                                $("#hintCode").attr("class","hintError");
                            }else if(item["result"]=="IsExist"){
                                $("#hintEmail").text("认证邮箱已被注册认证，请重新输入");
                                $("#hintEmail").attr("class","hintError");
                            }else if(item["result"]=="IsYours"){
                                $("#hintEmail").text("认证邮箱已是您的认证邮箱");
                                $("#hintEmail").attr("class","hintError");
                            }else{
                                alert(item["msg"]);
                            }
                            RefreshViewCode();
                        })
                    },
                    error:function(){
                        
                    }
                })
            }
        </script>
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
        <asp:HiddenField ID="hdCustID" runat="server" />
    </form>
</body>
</html>
