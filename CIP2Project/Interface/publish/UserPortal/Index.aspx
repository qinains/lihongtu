<%@ page language="C#" autoeventwireup="true" inherits="Index, App_Web_index.aspx.cdcab7d2" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="~/UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号码百事通,114,118114" />
    <meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." />
    <link href="css/account.css" type="text/css" rel="stylesheet" media="screen" />

    <script language="javascript" type="text/javascript" src="ModelJS/jquery.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-latest.pack.js"></script>
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

    <script language="javascript" type="text/javascript">
        function csstype(){
            var url = window.location.href;
            if(url.split("id")[1]!=null){
                param=url.split('id=')[1].split("&SPID")[0];
                document.getElementById("li"+param).className='vazn';
            }
            if ($("#ctl00_custidtxt").attr("value")==null){
                $("#sidebar").hide();
                document.getElementById("main").style.width="100%";
            }else{
                $("#sidebar").show();
            }          
        }             
        
        menuClick = function(url){
            $("#iframe1").attr("src",url);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:Top ID="Top1" runat="server" />
            <div class="btContain clfx accMana">
                <div id="sidebar" style="display: block;">
                    <ul id="accNav">
                        <li id="li1"><span><a onclick="menuClick('CustInfoManager/CustInfoModify.aspx');" href="#">
                            基本信息修改</a></span></li>
                        <li id="li2"><span><a href="#">
                            安全设置</a></span></li>
                        <li id="li8"><span><a onclick="menuClick('UserAccount/AccountRecharge.aspx');" href="#">
                            账户充值</a></span></li>
                        <li id="li6"><span><a onclick="menuClick('UserAccount/OrderDetailQuery.aspx');" href="#">
                            交易查询</a></span></li>
                    </ul>
                </div>
                <div id="main" style="height: 650px;">
                    <iframe id="iframe1" runat="server" width="100%" height="650px" frameborder="0" src="UserAccount/AccountRecharge.aspx"></iframe>
                </div>
            </div>
            <uc2:Foot ID="Foot" runat="server" />
            <input type="text" id="custidtxt" runat="server" style="display: none;" />
            <input type="text" id="spidtxt" runat="server" style="display: none;" />
            <br />
        </div>
    </form>
</body>
</html>
