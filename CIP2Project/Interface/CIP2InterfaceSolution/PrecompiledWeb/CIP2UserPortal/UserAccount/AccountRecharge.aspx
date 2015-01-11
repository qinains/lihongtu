<%@ page language="C#" autoeventwireup="true" inherits="UserAccount_AccountRecharge, App_Web_4a2iedvw" enableEventValidation="false" %>

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
        .rechargeBox1{width:863px;height:58px;border:1px solid #8fc4e0;margin:10px auto;padding:20px 0 20px 115px;background:url(images/bg_03.png) repeat-x;font-size:14px}
        .rechargeBox1 p{line-height:22px;margin-bottom:5px}
        .rechargeBox1 p strong{color:#41ab01}

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
    </style>

    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="scripts/jquery.artDialog.js?skin=local"></script>
    <script language="javascript" type="text/javascript" src="../JS/jquery-latest.pack.js"></script>
    
</head>
<body>
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
    <uc1:CommonHead ID="CommonHead1" runat="server"></uc1:CommonHead>
    <div class="rechargeBox1">
        <p>
            充值账户：<%=BestPayAccount %></p>
        <p>
            <ul class="price">
                <li>账户余额：</li>
                <li>
                    <img style="float: left; display: block" id="imgWait" src="images/runing.gif" width="20"
                        height="20">
                    <strong><span id="spanBalance" style="display: none; color: Red"></span></strong>
                    &nbsp; </li>
                <li>元</li>
            </ul>
        </p>
    </div>
    <div class="rechargeBox2">
        <dl>
            <dt>请选择充值方式</dt>
            <dd class="rechargeDd">
                <a href="javascript:void(0)" pos="0">号码百事通卡</a>
                <a href="javascript:void(0)" pos="1"> 百事购卡</a> 
                <a href="javascript:void(0)" pos="2">网银充值</a>
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
                <font style="font-family: tahoma,arial,宋体; color: #ff5243; font-size: 12px;">
                    <span id="spanErrMsg0"></span></font>
            </label>
            <br />
            <div style="padding-left: 40px; padding-top: 5px">
                <font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">提示：消费卡中的余额将会全额充入您的帐户，充值以后不能退款</font>
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
                <font style="font-family: tahoma,arial,宋体; color: red; font-size: 12px">
                <span id="spanErrMsg1"></span></font>
            </label>
            <br />
            <div style="padding-left: 40px; padding-top: 5px">
                <font style="font-family: tahoma,arial,宋体; color: #b2b2b2; font-size: 12px">提示：消费卡中的余额将会全额充入您的帐户，充值以后不能退款</font>
            </div>
                <input type="button" id="btnTwo" class="btn" value="立即充值" />
        </form>
        <!--个人网银充值-->
        <form id="form2" action="BankRechargeForm.aspx" method="post" target="_blank">
            <label>
                <font>充值金额</font>：<input type="text" id="txtBalance" name="TranAmount" class="input" /></label>元
            <label>
                <font style="font-family: tahoma,arial,宋体; color: #ff5243; font-size: 12px">
                <span id="spanErrMsg2"></span></font>
            </label>
            <input type="submit" value="立即充值" class="btn" id="btnThree" />
            <input type="hidden" id="hiddenCustID" name="hiddenCustID" value="" />
            <input type="hidden" id="hiddenSPID" name="hiddenSPID" value="" />
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
                url:"HttpHandler/GetInfoHandler.ashx",
                dataType:"JSON",
                data:{BestPayAccount:account,type:"QueryAcountBalance",time:time.getTime()},
                beforeSend: function(XMLHttpRequest) {
                    $("#imgWait").css("display","block");
                    $("#spanBalance").css("display","none");
                },
                complete: function(XMLHttpRequest, textStatus) {
                    $("#imgWait").css("display","none");
                    $("#spanBalance").css("display","block");
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
            $("#btnOne").click(function(){
                var cardNo = $("#txtCardNoOne").val();
                var pwd = $("#txtPwdOne").val();
                $("#spanErrMsg0").text("");
                if(cardNo == ""){
                    $("#spanErrMsg0").text("请输入充值卡号");
                    return false;
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
                url:"HttpHandler/AccountRechargeHandler.ashx",
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
                            
                            if(item["errorcode"]=="200023"){
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
                                $("#payMsgInfo").text("充值失败（提示：卡内余额不足）");
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
