/*********************************************************************************************************
 *     描述: 设置认证邮箱JS脚本
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 赵锐
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 *********************************************************************************************************/

function selEmail()
{
    var reg=/^[a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+$/;
    if ($("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value")==null)
    {
        $("#msgp").html("请输入密码");
        $("#msgp").show();
        return;
    }
    
    if ($("#ctl00_ContentPlaceHolder1_Emailtxt").attr("value")==null)
    {
        $("#msgp").html("请输入邮箱地址");
        $("#msgp").show();
        return;
    }
   else if (!reg.test($("#ctl00_ContentPlaceHolder1_Emailtxt").attr("value")))
    {
      $("#msgp").html("邮箱格式输入不正确");
      $("#msgp").show();
      return;
    }
   
    else
    {
       $("#yzmtr").show();
       $("#errtr").hide();
       $.get("Ajax/Email/setEmail_ajax.aspx",{
       custid:$("#ctl00_custidtxt").attr("value"),
       pwd:$("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value"),
       email:$("#ctl00_ContentPlaceHolder1_Emailtxt").attr("value"),
       SPID:$("#ctl00_spidtxt").attr("value"),
       pageyz:$("#pageyzm").attr("value"),
       typeId:1,
       Now:Math.random()
       },selResult);
    }
}

function selResult(k)
{
    if (k==0)
    {
        $("#msgp").html("邮件已发送，请查收邮件");
         $("#msgp").show();
         return;
       //$("#ctl00_ContentPlaceHolder1_urlRedirectButton").click();
        //alert('请查收邮件');
    }
    else
    {
        $("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value","");
         $("#pageyzm").attr("value","");
         $("#msgp").html(k);
         $("#msgp").show();
         $("#pageyzmbutton").click();
    }
}