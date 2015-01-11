<%@ page language="C#" autoeventwireup="true" inherits="CreateBesttoneAccount, App_Web_createbesttoneaccount.aspx.cdcab7d2" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>开通号码百事通账户</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    
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
	/*.content .label#code{display:none} */
    input#checkCode{width:50px;margin-right:5px}
    #sendCode{float:left;width:123px;height:28px;line-height:28px;font-size:14px;text-align:center;color:#2a609c;border:1px solid #95c7e2;background:#d3effe;cursor:pointer;margin-right:0}
    #register{width:191px;height:38px;margin-left:90px;border:none;background:url(images/bg_sprite.png) no-repeat 0 -152px #ccc;cursor:pointer;display:block;text-indent:-9999px}
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
    <script language="javascript" type="text/javascript" src="ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="JS/UserRegistry/jquery.inputHint.js"></script>
    <script type="text/javascript">
        //手机号是否已验证 验证=1；未验证=0；
        var mobileChecked=1;
        var globAjax_Result = 1;
        var hasSend=0;
        function checkContactTel(){
            var regMobile=/^1([3][0-9]|[5][0123456789]|[8][0123456789])\d{8}$/;		//手机号验证
            if(!regMobile.test($("#contactTel").val())){
                $("#hintContactTel").html("请输入正确格式的手机号码").addClass("hintError");
	            return false;
            }else{
                $("#hintContactTel").html("输入正确").addClass("hintCorrect").removeClass("hintError");
            }
        }

        function checkContactMail(){
            var regEmail=/^[0-9a-zA-Z_\-\.]*[0-9a-zA-Z_\-]@[0-9a-zA-Z]+\.+[0-9a-zA-Z_\-.]+$/;
            if(!regEmail.test($("#contactMail").val())){
                $("#hintContactMail").html("请输入正确格式的邮箱").addClass("hintError");
	            return false;
            }else{
                $("#hintContactMail").html("输入正确").addClass("hintCorrect").removeClass("hintError");
            }
        
        }


        function mobilePhone(){
            var regMobile=/^1([3][0-9]|[5][0123456789]|[8][0123456789])\d{8}$/;		//手机号验证
            if(!regMobile.test($("#mobile").val())){	//手机号格式不正确
	            $("#hintMobile").html("请输入正确格式的手机号码").addClass("hintError");
	            return false;
            }else{
	            $("#hintMobile").html("输入正确").addClass("hintCorrect").removeClass("hintError");
            }
            return true;
        }



        $(function(){
        
        	$("#certnum").blur(function(){
	            if($(this).val()!=""){
	                isIDCode($(this).val());
	            }
	        });
        
            $("#certnum").change(function(){
	            if($(this).val()!=""){
	                isIDCode($(this).val());
	            }
	        });
        
            var mobileNum=$("#mobile").val();
            
            //if($("#hidCheckMobile").val()!=mobileNum)
            //{
		    //    mobileChecked=0;
    		//    $("#code").css("display","block").children("input").val("");
            //}else
            //{
            //    mobileChecked=1;
		    //    $("#code").css("display","none");
		    //    $("#code").attr("value","");
            //}
            
          $("#code").css("display","block").children("input").val(""); 
            
            var tmp = Math.random();
            $.get("ExistBesttoneAccount_ajax.aspx",{
            PhoneNum:$("#mobile").attr("value"),
            SPID:$("#HiddenField_SPID").attr("value"),
            RAM:tmp,
            typeId:1}, ResultBestTonePhone);
        
        
        	$("#mobile").blur(function(){
                if(mobilePhone()){
                        $.get("ExistBesttoneAccount_ajax.aspx",{
                        PhoneNum:$("#mobile").attr("value"),
                        SPID:$("#HiddenField_SPID").attr("value"),
                        RAM:tmp,
                        typeId:1}, ResultBestTonePhone);
	        
                		if($(this).val()!=mobileNum && globAjax_Result==0){
	            		    mobileChecked=0;
	            		    $("#code").css("display","block").children("input").val("");
            		    }else{

	            		    //mobileChecked=1;
	            		    mobileChecked=0; 
	            		    //$("#code").css("display","none");
	            		    //$("#code").attr("value","");
            		    }	        
	        
                }else{
                    $("#hintMobile").html("请输入正确格式的手机号码").addClass("hintError");
                      mobileChecked=1;
		              $("#code").css("display","none");
		              $("#code").attr("value","");				 
                }   
	        });
        
        
	        $("#mobile").change(function(){
	        //手机输入框发生改变时，显示发送验证码，验证状态为0，未改变时则隐藏发送验证码，验证状态为1
	       
			    if(mobilePhone()){
	        		    $.get("ExistBindAuthenPhoneAndBesttoneAccount_ajax.aspx",{
                		    PhoneNum:$("#mobile").attr("value"),
                		    CustID:$("#myCustID").attr("value"), 
                		    SPID:$("#HiddenField_SPID").attr("value"),
                		    RAM:tmp,
                		    typeId:1}, ResultBestTonePhone);

                    		if($(this).val()!=mobileNum && globAjax_Result==0){
		            		    mobileChecked=0;
		            		    $("#code").css("display","block").children("input").val("");
	            		    }else{
		            		    //mobileChecked=1;
		            		    mobileChecked=0;
		            		    //$("#code").css("display","none");
		            		    //$("#code").attr("value","");
	            		    }
    	            

			    }else{
				    $("#hintMobile").html("请输入正确格式的手机号码").addClass("hintError");
		             mobileChecked=1;
		             $("#code").css("display","none");
		             $("#code").attr("value","");				
			    }
	        });
	        
	        
        })

        //用户验证ajax返回值
        function ResultBestTonePhone(Result)
        {
            globAjax_Result = Result;
            if(Result == 0)
            {   
                $("#hintMobile").html("该手机号可以开户,该手机号要收取支付密码，请注意查收");
                $("#hintMobile").attr("class","hintCorrect");
            }
            else
            {
               $("#hintMobile").html("该手机号码已经开过户"); 
               $("#hintMobile").attr("class","hintError");
            }
        }        

       //发送手机验证码ajax
        function SendPhoneAuth()
        {   
            if(hasSend==1){
			    return false	
		    }
		  
             if($("#mobile").attr("value") != ""){
                    if(mobilePhone()){
                        var tmp = Math.random();
                        $.get("PutAuthenCodeForOpenAccount.aspx",{
                        PhoneNum:$("#mobile").attr("value"),
                        SPID:$("#HiddenField_SPID").attr("value"),
                        RAM:tmp,
                        typeId:1}, ResultCheckCode); 
                    }else{
                        $("#mobile").focus();
                        $("#hintMobile").html("请输入正确的手机号码");
                        $("#hintMobile").attr("class","hintError");           
                    
                    }
             }
 
        }
        //ajax手机验证码返回值
        function ResultCheckCode(Result)
        {       
            hasSend=1;
            if(Result == 0)
            {
                $("#hintCode").html("手机验证码已经发送");
                $("#hintCode").attr("class","hintCorrect");
                codeCountDown($("#sendCode"));
            }
			else if(Result == "-30004")
            {
                $("#hintCode").html("超出发送次数");
                $("#hintCode").attr("class","hintError");
                return false;
            }
            
        }

        //验证码倒计时
        function codeCountDown($obj) {
	        var sec = 120;
	        $obj.html("<strong id='countdown'>" + sec + "</strong>秒后可重发").css({
		        "background": "#ececec"
	        });
	        function countDown() {
		        if (sec > 1) {
			        sec--;
			        $("#countdown").text(sec);
		        } else {
			        clearInterval(countDown_Timer);
			        $("#countdown").text("0");
			        $obj.html("发送验证码").css({
				        "background": "#d3effe"
			        });
			        hasSend=0;
		        }
	        }
	        countDown_Timer = setInterval(countDown, 1000);
        }

        function CheckForm(){   
	         if(mobilePhone() && $("#realName").val() != ""  && $("#sex").val() != ""  && isIDCode( $("#certnum").val() )){
		        if(mobileChecked==0){
		            if($("#checkCode").val() != ""){
		                return true;
		            }else{
		                $("#checkCode").focus();
		                $("#hintCode").html("请输入验证码").addClass("hintError");;
		                return false;
		            }
		        }else{
		            return true;
		        }  
	         
	         }else{
	           return false;
	         }
        }        



