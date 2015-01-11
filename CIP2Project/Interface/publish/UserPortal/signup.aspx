<%@ page language="C#" autoeventwireup="true" enableviewstate="true" inherits="signup, App_Web_signup.aspx.cdcab7d2" enableEventValidation="false" %>

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
    <script language="javascript" type="text/javascript" src="ModelJS/jquery-1.3.1.js"></script>

    <script language="javascript" type="text/javascript" src="ModelJS/Date/WdatePicker.js"></script>

    <script language="javascript" type="text/javascript" src="JS/UserRegistry/UserRegistry.js"></script>

    <script language="javascript" type="text/javascript" src="JS/ProandArea/ProandArea2.js"></script>

    <script language="javascript" type="text/javascript" event="onclick()" for="btn_OK">
               
               
               
//                if ($('#identifying_codeState').attr('value')== "open")
//                {
//                    document.getElementById("identifying").style.display ="block";
//                }
//                else
//                {
//                     document.getElementById("identifying").style.display ="none";
//                }

//                if ($('#detailinfoState').attr('value') == "open")
//                {
//                    document.getElementById("detailinfo").style.display ="block";
//                }
//                else
//                {
//                     document.getElementById("detailinfo").style.display ="none";
//                }

//                f_show();	
                
                
				if(password.value=="")
				{
				
				   document.getElementById("err_password").innerText="密码不能为空!";
				  
				   return false;
				}
				if(password_OK.value=="")
				{
				   document.getElementById("err_password_OK").innerText="确认密码不能为空!";
				   return false;
				}
				if(password_OK.value!=password.value)
				{
					
				   document.getElementById("err_password").innerText="密码不相同!";
				   return false;
				}
				
			
				
				var proInfoListtxt = document.all.proInfoList.value;
	
				if(proInfoListtxt=='-999')
				{
                   document.all.Err_Province.innerText ="请选择省!";
               
                   return false;
				}
				else
				{
				  document.all.Err_Province.innerText ="";	
				  return true;
				}	
				
				
			   
    </script>

    <script language="javascript">
    
