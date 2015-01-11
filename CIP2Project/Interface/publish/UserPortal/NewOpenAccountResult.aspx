<%@ page language="C#" autoeventwireup="true" inherits="NewOpenAccountResult, App_Web_newopenaccountresult.aspx.cdcab7d2" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>开户结果</title>
    <link rel="stylesheet" type="text/css" href="http://static.118114.cn/css/base.css" /> 


    <script language="javascript" type="text/javascript">
 
        function countDown(secs){
            tiao.innerText=secs;
            if(--secs>0){
                setTimeout("countDown("+secs+")",1000);
            }
            else{
                var ReturnUrl=document.getElementById("txtHid").value ;
                if(ReturnUrl =='close'){
                    top.close();
                }
                else{
                    location.href=ReturnUrl;
                }    
            }
        }
        
    </script>    
    
    
    
</head>
<body>




<asp:Panel ID="successpage" runat="server">
    <div style="position:absolute;left:50%;top:25%;margin-left:-270px">
    <div style="width:390px;padding:30px 10px 30px 140px;margin:50px auto;background:url(images/bg_haveAccount.png) no-repeat 0 50%">
	    <h3 style="margin:10px auto;font:bold 18px Microsoft Yahei;color:#039">恭喜！<span>您已成功开通号码百事通账户。</span></h3>
        <a href="#" style="background:url(images/bg_wait.gif) no-repeat 0 50%;display:block;height:32px;padding-left:35px;color:#39c;font:bold 14px/32px Microsoft Yahei"><span id="tiao">3</span>秒后自动跳转<script language="javascript">countDown(3);</script>，也可直接跳转</a>
        <input type="text" id="txtHid" runat="Server" style="display: none" />

    </div>
    </div>
</asp:Panel>

<!--以下是开通失败-->
<asp:Panel ID="failedpage" runat="server">
<div style="width:400px;height:150px;position:absolute;border:5px solid #39c;border-radius:5px">
	<h3 style="padding-left:5px;height:30px;color:#fff;font:14px/30px Microsoft Yahei;background:#2d94c8">开通号码百事通账户</h3>
	<p style="font:14px Microsoft Yahei;margin:30px auto 10px;text-align:center">抱歉，由于系统原因开通号码百事通账户失败，请稍后再试。</p>
    <a href="#" style="display:block;margin:0 auto;width:124px"><img src="images/retry.gif" height="26" width="124" alt=""></a>
</div>
</asp:Panel>
 


    
</body>
</html>
