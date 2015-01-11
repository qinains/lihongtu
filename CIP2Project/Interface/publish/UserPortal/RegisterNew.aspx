<%@ page language="C#" autoeventwireup="true" enableviewstate="true" inherits="RegisterNew, App_Web_registernew.aspx.cdcab7d2" enableEventValidation="false" %>

<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>号码百事通客户信息平台</title>
    <meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号 `码百事通,114,118114" />
    <meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." />
    <link href="css/account.css" type="text/css" rel="stylesheet" media="screen" />
    <link href="css/msgStyle.css" type="text/css" rel="stylesheet" media="screen" />

    <script language="javascript" type="text/javascript" src="ModelJS/jquery-1.3.1.js"></script>

    <script language="javascript" type="text/javascript" src="JS/ProandArea/ProandArea2.js"></script>

    <script language="javascript" type="text/javascript" src="JS/RegisterNew.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:Top ID="Top1" runat="server" />
            <div class="btContain clfx">
                <div id="main" class="clfx">
                    <div id="signr">
                        <div class="ca">
                            <h3>
                                用户注册</h3>
                            <span class="bizin"><a href="http://jiameng.118114.cn/selfReg/step1.jsp" target="_blank">
                                商家注册&gt;&gt;</a></span>
                        </div>
                        <div class="cb">
                            <dl>
                                <dt>
                                    <label for="username">
                                        <span class="mst">*</span>用户名：</label></dt>
                                <dd>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="text" value="" id="username" class="texti" name="username" runat="server" />
                                            </td>
                                            <td>
                                                <div class="msg msg-inline show" id="usernameError_tip">
                                                    <div class="msg-default msg-error">
                                                        <i class="msg-icon" id="usernameImg"></i><i class="msg-iconCheck" id="usernameCheckImg">
                                                        </i><i class="msg-iconOK" id="usernameOKImg"></i>
                                                        <div class="msg-content" id="usernameError_msg">
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                                <dt>
                                    <label for="password">
                                        <span class="mst">*</span>设置密码：</label>
                                </dt>
                                <dd>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="password" value="" id="password" class="texti" maxlength="20" name="password" />
                                            </td>
                                            <td>
                                                <div class="msg msg-inline show" id="passwordError_tip">
                                                    <div class="msg-default msg-error">
                                                        <i class="msg-icon"></i>
                                                        <div class="msg-content" id="passwordError_msg">
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                                <dt>
                                    <label for="password_OK">
                                        <span class="mst">*</span>确认密码：</label>
                                </dt>
                                <dd>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="password" value="" id="password_OK" class="texti" maxlength="20" name="password_OK" />
                                            </td>
                                            <td>
                                                <div class="msg msg-inline show" id="password_OKError_tip">
                                                    <div class="msg-default msg-error">
                                                        <i class="msg-icon"></i>
                                                        <div class="msg-content" id="password_OKError_msg">
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                                <dt>
                                    <label>
                                        <span class="mst">*</span>所属省：</label>
                                </dt>
                                <dd>
                                    <table>
                                        <tr>
                                            <td>
                                                <select id="proInfoList" runat="server" onchange="selpro(this.value,2)">
                                                </select>
                                            </td>
                                            <td>
                                                <div class="msg msg-inline show" id="stextError_tip">
                                                    <div class="msg-default msg-error">
                                                        <i class="msg-icon"></i>
                                                        <div class="msg-content" id="stextError_msg">
                                                            请选择
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                                <dt>
                                    <label>
                                        <span class="mst">*</span> 所属城市：</label>
                                </dt>
                                <dd>
                                    <table>
                                        <tr>
                                            <td>
                                                <select id="areaInfoList" runat="server" onchange="selcity(this.value)">
                                                </select>
                                                <select id="areaid" runat="server" style="display: none;">
                                                </select>
                                                <input type="text" id="stext" name="stext" style="display: none;" value="-999" runat="Server" />
                                                <input type="text" id="resulttxt" name="resulttxt" style="display: none;" value="-999" runat="Server" />
                                            </td>
                                            <td>
                                                <div class="msg msg-inline show" id="resulttxtError_tip">
                                                    <div class="msg-default msg-error">
                                                        <i class="msg-icon"></i>
                                                        <div class="msg-content" id="resulttxtError_msg">
                                                            请选择
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                                <dt>
                                    <label for="email">
                                        邮箱：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="email" class="texti" name="email" runat="Server" />
                                    <asp:CheckBox ID="Chk_Mail" runat="server" Text="我要认证" />
                                </dd>
                                <dt>
                                    <label for="telephone">
                                        手机号码：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="telephone" class="texti" name="telephone" onpropertychange="ChangePhoneState()"
                                        runat="server" />
                                    <asp:CheckBox ID="CB_Phone" runat="server" Text="我要认证" />
                                    <input type="button" onclick="return CheckInput()&&IsPhone()&& SendPhoneAuth();"
                                        value="我要认证" id="Button1" style="display: none" />
                                    <input id="phonestate" type="hidden" name="phonestate" value="" />
                                </dd>
                                <dt>
                                    <label for="phone_code" id="lblPhoneCode" style="display: none">
                                        <span class="mst">*</span>手机验证码：</label>
                                </dt>
                                <dd id="ddPhoneCode" style="display: none">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="text" value="" id="phone_code" maxlength="6" class="identi_code" name="phone_code" />
                                                <span class="buttonSpan">
                                                    <button type="button" id="btnGetCode">
                                                        点击获取验证码</button>
                                                </span>
                                            </td>
                                            <td>
                                                <div class="msg msg-inline show" id="phone_codeError_tip">
                                                    <div class="msg-default msg-error">
                                                        <i class="msg-icon"></i>
                                                        <div class="msg-content" id="phone_codeError_msg">
                                                            验证码不能为空
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                                <dt>
                                    <label for="page_code">
                                        <span class="mst">*</span>验证码：</label>
                                </dt>
                                <dd>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="text" value="" id="page_code" maxlength="6" class="identi_code" name="page_code"
                                                    onblur="codeBlur()" />
                                                <span class="code">
                                                    <script language="javascript" type="text/javascript">
				                                        document.write("<img id='IMG2' src='ValidateToken.aspx' width='62' height='22'>");
				                                        function RefreshCode(){
					                                        document.all.IMG2.src = 'ValidateToken.aspx?yymm='+Math.random();
				                                        }
                                                    </script>
                                                    <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span> </span>
                                                <span class="remark" id="err_page_code" runat="server"></span>
                                            </td>
                                            <td>
                                                <div class="msg msg-inline show" id="page_codeError_tip">
                                                    <div class="msg-default msg-error">
                                                        <i class="msg-icon"></i>
                                                        <div class="msg-content" id="page_codeError_msg">
                                                            不能为空
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dd>
                            </dl>
                            <dl>
                                <dt>
                                    <label>
                                        &nbsp;</label></dt>
                                <dd>
                                    <div class="subtn">
                                        <table>
                                            <tr>
                                                <td>
                                                    <span class="btn btnA"><span>
                                                        <img id="loadImg" src="Images/Loading.gif" style="position: absolute; display: none" />
                                                        <button type="button" id="btnSave">
                                                            完 成</button>
                                                        <asp:HiddenField ID="HiddenField_SPID" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField_URL" runat="server" Value="" />
                                                    </span></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </dd>
                            </dl>
                        </div>
                    </div>
                    <div id="funcIntro">
                        <p class="signup">
                            欢迎您成为号码百事通会员</p>
                        <p>
                            &nbsp;</p>
                        <p>
                            &nbsp;</p>
                        <p>
                            &nbsp;</p>
                        <p>
                            &nbsp;</p>
                        <ul class="list">
                            <li>认证手机、认证邮箱使您的手机和邮箱成为登录帐号，方便记忆。</li>
                            <li>认证手机、认证邮箱后可以通过手机和邮箱方便的找回密码，增加安全性。</li>
                            <li>认证手机后在您使用号码百事通语音等业务服务时准确识别您的身份，更多贴心的信息提示增强您的体验。</li>
                        </ul>
                    </div>
                </div>
            </div>
            <uc2:Foot ID="Foot1" runat="server" />
        </div>
        <%--0表示验证，1或者其他表示不验证--%>
        <asp:HiddenField ID="hdIsEmailAuthen" runat="server" />
        <%--0表示验证，1或者其他表示不验证--%>
        <asp:HiddenField ID="hdIsPhoneAuthen" runat="server" />
        <asp:HiddenField ID="hdRandomCustID" runat="server" />
    </form>
</body>
</html>

<script type="text/javascript">
    $(document).ready(function(){
        var objEmail = $("#email");
        var objPhone = $("#telephone");
        var objPhoneCode = $("#phone_code");
        
        $("#Chk_Mail").attr("disabled","disabled");
        $("#CB_Phone").attr("disabled","disabled");
        
        //邮箱、手机email telephone,Chk_Mail CB_Phone
        $("#email").blur(function(){
            var email = $("#email").val();
            if(email==""){
                $("#Chk_Mail").attr("disabled","disabled");
                $("#Chk_Mail").attr("checked",false);
                $("#hdIsEmailAuthen").val("1");
            }else{
                $("#Chk_Mail").removeAttr("disabled");
            }
        })
        
        $("#telephone").blur(function(){
            var phone = $("#telephone").val();
            if(phone==""){
                $("#CB_Phone").attr("disabled","disabled");
                $("#CB_Phone").attr("checked",false);
                $("#lblPhoneCode").css("display","none");
                $("#ddPhoneCode").css("display","none");
                $("#hdIsPhoneAuthen").val("1");
            }else{
                
                $("#CB_Phone").removeAttr("disabled");
            }
        })
        
        $("#Chk_Mail").click(function(){
            var checked = $("#Chk_Mail").attr("checked");
            if(checked){
                $("#hdIsEmailAuthen").val("0");
            }else{
                $("#hdIsEmailAuthen").val("1");
            }
        })
        
        $("#CB_Phone").click(function(){
            var checked = $("#CB_Phone").attr("checked");
            if(checked){
                $("#lblPhoneCode").css("display","block");
                $("#ddPhoneCode").css("display","block");
                $("#hdIsPhoneAuthen").val("0");
            }else{
                $("#lblPhoneCode").css("display","none");
                $("#ddPhoneCode").css("display","none");
                $("#hdIsPhoneAuthen").val("1");
            }
        })
        /*====================获取手机验证码===========================*/
        
        $("#btnGetCode").click(function(){
            var time = new Date();
            $.ajax({
                type:"POST",
                dataType:"json",
                url:"HttpHandler/GetPhoneAuthenCodeHandler.ashx",
                data:{PhoneNum:objPhone.val(),type:"authen",time:time.getTime()},
                success:function(data,textStatus){
                    $.each(data,function(index,item){
                        if(item["result"]=="true"){
                            //$("#hdRandomCustID").val(item["custid"]);
                            //alert(item["authencode"]);
                            //alert($("#hdRandomCustID").val());
                            StartInterval();
                        }else{
                            //$("#hdRandomCustID").val("");
                            alert(item["info"]);
                        }
                    })
                },
                error:function(data){
                    //alert(data);
                }
            })
        })
    })
    
    var n =0 ;
    var intervalID;
    function StartInterval(){
        $("#btnGetCode").attr("disabled","false");
        $("#btnGetCode").text("120秒后重新获取");
        intervalID = window.setInterval("IntervalFun()",1000);
        
    }
    function IntervalFun(){
        if(n <= 119){
            var nLeft = 119 - n;
            $("#btnGetCode").text(nLeft+"秒后重新获取");
            n++;
        }else{
            ClearInterval();
        }
    }
    function ClearInterval(){
        window.clearInterval(intervalID);
        $("#btnGetCode").text("重新发送验证码");
        $("#btnGetCode").removeAttr("disabled");
        n=0;
    }
</script>

