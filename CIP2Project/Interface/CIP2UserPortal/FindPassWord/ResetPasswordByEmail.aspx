<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPasswordByEmail.aspx.cs" Inherits="FindPassWord_ResetPasswordByEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>无标题文档</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css">

    <script language="javascript" type="text/javascript" src="JS/jquery-latest.pack.js"></script>

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
        <asp:Panel ID="ResetPanel" runat="server">
            <div class="resetBox">
                <label for="password">
                    <span>新密码：</span><input type="password" id="password" class="input"  onblur="doCheckPassword()"/><em>*</em><p
                        id="hintPassword"></p>
                </label>
                <label for="password2">
                    <span>确认密码：</span><input type="password" id="password2" class="input" onblur="doCheckSurePassword()" /><em>*</em><p
                        id="hintPassword2"></p>
                </label>
                <label for="checkCode">
                    <span>验证码：</span><input type="text" id="checkCode" class="input" maxlength="4" /><img
                        width="62" height="30" src="../ValidateToken.aspx"
                        id="IMG2"><a href="javascript:RefreshViewCode()">看不清？换一张</a><p id="hintCode">
                        </p>
                </label>
                <input type="button" value="完成" id="resetFinish" />
                <p id="successNote">
                    重置密码成功，3秒后跳转至登录页。</p>
            </div>
        </asp:Panel>
        <asp:Panel ID="MsgPanel" runat="server">
            <div>
                有错误
            </div>
        </asp:Panel>

        <script type="text/javascript">
            //刷新验证码
            RefreshViewCode = function(){
                document.all.IMG2.src = '../ValidateToken.aspx?yymm='+Math.random();
            }
            //密码验证
            doCheckPassword = function(){
                $("#hintPassword").text("请输入6-12位密码,不能含空格");
                $("#hintPassword").attr("class","");
                var pwd = $("#password").val();
                var regPwd = /^[0-9a-zA-Z_]{6,12}$/;
                if(pwd==""){
                    $("#hintPassword").text("请输入新密码");
                    $("#hintPassword").attr("class","hintError");
                    return false;
                }
                if(!regPwd.test(pwd)){
                    $("#hintPassword").text("请输入6-12位密码,不能含空格");
                    $("#hintPassword").attr("class","hintError");
                    return false;
                }else{
                    $("#hintPassword").text("输入正确");
                    $("#hintPassword").attr("class","hintCorrect");
                }
                
            }
            //确认密码验证
            doCheckSurePassword = function(){
                var pwd = $("#password").val();
                var surePwd = $("#password2").val();
                var regPwd = /^[0-9a-zA-Z_]{6,12}$/;
                if(surePwd==""){
                    $("#hintPassword2").text("请输入确认密码");
                    $("#hintPassword2").attr("class","hintError");
                    return false;
                }
                if(surePwd!=pwd){
                    $("#hintPassword2").text("输入有误");
                    $("#hintPassword2").attr("class","hintError");
                    return false;
                }else if(!regPwd.test(surePwd)){
                    $("#hintPassword2").text("请输入6-12位密码,不能含空格");
                    $("#hintPassword2").attr("class","hintError");
                    return false;
                }else{
                    $("#hintPassword2").text("输入正确");
                    $("#hintPassword2").attr("class","hintCorrect");
                }
            }
            //提交完成
            $("#resetFinish").click(function(){
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
                var pwd = $("#password").val();
                var custid = $("#hdCustID").val();
                var returnUrl = $("#hdReturnUrl").val();
                var email = $("#hdEmail").val();
                var authencode = $("#hdAuthenCode").val();
                var time = new Date();
                $.ajax({
                    type:"post",
                    url:"../HttpHandler/ResetPwdHandler.ashx",
                    dataType:"json",
                    data:{CustID:custid,Email:email,AuthenCode:authencode,PassWord:pwd,Code:viewCode,Type:"ResetPwdByEmail",PwdType:"2",time:time.getTime()},
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
                            if(item["result"]=="false"){
                                if(item["msg"]=="验证码错误"){
                                    $("#hintCode").text("验证码输入有误");
                                    $("#hintCode").attr("class","hintError");
                                }else{
                                    alert(item["msg"]);
                                }
                                RefreshViewCode();
                            }else{
                                window.location.href="Success.aspx?ReturnUrl="+encodeURI(returnUrl);
                            }
                        })
                    },
                    error:function(text){
                        //alert(text);
                    }
                })
            })
        </script>

        <asp:HiddenField ID="hdCustID" runat="server" />
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
        <asp:HiddenField ID="hdEmail" runat="server" />
        <asp:HiddenField ID="hdAuthenCode" runat="server" />
    </form>
</body>
</html>
