<%@ page language="C#" autoeventwireup="true" inherits="ErrorInfo, App_Web_errorinfo.aspx.81d98cab" enableEventValidation="false" %>

<%@ Register Src="~/UserCtrl/Top.ascx"TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="~/UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>号码百事通客户信息平台</title>
<meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号码百事通,114,118114" />
<meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." />
<link href="../../css/account.css"type="text/css" rel="stylesheet" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<uc1:Top ID="Top1" runat="server" />
        <div class="btContain clfx">
	<div id="main">
		<div class="cb">
			<div class="notice true">
				<table><tr>
					<th><img src="../../images/false.gif"width="73" height="65" /></th>
					<td>
						<strong><label runat="Server" id="lbFunctionName"></label></strong>
					</td>
				</tr></table>
			</div>
			<p class="note"><label id="lbErrorInfo" runat="Server"></label><br /><a href=""></a>&nbsp;</p>
		</div>
	</div>
</div>
       
        <uc2:Foot ID="Foot1" runat="server" OnLoad="Foot1_Load" />
    </div>
    </form>
</body>
</html>

