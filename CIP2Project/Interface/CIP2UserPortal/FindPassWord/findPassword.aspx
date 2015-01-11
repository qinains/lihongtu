<%@ Page Language="C#" AutoEventWireup="true" CodeFile="findPassword.aspx.cs" Inherits="FindPassWord_findPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>无标题文档</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css">
    <link type="text/css" rel="stylesheet" href="http://static.118114.cn/css/public/you_public_head.css">
    <link type="text/css" rel="stylesheet" href="http://static.118114.cn/css/public/you_public_foot.css">
    <%--<script type="text/javascript" src="http://static.118114.cn/js/public/you_public_head.js"></script>--%>
    <style>
        body{background:url(http://static.118114.cn/images/bg_body.jpg) repeat-x 50% -2px}
        .Yiyou_header,.content{width:980px;position:relative;margin:0 auto 10px}
        .Yiyou_logo{width:174px;height:71px;display:block;background:url(http://static.118114.cn/images/yiyou_logo.png);_background:url(http://static.118114.cn/images/pic_logo8.png);text-indent:-9999px}
        .word{float:left;padding-top:39px;_padding-top:41px;color:#007bc1;height:23px;line-height:23px;font-size:25px;font-family:Microsoft Yahei,simhei}
        .content{width:978px;border:1px solid #8fc4e0;background:#fff}
        #findTab{width:600px;height:43px;border-bottom:1px solid #e9e9e9;margin:30px;padding-left:20px}
        #findTab li{float:left;display:block;width:197px;height:52px;margin-right:20px;text-indent:-9999px;cursor:pointer;background-image:url(images/spriteLoginRegister.png)}
        #findTab li.tab1{background-position:0 -152px;height:52px}
        #findTab li.tab2{background-position:0 -204px;height:43px}
        #findTab li#s_tab1{background-position:0 -308px;height:43px}
        #findTab li#s_tab2{background-position:0 -256px;height:52px}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Yiyou_header">
	    <div class="clearfix m5">
    	    <a class="fl Yiyou_logo" href="http://www.118114.cn/">翼游旅行网</a><span class="word">号百通行证</span>
	    </div>
    </div>
    <div class="content">
	    <ul id="findTab">
    	    <li class="tab1">手机找回密码</li>
            <li class="tab2">邮箱找回密码</li>
        </ul>
	    <div class="findPasswordBox">
           <iframe src="findPasswordByPhone.aspx" width="660" height="270" scrolling="no" frameborder="0" id="iframe"></iframe>
        </div>
    </div>
    <script type="text/javascript">
        var tab=document.getElementById("findTab");
        var iframe=document.getElementById("iframe");
        tab.onclick=function(e){
	        var e= e||window.event;   
            var target=e.target||e.srcElement;
	        switch(target.className.substring(0,4)){
		        case "tab1":
			        iframe.src="findPasswordByPhone.aspx";
			        tab.getElementsByTagName("li")[1].id="";
			        target.id="";
			        break;
		        case "tab2":
			        iframe.src="findPasswordByEmail.aspx";
			        tab.getElementsByTagName("li")[0].id="s_tab1";
			        target.id="s_tab2";
			        break;
	        }
        }
    </script>
    <script type="text/javascript" src="http://static.118114.cn/js/public/you_public_foot.js"></script>
    </form>
</body>
</html>