//身份证验证
function isIDCode(num) {
    var len = num.length, re;
    if (len == 15){
        re = new RegExp(/^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})$/);
    }
    else if (len == 18){
         re = new RegExp(/^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})(\d|X)$/);
    }
    else {
        $("#hint_certnum").html("身份证号非法!").addClass("hintError");
        return false;
    }

    var a = num.match(re);
    if (a != null) {
        if (len == 15) {
            var D = new Date("19" + a[2] + "/" + a[3] + "/" + a[4]);
            var B = D.getYear() == a[2] && (D.getMonth() + 1) == a[3] && D.getDate() == a[4];
        }
        else {
            var D = new Date(a[2] + "/" + a[3] + "/" + a[4]);
            var B = D.getFullYear() == a[2] && (D.getMonth() + 1) == a[3] && D.getDate() == a[4];

            //check code verify
            var w = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2);
            var x = "10X98765432";
            var cc = 0;
            for (var i = 0; i < 17; i++) {
                cc = cc + (num.charAt(i) * w[i]);
            }
            cc = cc % 11;
            if (a[6] != x.charAt(cc)) { $("#hint_certnum").html("身份证号非法!").addClass("hintError");  return false; }
        }

        if (!B) {  $("#hint_certnum").html("身份证号非法!").addClass("hintError");    return false; }
    }
    else {
        $("#hint_certnum").html("身份证号非法!").addClass("hintError"); 
        return false;
    }
    $("#hint_certnum").html("身份证号合法!").addClass("hintCorrect").removeClass("hintError");  
    return true;
}



    </script>

