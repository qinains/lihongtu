<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="SSO_NewLogin2" %>

<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>号码百事通客户信息平台</title>
    <meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号码百事通,114,118114" />
    <meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." />

    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>

    <link href="~/css/common.css" rel="stylesheet" type="text/css" />
     <script language="javascript" type="text/javascript" src="../JS/tab.js"></script>
    <script language="javascript" type="text/javascript">			           
        function checkinput(){ 
             selvalue();
             if(checkUserNameEmpty()&&checkPassword()&&checkCode()&&checkUserName()){
                return true;
             }else{
               return false;
             }
        }
        function checkPassword(){
            if($("#txtPassword").attr("value") == ""){
                $("#txtPassword").focus();
                $("#err_Password").html("请输入密码");
                return false;
            }else{
              $("#err_Password").html("");
            }
            return true;
        }

        function checkCode(){
            if($("#code").attr("value") == ""){
                $("#code").focus();
                $("#err_code").html("请输入验证码");
                return false;
            }else{
              $("#err_code").html("");
            }
            return true;
        }

        function checkUserNameEmpty(){
            if($("#txtUsername").attr("value") == ""){
                $("#txtUsername").focus();
                $("#hint_Username").html("");
                $("#err_Username").html("请输入登陆名");
                return false;
            }else{
              $("#err_Username").html("");
            }
            return true;
        }

        function checkBigUserName(){
            if(checkUserNameEmpty()&&checkUserName()){
                 return true;
            }else{
                 return false;
            }
        }


        function checkUserName() {
            var i=$("input[name='ddlAuthenTypeList']:checked").val(); 
            if (i==2){
                var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9|0])\d{8}$/;
                if(!reg.test($("#txtUsername").attr("value")))
                {
                    $("#txtUsername").focus();
                    $("#hint_Username").html("");
                    $("#err_Username").html("请输入正确的手机号码");
                    return false;
                }
            }
            else  if (i==3){
                var reg =/^(^\d{9}$)|(^\d{16}$)/;
                if(!reg.test($("#txtUsername").attr("value"))){
                    $("#txtUsername").focus();
                    $("#hint_Username").html("");
                    $("#err_Username").html("请输入正确商旅卡号");
                    return false;
                }
            }
            else  if (i==4){
                var reg = /^[\.a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/;
                if(!reg.test($("#txtUsername").attr("value"))){
                    $("#txtUsername").focus();
                    $("#hint_Username").html("");
                    $("#err_Username").html("请输入正确的邮箱地址");
                    return false;
                }
            }
            else if (i==7){
                var reg = /^(133|153|18[0|9])\d{8}$/;
                if(!reg.test($("#txtUsername").attr("value"))){
                    $("#txtUsername").focus();
                    $("#hint_Username").html("");
                    $("#err_Username").html("请输入正确的手机号码");
                    return false;
                }
            }
           else if (i==9){

                var reg =/^0\d{2,3}-\d{7,8}$/;
                if(!reg.test($("#txtUsername").attr("value"))){
                    $("#txtUsername").focus();
                    $("#hint_Username").html("");
                    $("#err_Username").html("请输入正确的固定电话号码");
                    return false;
                }
           }
           else if (i==10){
                var reg =/^0\d{2,3}-\d{7,8}$/;
                if(!reg.test($("#txtUsername").attr("value"))){
                    $("#txtUsername").focus();
                    $("#err_Username").html("请输入正确的小灵通号码");
                    return false;
                }
           }
           else{
               selvalue();
           }
           return true;
        }

        function selvalue(){
            $("#err_Username").html("");
            $("#err_Password").html("");
            $("#err_code").html("");
            simpleSelvalue();
        }

        function simpleSelvalue()
        {
            var i=$("input[name='ddlAuthenTypeList']:checked").val();  
            if($("#txtUsername").attr("value")=="@")
            {
                $("#txtUsername").val("");
            }
          
            if(i==1){
                $("#accountname").html("用户名");
                $("#hint_Username").html("支持字母与数字组合输入");
            }
            else if(i==2){
                $("#accountname").html("手机号");
                $("#hint_Username").html("支持11位号手机码输入");
            }
            else if(i==3){
                $("#accountname").html("商旅卡");
                $("#hint_Username").html("支持9位数字商旅卡号输入");
            }
            else if(i==4){
                $("#accountname").html("邮　箱");
                if ($("#txtUsername").attr("value")==""){
                    $("#txtUsername").val("@");
                }
                $("#hint_Username").html("如：username@xxxx.xxx.xx");
            }
            else if(i==9){
                $("#err_Username").html("区号-号码  010-66668888 ");
            }else  if(i==10){
                $("#err_Username").html("区号-号码  010-12345678 ");
            }
        }


        function pload() { 
            var AuthType =$("input[name='ddlAuthenTypeList']:checked").val(); 
            simpleSelvalue();
            if (AuthType==5||AuthType==7||AuthType==9||AuthType==10||AuthType==11){
                $("#plable").show();
                $("#proInfoList").show();
            }

        }
    </script>

    <style>
