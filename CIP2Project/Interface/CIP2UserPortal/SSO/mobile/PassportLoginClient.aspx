﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PassportLoginClient.aspx.cs" Inherits="SSO_mobile_PassportLoginClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
   
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <link href="css/base.css"rel="stylesheet" type="text/css" />
    <link href="css/global.css"rel="stylesheet" type="text/css" />
    <link href="css/mobile.css"rel="stylesheet" type="text/css" />
    <link href= "css/pay.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JS/jquery-1.4.2.min.js"></script>   
    
  <script type="text/javascript">


$(function(){
	$("#table1").hide();
	$("#table2").show();
	$("div[class='tab']").css("background", "#dcdcdc");
	$("#tab1").css("background", "#eee");

	    $("div[class='tab']").click(function(){
		    $("div[class='tab']").css("background", "#dcdcdc");
		    $(this).css("background", "#eee");
		    var tabId = $(this).attr("id");
		    if("tab1"==tabId) {
			    $("#table2").hide();
			    $("#table1").show();
				$(".loginBG").show();
		    }else {
			    $("#table1").hide();
			    $("#table2").show();
				$(".loginBG").hide();
				$("#errorHint").hide();
		    }
    		
	    });
    });



</script>    
  
    
</head>
<body>
   <div class="wapPage" style="width:100%">
       <form id="form1" runat="server">

        <center>
          
          
          <div style="margin-top:0px; width:320px;">
            <div style="border-top: 2px solid #ff8a00;display:none">
              <div class="tab" id="tab1" style="width:50%; line-height:40px; font-weight:bold; float:left;background:#dcdcdc;display:none">用户登录</div>
              <div class="tab" id="tab2" style="width:50%; line-height:40px; font-weight:bold; float:left;">天翼帐号登录</div>
            </div>
            <div class="loginBG"></div>
            <span id="errorHint" style="color:Red" runat="server" ></span>
            <table class="tabLogin" id="table1" style="margin-top:0px">
              <tr>
                <th height="40" style=" text-indent:5px;">
                  账号：</th>
                <td>
                 <asp:TextBox ID="username" runat="server" style="width:250px; height:30px; font-size:16px; border:1px solid #ccc;" ></asp:TextBox></td>
                </tr>
              <tr>
                <th height="40" style=" text-indent:5px;">
                  密码：</th>
                <td>
                    <asp:TextBox ID="password" runat="server" TextMode="Password" style="width:250px; height:30px; font-size:16px; border:1px solid #ccc;"></asp:TextBox></td>
                </tr>
              <tr>
                <td height="40" colspan="2" style="text-align:center; padding:30px 0 0 0;">
                    <asp:ImageButton ID="ImageBtnLogin" runat="server" ImageUrl="~/SSO/images/bt_login.gif" OnClick="ImageBtnLogin_Click"  /></td>
                </tr>
              </table>
              <table class="tabLogin" id="table2" style="margin-top: 0px;">
              <tr>
                <td>
 
	                <iframe src="<%=login189Url %>" width="320" scrolling="no" height="310" frameborder="0"></iframe>
	                
                </td>
                </tr>
              </table>
            </div>
          </center>
      </form>
    </div>
</body>
</html>
