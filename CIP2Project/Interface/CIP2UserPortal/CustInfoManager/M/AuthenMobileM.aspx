<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuthenMobileM.aspx.cs" Inherits="CustInfoManager_M_AuthenMobileM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>手机验证</title>
    <link href="css2014/global.css" type="text/css" rel="stylesheet"/>
    <link href="css2014/lihongtu.css" type="text/css" rel="stylesheet"/> 
    <script type="text/javascript" src="../../JS/jquery-1.4.2.min.js"></script> 
    
   <script type="text/jscript">

     function showError(str,callback){
            var html="<div style='width:100%;position:absolute;top:0;left:0;z-index:9999;height:"+$(window).height()+"px' id='J-mask'></div>";
                  html+="<div id='J-error' style='width:200px;padding:0 10px;line-height:40px;background:rgba(0,0,0,0.7);color:#fff;text-align:center;position:absolute;top:"+($(window).scrollTop()+$(window).height()/2-20)+"px;left:50%;margin-left:-110px;;z-index:10000;-moz-border-radius:5px;-webkit-border-radius:5px;border-radius:5px'>";
                  html+="<p>"+str+"</p></div>";
                  $("body").append(html);
                  setTimeout(function(){
	              $("#J-mask").remove();
	              $("#J-error").remove();
	              if(callback){
		                callback();
	              }
                },2000)
     }
  
  function selmobile(s,t){
    $("#num").attr("value",parseInt(s)+t);
    var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;
    reg = /^1([3][0-9]|[5][0123456789]|[8][0123456789])\d{8}$/;	
    if ($("#Phone").attr("value")==null){
        showError('请输入手机号');
       return;
    }
    else if (!reg.test($("#Phone").attr("value"))){
       showError('号码输入有误'); 
        return;    
    }
    else{
        if ($("#HiddenField_SPID").attr("value")==null){
            spidvalue="";
        }
        else{
            spidvalue=$("#HiddenField_SPID").attr("value");
        }
         $.get("../../Ajax/Mobile/setMobile_ajax.aspx", {
               custid:$("#HiddenField_CUSTID").attr("value"),
               mnum:$("#Phone").attr("value"),
               count:$("#num").attr("value"),
               spid:spidvalue, 
               typeId: 1,
               Now: Math.random()
            }, mobileResult);
    }
  
}

function mobileResult(k)
{
   if(k==0)
   {   
        showError('信息已发送，请注意查收(信息的有效时间为两分钟,请在两分钟内操作!)'); 
         return;
   }
   else
   {
         showError(k); 
     
   }
}

  
  
  
  
      function RefreshCode(){
            document.all.IMG2.src = '../../ValidateToken.aspx?.tmp='+Math.random();
      }
        
   </script> 
   
    
</head>
<body>
    <form id="form1" runat="server">
            <div class="top">
              <a href="javascript:history.go(-1);" class="back"></a>
              <p>通过手机验证</p>
            </div>
            <div class="wrap" style=" padding:0 5px; background:#fafafa;">
              <div>
                <ul>
                  <li>
                    <div class="file">登录密码：</div>
                    <div class="input">
                      <input type="password" name="LoginPassword" id="LoginPassword" />
                    </div>
                  </li>
                  <li>
                    <div class="file">认证手机号：</div>
                    <div class="input">
                      <input type="text" name="Phone" id="Phone" />
                    </div>
                  </li>
                  <li>
                    <div class="file">手机验证码：</div>
                    <div class="input">
              <div style="float:left;">
                <input type="text" name="AuthenCode" id="AuthenCode" style=" width:100px;"/>
              </div>
                <a href="#"  onclick="selmobile($('#num').attr('value'),1);" style=" display:block; background:#3a7bed; border:1px solid #0e55d2; height:28px; line-height:28px; float:left; margin:5px 0 0 5px; text-align:center; color:#FFF; padding:0 5px;">获取验证码</a></div>
                  </li>
                  <li>
                    <div class="file">验证码：</div>
                    <div class="input">
                      <div style="float:left;">
                        <input type="text" name="CheckCode" id="CheckCode" style=" width:80px;" />
                      </div>
                    <a href="javascript:RefreshCode();" style=" display:block; background:#fff; border:1px solid #c8c9c9; height:28px; line-height:28px; float:left; margin:5px 0 0 5px; text-align:center; padding:0 5px;"><img width="57" height="30" src="../../ValidateToken.aspx" id="IMG2" alt=""/></a><a href="javascript:RefreshCode();" style=" float:left; color:#969696; text-decoration:underline; margin:0 0 0 5px;">看不清?</a></div>
                  </li>
                </ul> 
              </div>
              <div style=" margin:30px 0; text-align:center">
                  <asp:Button ID="SetAuthenPhoneBtn"  name="SetAuthenPhoneBtn"   CssClass="bt_bo" runat="server" Text="确 定" OnClick="SetAuthenPhoneBtn_Click" />
              </div>
            </div>
                <asp:HiddenField ID="HiddenField_SPID" runat="server" />
                <asp:HiddenField ID="HiddenField_CUSTID" runat="server" />
                <asp:HiddenField ID="HiddenField_URL" runat="server" />
                 <div id="errorHint" style="display:none" runat="server"></div>
                <input id="Hidden_Mobile" name="Hidden_Mobile"  type="hidden" value="" />       
                <input name="num" id="num"  value="0" type="hidden" />
    </form>
</body>
</html>