</head>
<body>
    
<asp:Panel ID="header" runat="server">

<div class="head clearfix"><div class="fl"><img src="images/logoct.gif" alt="" /><img style="padding-left:20px" src="images/logo_besttone.gif"  alt=""/></div><p class="fl">号码百事通账户</p><div class="fr" style="color:#333;font-size:12px;margin-top:30px">合作伙伴：天翼电子商务有限公司<span style="color:#007BC1;margin-left:10px">客服热线：4008-011-888</span></div></div>



</asp:Panel>



<div class="content">
    <h2>开通号码百事通账户</h2>
    <form id="frm1" runat="Server" >
   
            <label class="label" for="mobile">
                    <span>手机号：</span>
                    <asp:TextBox runat="Server"  id="mobile"   class="input select" maxlength="11" ></asp:TextBox>
                  
                    <div class="selectValue">
                    <% 
                        if (phones != null)
                        {
                            for (int i = 0; i < phones.Length; i++)
                            { 
                     %>
                        <a href="javascript:void(0)" ><%=phones[i].Phone%></a> 
                    <%    
                            }
                        }
                    %> 

                    </div>
                    <em>*</em><p id="hintMobile" class="hintCorrect" runat="Server">该手机号可以开通号码百事通账户</p>
          </label>    
     
        <label class="label" for="checkCode" id="code"><span>验证码：</span><input type="text" id="checkCode" name="checkCode" class="input" /><p id="sendCode" onclick="SendPhoneAuth()" >发送验证码</p><em>*</em><p id="hintCode" runat="Server">请输入收到的验证码</p></label>
        <label class="label" for="realName"><span>真实姓名：</span><input type="text" id="realName" name="realName" class="input" maxlength="11" /><em>*</em></label>
       
 
        <p class="label"><span>性别：</span>
          <input type="radio" class="radio" name="sex" value="1" checked="checked" /><i>男</i>
          <input type="radio" class="radio" name="sex" value="0" /><i>女</i>
		  <input type="radio" class="radio" name="sex" value="2" /><i>保密</i>
		</p>
        
        <label class="label" for="certnum"><span>身份证：</span><input type="text" id="certnum" name="certnum" class="input" maxlength="20" /><em>*</em> <p id="hint_certnum">请填写正确的身份证号，以确保能够正常使用账户功能。</p> </label>
        <asp:HiddenField ID="myCustID" Value ="" runat="server" />
        <asp:HiddenField ID="hidCheckMobile" runat="server" />
        <asp:HiddenField ID="myReturnUrl" Value="" runat="server" />
        <asp:HiddenField ID="HiddenField_SPID" runat="server" />
        <asp:Button ID="register" runat="server" Text="开通账户" OnClientClick="return CheckForm();" OnClick="register_Click" />
       
    </form>

