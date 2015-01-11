<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_RegisterSuccessV2, App_Web_registersuccessv2.aspx.8268bb4f" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>注册成功</title>
    <link href="CSS/global.css" type="text/css" rel="stylesheet"/>
    <link href="CSS/reg.css" type="text/css" rel="stylesheet"/>  
   
     <link rel="stylesheet" href="../css/base.css" />
       <style type="text/css">
        .Yiyou_header,.content{width:980px;position:relative;margin:0 auto 10px} 
         .content{width:978px;border:1px solid #8fc4e0;background:#fff}
         span.hintCorrect{padding-left:20px;color:#390;background:url(images/spriteLoginRegister.png) no-repeat -206px 5px}
         span.hintError{padding-left:20px;color:#C30;background:url(images/spriteLoginRegister.png) no-repeat -206px -21px}
        .foot{width:980px;margin:10px auto;text-align:center;color:#78797a;line-height:23px}
        .foot a{color:#acadaf;text-decoration: none;}
        .foot a:hover,.foot a:visited{color:#acadaf;text-decoration: none;}
        </style> 
   <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script> 
    
   <script type="text/javascript">
        
        $(document).ready(function(){
            $("#mailbox").show(); 
            $("#hint_Email").html("");
            $("#msgbox").hide();
            $("#imgWait").hide(); 
        })
  
  
        function CheckEmailFormat(){
                $("#mailbox").show(); 
                $("#hint_Email").html("");
                $("#msgbox").hide();                
                $("#hint_Email").removeClass("hintCorrect").removeClass("hintError");
                if($("#Email").val() != "")
                {
                            var regMail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
                            if(!regMail.test($("#Email").val()))
                            {
                                $("#hint_Email").removeClass("hintCorrect").addClass("hintError");
                                 $("#hint_Email").html("邮箱格式不合法");
                                return false;
                            }
                            else
                            {
                                $("#hint_Email").removeClass("hintError").addClass("hintCorrect");
                                return true;
                            }
                 }else
                 {
                        $("#hint_Email").removeClass("hintCorrect").addClass("hintError");
                        $("#hint_Email").html("邮箱不能为空"); 
                        return false;
                 } 
        
        }
  
  
  
        function SendEmail(){

            if(CheckEmailFormat()){
            
                    $("#Hidden_Email").val($("#Email").val());
               
                    $.ajax({
                        type:"post",
                        url:"../Ajax/Email/setEmailV2_ajax.aspx",
                        dataType:"JSON",
                        data:{custid:$("#HiddenField_CUSTID").attr("value"),email:$("#Email").attr("value"),returnUrl:$("#HiddenField_URL").attr("value"),SPID:$("#HiddenField_SPID").attr("value"),typeId:1,Now:Math.random()},
                        beforeSend: function(XMLHttpRequest) {
                            $("#imgWait").show();
                            $("#SendMailButton").hide();
                        },
                        complete: function(XMLHttpRequest, textStatus) {
                             $("#imgWait").hide();
                        },
                        success:function(data,textStatus){
                            var dataJson = eval(""+data+"");
                            $.each(dataJson,function(index,item){
                                if(item["result"]=="true"){
                                    $("#mailbox").hide(); 
                                    $("#msgbox").show();
                                    $("#sendedmail").html("");
                                    $("#sendedmail").html($("#Hidden_Email").val());  
                                }else{
                                     $("#msgbox").hide();
                                     $("#mailbox").show(); 
                                     $("#SendMailButton").show();
                                     $("#hint_Email").removeClass("hintCorrect").addClass("hintError");
                                     $("#hint_Email").html(item["info"]);
                                }
                            })
                        },
                        error:function(){
   
                        }
                    })
                
                
            }
        }

  
        function ReSendMail(){
           $("#Email").val($("#Hidden_Email").val());          
           if(CheckEmailFormat()){
                   $.ajax({
                        type:"post",
                        url:"../Ajax/Email/setEmailV2_ajax.aspx",
                        dataType:"JSON",
                        data:{custid:$("#HiddenField_CUSTID").attr("value"),email:$("#Email").attr("value"),returnUrl:$("#HiddenField_URL").attr("value"),SPID:$("#HiddenField_SPID").attr("value"),typeId:1,Now:Math.random()},
                        beforeSend: function(XMLHttpRequest) {
                            $("#imgWait").show();
                            $("#SendMailButton").hide();
                        },
                        complete: function(XMLHttpRequest, textStatus) {
                             $("#imgWait").hide();
                        },
                        success:function(data,textStatus){

                            var dataJson = eval(""+data+"");
                            $.each(dataJson,function(index,item){
                                if(item["result"]=="true"){
                                    $("#mailbox").hide(); 
                                    $("#msgbox").show();
                                    $("#sendedmail").html("");
                                    $("#sendedmail").html($("#Hidden_Email").val());  
                                }else{
                                     $("#msgbox").hide();
                                     $("#mailbox").show(); 
                                     $("#SendMailButton").show();
                                     $("#hint_Email").removeClass("hintCorrect").addClass("hintError");
                                     $("#hint_Email").html(item["info"]);
                                }
                            })
                        },
                        error:function(){
                      
                        }
                    })
                
            }
        }
  
  
        function ModifyEmail(){
            $("#mailbox").show();
            $("#SendMailButton").show();  
            $("#imgWait").hide(); 
            $("#msgbox").hide(); 
            $("#hint_Email").removeClass("hintCorrect").removeClass("hintError"); 
        }
  
   </script> 
    
</head>
<body>
   <asp:Panel ID="PanelForYiYouHeader" runat="server">
         <div class="Yiyou_header">
            <img src="../images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="../images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
        </div>
     </asp:Panel>

     <asp:Panel ID="PanelForYiGouHeader" runat="server">
         <div class="Yiyou_header">
            <img src="../images/logo_ct.gif" width="175" height="85" alt="中国电信" /><img src="../images/logo_besttone1.gif" width="177" height="85" alt="号码百事通" />
         </div>  
     </asp:Panel>
 <div class="content">   

    <form id="form1" runat="server">
         
                  <div style="text-align: center; font-size: 40px; color: rgb(0, 82, 154); margin: 36px 0px 0px; border-top-width: 0px; padding-bottom: 0px; border-bottom-width: 0px; height: 38px;"><img src="images/icon1.gif" width="33" height="31" /> 恭喜您注册成功！</div>
                  <div style=" text-align:center; font-size:18px; margin:20px 0 0 0;">请妥善保存您的登陆密码</div>
                  <div style=" text-align:center; margin:50px 0;"><a href="<%=ReturnUrl%>"><img src="images/bt_back.gif" width="212" height="38" /></a></div>
                  <div style=" background:#f8f8f8; padding:0 0 0 207px;">
                    <div style=" margin:30px 0 0 0; font-size:18px;">为了您的账户安全，防止遗忘密码，建议您立即验证邮箱或手机！</div>
                    
                    <div id="mailbox"  style=" margin:20px 0 0 0; line-height:28px;">
                      <div style=" float:left; padding:0 5px 0 0;">您的邮箱：</div>
                      <div style=" float:left; padding:0 5px 0 0;">
                          <input type="text" name="Email" id="Email" style=" height:26px; line-height:24px; border:1px solid #707070; font-size:14px; padding:0 5px;" />
                      </div>
                      
                      <div  id="SendMailButton"><a href="#" onclick="javascript:SendEmail();"><img  src="images/bt_send.gif" width="142" height="28" /></a></div ><div  id="imgWait"><img src="images/Loading.gif" width="20" height="20"/> </div>
                      <div style="margin:20px 0 0 0; padding:0 0 0 65px;"><span id="hint_Email" style=" color:#F00;">该邮箱已被使用，请更换其他邮箱</span><span>完成验证后您可以通过该邮箱重置密码</span></div>
                    </div>
                   

                      <div id="msgbox" style="margin:20px 0 0 0;line-height:28px;">
                                <div style=" margin:20px 0 0 0; line-height:28px;">系统已向您的邮箱 <a href="#" style="color:#FF0000"><span id="sendedmail">xxx@xxx.com</span></a> 发送了一封验证邮件，请您登陆邮箱，点击邮件中的验证链接完成邮箱验证。<br />
                                    如果您长时间没有收到验证邮件，您可以邮件已重新发送<a href="#" style="color:#3366cc" onclick="javascript:ReSendMail();">重新发送</a>或<a href="#" style="color:#3366cc" onclick="javascript:ModifyEmail();">更改邮箱</a>。</div>
                                <div style=" width:594px; height:40px; background:#FFF; border:1px solid #ccc; line-height:40px; text-align: center; font-size:14px;"><img src="images/icon2.gif" width="13" height="13" /> 邮件已重新发送</div>
                      </div>
                  
                    
                    <div style=" margin:20px 0;">
                      <div style="padding:0 0 0 40px; float:left;"><a href="../SetMobile3.aspx?id=4&SPID=<%=SPID%>&SPTokenRequest=<%=newSPTokenRequest%>"><img src="images/bt_yz.gif" width="156" height="38" /></a></div>
                      <div style="padding:0 0 0 40px; float:left;"><a href="../CreateBesttoneAccount.aspx?SPTokenRequest=<%=newSPTokenRequest%>"><img src="images/bt_kt.gif" width="216" height="38" /></a></div>
                    </div>
                  </div>
           

                <asp:HiddenField ID="HiddenField_SPID" runat="server" />
                <asp:HiddenField ID="HiddenField_CUSTID" runat="server" />
                <asp:HiddenField ID="HiddenField_URL" runat="server" />
                
                <input id="Hidden_Email" type="hidden" value="" />
    </form>


</div> 


    <asp:Panel ID="PanelForYiYouFooter" runat="server">
        <script type="text/javascript" src="../JS/register_foot.js"></script>
    </asp:Panel>

    <asp:Panel ID="PanelForYiGouFooter" runat="server">
         <div class="foot">
            <p>中国电信集团：<a href="">189邮箱</a> | <a href="">天翼宽带</a> | <a href="">号百商旅</a> | <a href="">号百导航</a> | <a href="">天翼手机网</a> | <a href="">翼支付</a> | <a href="">爱音乐</a> | <a href="">天翼视讯</a> | <a href="">协同通讯</a> | <a href="">物联网</a> | <a href="">天翼空间</a> | <a href="">天翼阅读</a> | <a href="">爱游戏</a> | <a href="">爱动漫</a></p>
            违法和不良信息举报 <a href="">service@118114.cn</a>   copyright© 2007-2011 号百商旅电子商务有限公司版权所有<br />   
            增值电信业务经营许可证：沪B2-20110026　沪ICP备11017770号  上海工商标识编号：20110624111724289
        </div>  
    </asp:Panel>

</body>
</html>
