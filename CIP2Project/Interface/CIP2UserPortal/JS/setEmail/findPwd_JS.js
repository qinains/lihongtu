/*********************************************************************************************************
 *     描述: 邮箱找回密码JS脚本
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
 $("#msgp").html("");
    var reg=/^[a-zA-Z0-9_\.]+@[a-zA-Z0-9-]+[\.a-zA-Z]+$/;
    if ($("#Emailtxt").attr("value")==null)
    {
        $("#msgp").html("请输入邮箱地址");
        return;
    }
    else if (!reg.test($("#Emailtxt").attr("value")))
    {
      $("#msgp").html("邮箱格式输入不正确");
      return;
    }
 $.get("Ajax/Email/findPwd_ajax.aspx",{
 name:$("#nametxt").attr("value"),
 email:$("#Emailtxt").attr("value"),
 pageyz:$("#pageyzm").attr("value"),
 typeId:1,
 Now:Math.random()
 },resultsel)
}

function resultsel(k)
{   
    if (k==0)
    {
      $("#msgp").html('密码已发送至您的邮箱，请注意查收并及时修改');
      $("#msgp").show();
      // $("#urlRedirectButton").click();
    }
    else
    {
        $("#msgp").html(k);
        $("#msgp").show();
        $("#pageyzmbutton").click();
        $("#nametxt").attr("value","");
        $("#Emailtxt").attr("value","");
        $("#pageyzm").attr("value","");
    }
}