<%@ page language="C#" autoeventwireup="true" inherits="BindingAndRegister, App_Web_bindingandregister.aspx.cdcab7d2" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
     <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    <script language="javascript" type="text/javascript" src="JS/UserRegistry/jquery.inputHint.js"></script>
    <style type="text/css">
    .content{width:938px;position:relative;margin:20px auto;padding:10px 20px;border:1px solid #8FC4E0}
    .content h2{font:18px/36px Microsoft Yahei,simhei;color:#007bc1;margin-bottom:30px}
    .content h2 span{color:#555;margin-right:5px}
    .content form{margin-bottom:30px}
    .content .label{display:block;height:35px;margin-bottom:15px}
    .content .label span{display:block;float:left;margin-right:10px;width:80px;text-align:right;font-size:14px;line-height:30px}
	.content .label i{font-style:normal;float:left;margin-right:13px;line-height:30px}
    .content .label em{line-height:30px;color:red;margin-left:5px;margin-right:10px;float:left;display:inline}
    .content .label p{line-height:30px;float:left}
    .content .label p.hintCorrect{padding-left:20px;color:#390;background:url(images/bg_sprite.png) no-repeat -206px 5px}
    .content .label p.hintError{padding-left:20px;color:#C30;background:url(images/bg_sprite.png) no-repeat -206px -21px}
    .content .input{display:block;float:left;width:180px;height:28px;padding:0 5px;border:1px solid #ceceb1;line-height:30px;font-size:14px}
    .content .radio{float:left;margin-right:5px;margin-top:9px;*margin-top:2px}
	.content .label#code{display:none}
    input#checkCode{width:50px;margin-right:5px}
    #sendCode{float:left;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer;margin-right:0}
    #register{width:108px;height:38px;margin-left:90px;border:none;background:url(images/bg_sprite.png) no-repeat 0 0;cursor:pointer;display:block;text-indent:-9999px}
    .conExplain{position:absolute;width:220px;top:75px;right:80px;line-height:22px;color:#666}
    .conExplain .blue{margin-bottom:10px;color:#007bc1}
    
    .foot{width:980px;margin:10px auto;text-align:center;color:#78797a;line-height:23px}
    .foot a{color:#acadaf}
    .head{width:980px;margin:10px auto;clear:both;font-family:Microsoft Yahei,simhei;color:#007bc1;font-size:24px}

    .head p{margin-top:12px;margin-left:20px;padding-left:20px;padding-top:8px;border-left:1px solid #c2c2c2;height:30px}
    .tabA{width:980px;margin:0 auto;clear:both;border-bottom:1px solid #8fc4e0}
    .tabA li{float:left;margin-top:16px;padding-left:10px;clear:none;width:162px;height:32px;line-height:32px;background:#f6f6f6;font-weight:700;font-size:14px;color:#007bc1; cursor:pointer;text-align:center;margin-left:20px;border-radius:10px 10px 0 0;}
    .tabA li.vazn{border:1px solid #8fc4e0;border-width:1px 1px 0 1px;background:#fff}
    .selectValue{width:190px;height:150px;overflow-y:scroll;margin:0;border:1px solid #ccc;position:absolute;top:32px;left:100px;background:#fff;display:none}
    .selectValue a{display:block;height:30px;line-height:30px;padding-left:5px;text-align:left;margin:0;color:#39c;text-decoration:none}
    .selectValue a:hover{background:#39c;color:#fff}
    
    </style>    
    <script language="javascript" type="text/javascript" src="http://customer.besttone.com.cn/UDBUserPortal//ModelJS/jquery-1.3.1.js"></script>   
    
    
    
</head>
<body>

<asp:Panel ID="header" runat="server">

<div class="head clearfix"><div class="fl"><img src="images/logoct.gif" alt="" /><img style="padding-left:20px" src="images/logo_besttone.gif"  alt=""/></div><p class="fl">号码百事通账户</p></div>



</asp:Panel>


    <form id="form1" runat="server">
    <div>
    
        <label class="label" for="mobile"><span>手机号：</span><asp:TextBox runat="Server" id="mobile" class="input" maxlength="11" ></asp:TextBox><em>*</em><p id="hintMobile" class="hintCorrect" runat="Server">该手机号可以开通号码百事通账户</p><!--<p id="hintMobile" class="hintError">该手机号已经开通号码百事通账户</p>--></label>
        <label class="label" for="checkCode" id="code"><span>验证码：</span><input type="text" id="checkCode" class="input" /><span id="sendCode" onclick="SendPhoneAuth()" >发送验证码</span><em>*</em><p id="hintCode" runat="Server">请输入收到的验证码</p></label>
        <label class="label" for="realName"><span>真实姓名：</span><input type="text" id="realName" class="input" maxlength="11" /><em>*</em></label>
       
        <label class="label" for="contactTel"><span>联系手机：</span><asp:TextBox runat="Server"  id="contactTel" class="input" maxlength="11" onblur="checkContactTel();" ></asp:TextBox><em>*</em><p id="hintContactTel"  runat="Server" ></p></label>
        <label class="label" for="contactMail"><span>联系邮箱：</span><input type="text" id="contactMail" class="input" maxlength="30" onblur="checkContactMail();"  /><em>*</em><p id="hintContactMail"  runat="Server"></p></label>
        <p class="label"><span>性别：</span>
          <input type="radio" class="radio" name="sex" value="1" checked="checked" /><i>男</i>
          <input type="radio" class="radio" name="sex" value="0" /><i>女</i>
		  <input type="radio" class="radio" name="sex" value="2" /><i>保密</i>
		</p>
        
        
        <label class="label" for="select">
        	<span>证件类型：</span>
            <input name="select" type="text" value="请选择" readonly="readonly" class="input select"/>
          
            <input name="certtype" id="certtype" type="hidden" runat="Server" value=""/>
            <div class="selectValue">
                <a href="javascript:void(0)" data="1">身份证</a>
                <a href="javascript:void(0)" data="2">护照</a>
                <a href="javascript:void(0)" data="3">军官证</a>
                <a href="javascript:void(0)" data="4">士兵证</a>
                <a href="javascript:void(0)" data="5">回乡证</a>
                <a href="javascript:void(0)" data="6">临时身份证</a>
                <a href="javascript:void(0)" data="7">户口薄</a>
                <a href="javascript:void(0)" data="8">警官证</a>
                <a href="javascript:void(0)" data="9">台胞证</a>
                <a href="javascript:void(0)" data="10">营业执照</a>
                <a href="javascript:void(0)" data="X">其他</a>
            </div>
            <em>*</em>
        </label>
        
        <label class="label" for="certnum"><span>证件号码：</span><input type="text" id="certnum" class="input" maxlength="20" /><em>*</em></label>
        <asp:HiddenField ID="myCustID" Value ="" runat="server" />
        
        <asp:HiddenField ID="myReturnUrl" Value="" runat="server" />
        
        <asp:Button ID="Button1" runat="server" Text="开通账户" OnClientClick="return CheckForm();" OnClick="register_Click" />
       
        <input type="hidden" id="binding" name="binding" value="1" />
    </div>
    </form>
    
    <form id="form2" runat="server">
    <div>
    
        <label class="label" for="mobile"><span>手机号：</span><asp:TextBox runat="Server" id="TextBox1" class="input" maxlength="11" ></asp:TextBox><em>*</em><p id="P1" class="hintCorrect" runat="Server">该手机号可以开通号码百事通账户</p><!--<p id="hintMobile" class="hintError">该手机号已经开通号码百事通账户</p>--></label>
        <label class="label" for="checkCode" id="Label1"><span>验证码：</span><input type="text" id="Text1" class="input" /><span id="Span1" onclick="SendPhoneAuth()" >发送验证码</span><em>*</em><p id="P2" runat="Server">请输入收到的验证码</p></label>
  
        <input type="hidden" id="register" name="register" value ="1" />
    
    </div>
    </form>
    
    
    
    
    
    
  <asp:panel ID="footer" runat="server" > 
<div class="foot">
<p>中国电信集团：<a href="">189邮箱</a> | <a href="">天翼宽带</a> | <a href="">号百商旅</a> | <a href="">号百导航</a> | <a href="">天翼手机网</a> | <a href="">翼支付</a> | <a href="">爱音乐</a> | <a href="">天翼视讯</a> | <a href="">协同通讯</a> | <a href="">物联网</a> | <a href="">天翼空间</a> | <a href="">天翼阅读</a> | <a href="">爱游戏</a> | <a href="">爱动漫</a></p>
违法和不良信息举报 <a href="">service@118114.cn</a>   copyright© 2007-2011 号百商旅电子商务有限公司版权所有<br />   
增值电信业务经营许可证：沪B2-20110026　沪ICP备11017770号  上海工商标识编号：20110624111724289
</div>

</asp:panel>  
    
    
    
    <script type="text/javascript">
        //模拟下拉框
        $("input.select").click(function(){
			var jQueryselect=$(this);
			jQueryselect.siblings("div").css({"left":jQueryselect.position().left+"px","top":jQueryselect.position().top+30+"px"}).slideDown("fast").children("a").click(function(){
				jQueryselect.attr("value",$(this).text());
				$(this).parent("div").hide();
				$("#certtype").val($(this).attr("data"));
				});
			$("body").click(function(){$(".selectValue").hide()});
		});
</script>
    
</body>
</html>
