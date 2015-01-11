<%@ page language="C#" autoeventwireup="true" inherits="FindPassWord_findPasswordNew, App_Web_findpasswordnew.aspx.d026a163" enableEventValidation="false" %>


<%@ Register Src="../UserCtrl/CommonHead.ascx" TagName="CommonHead" TagPrefix="uc1" %>
<%@ Register Src="../UserCtrl/CommonFoot.ascx" TagName="CommonFoot" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>找回密码</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    <style type="text/css">
        body{background:url(http://static.118114.cn/images/bg_body.jpg) repeat-x 50% -2px}
        .content{width:980px;position:relative;margin:0 auto 10px;border:1px solid #8fc4e0;background:#fff}
        .word{float:left;padding-top:39px;_padding-top:41px;color:#007bc1;height:23px;line-height:23px;font-size:25px;font-family:Microsoft Yahei,simhei}
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
    <uc1:CommonHead ID="CommonHead1" runat="server"></uc1:CommonHead>
    <div class="content">
	    <ul id="findTab">
    	    <li class="tab1">手机找回密码</li>
            <li class="tab2">邮箱找回密码</li>
        </ul>
	    <div class="findPasswordBox">
           <iframe src="findPasswordByPhone.aspx" width="660" height="270" scrolling="no" frameborder="0" id="iframe"></iframe>
        </div>
    </div>
    
    <uc2:CommonFoot ID="CommonFoot1" runat="server"></uc2:CommonFoot>
    </form>
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
</body>
</html>
