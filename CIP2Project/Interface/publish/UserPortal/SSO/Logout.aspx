<%@ page language="C#" autoeventwireup="true" inherits="SSO_Logout, App_Web_logout.aspx.27254924" enableEventValidation="false" %>

<%@ Register Src="../UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="../UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="../css/account.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="../FindPassWord/css/Stylecss.css" type="text/css" rel="stylesheet" media="screen" />

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
        <%--<div><span id="tiao">3</span>秒后自动跳转<script language="javascript">countDown(3);</script>
        </div>--%>
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
                        <%--<div class="subtn">
                            <span class="btn btnA"><span>
                                <button id="btnClose" onclick="javascript:window.close()">
                                    关闭此页</button></span></span></div>--%>
                    </div>
                </div>
            </div>
            <div class="btFoot" style="text-align:left">
	            <p class="miibeian">[电信及增值业务经营许可证：沪ICP备06026347号]</p>
	            <p class="btCopy">Copyright&copy; 2007-2010 中国电信集团号百信息服务有限公司 版权所有
		            <span class="nece">违法和不良信息举报<a href="mailto:service@118114.cn">service@118114.cn</a></span>
		            <span class="stat">站长统计</span>
	            </p>
            </div>
        </div>
        
        <input type="text" id="txtHid" runat="Server" style="display: none" />
        </center>
    </form>
</body>
</html>