/*reset*/
html{color:#000;background:#fff;font-size:12px;font-family:Arial, Helvetica, sans-serif;}
body,div,dl,dt,dd,ul,ol,li,h1,h2,h3,h4,h5,h6,pre,code,form,fieldset,legend,input,button,textarea,p,blockquote,th,td{margin:0;padding:0}
table{border-collapse:collapse;border-spacing:0}
fieldset,img{border:0}
address,caption,cite,code,dfn,em,strong,th,var{font-style:normal;font-weight:normal}
li{list-style:none}
caption,th{text-align:left}
h1,h2,h3,h4,h5,h6{font-weight:normal;font-size:100%}
q:before,q:after{content:''}
abbr,acronym{border:0;font-variant:normal}
sup{vertical-align:text-top}
sub{vertical-align:text-bottom}
input,button,textarea,select{font-family:inherit;font-size:inherit;font-weight:inherit}
input,button,textarea,select{*font-size:100%}
legend{color:#000}
/*hack*/
.clfx:after{display:block;clear:both;visibility:hidden;height:0;font-size:0px;line-height:0px;content:"."}
.clfx{zoom:1;_height:1%}
.l{float:left}
.r{float:right}
.but{display:block;border:none;text-align:center;cursor:pointer}
.but:hover{text-decoration:none!important}
.noborder{border:none!important;background:none!important}
.nobg{background:none!important}
.mr0{margin-right:0!important}
.m0{margin:0!important}
.mt10{margin-top:10px}
.ml10{margin-left:10px}
.rimg{border:1px solid #c6c6c6;padding:2px;background:#fff}

/*.sinput input{width:10px;height:10px;margin-left:0!important;height:12px;width:12px!important;vertical-align:text-top}
.sinput label{font-size:12px!important}
*/

.stable {float:left;}
.stable input{margin-left:8px;margin-top:6px;_margin-top:10px;float:left;width:15px!important;height:15px;vertical-align:text-top}
.stable label{font-size:12px!important}

.sinputd {border:1px solid #999;float:left}
.sinputb {width:88px!important}
.texti {border:1px solid #999}

.remark{color:red!important}
.hint{color:#737373}



.more a,.more a:visited{float:right;margin-top:-20px;color:#001d75}
.top_infoA,.top_info,.head,.menu,.menuC,.head_con,.content,.foot{width:980px;margin:0px auto}
.boxA{border:1px solid #d8d8d8}

.listC li{float:left}
.listC img{float:left;margin-right:9px}
.listD img{float:left;margin-right:6px}
.listD h6,.listD h6 a,.listD h6 a:visited{float:left;font-weight:700;color:#666;margin-bottom:6px;line-height:18px}
.listD {color:#666;line-height:18px;background-color:#fff}
.listD .TRS_Editor{display:inline}
.listD span,.listD a,.listD a:visited{color:#f60}
.head,.content,.foot,.nav_tab clfx{background-color:#fff}
/*头部*/
.menuB{padding-left:23px}
.menuB a{padding:0 13px;cursor:pointer}
.menuB a.vazn{font-weight:700}
.head .menuB{background:url(images/head_bg2.gif) 0 -28px repeat-x;height:30px;line-height:30px}
.menuC{color:#999}
.menuC a,.menuC a:visited{color:#0067cb; text-decoration:none}
.menuC a:hover{ text-decoration:underline}

.top_info{background:#f5f5f5 url(images/icon_8.gif) 7px 6px no-repeat;color:#7c7c7c}
.top_info p{padding:5px 0 5px 25px;}
.top_info p em{color:#ff6600}
.top_info .close a,.top_info .close a:visited{float:right;margin-right:10px;margin-top:-20px;padding-top:2px;padding-

right:20px;color:#7c7c7c; background:url(images/close.gif) 52px 0 no-repeat}

.top_infoA{background:#f5f5f5;height:25px;line-height:25px}
.top_infoA a,.top_infoA a:visited{color:#666;padding:0 7px; text-decoration:none}
.top_infoA .l a{float:left;height:16px;line-height:16px;margin-top:4px;background:url(images/channel_bg1.gif) 0 -409px no-repeat}
.top_infoA a.vazn{background:#FFF;border:1px solid #E0E0E0;border-bottom:none;height:23px;line-height:21px;margin-top:1px; }
.top_infoA a.vazn:hover{text-decoration:none}
.top_infoA a:hover{ text-decoration:underline}
.top_infoA a.nobg{margin-left:5px}
.top_infoA .r{background:#fff url(images/conm_bg2.gif);height:18px;line-height:18px;margin-top:4px;margin-bottom:3px;margin-right:3px}
.top_infoA .r a{border:none!important;}
.top_infoA .more { background:url(images/head_icon1.gif) 2px -79px no-repeat;padding:0 3px!important}
.top_infoA a.more:hover{ text-decoration:none}
.top_infoA .down{padding-right:9px;background:url(images/icon_15.gif) 52px 6px no-repeat}

.head{margin:0 auto 10px;color:#666}
.head a,.head a:visited {color:#666;text-decoration:none}
.head a:hover{text-decoration:underline}
.head_con{margin-bottom:8px}
.head_con img{display:block}
.head_con h1.fu{float:right;margin:20px 3px 10px 10px}

.head_con .r{margin-top:19px}
.head_con .r a{float:right}

.channel_name{float:left;margin-top:14px;padding-left:63px;height:49px;line-height:35px;color:#c7c7c7;font-size:22px;font-family:微软雅黑,黑体;}
.travel_book .channel_name{ background:url(images/channel_bg1.gif) no-repeat}
.travel_book .menuB span{padding:0 13px; background:url(images/channel_bg1.gif) 0 -406px no-repeat}
.travel_book .menuB span em{font-weight:700;color:#00a1ec;font-size:14px}
.travel_play .channel_name{ background:url(images/channel_bg1.gif) 0 -53px no-repeat}
.head .channel_name{ background:url(images/channel_bg1.gif) no-repeat;}
.info .channel_name{ background-position: 0 -363px;padding-left:20px}
.logo{display:inline;float:left;height:47px;margin:6px 20px 7px 3px}
.city{float:left;margin-top:12px;margin-right:19px;padding-left:15px;line-height:24px}
.city .current_city{font-family:微软雅黑,黑体;letter-spacing:7px;font-size:20px;font-weight:700;color:#000}
.city .changer_city{cursor:pointer;color:#7c7c7c;text-decoration:underline}
/*底部*/
.nav_tab{width:965px;margin:0 auto;margin-bottom:20px!important;padding:7px 0 7px 15px;background-color:#f5f5f5;color:#666;clear:both}
.nav_tab a,.nav_tab a:visited{text-decoration:none;color:#666}
.nav_tab a:hover{ text-decoration:underline}
.bot_nav,.bot_nav a,.bot_nav a:visited{color:#6699cc!important}
.nav_tab .nece {background:url(images/icon.gif) no-repeat scroll 10px 0 transparent;padding-left:30px;color:#666}
.copyright a.r,.copyright a.r:visited{text-decoration:underline;margin-right:10px;*margin-top:-15px}
.copyright{margin-top:5px; font-family:Arial}
.login{width:972px;padding:2px;border:2px solid #fa8408;height:370px}
.login .listC{width:608px;_height:57px}
.login .listC li{width:304px;height:29px;line-height:29px;text-align:center;font-size:14px;color:#c00;font-weight:700;cursor:pointer}
.login .listC li#tabD1{ background:url(images/login_tab1.gif) no-repeat;color:#666}
.login .listC li#tabD2{ background:url(images/login_tab2.gif) no-repeat;color:#666}
.login .listC li#tabD3{ background:url(images/login_tab3.gif) no-repeat;color:#666}
.login .listC li#tabD1.vazn{ background-color:#fff; background:none;color:#c00}
.login .listC li#tabD2.vazn{ background-color:#fff; background:none;color:#c00}
.login .listC li#tabD3.vazn{ background-color:#fff; background:none;color:#c00}
.login .listD{_margin-top: -29px}
.login .listD .con_l{float:left;margin:30px 0 auto 30px;width:525px;_width:510px}
.login .listD .con_l li{height:36px;line-height:36px}
.login .listD .con_l li span{color:#afafaf}
.login .listD .con_l li em{color:red}
.login .listD .con_l li a,.login .listD .con_l li a:visited{color:#618ade!important}
.login .listD .con_l li{clear:both}
.login .listD .con_l label{font-size:14px}
.login .listD .con_l label span{color:#666!important}



.login .listD .con_l input{padding:3px;width:230px;margin-right:10px}
.login .listD .con_l .ml0{margin-left:0!important}
.login .listD .con_l .sinput{margin-left:30px}
.login .listD .con_l span a.ml10,.login .listD .con_l span a.ml10:visited{color:#c00!important;font-weight:700;font-size:14px}
.login .listD .con_r{padding:10px 15px;display:inline;float:right;width:338px;background:url(images/login_bg1.gif) repeat-x;margin-top:-29px;_margin-top:0;height:350px;border-left:1px solid #cecece}
.login .listD .con_r h4{color:#039;font-size:14px;font-weight:700;margin-top:15px;margin-bottom:30px}
.login .listD .con_r h4 a,.login .listD .con_r h4 a:visited{color:#c00!important;font-weight:200}
.login .listD .con_r h5{color:#666;font-weight:700}
.login .listD .con_r li{padding:10px 0;border-bottom:1px dotted #999;background:url(images/bluedian.gif) no-repeat scroll 0 -24px transparent;;margin-bottom: 10px;padding: 0 0 14px 10px}
.login .listD .con_r .no{color:#c00;border:none}
.login .listD .con_r span.r a,.login .listD .con_r span.r a:visited{ text-decoration:none}
#conD2 .r{margin-top:78px}
#conD3 .r{margin-top:78px}
.nav_tab{margin-top:10px}
</style>
</head>

<body onload="pload();">
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
    <div class="head info">
        <div class="head_con clfx">
            <h1 class="logo">
               
                    <img src="images/besttonelogo.gif" width="153" height="59" alt="号码百事通" title="号码百事通" /></a></h1>
            <div class="channel_name">
                号码百事通用户登录</div>
            <div class="r" style="display: none;"><a href=""><img style="margin-left:10px" src="images/login_but3.gif" /></a><a href=""><img src="images/login_but2.gif" /></a></div>
        </div>
    </div>
    <form id="form1" runat="server" onsubmit="return checkinput($('#ddlAuthenTypeList').attr('value'));">
        <div class="content login clfx">
            <ul class="listC clfx">
                <li id="tabD1" onclick="tabconD(1);" class="vazn">个人登录</li>
                <li id="tabD3" onclick="tabconD(3);">天翼账号登录</li>

               
            </ul>
            <div id="conD1" class="listD clfx">
                <ul class="con_l">
                    <li>
                        <label class="l">
                            <em>*</em> 帐号类型：</label>
                        <asp:RadioButtonList ID="ddlAuthenTypeList" runat="server" CssClass="stable" RepeatDirection="Horizontal">
                        </asp:RadioButtonList></li>
                    <li>
                        <label for="username">
                            <span id="accountname" runat="server">用户名</span><span>：</span></label>
                        <asp:TextBox ID="txtUsername" CssClass="texti" runat="server" onMouseOver="return  selvalue();"
                            onchange="return checkBigUserName();"></asp:TextBox>
                        <span id="err_Username" class="remark" runat="server"></span>
                        <span id="hint_Username"
                            class="hint" runat="server">支持字母与数字组合输入</span>
                    <li>
                        <label for="password">
                            密　码：</label>
                        <asp:TextBox ID="txtPassword" CssClass="texti" runat="server" TextMode="Password"
                            onchange="return checkPassword();"></asp:TextBox>
                        <span id="err_Password" class="remark" runat="server"></span><span class="forget"><a
                            href="../FindPwdSelect.aspx" target="_blank">忘记密码</a></span></li>
                    <li>
                        <label style="margin-right: 3px; _margin-right: 6px" for="code" class="l">
                            验证码：</label>
                        <input type="text" value="" id="code" name="code" class="sinputd" onchange="return checkCode();" />
                        <span class="code">

                            <script language="javascript" type="text/javascript">
				            document.write("<img id='IMG2' src='../ValidateToken.aspx' width='62' height='22'>");
				            function RefreshCode()
				            {
					            document.all.IMG2.src = '../ValidateToken.aspx?.tmp='+Math.random();
				            }
                            </script>

                            <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span> </span>
                    </li>
                    <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span id="err_code" class="remark" runat="server"></span></li>
                    <li style="margin-left: 57px; margin-top: 0px">
                        <asp:ImageButton ID="btnlogin" CssClass="sinputb" OnClick="btnlogin_Click" OnClientClick="return checkinput();"
                            ImageUrl="images/login_but1.gif" runat="server" />
                        <span><a class="ml10" href="javascript:window.history.go($('#backCount').attr('value'));">
                            返回>></a></span></li>
                </ul>
                <input type="text" value="0" id="backCount" runat="server" name="code" class="sinputd"
                    style="display: none;" />
                <div class="con_r" style="_width:334px">
                    <h4>
                        如果您是号码百事通的新朋友请<a href="../signup.aspx?SPID=35000000&ReturnUrl=http://www.118114.cn"
                            runat="Server" id="linkU1">免费注册</a></h4>
                    <ul>
                        <li>用户只需注册一次，即可拥有用户名、手机号码、商旅卡 号等多种登录模式，并可使用于移动号百、商旅平台、号 码百事通生活频道。</li>
                        <li>用户可使用点评、发布信息、留言、纠错等多种互动功能。</li>
                        <li>用户可在网站上找朋友，建立自己的个性化家园。</li>
                        <li>用户可自主选择是否愿意接受推广及活动参与短信。</li>
                        <li class="no">用户登录成功后，页面会自动返回到首页。单点登录，全网站适用。</li>
                    </ul>
                    <span class="r"><a href="../signup.aspx?SPID=35000000&ReturnUrl=http://www.118114.cn"
                        runat="Server" id="linkU2">就是这么简单 还不火速加入！</a></span>
                </div>
            </div>
            <div id="conD2" class="listD clfx" style="display: none">
                <iframe style="margin-left: 20px; width: 515px; height: 240px" class="con_l" border="0"
                    frameborder="no" src="http://m.118114.cn/login_iframe.jsp"></iframe>
                <div class="con_r" style="*margin-top:-29px;_margin-top:0;_width:334px">
                    <h4>
                        如果您是企业客户，请点击这里<a href="">申请加盟</a></h4>
                    <h5>
                        加盟优势：</h5>
                    <ul>
                        <li>一点发布，语音、互联网、手机、黄页多载体同步展现</li>
                        <li>通过网络自主管理、更新快、准确度高</li>
                        <li>用户覆盖广，涵盖不同载体不同层次用户群</li>
                        <li>庞大的支撑团队，运营机构遍布全国各地</li>
                    </ul>
                    <span class="r"><a href="">就是这么简单 还不火速加入！</a></span>
                </div> 
            </div>
            <div id="conD3" class="listD clfx" style="display:none">
               <iframe name="ifrm_udb" src="http://zx.passport.189.cn/Logon/S/PassportLogin.aspx?PassportLoginRequest=<%=PassportLoginRequestValue %>"   width="100%" height="278"   scrolling="no" class="con_l"  frameborder="0">
	           </iframe>   
                <div class="con_r" style="*margin-top:-29px;_margin-top:0;_width:334px">
                    <h4>
                        如果您还不是号百客户，用天翼账号登录后，系统将自动为您注册为号百客户
                    </h4>
                    <h5>
                        号百客户优势：
                     </h5>
                    <ul>
                        <li>机票、酒店、订餐、精品商城购物，为您带来衣食住行的方便</li>
                        <li>庞大的支撑团队，运营机构遍布全国各地</li>
                        <li></li>
                        <li></li>
                    </ul>
                    <span class="r"></span>
                </div> 	           
	           
             </div>
            
        </div>
        
        
        
        
 	<div id="content_wrap" class="row" visible="false">
		<div id="content_left_wrap">
			<div class="panel">
				
				<div class="box">
				<asp:HyperLink id="authUrl" runat="server">
				  <img src="../images/SinaOAuth/240.png" alt="点击此处进行授权"/>
			    </asp:HyperLink>
				</div>
				<div class="box">
				<asp:HyperLink id="qqUrl" runat="server">
				  <img  src="../images/QQOAuth/Connect_logo_7.png"  alt="点击此处进行授权"/>
			    </asp:HyperLink>
				</div>			
				
				
			</div>

		</div>

	</div>       
        
    </form>
   
				
    <link href=" http://static.118114.cn/css/besttone_footer/besttone_bottom_css.css" type="text/css" rel="stylesheet" media="screen" /><script src=" http://static.118114.cn/js/besttone_footer/besttone_bottom.js" type="text/javascript"></script>
    <script src="http://w.cnzz.com/c.php?id=30042678" language="javascript" charset="gb2312"></script>
</body>
</html>
