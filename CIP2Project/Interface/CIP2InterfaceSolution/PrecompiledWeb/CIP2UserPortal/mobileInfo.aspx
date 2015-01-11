<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="mobileInfo, App_Web_waxseis2" title="Untitled Page" enableEventValidation="false" %>

<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

    <script language="javascript" type="text/javascript" src="JS/setMobile/setMoblie_JS.js"></script>

    <div class="ca">
        <h3>
            设置认证手机</h3>
    </div>
    <div class="cb">
        <form>
            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_ItemDataBound">
                <HeaderTemplate>
                    <table align="center" border="0" style="width: 500px;">
                        <tr>
                            <td>
                                手机号码</td>
                            <td>
                                状态</td>
                            <td>
                                操作</td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "phone")%>
                            <input type="text" id="phoneIdTxt" value='<%#DataBinder.Eval(Container.DataItem, "SequenceID")%>'
                                style="display: none" runat="server">
                            <input type="text" id="phoneTxt" value='<%#DataBinder.Eval(Container.DataItem, "Phone")%>'
                                style="display: none" runat="server">
                        </td>
                        <td>
                            <input type="text" id="typeTxt" value='<%#DataBinder.Eval(Container.DataItem, "phoneclass")%>'
                                style="display: none" runat="server">
                            <asp:Label ID="typelable" runat="server"></asp:Label></td>
                        <td>
                            <input id="delbutton" type="button" value="删除" runat="server" onclick="del(this.id);" />
                            <input id="rzbutton" type="button" value="我要认证" runat="server" onclick="renzheng(this.id);" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table></FooterTemplate>
            </asp:Repeater>
            <div align="center" class="subtn">
                <span class="btn btnA"><span>
                    <button type="button" id="setButton" onclick="tiaozhuan();">
                        新增认证电话</button></span></span>
             </div>
            <p id="msgp" class="note" style="display: block; color: Red;">
            </p>
        </form>
    </div>
    <input type="text" id="custidtxt" runat="server" style="display: none;" />
    <input name="num" id="num" size="3" value="0" style="display: none;" />
    <input type="button" id="urlRedirectButton" style="display: none;" runat="server" />
</asp:Content>
