<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Success, App_Web_waxseis2" title="Untitled Page" enableeventvalidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ register tagprefix="CIPUserCtrl" tagname="TokenValidate" src="UserCtrl/ValidateCIPToken.ascx" %>
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

    <script language="javascript" type="text/javascript">
        function dd () {
            var ReturnUrl = '<%=ReturnUrl%>'
            if(ReturnUrl=="")
            {
                 ReturnUrl = 'Http://118114.cn';
            }
 

            //*  等待5秒*/
            //setInterval(dd, 3000)
                //alert(222)
            window.location.href = ReturnUrl
        }
    </script>

    <div class="cb">
        <div class="notice true">
            <table>
                <tr>
                    <th>
                        <img alt="" src="images/true.gif" width="57" height="45" /></th>
                    <td>
                        <strong>
                            <label runat="Server" id="lbDescription">
                            </label>
                        </strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 50px" valign="bottom" align="right">
                        <label runat="Server" id="lbHint">
                        </label>
                    </td>
                </tr>
                <tr>
                    <td class="subtn" align="center">
                        <span class="btn btnA">
                            <button type="button" id="btnBack" onclick="dd()">
                                返回</button>
                        </span>
                    </td>
                </tr>
                <tr>
            </table>
        </div>
    </div>
</asp:Content>
