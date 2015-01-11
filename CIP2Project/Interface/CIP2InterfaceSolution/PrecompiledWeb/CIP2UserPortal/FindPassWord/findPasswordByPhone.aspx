<%@ page language="C#" autoeventwireup="true" inherits="FindPassWord_findPasswordByPhone, App_Web_uglirgsr" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>无标题文档</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css">

    <script type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-latest.pack.js"></script>

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
        #getPassword
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
            <label for="mobile">
                <span>手机号：</span><input type="text" id="mobile" class="input" maxlength="11" onblur="doCheckPhone();" /><em>*</em><p
                    id="hintMobile">
                    请输入11位手机号码</p>
            </label>
            <label for="checkCode">
                <span>验证码：</span><input type="text" id="checkCode" class="input" onblur="doCheckCode();" />
                <div id="sendCode" onclick="sendPhoneCode()">
                    发送验证码</div>
                <em>*</em><p id="hintCode">
                    请输入收到的验证码</p>
            </label>
            <input type="button" value="下一步" id="getPassword" onclick="checkSubmit()" /><img
                id="loadImg" src="Images/Loading.gif" style="display: none" />
            <p id="successNote">
                密码已发送到您的手机，请在成功<a href="#">登录</a>后前往<a href="#">用户中心</a>修改您的密码</p>
        </div>
        <asp:HiddenField ID="hdCustID" runat="server" />
        <asp:HiddenField ID="hdReturnUrl" runat="server" />

        <script type="text/javascript">
            var n = 0;
            var intervalID;
            function startInterval(){
                $("#sendCode").attr("disabled","false");
                $("#sendCode").text("120秒后重新发送");
                intervalID = window.setInterval("intervalFun()",1000);
            }
            function intervalFun(){
                
                if(n<=119){
                    var nLeft = 119 - n;
                    $("#sendCode").text(nLeft+"秒后重新发送");
                    n++;
                }else{
                    clearFun();
                }
            }
            
            function clearFun(){
                window.clearInterval(intervalID);
                n=0;
                $("#sendCode").removeAttr("disabled");
                $("#sendCode").text("重新获取验证码");
            }
            doCheckPhone = function(){
                $("#hintMobile").text("");		
			    $("#hintMobile").attr("class","");
                var phoneNum = $("#mobile").val();
                if(phoneNum==""){
                    $("#hintMobile").text("手机号不能为空，请输入");		
			        $("#hintMobile").attr("class","hintError");
			        return false;
                }
                var regMobile=/^1([3][0-9]|[5][012356789]|[8][056789])\d{8}$/;		//手机号验证
                if(!regMobile.test(phoneNum)){	//手机号格式不正确
			        $("#hintMobile").text("输入的手机格式有误，请重新输入");		
			        $("#hintMobile").attr("class","hintError");
			        return false;
		        }else{
		            //$("#hintMobile").text("输入正确");
			        //$("#hintMobile").attr("class","hintCorrect");
		        }
            }
            doCheckCode = function(){
                $("#hintCode").text("");		
			    $("#hintCode").attr("class","");
                var code = $("#checkCode").val();
                if(code == ""){
                    $("#hintCode").text("请输入验证码");		
			        $("#hintCode").attr("class","hintError");
			        return false;
                }
            }
            
            /*************************************发送手机验证码*******************************************/
            sendPhoneCode = function(){
                if(doCheckPhone() == false){
                    return false;
                }else{
			        $("#hdCustID").val("");
			        var phoneNum = $("#mobile").val();
			        var time = new Date();
			        startInterval();
			        $.ajax({
			            type:"post",
			            url:"../HttpHandler/GetPhoneAuthenCodeHandler.ashx",
			            dataType:"json",
			            data:{PhoneNum:phoneNum,time:time.getTime()},
			            success:function(data,textStatus){
			                $.each(data,function(index,item){
                                if(item["result"]=="false"){
                                    $("#hintMobile").text("输入的认证手机不存在，请重新输入");		
			                        $("#hintMobile").attr("class","hintError");
			                        clearFun();
                                }else{
							        //$("#hintMobile").text("验证码已发送手机");		
			                        //$("#hintMobile").attr("class","hintCorrect");
                                    $("#hdCustID").val(item["custid"]);
                                    //alert(item["authencode"]);
                                }
                            })
			            },
			            error:function(){
    			            clearFun();
			            }
			        })
                }
            }
            
            /*************************************提交*********************************************/
            checkSubmit = function(){
                if(doCheckPhone() == false){
                    return false;
                }
                if(doCheckCode() == false){
                    return false;
                }
                var phoneNum = $("#mobile").val();
                var code = $("#checkCode").val();
                var custid = $("#hdCustID").val();
                var returnUrl = $("#hdReturnUrl").val();
                var time = new Date();
                $("#hintCode").text("");		
		        $("#hintCode").attr("class","");
    		    
		        var urlParam = encodeURI(custid+'$'+returnUrl);
                $.ajax({
                    type:"post",
                    url:"../HttpHandler/CheckPhoneCodeHandler.ashx",
                    dataType:"json",
                    data:{CustID:custid,PhoneNum:phoneNum,Code:code,time:time.getTime()},
                    beforeSend: function(XMLHttpRequest) {
                        //$("#loadImg").css("display", "block");
                        $("#getPassword").attr("disabled","false");
                    },
                    complete: function(XMLHttpRequest, textStatus) {
                        //$("#loadImg").css("display", "none");
                        $("#getPassword").removeAttr("disabled","false");
                    },
                    success:function(data,textStatus){
                        $.each(data,function(index,item){
                            if(item["result"]=="false"){
                                $("#hintCode").text("验证码输入有误");		
		                        $("#hintCode").attr("class","hintError");
                            }else{
                                window.location.href='ResetPasswordByPhone.aspx?urlParam='+urlParam;
                            }
                        })
                    },
                    error:function(){
                        
                    }
                })
            }
        </script>

    </form>
</body>
</html>
