<%@ Page Language="C#" AutoEventWireup="true" CodeFile="emailByPwd.aspx.cs" Inherits="emailByPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/account.css" type="text/css" rel="stylesheet" media="screen" />

    <script language="javascript" type="text/javascript" src="ModelJS/jquery.js"></script>

    <script language="javascript" type="text/javascript" src="JS/setEmail/findPwd_JS.js"></script>

</head>
<body>
    <form id="form1" runat="server">
      <uc1:Top ID="Top1" runat="server" />
        <div>
          
            <div class="btContain clfx">
                <div id="main">
                    <div class="ca">
                        <h3>
                            认证邮箱找回密码</h3>
                    </div>
                    <div class="cb">
                        <form>
                            <table class="tableA">
                                <tr>
                                    <th>
                                        <label for="email">
                                            用户名：</label>
                                    </th>
                                    <td>
                                        <input type="text" value="" id="nametxt" class="texti" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label for="email">
                                            邮箱：</label>
                                    </th>
                                    <td>
                                        <input type="text" value="" id="Emailtxt" class="texti" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
					<th><label>验证码：</label></th>
					<td>
						<input type="text" id="pageyzm" />
						<script language="javascript" type="text/javascript">
				            document.write("<input type=hidden name=pageyzm id=pageyzm value=",Math.random(),">")
				            var tmp = document.getElementById("pageyzm").value;
				            document.write("<img id='IMG2' src='ValidateToken.aspx?yymm=",tmp,"' width='62' height='22'>");
				            function RefreshCode()
				            {
					            document.all.IMG2.src = 'ValidateToken.aspx?yymm='+Math.random();
				            }
                        </script>
                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span>
					</td>
				</tr>
                                <tr style="display: none;" id="errtr">
                                    <th>
                                        <img src="images/false.gif" width="73" height="65" /></th>
                                    <td>
                                        <strong id="errstrong"  style="display: block;"></strong>
                                    </td>
                                </tr>
                            </table>
                            <div class="subtn">
                                <span class="btn btnA"><span>
                                    <button type="button" onclick="selEmail();">
                                        确定</button></span></span></div>
                                         <p id="msgp" class="note"  style="display:block;color:Red;"></p>
                        </form>
                    </div>
                    <input type="button" id="urlRedirectButton" style="display:none;" runat="server"
                        onserverclick="urlRedirectButton_ServerClick" />
                            <input type="button" id="pageyzmbutton" onclick="RefreshCode();" style="display:none;" />

                </div>
            </div>
           
        </div>
         <uc2:Foot ID="Foot1" runat="server" />
    </form>
</body>
</html>
