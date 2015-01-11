<%@ page language="C#" autoeventwireup="true" inherits="CustInfoManager_M_AuthenEmailM, App_Web_authenemailm.aspx.7caca394" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="css2014/global.css" type="text/css" rel="stylesheet"/>
    <link href="css2014/lihongtu.css" type="text/css" rel="stylesheet"/>
    <title>邮箱验证</title>
   
   <script type="text/javascript" src="../../JS/jquery-1.4.2.min.js"></script>
    
   <script type="text/javascript">
        
 
        
        $(document).ready(function(){
           
        })
  
  
        function CheckEmailFormat(){
             
                if($("#Email").val() != "")
                {
                            var regMail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
                            if(!regMail.test($("#Email").val()))
                            {
                                showError("邮箱格式不合法!");
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                 }else
                 {
                        showError("邮箱不能为空!");
                        return false;
                 } 
        }
  
  
  
        function SendEmail(){

            if(CheckEmailFormat()){
            
                    $("#Hidden_Email").val($("#Email").val());
               
                    $.ajax({
                        type:"post",
                        url:"../../Ajax/Email/setEmailV2_ajax.aspx",
                        dataType:"JSON",
                        data:{custid:$("#HiddenField_CUSTID").attr("value"),email:$("#Email").attr("value"),returnUrl:$("#HiddenField_URL").attr("value"),SPID:$("#HiddenField_SPID").attr("value"),typeId:1,Now:Math.random()},
                        beforeSend: function(XMLHttpRequest) {
                            showWaiting('img2014/5-130H2191536-50.gif');
                        },
                        complete: function(XMLHttpRequest, textStatus) {
                            
                        },
                        success:function(data,textStatus){
                           clearWaiting(); 
                            var dataJson = eval(""+data+"");
                            $.each(dataJson,function(index,item){
                                if(item["result"]=="true"){
                                      showError("系统已经向您的邮箱发送了一份邮件！");  // 这里要先清除loading....
                                }else{
                                     showError(item["info"]);
                                }
                            })
                        },
                        error:function(){
                            showError("发送失败！");
                        }
                    })
                
                
            }
        }

   
      function clearWaiting(){
             $("#J-mask").remove();
		     $("#J-error").remove();
      }
       
       
       function showWaiting(){
               var html="<div style='width:100%;position:absolute;top:0;left:0;z-index:9999;height:"+$(window).height()+"px' id='J-mask'></div>";
                html+="<div id='J-error' style='width:200px;padding:0 10px;line-height:40px;color:#fff;text-align:center;position:absolute;top:"+($(window).scrollTop()+$(window).height()/2-20)+"px;left:50%;margin-left:-110px;;z-index:10000;-moz-border-radius:5px;-webkit-border-radius:5px;border-radius:5px'>";
                html+="<img src='img2014/5-130H2191536-50.gif'/></div>";
                $("body").append(html);
        }


  
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
  
   </script> 
    
</head>
<body>
    <form id="form1" runat="server">
    
            <div class="top">
              <a href="javascript:history.go(-1);" class="back"></a>
              <p>邮箱验证</p>
            </div>
            <div class="wrap" style=" padding:0 5px; background:#fafafa;">
              <div>
                <ul>
                  <li>
                    <div class="file">您的邮箱：</div>
                    <div class="input">
                      <input name="Email" type="text" id="Email"/>
                    </div>
                  </li>
                </ul>
              </div>
              <div style=" margin:30px 0; text-align:center">
                    <a href="#" id="SendMailButton"  onclick="javascript:SendEmail();"  class="bt_bo">提 交</a>
               </div>
              <div style=" text-align:center; font-size:12px; color:#808080;">完成验证后您可以通过该邮箱重置密码</div>
            </div>
           
                <asp:HiddenField ID="HiddenField_SPID" runat="server" />
                <asp:HiddenField ID="HiddenField_CUSTID" runat="server" />
                <asp:HiddenField ID="HiddenField_URL" runat="server" />
                <input id="Hidden_Email" type="hidden" value="" />          
   
    </form>
</body>
</html>
