// JScript 文件

function findpwd()
{
     if ($("#txtUserName").attr("value")==null)
    {

     document.getElementById("err_username").innerHTML = "用户名不能为空";
     return false;
    }
    if ($("#txtAnswer").attr("value")==null)
    {
     document.getElementById("err_answer").innerHTML = "答案不能为空";
     return false;
    }

    if ($("#code").attr("value")==null)
    {
     document.getElementById("err_code").innerHTML = "验证码不能为空";
     return false;
    }
    
//    document.getElementById("err_username").innerHTML = "";
//    document.getElementById("err_answer").innerHTML = "";
//    document.getElementById("err_code").innerHTML = "";
    
    
    $.get("Ajax/paswdByQnA/paswdByQnA_ajax.aspx",{
    name :$("#txtUserName").attr("value"),
    answer :$("#txtAnswer").attr("value"),
    questionID :$("#txtQuestion").attr("value"),
    code :$("#code").attr("value"),
    typeId:1,
    Now:Math.random()
    },Result)

    
}

function resetpwd()
{
    findpwd()

    if($("#txtPwd1").attr("value")!=$("#txtPwd2").attr("value"))
    {
     document.getElementById("err_pwd").innerHTML = "密码输入不一致";     
     return false;
    }
    if ($("#txtcustid").attr("value")==null)
    {
     document.getElementById("err_pwd").innerHTML = "用户未找到";  
     return false;
    }

    $.get("Ajax/paswdByQnA/paswdByQnA_ajax.aspx",{
    custID:$("#txtcustid").attr("value"),
    custType :$("#txtCustType").attr("value"),
    name :$("#txtUserName").attr("value"),
    pwd :$("#txtPwd1").attr("value"),
    typeId:2,
    Now:Math.random()
    },Result2)
}

function Result(k)
{
   RefreshCode();
   if(k=="1")
   {
        document.getElementById("err_code").innerHTML = "验证码输入错误";
        return false; 
   }
   
    arr = "{" + k + "}"; 
    arr = eval('(' + arr + ')');
    $("#txtcustid").attr("value", arr.CustID);
    $("#txtCustType").attr("value", arr.CustType);
   
    if(arr.Result1!=-21501){
   
        document.getElementById("txtUserName").focus();
        document.getElementById("err_username").innerHTML = "用户名不存在";
        return false;  
    }
    if(arr.Result2!=0){
    
        document.getElementById("txtAnswer").focus();
        document.getElementById("err_answer").innerHTML = "答案回答错误";
        return false;       
    }
    $("#question").hide();
    $("#mmczdiv").show();

     document.getElementById("err_check").innerHTML = "答案回答正确请重置密码";  
    
}
function Result2(k)
{

    if (k==0)
    {
        document.getElementById("err_pwd").innerHTML = "密码重置成功";
        var returnUrl = $("#hdReturnUrl").val();
        window.location.href='FindPassWord/Success.aspx?ReturnUrl='+encodeURI(returnUrl);
        return false; 
    }else
    {
     document.getElementById("err_pwd").innerHTML = "密码重置失败";
       return false; 
    }

}


function selvalue(i)
{
    $("#txtQuestion").attr("value",i);
}