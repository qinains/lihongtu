<%@ page language="C#" autoeventwireup="true" inherits="signin, App_Web_kpte7rfs" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>号码百事通客户信息平台</title>
<meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号码百事通,114,118114" />
<meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." />
<link href="css/account.css" type="text/css" rel="stylesheet" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
   
<uc1:Top ID="Top1" runat="server" />
<div class="btContain clfx">
	<div id="main" class="clfx">
	
		<div id="signin">
			<div class="ca">
				<h3>个人登录
                   
                </h3>
				<span class="bizin"><a href="">商家登录&gt;&gt;</a></span>
			</div>
			<div class="cb">
				<dl>
					<dt><label><em>*</em>帐号类型：</label></dt>
					<dd>
						<input type="radio" checked="checked" id="tperson" /><label for="tperson">用户名</label>
						<input type="radio" id="tcard" /><label for="tcard">商旅卡</label>
						<input type="radio" id="tmobile" /><label for="tmobile">手机号</label>
					</dd>
					<dt>
						<label for="username">帐号：</label>
					</dt>
					<dd>
						<input type="text" value="" id="username" class="texti" />
					</dd>
					<dt>
						<label for="password">密码：</label>
					</dt>
					<dd>
						<input type="text" value="" id="password" class="texti" />
						<span class="forget"><a href="">忘记密码</a></span>
					</dd>
					<dt>
						<label for="code">验证码：</label>
					</dt>
					<dd>
						<input type="text" value="" id="code" class="texti" />
						<span class="code"><img src="images/aa/code.gif" /></span>
						<span class="reload">重新获取</span>
					</dd>
				</dl>
				<div class="subtn"><span class="btn btnA"><span><button type="submit">登录</button></span></span></div>
			</div>
		</div>
		<div id="funcIntro">
			<p class="signup">如果您是号码百事通的新朋友请<a href="signup.html">免费注册</a></p>
			<ul class="list">
				<li>用户只需注册一次，即可拥有用户名、手机号码、商旅卡号等多种登录模式，并可使用于移动号百、商旅平台、号码百事通生活频道。</li>
				<li>用户可使用点评、发布信息、留言、纠错等多种互动功能。</li>
				<li>用户可在网站上找朋友，建立自己的个性化家园。</li>
				<li>用户可自主选择是否愿意接受推广及活动参与短信。</li>
				<li class="hl">用户登录成功后，页面会自动返回到首页。单点登录，全网站适用。</li>
			</ul>
			<p class="go"><a href="signup.html">就是这么简单 还不火速加入！</a></p>
		</div>
	</div>
</div>
<uc2:Foot ID="Foot1" runat="server" />

    </form>
</body>
</html>
