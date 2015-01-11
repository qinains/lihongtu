<%@ page language="C#" autoeventwireup="true" inherits="SSO_PassportLogout, App_Web_2mjfkmpd" enableEventValidation="false" %>
<%@ Register Src="../UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="../UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    
    
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
    <form id="form1" runat="server">
       <center>
        <uc1:Top ID="Top1" runat="server" />
        <div>
            <div class="btContain clfx">
                <div id="main">
                    <div class="ca">
                        <h3>
                            注销成功</h3>
                    </div>
                    <div class="cb">
                        <center>
                            <div class="login_cont06">
                                <span id="tiao">3</span>秒后自动跳转<script language="javascript">countDown(3);</script>，请耐心等待…
                                <br />
                                <img src="../Images/large-loading.gif" /></div>
                        </center>

                    </div>
                </div>
            </div>

        </div>
        
        <input type="text" id="txtHid" runat="Server" style="display: none" />
        </center>
    </form>
</body>
</html>
