<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountRecharge.aspx.cs"
    Inherits="UserAccount_AccountRecharge" %>

<%@ Register Src="../UserCtrl/CommonHead.ascx" TagName="CommonHead" TagPrefix="uc1" %>
<%@ Register Src="../UserCtrl/CommonFoot.ascx" TagName="CommonFoot" TagPrefix="uc2" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>账户充值</title>
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    <link href="css/pay.css" type="text/css" rel="stylesheet" media="screen" />
    <style type="text/css">
        
        .rechargeBox1{width:863px;height:68px;border:1px solid #8fc4e0;margin:10px auto;padding:10px 0 20px 115px;background:url(images/bg_03.png) repeat-x;font-size:14px}
        .rechargeBox1 p{line-height:22px;margin-bottom:5px}
        .rechargeBox1 p strong{color:#41ab01}
        .rechargeBox1 p span{color:#f60;font-weight:700;margin:0 3px}

        .rechargeBox2{width:863px;border:1px solid #8fc4e0;margin:10px auto;padding:0 0 20px 115px;font-size:14px}
        .rechargeBox2 dl{margin-left:-115px;height:48px;background:url(images/bg_03.png) repeat-x}
        .rechargeBox2 dt{color:#007bc1;line-height:48px;margin-left:80px;font-weight:700}
        .rechargeBox2 dd{width:600px;height:59px;margin-left:230px;margin-top:-48px}
        .rechargeBox2 dd a{display:inline-block;*display:inline;zoom:1;width:141px;height:59px;margin-right:10px;line-height:48px;color:#007bc1;text-align:center;font-weight:700}
        .rechargeBox2 dd a:hover,.rechargeBox2 dd a.hover{text-decoration:none;background:url(images/bg_07.png);color:#fff}
        .rechargeBox2 form{margin-top:30px;display:none}
        .rechargeBox2 form label{margin-bottom:10px;margin-right:10px}
        .rechargeBox2 form .input{width:125px;height:18px;line-height:18px;border:1px solid #ccc;padding:5px}
        .btn{
            width:108px;
            height:38px;
            margin-left:40px;
            margin-top:20px;
            border:none;
            background:url(images/bg_sprite.png) no-repeat 0 -38px;
            cursor:pointer;
            display:block;
            text-indent:-9999px}
        .btnUnEnable{
            width:108px;
            height:38px;
            margin-left:40px;
            margin-top:20px;
            border:none;
            background:url(images/bg_sprite.png) no-repeat 0 -38px;
            background-position:0 -114px;
            cursor:pointer;
            display:block;
            text-indent:-9999px}
        
        .price,.price li{ float:left; margin:0; padding:0; }
        .price li{list-style-type: none;}
        
         /*4.10新增内容*/
        .rechargeBox2 form .input{font-size:14px}
        .rechargeBox2 form li{margin-bottom:15px;line-height:30px;position:relative;padding-left:80px}
        .rechargeBox2 form li.bill{display:none}
        .rechargeBox2 form li strong{display:block;width:70px;text-align:right;font-weight:400;position:absolute;left:0;_left:-80px}
        .rechargeBox2 form li textarea{width:400px;height:100px;padding:5px 0 5px}
        .rechargeBox2 form .radio{margin:0 10px}
        .selectValue{width:135px;height:150px;overflow-y:scroll;margin:0;border:1px solid #ccc;position:absolute;top:32px;left:100px;background:#fff;display:none;z-index:99}
        .selectValue a{display:block;height:30px;line-height:30px;padding-left:5px;text-align:left;margin:0;color:#39c;text-decoration:none}
        .selectValue a:hover{background:#39c;color:#fff}
       
        
    </style>

    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="scripts/jquery.artDialog.js?skin=local"></script>
    <script language="javascript" type="text/javascript" src="../JS/jquery-latest.pack.js"></script>
    <script type="text/javascript">
        //window.location.href="../SystemUpgrade.htm";
    </script>
</head>
<body>
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
    <uc1:CommonHead ID="CommonHead1" runat="server"></uc1:CommonHead>
    <div class="rechargeBox1">
        <p>
            充值账户：<%=BestPayAccount %></p>
        <p>
            账户余额：<img id="imgWait" src="images/runing.gif" width="20" height="20"/><strong><span
                id="spanBalance" style="display: none; color: red"></span></strong>元</p>
        <p>
            <font color="red">充值提示</font>：单笔充值限额为<span><%=OnceRechargeLimited%></span>元；当日累计充值限额为<span><%=DayRechargeLimited%></span>元；账户余额上限为<span><%=AccountBalanceLimited %></span>元。</p>
    </div>
    <div class="rechargeBox2">
        <dl>
            <dt>请选择充值方式</dt>
            <dd class="rechargeDd">
                <a href="javascript:void(0)" pos="0">号码百事通卡</a> <a href="javascript:void(0)" pos="1">
                    百事购卡</a> <a href="javascript:void(0)" pos="2">网银充值</a>
            </dd>
        </dl>
        <!--号码百事通卡充值-->
        <form id="form0">
            <label>
                <font>卡号</font>：<input type="text" id="txtCardNoOne" style="width: 200px" class="input"
                    onkeyup="this.value=this.value.replace(/[^0-9|a-z|A-Z]/,'')" onafterpaste="this.value=this.value.replace(/[^0-9|a-z|A-Z]/,'')" /></label>
            <label>
                <font>密码</font>：<input type="password" id="txtPwdOne" class="input" /></label>
            <label>
                <font style="font-family: tahoma,arial,宋体; color: #ff5243; font-size: 12px;"><span
                    id="spanErrMsg0"></span></font>
            </label>
            <br />
            <div style="padding-left: 40px; padding-top: 5px">
                <table border="0">
                    <tr><td><font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">提示：</font></td></tr>
                   <tr><td><font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">卡中的余额将会全额充入您的帐户，充值后不可退回消费卡</font></td></tr>
                  <tr><td><font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">本卡在销售发放时已开具发票，故购物时不再开具购物发票</font></td></tr>  
                </table>
            </div>
            <input type="button" id="btnOne" class="btn" value="立即充值" />
        </form>
        <!--百事购卡充值-->
        <form id="form1">
            <label>
                卡号：<input type="text" id="txtCardNoTwo" style="width: 200px" class="input" /></label>
            <label>
                密码：<input type="password" id="txtPwdTwo" class="input" /></label>
            <label>
                <font style="font-family: tahoma,arial,宋体; color: red; font-size: 12px"><span id="spanErrMsg1">
                </span></font>
            </label>
            <br />
            <div style="padding-left: 40px; padding-top: 5px">
                <table border="0">
                    <tr><td><font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">提示：</font></td></tr>
                   <tr><td><font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">卡中的余额将会全额充入您的帐户，充值后不可退回消费卡</font></td></tr>
                  <tr><td><font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">本卡在销售发放时已开具发票，故购物时不再开具购物发票</font></td></tr>  
                </table>           
            </div>
            <input type="button" id="btnTwo" class="btn" value="立即充值" />
        </form>
        <!--个人网银充值-->
        <form id="form2" action="BankRechargeForm.aspx" method="post" target="_blank">


          <ul>
          
   		        <li><strong>充值金额：</strong><input type="text" id="txtBalance" name="TranAmount" class="input" /></li>
                <li>
                    <font style="font-family: tahoma,arial,宋体; color: #ff5243; font-size: 12px"><span  id="spanErrMsg2"></span></font>
                </li>
        	    <li><strong>发票提醒：</strong><label><input type="radio"  name="NeedInvoiceFlag" value="0" class="radio" checked/>不需要发票</label><label><input type="radio"  name="NeedInvoiceFlag" class="radio" value="1"/>需要发票</label>
            	        <p class="red">提示：<br />如需发票，请选发票选项，我们提供机打充值发票。<br /> 通过号码百事通账户消费将不再提供购物发票。  </p>
                </li>
                    <li class="bill">
            	        <strong>发票内容：</strong>
                        <input name="select" type="text" value="请选择" readonly="readonly" class="input select" />
                        <input  id="InvoiceContent" name="InvoiceContent"  type="hidden" value="" />
                        <div class="selectValue">
                            <a href="javascript:void(0)" data="1">工艺品</a>
                            <a href="javascript:void(0)" data="2">服装费</a>
                            <a href="javascript:void(0)" data="3">会务费</a>
                            <a href="javascript:void(0)" data="4">服务费</a>
                            <a href="javascript:void(0)" data="5">咨询费</a>
                            <a href="javascript:void(0)" data="6">资料费</a>
                            <a href="javascript:void(0)" data="7">布展费</a>
                            <a href="javascript:void(0)" data="8">广告费</a>
                            <a href="javascript:void(0)" data="9">宣传费</a>
                            <a href="javascript:void(0)" data="10">票务费</a>
                            <a href="javascript:void(0)" data="11">信息费</a>
                            <a href="javascript:void(0)" data="12">酒</a>
                            <a href="javascript:void(0)" data="13">号码百事通充值卡</a>
                            <a href="javascript:void(0)" data="14">手机礼品兑换卡</a>
                            <a href="javascript:void(0)" data="15">礼品</a>
                            <a href="javascript:void(0)" data="16">礼盒</a>
                            <a href="javascript:void(0)" data="17">提货券</a>  
                        </div>
                    </li>
                   
                    
                    <li class="bill">
	                    <strong>发票抬头：</strong>
	                    <label><input type="radio" name="t_InvoiceType" value="0" class="radio" checked />个人</label>
	                    <label><input type="radio" name="t_InvoiceType" class="radio" value="1" />公司</label><span class="red ml5">注意：公司请填写公司名称</span>
	                    <input type="text" name="InvoiceTitle" id="InvoiceTitle" class="input ml10 J-company none" style="width:300px" />
                    </li>                   
                    
                    <li class="bill"><strong>联系人：</strong><input type="text" id="ContactPerson" name="ContactPerson" class="input"/><span class="red ml5">*</span></li>
                    <li class="bill"><strong>手机：</strong><input type="text" id="ContactPhone" name="ContactPhone" class="input"/><span class="red ml5">*</span></li>
                    <li class="bill"><strong>邮寄地址：</strong><input type="text" id="Address" name="Address" class="input" style="width:300px"/><span class="red ml5">*</span></li>
                    <li class="bill"><strong>邮编：</strong><input type="text" id="Zip" name="Zip" class="input"/></li>
                    <li class="bill"><strong>备注：</strong><textarea  id="Mem" name="Mem"></textarea></li>
          
          </ul>
            
            <input type="submit" value="立即充值" class="btn" id="btnThree" />
            <input type="hidden" id="hiddenCustID" name="hiddenCustID" value="" />
            <input type="hidden" id="hiddenSPID" name="hiddenSPID" value="" />
            <input type="hidden"  id="NeedInvoice" name="NeedInvoice" value="0" /> 
            <input type="hidden"  id="InvoiceType" name="InvoiceType" value="0" />  
        </form>
    </div>
    <form id="Form4" runat="server">
        <asp:HiddenField ID="hdCustID" runat="server" />
        <asp:HiddenField ID="hdHeadFoot" runat="server" />
        <asp:HiddenField ID="hdSPID" runat="server" />
        <asp:HiddenField ID="hdIsNeedHeadFoot" runat="server" />
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
        <div id="goToPay" style="display: none; width: 400px;">
            <div style="height: 50px; padding-top: 10px; padding-bottom: 10px;">
                <div class="infoImg">
                    <img alt="在线充值提示" src="images/info.png" /></div>
                <div class="infoTip">
                    请您在新打开的充值页面完成充值，充值完成前请不要关闭本窗口！</div>
            </div>
            <div class="middle">
                <span><a href="#" onclick="completeRecharge();">
                    <img class="payButton" src="images/finishRecharge.gif" /></a></span>&nbsp; <span><a
                        href="#" id="A1" onclick="continueRecharge()">
                        <img class="payButton" src="images/continueRecharge.gif" /></a></span>
            </div>
            <div class="bottom">
            </div>
        </div>
        <div id="goToRecharge" style="display: none; width: 400px;">
            <div id="divWaitingPay" style="background: url(images/load.gif) no-repeat 50% 0;
                text-align: center; padding-top: 25px; margin-top: 25px; display: block; font-size: 14px">
                充值正在进行，请勿关闭窗口</div>
            <div id="msgDiv" style="height: 50px; padding-top: 10px; padding-bottom: 10px; display: none">
                <div class="infoImg">
                    <img alt="在线充值提示" src="images/info.png" /></div>
                <div id="payMsgInfo" class="infoTip">
                </div>
            </div>
            <div id="btnDiv" class="middle" style="display: none">
                <span><a href="#" onclick="completeRecharge();">
                    <img id="imgComplete" class="payButton" src="images/finishRecharge.gif" /></a></span>
                &nbsp; <span><a href="#" id="btnContinue" onclick="continueRecharge()">
                    <img id="imgContinute" class="payButton" src="images/continueRecharge.gif" /></a></span>
            </div>
        </div>
    </form>
    <uc2:CommonFoot ID="CommonFoot1" runat="server"></uc2:CommonFoot>
    
    <script type="text/javascript">
        
        $(document).ready(function(){
            var time = new Date();
            var account = <%=BestPayAccount %>;
            $.ajax({
                type:"post",
                url:"ajax/QueryInfoAjax.aspx",
                dataType:"JSON",
                data:{BestPayAccount:account,type:"QueryAcountBalance",time:time.getTime()},
                beforeSend: function(XMLHttpRequest) {
                    $("#imgWait").css("display","inline");
                    $("#spanBalance").css("display","none");
                },
                complete: function(XMLHttpRequest, textStatus) {
                    $("#imgWait").css("display","none");
                    $("#spanBalance").css("display","inline");
                },
                success:function(data,textStatus){
                    var dataJson = eval(""+data+"");
                    $.each(dataJson,function(index,item){
                        if(item["result"]=="true"){
                            $("#spanBalance").text(item["balance"]);
                        }
                    })
                },
                error:function(){
                    
                }
            })
        })
        
        $("form").eq(0).show();
        $(".rechargeDd").children("a").eq(0).addClass("hover");
        $(".rechargeDd").children("a").click(function(){
	        $(this).addClass("hover").siblings().removeClass("hover");
	        $("form").eq($(this).attr("pos")).show().siblings("form").hide();
        });
        
        $(document).ready(function(){
        
            //4.10新增


           $("#btnThree").click(function(){
          
                   if($("#NeedInvoice").val()=='1')
                   {
                       if($("#ContactPerson").val()=='')
                       {
                            $("#ContactPerson").focus();
                            return false; 
                       } 
                        
                      if($("#ContactPhone").val()=='')
                      {
                            $("#ContactPhone").focus();
                            return false;
                      } 
                      
                      if($("#Address").val()=='')
                      {
                            $("#Address").focus();
                            return false;
                      }
                   } 
            });

             if($("#NeedInvoice").val()=='1')
             {
                 $(".bill").show();	
             }else
             {
                   $(".bill").hide();  
             }
            

            $("input[name='NeedInvoiceFlag']").click(function(){
                $("#NeedInvoice").val($(this).val());  
	            if($(this).val()=="1"){
		            $(".bill").show();	
	            }else{
		            $(".bill").hide();
	            }	
            });
            
           
            $("input[name='t_InvoiceType']").click(function(){
                 
	            if($(this).val()=="1"){
		            $(".J-company").show();	
	            }else{
		            $(".J-company").hide();
	            }	
	            $("#InvoiceType").val($(this).val());
            });
           
            $("input.select").click(function(){
	            var jQueryselect=$(this);
	            jQueryselect.siblings("div").css({"left":jQueryselect.position().left+"px","top":jQueryselect.position().top+30+"px"}).slideDown("fast").children("a").click(function(){
		            jQueryselect.attr("value",$(this).text());
		            $(this).parent("div").hide();
		            $("#InvoiceContent").val(jQueryselect.attr("value"));
		            });
	            $("body").click(function(){$(".selectValue").hide()});
            });
            //4.10新增over
                    
        
        
        
        
            $("#btnOne").click(function(){
                var cardNo = $("#txtCardNoOne").val();
                var pwd = $("#txtPwdOne").val();
                $("#spanErrMsg0").text("");
                if(cardNo == ""){
                    $("#spanErrMsg0").text("请输入充值卡号");
                    return false;
                }else{
                   if(cardNo.substring(0,6)!='888800' && cardNo.substring(0,4)!='8811'){
                        $("#spanErrMsg0").text("您输入的卡号不正确，请核对后再次输入，谢谢！");
                        return false;
                   }
                }
                if(pwd == ""){
                    $("#spanErrMsg0").text("请输入卡号密码");
                    return false;
                }
                CardRechargeFun($(this),cardNo,pwd,"2");
            })
            
            $("#btnTwo").click(function(){
                var cardNo = $("#txtCardNoTwo").val();
                var pwd = $("#txtPwdTwo").val();
                $("#spanErrMsg1").text("");
                if(cardNo == ""){
                    $("#spanErrMsg1").text("请输入充值卡号");
                    return false;
                }
                if(pwd == ""){
                    $("#spanErrMsg1").text("请输入卡号密码");
                    return false;
                }
                CardRechargeFun($(this),cardNo,pwd,"3");
            })
            $("#form2").submit(function(){
                var balance = $("#txtBalance").val();
                $("#spanErrMsg2").text("");
                if(balance==""){
                    $("#spanErrMsg2").text("请输入充值金额");
                    return false;
                }
                var regRex = /^\d+(\.\d{1,2})?$/;
                if(!regRex.test(balance)){
                    $("#spanErrMsg2").text("金额必须为整数或小数，小数点后不超过2位");
                    return false;
                }
                if(parseFloat(balance)<=0){
                    $("#spanErrMsg2").text("请输入有效金额");
                    return false;
                }
                $("#hiddenCustID").val($("#hdCustID").val());
                $("#hiddenSPID").val($("#hdSPID").val());
                
                var myDialog = art.dialog({
			        title: "在线充值",
			        fixed: true,
			        lock: true,
			        drag: false,
			        esc: false,
			        resize: false,
			        opacity: 0.35,
			        width: 450,
			        height: 200
		        });
		        myDialog.content(document.getElementById("goToPay"));
		        $("#btnThree").attr("class","btnUnEnable");
		   		$("#btnThree").attr("disabled","false");
            })
        })
    </script>
    
    <script type="text/javascript">

        continueRecharge = function(){
            window.location.reload();
        }
        //完成充值
        completeRecharge = function(){
            var returnUrl = $("#hdReturnUrl").val();
            if(returnUrl==""){
                window.close();
            }else{
                window.location.href=returnUrl;
            }
        }
        //卡充值
        CardRechargeFun = function(obj,cardNo,cardPwd,cardType){
            var myDialog = art.dialog({
                title: "在线充值",
   				fixed: true,
   				lock: true,
   				drag: false,
   				esc: false,
   				resize: false,
   				opacity: 0.35,
   				width: 450,
   				height: 200
            });
            var custid = $("#hdCustID").val();
            var spid = $("#hdSPID").val();
            var time = new Date();
            $.ajax({
                type:"post",
                url:"ajax/RechargeAjax.aspx",
                dataType:"JSON",
                data:{CustID:custid,SPID:spid,CardNo:cardNo,CardPassword:cardPwd,CardType:cardType,Type:"AccountRechargeByCard",time:time.getTime()},
                beforeSend: function(XMLHttpRequest) {
                    myDialog.content(document.getElementById("goToRecharge"));
		   			obj.attr("class","btnUnEnable");
		   			obj.attr("disabled","false");
                },
                complete: function(XMLHttpRequest, textStatus) {
                    $("#divWaitingPay").css("display","none");
                    $("#msgDiv").css("display","block");
                    $("#btnDiv").css("display","block");
                },
                success:function(data,textStatus){
                    var dataJson = eval("("+data+")");
                    $.each(dataJson,function(index,item){
                        if(item["result"]=="true"){
                            
                            $("#imgComplete").attr("src","images/finishRecharge.gif");
                            $("#imgContinute").attr("src","images/continueRecharge.gif");
                            $("#payMsgInfo").html("您已成功向账户充值<font color=red>"+item["deductionBalance"]+"</font>元，账户余额为&nbsp;<font color=red>"+item["accountBalance"]+"</font>&nbsp;元");
                            
                        }else if(item["result"]=="false" && item["step"]=="query"){
                            if(item["errorcode"]=="100001"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：您今日已累计充值"+item["rechargeamount"]+"元，本次充值将超过您的当日累计充值限额"+item["rechargeamountlimit"]+"元，请您明日以后再进行充值。）");
                            }else if(item["errorcode"]=="100002"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：您的账户余额为"+item["accountbalance"]+"元，本次充值将超过您的账户余额上限"+item["CurrentAmountLimit"]+"元，请您在消费适当余额后再进行充值。）");
                            }else if(item["errorcode"]=="100003"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：卡余额为"+item["rechargeamount"]+"元，超过单笔充值限额"+item["rechargeamountlimit"]+"元。）");
                            }else if(item["errorcode"]=="200010" || item["errorcode"]=="200011"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：无效或未激活的卡号）");
                            }else if(item["errorcode"]=="200020"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：卡内余额为0，无法充值）");
                            }else if(item["errorcode"]=="200023"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：卡密码错误次数超限被锁定）");
                            }else{
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：请确认卡号密码输入有效）");
                            }
                            
                        }else if(item["result"]=="false" && item["step"]=="deduction"){
                            
                            if(item["errorcode"]=="200020"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：卡内余额为0，无法充值）");
                            }else if(item["errorcode"]=="200022"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：请确认卡号密码输入有效）");
                            }else if(item["errorcode"]=="200023"){
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：卡密码错误次数超限被锁定）");
                            }else{
                                $("#imgComplete").attr("src","images/finishReturn.gif");
                                $("#imgContinute").attr("src","images/continueRecharge.gif");
                                $("#payMsgInfo").text("充值失败（提示：请确认卡号密码输入有效）");
                            }
                            
                        }else if(item["result"]=="false" && item["step"]=="recharge"){
                            
                            $("#imgComplete").attr("src","images/finishRecharge.gif");
                            $("#imgContinute").attr("src","images/continueRecharge.gif");
                            $("#payMsgInfo").text("充值申请已提交，系统正在处理中……请耐心等待");
                            
                        }else{
                            
                            $("#imgComplete").attr("src","images/finishReturn.gif");
                            $("#imgContinute").attr("src","images/continueRecharge.gif");
                            $("#payMsgInfo").text("充值失败（提示：请确认卡号密码输入有效）");
                            
                        }
                    })
                },
                error:function(){
                    $("#imgComplete").attr("src","images/finishReturn.gif");
                    $("#imgContinute").attr("src","images/continueRecharge.gif");
                    $("#payMsgInfo").text("充值失败！");
                }
            })
        }
        
    </script>
    
</body>
</html>
