﻿<%@ page language="C#" autoeventwireup="true" inherits="UserAccount_AccountNotBind, App_Web_4a2iedvw" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" type="text/css" />
    <style type="text/css">
        a,a:visited{color:#000;text-decoration:none}a:hover{text-decoration:underline}.resultsA thead td,.resultsA td{padding-left:15px;text-align:left}.resultsA td{padding:3px;padding-left:15px;color:#666}
        .boxA dt{cursor:pointer}
        .boxA dd{display:none}
        .balance p{color:#666}
        .balance strong{color:#39c;font-size:20px;margin-right:10px}
        .balance span{font-size:14px;font-weight:700}
        .air_boxB{border:1px solid #e1e1e1}
        .userGuide h3{color:#039;font:700 14px/18px simsun}
        .userGuide ul{padding:6px 10px;border-top:1px dashed #ebe6c9;color:#666;font:13px/24px simsun}
        .userGuide li{float:left;width:260px;padding-left:10px;background:url(http://myspace.118114.cn/images/icon.gif) 0 -382px no-repeat}
        .userGuide .fu{width:600px}
        .userGuide p{color:#5F8D02;font-family:simsun;margin:10px 0 5px;}
        .fu2{padding-left:0!important}
        .fu2 li{padding-left:0}
        .userGuide ul strong{float:left;font-weight:400;padding-right:5px;}
        .iconA{float:left;background:url(images/icon3.jpg) no-repeat;padding-left:23px;line-height:30px}
        .iconB{float:left;background:url(images/icon3.jpg) 0 -27px no-repeat;padding-left:23px;line-height:30px}
        .iconC{float:left;background:url(images/icon3.jpg) 0 -54px no-repeat;padding-left:23px;line-height:30px}
        .blue,a.blue:visited{color:#39c;text-decoration:none;font-weight:700}
        a.blue:hover{text-decoration:underline}
        .orange,a.orange:visited{color:#f60!important;text-decoration:none;font-weight:700}
        a.orange:hover{text-decoration:underline}
        .resultsA{width:100%;font-family:Verdana,Simsun;color:#666}
        .resultsA thead{border:1px solid #e2e2e2;background:url(http://myspace.118114.cn/images/jb_2.gif) repeat-x 0 -201px;background-position:0 -200px\9;color:#666}
        .resultsA td{padding:10px 5px 10px 15px;line-height:19px;text-align:center}
        .resultsA thead td{padding:0 0 0 15px!important;height:27px;text-align:center}
        .foot{width:980px;margin:10px auto;text-align:center;color:#78797a;line-height:23px}
        .foot a{color:#acadaf}
        .head{width:980px;margin:0 auto;clear:both;font-family:Microsoft Yahei,simhei;color:#007bc1;font-size:24px}
        .head p{margin-top:12px;margin-left:20px;padding-left:20px;padding-top:8px;border-left:1px solid #c2c2c2;height:30px}
        .tabA{width:980px;margin:0 auto;clear:both;border-bottom:1px solid #8fc4e0}
        .tabA li{float:left;margin-top:16px;padding-left:10px;clear:none;width:162px;height:32px;line-height:32px;background:#f6f6f6;font-weight:700;font-size:14px;color:#007bc1; cursor:pointer;text-align:center;margin-left:20px;border-radius:10px 10px 0 0;}
        .tabA li.vazn{border:1px solid #8fc4e0;border-width:1px 1px 0 1px;background:#fff}
    </style>
    
    <script type="text/javascript" src="../js/jquery-1.4.2.min.js"></script>
    
</head>
<body>
    <asp:Panel ID="header" runat="server">
        <div class="head clearfix">
            <div class="fl">
                <img src="../images/logoct.gif" alt="" /><img style="padding-left: 20px" src="../images/logo_besttone.gif"
                    alt="" /></div>
            <p class="fl">
                号码百事通账户</p>
        </div>
    </asp:Panel>
    <div style="width: 390px; padding: 30px 10px 30px 140px; margin: 50px auto; background: url(../images/bg_noAccount.png) no-repeat 0 50%">
        <h3 style="margin: 10px auto; font: bold 18px Microsoft Yahei; color: #039">
            抱歉！<span>您的号码百事通账户尚未开通。</span></h3>
        <a href="../CreateBesttoneAccount.aspx?SPTokenRequest=<%=newSPTokenRequest%>"
            style="color: #39c; font: bold 14px Microsoft Yahei" target="_top">马上开通>></a>
    </div>
    <asp:Panel ID="footer" runat="server">
        <div class="foot">
            <p>
                中国电信集团：<a href="">189邮箱</a> | <a href="">天翼宽带</a> | <a href="">号百商旅</a> | <a href="">
                    号百导航</a> | <a href="">天翼手机网</a> | <a href="">翼支付</a> | <a href="">爱音乐</a> |
                <a href="">天翼视讯</a> | <a href="">协同通讯</a> | <a href="">物联网</a> | <a href="">天翼空间</a>
                | <a href="">天翼阅读</a> | <a href="">爱游戏</a> | <a href="">爱动漫</a></p>
            违法和不良信息举报 <a href="">service@118114.cn</a> copyright? 2007-2011 号百商旅电子商务有限公司版权所有<br />
            增值电信业务经营许可证：沪B2-20110026 沪ICP备11017770号 上海工商标识编号：20110624111724289
        </div>
    </asp:Panel>
</body>
</html>
