/*********************************************************************************************************
 *     描述: 设置认证手机JS脚本
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 赵锐
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 *********************************************************************************************************/

function selmobile(s,t){
    $("#num").attr("value",parseInt(s)+t);
    var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;

    if ($("#ctl00_ContentPlaceHolder1_verifyMobile").attr("value")==null){
       $("#msgp").html('请输入手机号码');
       $("#msgp").show();
       return;
    }
    else if (!reg.test($("#ctl00_ContentPlaceHolder1_verifyMobile").attr("value"))){
       $("#msgp").html('号码输入有误');
       $("#msgp").show();
        return;    
    }
    else{
        if ($("#ctl00_spidtxt").attr("value")==null){
            spidvalue="";
        }
        else{
            spidvalue=$("#ctl00_spidtxt").attr("value");
        }
         $.get("Ajax/Mobile/setMobile_ajax.aspx", {
               custid:$("#ctl00_custidtxt").attr("value"),
               mnum:$("#ctl00_ContentPlaceHolder1_verifyMobile").attr("value"),
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
        $("#msgp").html('信息已发送，请注意查收(信息的有效时间为两分钟,请在两分钟内操作!)');
        $("#msgp").show();
         return;
   }
   else
   {
         $("#msgp").html(k);
         $("#msgp").show();
         $("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value","");
         $("#tPasswd").attr("value","");
         $("#pageyzm").attr("value","");
         $("#pageyzmbutton").click();
   }
}


function setmobile()
{   
    $("#msgp").hide();
    if ($("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value")==null)
    {
       $("#msgp").html('请输入密码');
       $("#msgp").show();
       return;
    }
    var reg = /^(13[0-9]|15[0|3|6|7|8|9]|18[8|9])\d{8}$/;
    if ($("#ctl00_ContentPlaceHolder1_verifyMobile").attr("value")==null)
    {
       $("#msgp").html('请输入手机号码');
        $("#msgp").show();
       return;
    }
    else if (!reg.test($("#ctl00_ContentPlaceHolder1_verifyMobile").attr("value")))
    {
        $("#msgp").html('号码输入有误');
        $("#msgp").show();
        return;    
    }
    else if ($("#tPasswd").attr("value")==null)
    {
        $("#msgp").html('请输入验证码');
        $("#msgp").show();
        return;
    }
    else
    {
        if ($("#ctl00_spidtxt").attr("value")==null)
        {
            spidvalue="";
        }
        else
        {
            spidvalue=$("#ctl00_spidtxt").attr("value");
        }
        $.get("Ajax/Mobile/setMobile_ajax.aspx", {
               pwd:$("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value"),
               custid:$("#ctl00_custidtxt").attr("value"),
               mnum:$("#ctl00_ContentPlaceHolder1_verifyMobile").attr("value"),    
               auth:$("#tPasswd").attr("value"),
               spid:spidvalue,
               pageyz:$("#pageyzm").attr("value"),
               typeId: 2,
               Now: Math.random()
            }, Result);
    }
}

function Result(k)
{
    if (k==0)
    {
        $("#ctl00_ContentPlaceHolder1_urlRedirectButton").click();
        return;
    }
    else
    {
    $("#msgp").html(k);
    $("#msgp").show();
    pageyz:$("#pageyzm").attr("value","")
    $("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value","");
         $("#tPasswd").attr("value","");
         $("#pageyzm").attr("value","");
         $("#pageyzmbutton").click();
    }
}


function del(i)
{
  if (confirm('您是否要删除这个号码?')==true)
  {
  var Id=i.substr(39,2);
  phoneId="ctl00_ContentPlaceHolder1_Repeater1_ctl"+Id+"_phoneIdTxt";
  phoneNum="ctl00_ContentPlaceHolder1_Repeater1_ctl"+Id+"_phoneTxt";
  phoneClass="ctl00_ContentPlaceHolder1_Repeater1_ctl"+Id+"_typeTxt";
  var pId=document.getElementById(phoneId);
  var pNum=document.getElementById(phoneNum);
  var pClass=document.getElementById(phoneClass);
  if (pId.value!=null && pNum.value!=null && pClass.value!=null)
  {
    $.get("Ajax/Mobile/setMobile_ajax.aspx", {
               a: pId.value,
               b: pNum.value,
               c: pClass.value,
               typeId: 3,
               Now: Math.random()
            }, ResultDel);
  }
  }
}

function ResultDel(k)
{
    if (k==0)
    {
       document.location=document.location;
    }
    else
    {
       $("#msgp").html(k);
       $("#msgp").show();
       $("#ctl00_ContentPlaceHolder1_pwdTxt").attr("value","");
         $("#tPasswd").attr("value","");
         $("#pageyzm").attr("value","");
         $("#pageyzmbutton").click();
    }
}


function tiaozhuan()
{
    document.location='SetMobile.aspx??id=4&SPID='+$("#ctl00_spidtxt").attr("value");
}


function renzheng(i)
{
  if (confirm('您是否要认证这个号码?')==true)
  {
      var Id=i.substr(39,2);
      phoneNum="ctl00_ContentPlaceHolder1_Repeater1_ctl"+Id+"_phoneTxt";
      var pNum=document.getElementById(phoneNum);
      document.location="SetMobile.aspx?id=4&SPID=35000000&Phone="+pNum.value;
  }
}