function f_show()
{

    //var style1=document.getElementById("CertificateType").value;

    if (document.getElementById("CertificateType").value=="")
    {
        document.getElementById("certno").style.display ="none";
        document.getElementById("certnoL").style.display ="none";       
    //   document.GetElementById("Err_certno").style.display ="none";    
    }
    else
    {
        document.getElementById("certno").style.display ="block";
        document.getElementById("certnoL").style.display ="block";
      //  document.GetElementById("Err_certno").style.display ="block";    

    } 
}
function DetailInformation()
{


        if(document.getElementById("detailinfo").style.display =="none")
        {
          document.getElementById("detailinfo").style.display ="block";
          document.getElementById("detailinfoHtml").innerHTML = "隐藏更多注册信息";
          $("#detailinfoState").attr("value", "open");
        }else{
          document.getElementById("detailinfo").style.display ="none";
          document.getElementById("detailinfoHtml").innerHTML = "显示更多注册信息";
          $("#detailinfoState").attr("value", "close");
        }   
}


    </script>

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
                                    <input type="text" value="" id="username" class="texti" name="username" runat="server" />
                                    <span id="err_username" class="remark" runat="Server"></span>
                                    <input id="userstate" type="hidden" value="" />
                                </dd>
                                <dt>
                                    <label for="fullname">
                                        <span class="mst">*</span>真实姓名：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="fullname" class="texti" name="fullname" runat="Server" /><span
                                        id="err_fullname" class="remark"></span>
                                </dd>
                                <dt>
                                    <label for="password">
                                        <span class="mst">*</span>密码：</label>
                                </dt>
                                <dd>
                                    <input type="password" value="" id="password" class="texti" maxlength="20" name="password" /><span
                                        id="err_password" class="remark" runat="Server"></span>
                                </dd>
                                <dt>
                                    <label for="password_OK">
                                        <span class="mst">*</span>确认密码：</label>
                                </dt>
                                <dd>
                                    <input type="password" value="" id="password_OK" class="texti" maxlength="20" name="password_OK" /><span
                                        id="err_password_OK" class="remark"></span>
                                </dd>
                                <dt>
                                    <label>
                                        性别：</label>
                                </dt>
                                <dd>
                                    <select id="sex" name="sex" runat="Server">
                                        <option selected="selected" value="2">未知</option>
                                        <option value="1">男</option>
                                        <option value="0">女</option>
                                    </select>
                                </dd>
                                <dt>
                                    <label>
                                        <span class="mst">*</span>所属省：</label>
                                </dt>
                                <dd>
                                    <select id="proInfoList" runat="server" onchange="selpro(this.value,2)">
                                    </select>
                                    <span id="Err_Province" class="remark" runat="Server"></span>
                                </dd>
                                <dt>
                                    <label>
                                        <span class="mst">*</span> 所属城市：</label>
                                </dt>
                                <dd>
                                    <select id="areaInfoList" runat="server" onchange="selcity(this.value)">
                                    </select>
                                    <select id="areaid" runat="server" style="display: none;">
                                    </select>
                                    <input type="text" id="stext" name="stext" style="display: none;" value="-999" runat="Server" />
                                    <input type="text" id="resulttxt" name="resulttxt" style="display: none;" value="-999"
                                        runat="Server" />
                                </dd>
                                <dt>
                                    <label for="email">
                                        邮箱：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="email" class="texti" name="email" runat="Server" />
                                    <asp:CheckBox ID="Chk_Mail" runat="server" Text="我要认证" />
                                    <span class="remark" id="err_email" runat="Server"></span>
                                </dd>
                                <dt>
                                    <label for="telephone">
                                        联系电话：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="telephone" class="texti" name="telephone" onpropertychange="ChangePhoneState()"
                                        runat="server" />
                                    <input type="button" onclick="return CheckInput()&&IsPhone()&& SendPhoneAuth();"
                                        value="我要认证" id="Button1" />
                                    <span class="remark" id="err_telephone"></span>
                                    <input id="phonestate" type="hidden" name="phonestate" value="" />
                                </dd>
                            </dl>
                            <dl id="identifying_code" style="display: none;" runat="Server">
                                <dt>
                                    <label for="phone_code">
                                        手机验证码：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="phone_code" maxlength="6" class="identi_code" name="phone_code"
                                        onblur="codeBlur()" />
                                    <span class="remark" id="err_phone_code" runat="Server"></span>
                                </dd>
                                <dt>
                                    <label for="page_code">
                                        页面验证码：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="page_code" maxlength="6" class="identi_code" name="page_code"
                                        onblur="codeBlur()" />
                                    <span class="code">

                                        <script language="javascript" type="text/javascript">
				                            document.write("<img id='IMG2' src='ValidateToken.aspx' width='62' height='22'>");
				                            function RefreshCode()
				                            {
					                            document.all.IMG2.src = 'ValidateToken.aspx?yymm='+Math.random();
				                            }
                                        </script>

                                        <span><a href="javascript:RefreshCode()" class="hui12">看不清？换一张</a></span> </span>
                                    <span class="remark" id="err_page_code" runat="server"></span>
                                </dd>
                            </dl>
                            <dl>
                                <dt><a href="javascript:DetailInformation()"><font color="OrangeRed"><span id="detailinfoHtml">
                                    显示更多注册信息</span></font></a></dt>
                                <dd>
                                </dd>
                            </dl>
                            <dl id="detailinfo" style="display: none;" runat="Server">
                                <dt>
                                    <label for="NickName">
                                        昵称：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="NickName" class="texti" name="NickName" runat="Server" />
                                </dd>
                                <dt>
                                    <label>
                                        证件类型：</label>
                                </dt>
                                <dd>
                                    <select name="CertificateType" id="CertificateType" runat="Server">
                                        <option selected="selected" value="">请选择</option>
                                        <option value="0">身份证</option>
                                        <option value="1">士兵证</option>
                                        <option value="2">军官证</option>
                                        <option value="3">护照</option>
                                        <option value="4">保留</option>
                                        <option value="5">台胞证</option>
                                        <option value="6">港澳通行证</option>
                                        <option value="7">国际海员证</option>
                                        <option value="9">其它</option>
                                        <option value="10">部队干部离休证</option>
                                        <option value="11">工商营业执照</option>
                                        <option value="12">单位证明</option>
                                        <option value="13">驾驶证</option>
                                        <option value="14">学生证</option>
                                        <option value="15">教师证</option>
                                        <option value="16">户口本/居住证</option>
                                        <option value="17">老人证</option>
                                        <option value="18">组织机构代码证</option>
                                        <option value="19">工作证</option>
                                        <option value="20">暂住证</option>
                                        <option value="21">电信识别编码</option>
                                        <option value="22">集团客户标识码</option>
                                        <option value="23">VIP卡</option>
                                        <option value="24">警官证</option>
                                    </select>
                                </dd>
                                <dt>
                                    <label for="certno" id="certnoL" runat="Server" style="display: none;">
                                        证件号：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="certno" class="texti" name="certno" runat="Server"
                                        style="display: none;" />
                                    <span class="remark" id="Err_certno" runat="Server"></span>
                                </dd>
                                <dt>
                                    <label for="birthday">
                                        生日：</label>
                                </dt>
                                <dd>
                                    <input type="text" value="" id="birthday" class="texti" name="birthday" onclick="WdatePicker()"
                                        runat="Server" />
                                    <span id="err_birthday" class="remark"></span>
                                </dd>
                                <dt>
                                    <label>
                                        文化程度：</label>
                                </dt>
                                <dd>
                                    <select name="EduLevel" id="EduLevel" runat="Server">
                                        <option selected="selected" value="">未知</option>
                                        <option value="4">大学/专科</option>
                                        <option value="1">小学</option>
                                        <option value="2">初中</option>
                                        <option value="3">高中/中专</option>
                                        <option value="5">研究生及以上</option>
                                    </select>
                                </dd>
                                <dt>
                                    <label>
                                        收入水平：</label>
                                </dt>
                                <dd>
                                    <select name="IncomeLevel" id="IncomeLevel" runat="Server">
                                        <option selected="selected" value="">未知</option>
                                        <option value="1">1000~3000</option>
                                        <option value="0"><=1000</option>
                                        <option value="2">3000~5000</option>
                                        <option value="3">5000~8000</option>
                                        <option value="4">8000~20000</option>
                                        <option value="5">20000以上</option>
                                    </select>
                                </dd>
                            </dl>
                            <dl>
                                <dt>
                                    <label>
                                        &nbsp;</label></dt>
                                <dd>
                                    <div class="subtn">
                                        <span class="btn btnA"><span>
                                            <asp:Button ID="btn_OK" runat="server" class="btn_OK" Text="确定" OnClick="btn_OK_Click" />
                                            <asp:HiddenField ID="HiddenField_SPID" runat="server" Value="" />
                                            <asp:HiddenField ID="HiddenField_URL" runat="server" Value="" />
                                        </span></span>
                                        <input type="text" value="0" id="backCount" runat="server" name="code" class="texti"
                                            style="display: none;" />
                                        <input type="text" value="close" id="detailinfoState" runat="server" name="code"
                                            class="texti" style="display: none;" />
                                        <input type="text" value="close" id="identifying_codeState" runat="server" name="code"
                                            class="texti" style="display: none;" />
                                        <span class="btn btnA"><span>
                                            <input type="button" value="返回" class="btn_OK" onclick="window.history.go($('#backCount').attr('value'));">
                                        </span></span>
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
    </form>
</body>
</html>
