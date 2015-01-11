<%@ page language="C#" autoeventwireup="true" inherits="verifyMobile, App_Web_verifymobile.aspx.cdcab7d2" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>号码百事通客户信息平台</title>
    <meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号码百事通,114,118114" />
    <meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." />
    <link href="css/account.css" type="text/css" rel="stylesheet" media="screen" />

    <script language="javascript" type="text/javascript" src="ModelJS/jquery.js"></script>

    <script language="jscript" type="text/javascript" src="JS/setMobile/findPwd_JS.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:Top ID="Top1" runat="server" />
            <div class="btContain clfx">
                <div id="main">
                    <div class="ca">
                        <h3>
                            通过认证手机找回密码</h3>
                    </div>
                    <div class="cb">
                        <form>
                            <table class="tableA">
                                <tr>
                                    <th>
                                        <label for="verifyMobile">
                                            认证手机号码：</label></th>
                                    <td>
                                        <input type="text" value="" id="verifyMobile" class="texti" onkeyup="clearmsg();"
                                            onchange="clearmsg();" /></td>
                                </tr>
                                <tr>
                                    <th>
                                        <label>
                                            密码类型：</label></th>
                                    <td>
                                        <input type="radio" id="tPasswd" onclick="radio(2);" /><label for="tPasswd">登录密码</label>
                                        <input type="radio" id="tVoicePasswd" onclick="radio(1);" /><label for="tVoicePasswd">语音密码</label><input
                                            type="text" id="radiotxt" style="display: none;" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label for="verifyMobile">
                                            验证码：</label></th>
                                    <td>
                                        <input type="text" id="pageyzm" class="texti" />

                                        <script language="javascript" type="text/javascript">
				                            document.write("<input type=hidden name=codeId id=codeId value=",Math.random(),">")
				                            var tmp = document.getElementById("codeId").value;
				                            document.write("<img id='IMG2' src='ValidateToken.aspx?yymm=",tmp,"' width='62' height='22'>");
				                            function RefreshCode()
				                            {
					                            document.all.IMG2.src = 'ValidateToken.aspx?yymm='+Math.random();
				                            }
                                        </script>

                                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span></td>
                                </tr>
                            </table>
                            <p id="msgp" class="note" style="display: none; color: Red;">
                                <br />
                            </p>
                            <div class="subtn">
                                <span class="btn btnA"><span>
                                    <button name="btnSubmit" id="btnSubmit" type="button" onclick="SubmitRedirect();findpwd();RefreshCode()">
                                        确定</button></span></span></div>
                            <input type="text" id="iptxt" runat="server" style="display: none" />
                            <input type="text" id="spidtxt" runat="server" style="display: none" />
                        </form>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdReturnUrl" runat="server" />
    </form>
    <uc2:Foot ID="Foot1" runat="server" />
</body>

<script language="javascript" type="text/javascript">
        $(document).ready(function(){
            $("#tPasswd").attr("checked","true");
            radio(2);
        }); 
        
        function clearmsg(){
            $("#btnSubmit").html("确定");
            $("#msgp").html("");
            var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;


            if ($("#verifyMobile").attr("value")==null||reg.test($("#verifyMobile").attr("value")))
            {
                   RefreshCode();
            }
        }   
</script>

<script language="jscript" type="text/javascript">

    function SubmitRedirect()
    {
        if ($("#btnSubmit").attr("value")=="继续")
           {
                //跳转到登录页面
                window.location = "./SSO/Login.aspx"
           }
    }
        
</script>

</html>
