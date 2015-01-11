$(document).ready(function(){
    $("#txtPhone").blur(function(){
        var phone = $("#txtPhone").val();
        if(phone == ""){
            $("#phoneError_tip").css("display","block");
        }else{
            $("#phoneError_tip").css("display","none");
        }
    })
    
    //下一步按钮
    $("#btnSave").click(function(){
        var phone = $("#txtPhone").val();
        var code = $("#txtCode").val();
        var AuthenCode = $("#hdAuthenCode").val();
        if(phone == ""){
            $("#phoneError_tip").css("display","block");
        }else{
            $("#phoneError_tip").css("display","none");
        }
        if(code == ""){
            $("#codeError_msg").text("不能为空");
            $("#codeError_tip").css("display","block");
        }else{
            $("#codeError_tip").css("display","none");
            $("#codeError_msg").text("");
        }
        
        if(phone=="" || code==""){
            return false;
        }
        var returnUrl =$("#hdReturnUrl").val();
        var custid = $("#hdCustID").val();
        var phone=$("#txtPhone").val();
        //1:语音密码，2:web密码
        var pwdType = "2";          
        if($("#tPasswd").attr("checked")){
            pwdType="2";
        }else{
            pwdType="1";
        }
        var urlParam = encodeURI(custid+'$'+pwdType+'$'+returnUrl);
        var time = new Date();
        $.ajax({
            type:"post",
            dataType: "json",//返回json格式的数据
            url:"../HttpHandler/CheckPhoneCodeHandler.ashx",            
            data:{CustID:custid,PhoneNum:phone,Code:code,time:time.getTime()},
            beforeSend: function(XMLHttpRequest) {
                $("#loadImg").css("display", "block");
                $("#btnSave").css("display","none");
            },
            complete: function(XMLHttpRequest, textStatus) {
                $("#loadImg").css("display", "none");
                $("#btnSave").css("display","block");
            },
            success:function(data,testStatus){
                $.each(data,function(index,item){
                    if(item["result"]=="false"){
                        alert(item["msg"]);
                    }else{
                        window.location.href='ResetPwdByPhone.aspx?UrlParam='+urlParam;
                    }
                })
            },
            error:function(text){
                alert(text);
            }
        })
    })
})

        
        