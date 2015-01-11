<%@ page language="C#" autoeventwireup="true" inherits="FindPwdSelect, App_Web_kpte7rfs" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>号码百事通客户信息平台</title>
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
                            请选择您要找回密码的方式</h3>
                    </div>
                    <div class="cb">
                        <form>
                            <table class="tableA">
                                <tr>
                                    <th>
                                        <%--<label for="email">
                                          <a href="verifyMobile.aspx">认证手机找回密码</a></label>--%>
                                        <asp:HyperLink runat="server" ID="linkPhone" NavigateUrl="FindPassWord/FindByEmail.aspx">认证手机找回密码</asp:HyperLink>
                                    </th>
                                    <td>
                                       
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <%--<label for="email">
                                           <a href="emailByPwd.aspx">认证邮箱找回密码</a></label>--%>
                                           <asp:HyperLink runat="server" ID="linkEmail" NavigateUrl="FindPassWord/FindByPhone.aspx">认证邮箱找回密码</asp:HyperLink>
                                    </th>
                                    <td>
                                      
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <th>
                                        <asp:HyperLink runat="server" ID="linkPwdQuestion" NavigateUrl="paswdByQnA.aspx">密码提示问题找回密码</asp:HyperLink>
                                            </th>
                                    <td>
                                    </td>
                                </tr>--%>
                            </table>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
    </form>
        <uc2:Foot ID="Foot1" runat="server" />
</body>
</html>
