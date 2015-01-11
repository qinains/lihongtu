<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_SetPayPassword, App_Web_ac68v7gr" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="findPasswordBox">
            <label for="mobile">
                <span>旧密码：</span><input type="text" id="mobile" class="input" maxlength="11" onblur="doCheckPhone();" /><em>*</em><p
                    id="hintMobile">
                    请输入旧密码</p>
            </label>
            <label for="mobile">
                <span>新密码：</span><input type="text" id="Text1" class="input" maxlength="11" onblur="doCheckPhone();" /><em>*</em><p
                    id="P1">
                    请输入旧密码</p>
            </label>
            <label for="mobile">
                <span>确认新密码：</span><input type="text" id="Text2" class="input" maxlength="11" onblur="doCheckPhone();" /><em>*</em><p
                    id="P2">
                    请输入旧密码</p>
            </label>
            <label for="checkCode">
                <span>验证码：</span><input type="text" id="checkCode" class="input" maxlength="4" /><img
                    width="62" height="30" src="http://customer.besttone.com.cn/UserPortal/ValidateToken.aspx"
                    id="IMG2"><a href="javascript:RefreshViewCode()">看不清？换一张</a><p id="hintCode">
                    </p>
            </label>
            <input type="button" value="下一步" id="getPassword" onclick="checkSubmit()" /><img
                id="loadImg" src="Images/Loading.gif" style="display: none" />
        </div>
    </form>
</body>
</html>
