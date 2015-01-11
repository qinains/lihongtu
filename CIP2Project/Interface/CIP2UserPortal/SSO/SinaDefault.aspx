<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SinaDefault.aspx.cs" Inherits="SSO_SinaDefault" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    
	<title>新浪微博SDK for .Net 2.0 Web Demo</title>
    <link rel="stylesheet" type="text/css" href="../css/SinaOAuth/reset.css" />
	<link rel="stylesheet" type="text/css" href="../css/SinaOAuth/stylesheet.css" />
	<script type="text/javascript" src="../JS/SinaOAuth/jquery-1.7.2.js"></script>
	<script type="text/javascript">
		
		var userInfo = <%=LoadUserInfo()%>;
	
		$(function(){
			$("#user-face").attr("src",userInfo["profile_image_url"]);
			$("#user-name").text(userInfo["screen_name"]);
			$("#user-intro").text(userInfo["description"]);
			$("#user-statuses-count").text(userInfo["statuses_count"]);
			$("#user-follow-count").text(userInfo["friends_count"]);
			$("#user-follower-count").text(userInfo["followers_count"]);
		});
	</script>    
    
</head>
<body>
	<form id="form1" runat="server">
	<div id="header">
		<div id="header_wrap" class="row">
			<p id="logo">
				<a href="#">Weibo<span>DotNet</span>2.0</a>
			</p>
			<ul id="page-links">
				<li><a href="#" class="selected">主页</a></li>
				<li><a href="#">联系</a></li>
				<li><a href="#">评论</a></li>
			</ul>
			<div id="search-box" class="row">
				<input type="text" id="search-box-text" />
				<span id="search-box-button"><a href="#" class="magnify"></a></span><span id="search-box-tips">搜索微博内容</span>
				<script type="text/javascript">
					var searchTips = $("#search-box-tips");
					var searchText = $("#search-box-text");
					searchText.focus(function () {
						searchTips.hide();
						$(this).addClass("focus");
					}).blur(function () {
						if ($(this).val().length == 0) {
							$(this).val("");
							searchTips.show();
						}
						$(this).removeClass("focus");
					});
					searchTips.click(function () {
						searchTips.hide();
						searchText.trigger("focus");
					});
				</script>
			</div>
		</div>
	</div>
	<div id="content_wrap" class="row">
		<div id="content_left_wrap">
			<div class="panel">
				<div class="box">
					<a href="#" class="user-self">
						<img id="user-face" src="http://tp3.sinaimg.cn/1732351714/50/5610733890/1" alt="" />
						<span class="name" id="user-name">#Name#</span><span class="intro" id="user-intro">#Introduction#</span>查看我的个人资料页面 </a>
				</div>
				<div class="status-box row">
					<a href="#"><span id="user-statuses-count">1</span>
						<br />
						微博 </a><a href="#" class="middle"><span id="user-follow-count">1</span>
							<br />
							正在关注 </a><a href="#"><span id="user-follower-count">1</span>
								<br />
								粉丝 </a>
				</div>
				<div id="status-box" class="box">
					<asp:TextBox runat="server" ID="txtStatusBody" CssClass="status-box-editor" TextMode="MultiLine"></asp:TextBox>
					<script type="text/javascript">
						var editor = $("#<%=txtStatusBody.ClientID %>");
						editor.focus(function () {
							$(this).addClass("focus");
						}).blur(function () {
							$(this).removeClass("focus");
						});
					</script>
					<div id="status-box-tools" class="row">
						<label title="添加图片" id="status-box-picture">
							<span></span>
							<asp:FileUpload runat="server" ID="fileUpload1" CssClass="status-box-input"></asp:FileUpload>
						</label>
						<asp:Button runat="server" ClientIDMode="Static" ID="btnSend" CssClass="status-box-submit" Text="发送微博" OnClick="btnSend_Click" />
					</div>
				</div>
			</div>
			<div class="panel">
				<h2 class="title">推荐关注</h2>
				<div class="box">
					<asp:Repeater ID="rtpFamous" runat="server">
						<ItemTemplate>
							<a href="#" class="user-self">
								<img id="user-face" src="<%#Eval("ProfileImageUrl") %>" alt="" />
								<span class="name" id="user-name"><%#Eval("ScreenName")%></span>
								<span class="intro" id="user-intro"><%#Eval("Description")%></span>
								加关注
							</a>
							<br />
						</ItemTemplate>
					</asp:Repeater>
				</div>
			</div>
			<div class="panel">
				<h2 class="title">关于新浪微博SDK for .Net 4.0</h2>
				<div class="box">
					<ul>
						<li>发布地址：<a href="http://user.cip2.dep.114.cn:8081/">cnblogs</a></li>
						<li>项目地址：<a href="http://user.cip2.dep.114.cn">http://user.cip2.dep.114.cn</a></li>
						<li>开发者：李宏图</li>
						<li>公司：<a href="http://www.118114.cn">号百信息服务有限公司</a></li>
					</ul>
				</div>
			</div>
		</div>
		<div id="content_right_wrap">
			<h2 class="title">//最新微博/ Code for fun! </h2>
			<asp:Literal runat="server" ID="statusesHtml"></asp:Literal>
	        <asp:TextBox runat="Server" ID="UID"></asp:TextBox>
		</div>
	</div>
	</form>
</body>
</html>
