<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_SetPassword, App_Web_setpassword.aspx.8268bb4f" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../JS/jquery-latest.pack.js"></script>
    <style>
        .resetBox{width:600px;padding:20px 30px 20px 30px;background:#fff;color:#555}
        .resetBox a{color:#618ADE}
        .resetBox p{margin-bottom:5px}
        .resetBox h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1;margin-bottom:30px}
        .resetBox h2 span{color:#555;margin-right:5px}
        .resetBox form{margin-bottom:30px}
        .resetBox label{display:block;height:35px;margin-bottom:15px}
        .resetBox label span{display:block;float:left;margin-right:10px;width:80px;text-align:right;font-size:14px;line-height:30px}
        .resetBox label em{line-height:30px;color:red;margin-left:5px;margin-right:10px;float:left;display:inline}
        .resetBox label p{line-height:30px;float:left;margin-left:5px}
        .resetBox label p.hintCorrect{padding-left:20px;color:#390;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px 5px}
        .resetBox label p.hintError{padding-left:20px;color:#C30;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px -21px}
        .resetBox label .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
        input#checkCode{width:50px;margin-right:5px}
        #sendCode{float:left;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer}
        .resetBox label .checkbox{margin-left:90px;margin-right:10px}
        .resetBox label img{display:inline-block;vertical-align:middle;float:left}
        .resetBox label a{line-height:30px;margin-left:5px}
        #resetFinish{width:164px;height:38px;margin-left:90px;border:none;background:url(images/spriteLoginRegister.png) no-repeat -108px -360px;cursor:pointer;display:block;text-indent:-9999px}
        #successNote{margin-left:90px;margin-top:10px;padding-left:20px;line-height:30px;background:url(http://static.118114.cn/images/spriteLoginRegis.png) no-repeat -206px 5px;display:none}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="findPasswordBox">
        <label for="mobile">
            <span>旧密码：</span><input type="text" id="passwordOld" class="input" maxlength="11" onblur="doCheckPwdOld();" /><em>*</em><p
                id="hintPwdOld"></p>
        </label>
        <label for="mobile">
            <span>新密码：</span><input type="text" id="passwordNew" class="input" maxlength="11" onblur="doCheckPassword();" /><em>*</em><p
                id="hintPwdNew">
                请输入旧密码</p>
        </label>
        <label for="mobile">
            <span>确认新密码：</span><input type="text" id="passwordSure" class="input" maxlength="11" onblur="doCheckSurePassword();" /><em>*</em><p
                id="hintPwdSure">
                请输入旧密码</p>
        </label>
        <label for="checkCode">
            <span>验证码：</span><input type="text" id="checkCode" class="input" maxlength="4" />
            <img width="62" height="30" src="../ValidateToken.aspx"
                id="IMG2"><a href="javascript:RefreshViewCode()">看不清？换一张</a><p id="hintCode">
                </p>
        </label>
        <input type="button" value="下一步" id="getPassword" onclick="checkSubmit()"/><img id="loadImg" src="Images/Loading.gif" style="display:none" />
    </div>
    <asp:HiddenField ID="hdCustID" runat="server" />
    <asp:HiddenField ID="hdPasswordType" runat="server" />
    </form>
    <script type="text/javascript">
        //刷新验证码
        RefreshViewCode = function(){
            document.all.IMG2.src = '../ValidateToken.aspx?yymm='+Math.random();
        }
        doCheckPwdOld = function(){
            $("#hintPwdOld").attr("class","");
            var pwdOld = $("#passwordOld").val();
            if(pwdOld==""){
                $("#hintPwdOld").text("请输入旧密码");
                $("#hintPwdOld").attr("class","hintError");
                return false;
            }
        }
        //密码验证
        doCheckPassword = function(){
            $("#hintPwdNew").text("6-12位,不能含空格");
            $("#hintPwdNew").attr("class","");
            var pwd = $("#passwordNew").val();
            var regPwd = /^[0-9a-zA-Z_]{6,12}$/;
            if(pwd==""){
                $("#hintPwdNew").text("请输入新密码");
                $("#hintPwdNew").attr("class","hintError");
                return false;
            }
            if(!regPwd.test(pwd)){
                $("#hintPwdNew").text("6-12位,不能含空格");
                $("#hintPwdNew").attr("class","hintError");
                return false;
            }else{
                $("#hintPwdNew").text("输入正确");
                $("#hintPwdNew").attr("class","hintCorrect");
            }
        }
        //确认密码验证
        doCheckSurePassword = function(){
            var pwd = $("#passwordNew").val();
            var surePwd = $("#passwordSure").val();
            if(surePwd==""){
                $("#hintPwdSure").text("请输入确认密码");
                $("#hintPwdSure").attr("class","hintError");
                return false;
            }
            if(surePwd!=pwd){
                $("#hintPwdSure").text("密码输入有误");
                $("#hintPwdSure").attr("class","hintError");
                return false;
            }else{
                $("#hintPwdSure").text("输入正确");
                $("#hintPwdSure").attr("class","hintCorrect");
            }
        }
        checkSubmit = function(){
            if(doCheckPwdOld() == false){
                return false;
            }
            if(doCheckPassword() == false){
                return false;
            }
            if(doCheckSurePassword()== false){
                return false;
            }
            
            $("#hintCode").text("");
            $("#hintCode").attr("class","");
            var viewCode = $("#checkCode").val();
            if(viewCode==""){
                $("#hintCode").text("请输入验证码");
                $("#hintCode").attr("class","hintError");
                return false;
            }
            
            var pwdOld = $("#passwordOld").val();
            var pwdNew = $("#passwordNew").val();
            var custid = $("#hdCustID").val();
            var pwdType = $("#hdPasswordType").val();
            var optionType = "";
            if(pwdType == "1"){             //语音密码
                optionType = "ResetVoicePwd";
            }else if(pwdType == "3"){       //支付密码
                optionType = "ResetPayPwd";
            }else{                          //web密码
                optionType = "ResetWebPwd";
            }
            //var returnUrl = $("#hdReturnUrl").val();
            var time = new Date();
            $.ajax({
                type:"post",
                url:"../HttpHandler/ResetPwdHandler.ashx",
                dataType:"JSON",
                data:{CustID:custid,PasswordOld:pwdOld,PasswordNew:pwdNew,Code:viewCode,Type:optionType,time:time.getTime()},
                beforeSend: function(XMLHttpRequest) {
//                    $("#loadImg").css("display", "block");
//                    $("#btnSave").css("display","none");
                },
                complete: function(XMLHttpRequest, textStatus) {
//                    $("#loadImg").css("display", "none");
//                    $("#btnSave").css("display","block");
                },
                success:function(data,testStatus){
                    $.each(data,function(index,item){
                        var dataJson = eval("("+data+")");
                        $.each(dataJson,function(index,item){
                            if(item["result"]=="true"){
                                
                            }else if(item["result"]=="CodeError"){
                                $("#hintCode").text("验证码输入有误");
                                $("#hintCode").attr("class","hintError");
                            }else if(item["result"]=="OldPwdError"){
                                $("#hintPwdOld").text("旧密码输入有误");
                                $("#hintPwdOld").attr("class","hintError");
                            }else{
                                alert(item["msg"]);
                            }
                        })
                    })
                },
                error:function(text){
                    //alert(text);
                }
            })
        }
    </script>
    
</body>
</html>
