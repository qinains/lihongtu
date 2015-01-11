// JScript 文件
// type 0 - 语音页面  1 - 登录页面
    function CheckInput(type)
    {
        //alert(document.getElementById("ctl00_ContentPlaceHolder1_err_code"));
        if(document.getElementById("oldPasswd").value == "")
        {
            document.getElementById("oldPasswd").focus();
            document.getElementById("err_oldPasswd").innerHTML = "请输入原密码";
            return false;
        }
        
        if(document.getElementById("passwd").value == "")
        {
            document.getElementById("passwd").focus();
            document.getElementById("err_passwd").innerHTML = "请输入新密码";
            return false;
        }
        
        if(type == "0")
        {
            if(document.getElementById("passwd").value.length != 6)
            {
                document.getElementById("passwd").focus();
                document.getElementById("err_passwd").innerHTML = "语音密码长度为6位数字";
                return false;
            }
            
            if(isNum(document.getElementById("passwd").value))
            {
                document.getElementById("passwd").focus();
                document.getElementById("err_passwd").innerHTML = "语音密码只能是数字";
                return false;
            }
        }
        else if(type == "1")
        {
            if(document.getElementById("passwd").value.length < 6)
            {
                document.getElementById("passwd").focus();
                document.getElementById("err_passwd").innerHTML = "密码长度不能少于6位";
                return false;
            }
        
        }
              
        if(document.getElementById("verifyPasswd").value != document.getElementById("passwd").value)
        {
            document.getElementById("passwd").focus();
            document.getElementById("passwd").value = "";
            document.getElementById("verifyPasswd").value = "";
            document.getElementById("err_verifyPasswd").innerHTML = "密码不一致";
            return false;
        }
        
        if(document.getElementById("code").value == "")
        {
            document.getElementById("code").focus();
            document.getElementById("ctl00_ContentPlaceHolder1_err_code").innerHTML = "请输入验证码";
            return false;
        }
        
        return true;
        
    }
   //判断是否为数字
   function isNum(Num)
   {
        var temp = false;
        var reg = /^\d+(\.\d+)?$/;
        if(!reg.test(Num))
        {
            //document.getElementById("err_passwd").innerHTML = "语音密码只能是数字";
            temp = true;
        }
        return temp;
   }
   
   function OldPwdBlur()
   {
        var objPwd = document.getElementById("oldPasswd");
        var objErr = document.getElementById("err_oldPasswd");
        if(objPwd.value == "")
        {
            objErr.innerHTML = "请输入原密码";
        }
        else
        {
            objErr.innerHTML = "";
        }
   }
   
   function NewPwdBlur(type)
   {
        var objNewPwd = document.getElementById("passwd");
        var objErr = document.getElementById("err_passwd");
        if(type == "0")
        {
            if(objNewPwd.value == "")
            {
                objErr.innerHTML = "请输入新密码";
            }
            else if(objNewPwd.value.length != 6)
            {
                objErr.innerHTML = "语音密码长度为6位数字";
            }
            else if(isNum(objNewPwd.value))
            {
                objErr.innerHTML = "语音密码只能是数字";
            }
            else
            {
                objErr.innerHTML = "";
            }
        }
        else if(type == "1")
        {
            if(objNewPwd.value == "")
            {
                objErr.innerHTML = "请输入新密码";
            }
            else if(objNewPwd.value.length < 6)
            {
                objErr.innerHTML = "密码长度不能少于6位";
            }
            else
            {
                objErr.innerHTML = "";
            }
        }
   }
   
   function VerifyPwdBlur()
   {
        var objNewPwd = document.getElementById("passwd");
        var objVerifyPwd = document.getElementById("verifyPasswd");
        var objErr = document.getElementById("err_verifyPasswd");
        if(objVerifyPwd.value != objNewPwd.value)
        {
            objNewPwd.value = "";
            objVerifyPwd.value = "";
            objErr.innerHTML = "密码不一致"
        }
        else
        {
            objErr.innerHTML = ""
        }
   }
   
   function CodeBlur()
   {
        var objCode = document.getElementById("code");
        var objErr = document.getElementById("ctl00_ContentPlaceHolder1_err_code");
        if(objCode.value == "")
        {
            objErr.innerHTML = "请输入验证码"
        }
        else
        {
            objErr.innerHTML = ""
        }
   }
   
   function clear1()
   {
   //alert();
        document.getElementById("oldPasswd").value = "";
        document.getElementById("passwd").value = "";
        document.getElementById("verifyPasswd").value = "";
        document.getElementById("code").value = "";
        
        document.getElementById("err_oldPasswd").innerHTML = "";
        document.getElementById("err_passwd").innerHTML = "";
        document.getElementById("err_verifyPasswd").innerHTML = "";
        document.getElementById("ctl00_ContentPlaceHolder1_err_code").innerHTML = "";
        document.getElementById("ctl00_ContentPlaceHolder1_error").innerHTML = "";
        
   }