<textarea  style="width:930px;height:150px;padding:5px;margin-top:10px;white-space:pre;background:#fff;border:1px solid #ccc;overflow-y:scroll" readonly>尊敬的客户，在您使用号码百事通账户自助交易各项服务前，请您务必仔细阅读并正确理解本协议全部内容，尤其是双方的权利义务和有关号码百事通公司的免责事项。您使用号码百事通账户自助交易服务的行为将被视为对本交易协议全部内容的完全认可。
第一条	自助交易的前提
1.1您应具有完全的民事行为能力。号百商旅电子商务有限公司不为无民事行为能力或限制民事行为能力人提供本服务。
1.2在签署本协议前，请您仔细阅读本协议条款，如您对本协议有疑义，请要求本公司说明。若您在进行注册程序过程中点击"同意"按钮即表示本公司已按您的要求予以说明，并表示您完全理解并接受该部分内容。 
1.3客户承诺自己在使用号百商旅电子商务有限公司提供的服务时，实施的所有行为均遵守国家相关法律、法规、部门规章和号码百事通公司的相关规定，亦不违反社会公共利益或公共道德。客户从事非法活动或不正当交易产生的一切后果与责任，由客户独立承担。
第二条	客户的账号、密码和安全
2.1客户账号和密码是客户重要的个人资料，客户务必注意账号和密码的保密；客户应按照机密的原则设置和保管自设密码，避免使用姓名、生日、电话号码等与本人明显相关的信息作为密码，不应将本人自设密码提供给除法律规定必须提供之外的任何人；客户应采取合理措施，防止本人密码被窃取，由于密码泄露造成的任何后果由客户自行承担。
2.2凡使用客户账号和密码办理的一切业务，均视为客户亲自办理的业务，由客户承担由此所导致的相关后果和责任，包括但不限于业务费用的支付等。
2.3号百商旅电子商务有限公司有相应的安全措施来保障客户的交易安全，但并不保证绝对安全。
第三条	双方的权利义务
3.1客户进行缴费的帐户，必须为本人属有，并有可支付额度。一旦客户点击确认支付，亦不得要求变更或撤销该指令。
3.2客户不得以与第三方发生纠纷为理由而拒绝支付使用本服务的应付款项。
3.3客户应保证提供的资料真实、准确、完整、合法。对于因客户提供信息不真实或不完整所造成的损失由客户自行承担。客户相关信息发生变化时，应即时更新，号百商旅电子商务有限公司不承担由于客户未及时更新相关信息所导致的相关责任。
3.4号百商旅电子商务有限公司有权根据经依法核准或备案的资费标准，向客户收取相关费用：通过号百商旅电子商务有限公司办理的业务，若该业务须缴纳相应的月租费及通信费，则相关的费用将在客户的号码百事通账户账户中一并收取，号百商旅电子商务有限公司不再另行通知。
3.5本帐户运营由号百商旅电子商务有限公司授权天翼电子商务有限公司执行，客户有任何疑问或发现交易指令未执行、未适当执行、延迟执行的，应第一时间通过拨打客服电话4008011888或通过在线客服通知天翼电子商务有限公司。号百商旅电子商务有限公司可以通过电话、短信、邮件、媒体广告等方式向客户告知处理进展或推荐业务。
3.6如客户在号百商旅电子商务有限公司帐户平台上存在违法行为或违反本协议的行为，或因客户此前使用本服务的行为引发争议的，号百商旅电子商务有限公司仍可行使追究的权利。客户同意，号百商旅电子商务有限公司不对因下述任一情况而发生的服务中断或终止而承担任何赔偿责任：
3.6.1 号百商旅电子商务有限公司基于单方判断，认为客户已经违反本服务条款的规定，将中断或终止向客户提供号百商旅电子商务有限公司部分或全部服务功能，并将相关资料加以删除。
3.6.2号百商旅电子商务有限公司在发现客户注册资料虚假、异常交易或有疑义或有违法嫌疑时，不经通知有权先行中断或终止客户的账户、密码，并拒绝客户使用号百商旅电子商务有限公司部分或全部服务功能。
第四条	免责事项
4.1号百商旅电子商务有限公司仅对本协议中列明的责任范围负责。除本协议另有规定外，在任何情况下，本公司对本协议所承担的违约赔偿责任总额应不超过所收取的当次服务费用总额。
4.2号百商旅电子商务有限公司仅以现有的、按其现状提供有关的信息、材料及运行。号百商旅电子商务有限公司不对其所提供的材料和信息的可用性、准确性或可靠性做出任何种类的保证，包括但不限于对所有权，知识产权，风险，不侵权，准确性，可靠性，适销性，使用正确性，适合特定用途或其它适当性。
4.3号百商旅电子商务有限公司不保证提供的服务一定能满足客户的要求，不保证服务不会中断，不保证服务的绝对及时、安全、真实和无差错，也不保证客户发送的信息一定能完全准确、及时、顺利地被传送。
4.4不论何种情形，号百商旅电子商务有限公司都不对任何由于使用或无法使用号百商旅电子商务有限公司提供的服务所造成间接的、附带的、特殊的或余波所及的损失、损害、债务或商务中断承担任何责任。
4.5号百商旅电子商务有限公司不承担交易货物的交付责任，因货物延迟送达或在送达过程中的丢失、损坏等，应由您与交易对方自行处理。包括客户的账单逾期，导致其交易的结果为销账失败和由此产生的违约金或赔偿金等。
第五条 法律适用及协议解释
5.1本协议的成立、生效、履行和解释，均适用中华人民共和国法律； 在法律允许范围内，本协议由号百商旅电子商务有限公司负责解释。
第六条争议的解决 
6.1客户和号百商旅电子商务有限公司在履行本协议的过程中，如发生争议，应友好协商解决。协商不成的，任何一方均可向北京仲裁委员会申请仲裁。
第七条协议生效和效力
7.1本协议自客户点击“同意”按钮时生效。本协议的任何条款如因任何原因而被确认无效，都不影响本协议其他条款的效力。
7.2号百商旅电子商务有限公司有权对本协议所有条款适时进行修改，如果客户对此持有异议，可以选择终止本协议。如果继续使用本协议项下的相关服务，则视为客户已经完全接受本协议相关条款的修改。  
</textarea>    
    
    <div class="conExplain">
        <p class="blue">什么是号码百事通账户？</p>
        <p>号码百事通账户是针对翼游旅行网和翼购商城的专享账户，可购买网站上机票、酒店、旅游线路、景点门票、订餐、各地特产、日用商品等各项产品。</p>
    </div>
</div>

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
        if($(".selectValue").children().length>0){
			var jQueryselect=$(this);
			jQueryselect.siblings("div").css({"left":jQueryselect.position().left+"px","top":jQueryselect.position().top+30+"px"}).slideDown("fast").children("a").click(function(){
				jQueryselect.attr("value",$(this).text());
				$(this).parent("div").hide();
				//$("#certtype").val($(this).attr("data"));
				});
			$("body").click(function(){$(".selectValue").hide()});
		}
});
</script>
</body>
</html>

