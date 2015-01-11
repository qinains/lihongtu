<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="CustInfoManager_SetPasswordQuestion, App_Web_setpasswordquestion.aspx.8268bb4f" enableEventValidation="false" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ca">
        <h3>
            设置密码提示问题
        </h3>
    </div>
    <div class="cb">
        <table class="tableA">
            <tr>
                <th>
                    用户名：</th>
                <td>
                    <asp:TextBox ID="txtUserName" class="texti" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    姓&nbsp;&nbsp;&nbsp;名：</th>
                <td>
                    <asp:TextBox ID="txtRealName" class="texti" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    昵&nbsp;&nbsp;&nbsp;称：</th>
                <td>
                    <asp:TextBox ID="txtNickName" class="texti" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    性&nbsp;&nbsp;&nbsp;别：</th>
                <td>
                    <asp:DropDownList ID="DDLSex" runat="server">
                        <asp:ListItem Value="2">未知</asp:ListItem>
                        <asp:ListItem Value="1">男</asp:ListItem>
                        <asp:ListItem Value="0">女</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="subtn">
                        <span class="btn btnA"><span>
                            <img id="loadImg" src="../Images/Loading.gif" style="position:absolute; display:none" />
                            <button id="btnSave" type="button" onclick="SaveFun();">
                                保 存</button></span></span></div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>