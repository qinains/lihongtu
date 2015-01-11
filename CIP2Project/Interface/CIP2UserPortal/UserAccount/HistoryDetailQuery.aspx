<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HistoryDetailQuery.aspx.cs"
    Inherits="UserAccount_HistoryDetailQuery" %>

<%@ Register Src="../UserCtrl/CommonHead.ascx" TagName="CommonHead" TagPrefix="uc1" %>
<%@ Register Src="../UserCtrl/CommonFoot.ascx" TagName="CommonFoot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="description" content="" />
    <meta name="keywords" content="机票 国内机票 特价机票 旅行度假 商旅管理 号码百事通" />
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    <style type="text/css">
        .liC{margin:4px;clear:both;height:37px;border-bottom:1px solid #e0e0e0}
        .liC li{float:left;margin-right:-1px;width:102px;height:37px;line-height:37px;text-align:center;color:#666; background:url(images/icon1.jpg) no-repeat 0 3px;cursor:pointer}
        .liC li.vazn{ background:url(images/icon1.jpg) no-repeat;color:#f60;font-weight:700}
        .butA{margin:0 auto;background:url(images/but_1.png) no-repeat!important;width:66px;height:21px;line-height:21px;color:#39c;font-size:12px}
        .blue,a.blue:visited{color:#39c!important;text-decoration:none;font-weight:700}
        a.blue:hover{text-decoration:underline}
        .orange,a.orange:visited{color:#f60!important;text-decoration:none;font-weight:700}
        a.orange:hover{text-decoration:underline}
    </style>

    <script type="text/javascript" src="scripts/tab.js"></script>
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../ModelJS/Date/WdatePicker.js"></script>

    <link href="~/CSS/aspNetPage.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="http://myspace.118114.cn/css/myhome.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <uc1:CommonHead ID="CommonHead1" runat="server"></uc1:CommonHead>
    <form id="form1" runat="server">
        <div class="air_boxB clfx" style="width: 790px; padding: 0 10px 10px; margin: 10px auto">
            <div class="flight_sosoB clfx">
                <ul style="margin-top: 20px; width: 800px" class="ml10 clfx">
                    <li class="l">
                        <label class="l" style="height: 30px; width: 80px">
                            起止时间：</label>
                        <asp:TextBox ID="txtStartDate" onclick="WdatePicker();" Style="display: _inline;
                            *margin-top: -15px" class="l" runat="server"></asp:TextBox><div class="l" style="margin-left: 10px;
                                *margin-top: -10px">
                                ─</div>
                        <asp:TextBox ID="txtEndDate" runat="server" onclick="WdatePicker();" Style="margin-left: 10px;
                            *margin-top: -15px" class="l"></asp:TextBox>
                        <asp:Button ID="btnSearch" Style="margin-top: -5px; _margin-top: 0; margin-left: 10px;
                            *margin-top: -17px" class="but butA l" runat="server" Text="查 询" />&nbsp; 
                        <font style="color:Red"><asp:Label ID="lblErrMsg" runat="server" Text=""></asp:Label></font>
                            <%--&nbsp; <a href="#">今天</a> <a href="#">最近7天</a> <a href="#">1个月</a>--%>
                        <%--<a href="">3个月</a> <a href="">1年</a>--%>
                </ul>
            </div>
            <ul class="liC">
                <li class="vazn" onclick="tabconAlt(0,'E',8);" id="tabE0">交易记录</li>
                <li onclick="tabconAlt(1,'E',8);" id="tabE1">充值记录</li>
                <li onclick="tabconAlt(2,'E',8);" id="tabE2">退款记录</li>
            </ul>
            <div id="conE0">
                <div class="details">
                    <table class="resultsA">
                        <thead>
                            <tr>
                                <td>
                                    日期</td>
                                <td>
                                    交易摘要</td>
                                <td>
                                    交易金额</td>
                                <td>
                                    交易渠道</td>
                                <td>
                                    商户名称</td>
                                <td>
                                    状态</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Literal ID="LiteralBusiness" runat="server"></asp:Literal>
                        </tbody>
                    </table>
                </div>
            </div>
            <div id="conE1" style="display: none">
                <div class="details">
                    <table class="resultsA">
                        <thead>
                            <tr>
                                <td>
                                    日期</td>
                                <td>
                                    交易摘要</td>
                                <td>
                                    交易金额</td>
                                <td>
                                    交易渠道</td>
                                <td>
                                    商户名称</td>
                                <td>
                                    状态</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Literal ID="LiteralRecharge" runat="server"></asp:Literal>
                        </tbody>
                    </table>
                </div>
            </div>
            <div id="conE2" style="display: none">
                <div class="details">
                    <table class="resultsA">
                        <thead>
                            <tr>
                                <td>
                                    日期</td>
                                <td>
                                    交易摘要</td>
                                <td>
                                    交易金额</td>
                                <td>
                                    交易渠道</td>
                                <td>
                                    商户名称</td>
                                <td>
                                    状态</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Literal ID="LiteralRefund" runat="server"></asp:Literal>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hdStartDate" runat="server" />
        <asp:HiddenField ID="hdEndDate" runat="server" />
    </form>
    <uc2:CommonFoot ID="CommonFoot1" runat="server"></uc2:CommonFoot>

    <script type="text/javascript">
        $(document).ready(function(){
            $("#btnSearch").click(function(){
                var startDate = $("#txtStartDate").val();
                var endDate = $("#txtEndDate").val();
                if(startDate == "" || endDate == ""){
                    $("#lblErrMsg").text("*请输入起止日期");
                    return false;
                }else{
                    $("#lblErrMsg").text("");
                }
                var startArray = startDate.split('-');
                var endArray = endDate.split('-');
                var sDate = new Date(startArray[0],startArray[1],startArray[2]);
                var eDate = new Date(endArray[0],endArray[1],endArray[2]);
                if(sDate>eDate){
                    $("#lblErrMsg").text("*开始日期不能大于结束日期");
                    return false;
                }
                //if(parseInt(startArray[0],10)!=parseInt(endArray[0],10))
            })
        })
    </script>

</body>
</html>
