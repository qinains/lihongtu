<%@ Page Language="C#" AutoEventWireup="true" CodeFile="paswdByQnA.aspx.cs" Inherits="paswdByQnA" %>

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

    <script language="javascript" type="text/javascript" src="JS/paswdByQndA/paswdByQnA_JS.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:Top ID="Top1" runat="server" />
            <div class="btContain clfx">
                <div id="main">
                    <div id="signup">
                        <div class="ca">
                            <h3>
                                请输入您当时填写的密码提示问题答案找回密码</h3>
                        </div>
                        <div class="cb">
                         <div id="question" > 
                            <table class="tableA"  align="center" style="height:200px;">
                                <tr>
                                    <th style="width: 96px">
                                        <asp:Label ID="Label2" runat="server" Text="用户名"></asp:Label></th>
                                    <td>
                                        <input type="text" value="" id="txtUserName" class="texti" /></td>
                                    <td >
                                        <span id="err_username"   class="remark"  />
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 96px">
                                        <asp:Label ID="Label1" runat="server" Text="问题"></asp:Label></th>
                                    <td>
                                        <select id="ddlQuestion" runat="server" onchange="selvalue(this.value);">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <th style="width: 96px">
                                        <asp:Label ID="Label3" runat="server" Text="答案"></asp:Label></th>
                                    <td>
                                        <input type="text" value="" id="txtAnswer" class="texti" /></td>
                                    <td >
                                        <span id="err_answer" class="remark" />
                                    </td>
                                </tr>
                               <tr>
					                <th style="width: 96px"><span class="mst">*</span>输入验证码</th>
					                <td>
						                <input type="text" value="" id="code" class="texti" name="code" />
					                </td>
					                <td>
					                   <span id="err_code" class="remark" runat="server"></span>
					                </td>
				             </tr>
                                <tr>
					                <th style="width: 96px"><label for="code"></label></th>
					                <td> 
						                <span class="code">                        
						                <script language="javascript" type="text/javascript">
				                            document.write("<img id='IMG2' src='ValidateToken.aspx' width='62' height='22'>");
				                            function RefreshCode()
				                            {
					                            document.all.IMG2.src = 'ValidateToken.aspx?.tmp='+Math.random();
				                            }
                                        </script>
                                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span>
                                        </span>
					                </td>
				                </tr>
     
                            </table>
                            <div class="subtn">
                                <span class="btn btnA"><span>
                                    <button type="button" runat="server" id="btByQnA" onclick="findpwd();">
                                        确定</button></span></span></div>
                                      
                             </div>           
                                        
                            <div id="mmczdiv" class="subtn" style="display: none;">
                                <table class="tableA"  align="center" style="height:150px;">
                                  <tr align="center">
                                     <th></th>
                                    <td>
                                    <span id="err_check" class="remark" />
                                     </td>
                                    
                                    </tr>
                                    <tr>
                                       <th><label for="passwd"><span class="mst">*</span>输入新密码：</label></th>
                                        <td>
                                            <input type="password" id="txtPwd1" runat="server"   class="texti" /></td>
                                    </tr>
                                    <tr>
                                        <th><label for="verifyPasswd"><span class="mst">*</span>确认新密码：</label></th>
                                        <td>
                                            <input type="password" id="txtPwd2" runat="server" class="texti" />
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                     <th></th>
                                    <td>
                                     <span id="err_pwd" class="remark" />
                                    </td>
                                    </tr>
                                    <tr>
                                    <th></th>
                                        <td>
                                           <span class="btn btnA"><span>
                                                <button type="button" runat="server" id="btPwdReset" onclick="resetpwd();">
                                                    密码重置</button></span></span></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <input type="text" id="txtQuestion" runat="server" style="display: none;" />
                <input type="text" id="txtcustid" runat="server" style="display: none;" />
                <input type="text" id="txtCustType" runat="server" style="display: none;" />
                <asp:HiddenField ID="hdReturnUrl" runat="server" />
            </div>
            <uc2:Foot ID="Foot1" runat="server" />
        </div>
    </form>
</body>
</html>
