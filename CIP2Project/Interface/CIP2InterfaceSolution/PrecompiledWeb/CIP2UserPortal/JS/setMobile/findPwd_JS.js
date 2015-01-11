/*********************************************************************************************************
 *     描述: 手机找回密码JS脚本
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 赵锐
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 *********************************************************************************************************/

function radio(i)
{
    $("#radiotxt").attr("value",i);
    if (i==2)
    {
        $("#tVoicePasswd").attr("checked",false);
        $("#tPasswd").attr("checked",true);
    }
    else
    {
        $("#tVoicePasswd").attr("checked",true);
        $("#tPasswd").attr("checked",false);
    }
}

  
function findpwd()
{
    var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;
    if ($("#verifyMobile").attr("value")==null)
    {
        //alert('请输入手机号码');
        $("#msgp").html('请输入手机号码');
        $("#msgp").show();
        return
    }
    else if (!reg.test($("#verifyMobile").attr("value")))
    {
         //alert('请输入正确的手机号码');
          $("#msgp").html('请输入正确的手机号码');
          $("#msgp").show();
         return;
    }
    else if ($("#radiotxt").attr("value")==null)
    {
        //alert('请选择需要找回密码的类型');
         $("#msgp").html('请选择需要找回密码的类型');
         $("#msgp").show();
        return;
    }
    else
    {
        $.get("Ajax/Mobile/findPwd_ajax.aspx",{
        type: $("#radiotxt").attr("value"),
        phone: $("#verifyMobile").attr("value"),
        ip:$("#iptxt").attr("value"),
        spid:$("#spidtxt").attr("value"),
        pageyz:$("#pageyzm").attr("value"),
        typeId:1,
        Now:Math.random()
   },result);
    }
    
     $("#btnSubmit").html("继续");
}


function result(k)
{
   if (k==0)
   {
     $("#msgp").html('已将密码发送至您的手机，请注意查收并修改');
     $("#msgp").show();
     return;
   }
   else if (k=="voicePassword.aspx")
   {
    document.location="voicePassword.aspx";
   }
   
   else
   {
    $("#msgp").html(k);
    $("#msgp").show();
   }
}

