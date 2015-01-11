<%@ page language="C#" autoeventwireup="true" inherits="FindByPhone, App_Web_uglirgsr" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="~/CSS/account.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/Stylecss.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/msgStyle.css" type="text/css" rel="stylesheet" media="screen" />
    <%--<script language="javascript" type="text/javascript" src="../ModelJS/jquery.js"></script>--%>

    <script language="javascript" type="text/javascript" src="JS/jquery-latest.pack.js"></script>

    <script language="javascript" type="text/javascript" src="JS/FindPwdByPhone.js"></script>
    <style type="text/css">
        .tableA th,.tableA td{padding-left:7px;padding-right:7px;padding-top:5px;padding-bottom:2px}
        .msg .msg-inline .show{display:none}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="Top1" runat="server" />
        <div>
            <div class="btContain clfx">
                <div id="main">
                    <div class="ca">
                        <h3>
                            认证手机找回密码</h3>
                    </div>
                    <div class="cb">
                        <center>
                            请输入您的认证手机号，我们会将相应的验证码发到您的手机上！</center>
                        <table class="tableA">
                            <tr>
                                <th>
                                    <label for="email">
                                        认证手机号：</label>
                                </th>
                                <td style="width: 189px">
                                    <input type="text" value="" id="txtPhone" class="texti" />
                                </td>
                                <td style="width: 150px">
                                    <div class="msg msg-inline show" id="phoneError_tip">
                                        <div class="msg-default msg-error">
                                            <i class="msg-icon"></i>
                                            <div class="msg-content" id="phoneError_msg">
                                                不能为空
                                            </div>
                                        </div>
                                    </div>
                                    <%--<span id="nameMsgSpan" class="remark">用户名不能为空</span>--%>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="email">
                                        密码类型：</label>
                                </th>
                                <td style="width: 189px">
                                    <input type="radio" id="tPasswd" name="123" checked="checked" /><label for="tPasswd">登录密码</label>
                                    <input type="radio" id="tVoicePasswd" name="123" /><label for="tVoicePasswd">语音密码</label><input
                                        type="text" id="radiotxt" style="display: none;" />
                                </td>
                                <td style="width: 150px">
                                    <div class="msg msg-inline show" id="emailError_tip">
                                        <div class="msg-default msg-error">
                                            <i class="msg-icon"></i>
                                            <div class="msg-content" id="emailError_msg">
                                                不能为空
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label>
                                        验证码：</label></th>
                                <td style="width: 189px">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td style="text-align: left; padding: 0">
                                                <input type="text" id="txtCode" class="texti" style="width: 60px" /></td>
                                            <td>
                                                <span class="buttonSpan">
                                                    <button type="button" id="btnGetCode">
                                                        点击获取验证码</button>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--<script language="javascript" type="text/javascript">
				                            document.write("<input type=hidden name=pageyzm id=pageyzm value=",Math.random(),">")
				                            var tmp = document.getElementById("pageyzm").value;
				                            document.write("<img id='IMG2' src='../ValidateToken.aspx?yymm=",tmp,"' width='62' height='22'>");
				                            function RefreshCode()
				                            {
					                            document.all.IMG2.src = '../ValidateToken.aspx?yymm='+Math.random();
				                            }
                                    </script>
                                    <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span>--%>
                                </td>
                                <td style="width: 150px">
                                    <div class="msg msg-inline show" id="codeError_tip">
                                        <div class="msg-default msg-error">
                                            <i class="msg-icon"></i>
                                            <div class="msg-content" id="codeError_msg">
                                                不能为空
                                            </div>
                                        </div>
                                    </div>
                                    <%--<span id="codeMsgSpan" class="remark">验证码不能为空</span>--%>
                                </td>
                            </tr>
                        </table>
                        <div class="subtn" style="height:40px">
                            <span class="btn btnA"><span>
                            <img id="loadImg" src="Images/Loading.gif" style="position:absolute; display:none" />
                                <button type="button" id="btnSave">
                                    下一步</button>
                                <%--<input type="button" value="完 成" id="btnSave" />--%>
                            </span></span>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <uc2:Foot ID="Foot1" runat="server" />
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
        <asp:HiddenField ID="hdCustID" runat="server" />
    </form>
    <script type="text/javascript">
        var n = 0;
        var intervalID;
        function startInterval(){
            $("#btnGetCode").attr("disabled","false");
            $("#btnGetCode").text("120秒后重新发送");
            intervalID = window.setInterval("intervalFun()",1000);
        }
        function intervalFun(){
            
            if(n<=119){
                var nLeft = 119 - n;
                $("#btnGetCode").text(nLeft+"秒后重新发送");
                n++;
            }else{
                clearFun();
            }
            
        }
        
        function clearFun(){
            window.clearInterval(intervalID);
            n=0;
            $("#btnGetCode").removeAttr("disabled");
            //$("#btnGetCode").attr("disabled",true);
            $("#btnGetCode").text("重新获取验证码");
        }
        
        //获取手机验证码
        $("#btnGetCode").click(function(){
            
            var phone = $("#txtPhone").val();
            if(phone == ""){
                $("#phoneError_tip").css("display","block");
            }else{
                $("#phoneError_tip").css("display","none");
            }
            if(phone == ""){
                return false;
            }
            
            var time = new Date();
            $.ajax({
                type:"POST",
                url:"../HttpHandler/GetPhoneAuthenCodeHandler.ashx",
                dataType:"json",
                data:{PhoneNum:phone,time:time.getTime()},
                success:function(data,testStatus){
                    $.each(data,function(index,item){
                        if(item["result"]=="false"){
                            $("#hdCustID").val("");
                            alert(item["msg"]);
                        }else{
                            $("#hdCustID").val(item["custid"]);
                            startInterval();
                        }
                    })
                },
                error:function(data){
                    
                }
            })
        })
    </script>
</body>
</html>
