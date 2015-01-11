<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Success.aspx.cs" Inherits="CustInfoManager_Success" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../css/account.css" type="text/css" rel="stylesheet" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="cb">
        <div class="notice true">
            <table>
                <tr>
                    <th>
                        <img alt="" src="../images/true.gif" width="57" height="45" /></th>
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
    </form>
</body>
</html>
