<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountRechargeMobile.aspx.cs" Inherits="UserAccount_AccountRechargeMobile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta name="viewport" content="width=device-width, initial-scale=1"/> 
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<link href="css/mobile_accountcharge.css" type="text/css" rel="stylesheet" />
<link href="css/secceed.css" type="text/css" rel="stylesheet" />
<link href="css/pay.css" type="text/css" rel="stylesheet" media="screen" />
<style type="text/css">
     .btn{
            width:196px;
            height:42px;
            border:none;
            background:url(images/bt_submit.gif) no-repeat 0 -38px;
            cursor:pointer;
            display:block;
            text-indent:-9999px}
        .btnUnEnable{
            width:196px;
            height:42px;
            border:none;
            background:url(images/bt_submit.gif) no-repeat 0 -38px;
            background-position:0 -114px;
            cursor:pointer;
            display:block;
            text-indent:-9999px}
</style> 
<script type="text/javascript" >
   
        function sendToPhone(action, requestData) {
	        try {
		        if (/(iPhone|iPad|iPod)/i.test(navigator.userAgent)){
			        sendToIos(action, requestData);
		        }else{
			        sendToAndroid(action, requestData)
		        }
	        } catch (e) {
		        //console.log(e.stack);
	        }
        }

        function sendToIos(action, requestData){
            window.location="action="+action+"&requestData="+encodeURI("{'errcode':'0','errmsg':'success'}");
        }
        
        function sendToAndroid(action, requestData){
	        window.android.execute(action, requestData);
        }  
  
   
    </script>

<title>账户充值</title>

    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="scripts/jquery.artDialog.js?skin=local"></script>
    <script language="javascript" type="text/javascript" src="../JS/jquery-latest.pack.js"></script>
  
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
                    $("#spanBalance").css("display","none");
                },
                complete: function(XMLHttpRequest, textStatus) {
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
  
  
        function confirmCharge()
        {
               var cardNo = $("#txtCardNo").val();
               var pwd = $("#txtPwd").val();
               $("#spanErrMsg0").text("");
                if(cardNo == ""){
                    $("#spanErrMsg0").text("请输入充值卡号");
                    sendToPhone("complete",""); 
                    return false;
                }else{
                   if(cardNo.substring(0,6)!='888800' && cardNo.substring(0,4)!='8811'){
                        $("#spanErrMsg0").text("您输入的卡号不正确，请核对后再次输入，谢谢！");
                        sendToPhone("complete","");
                        return false;
                   }
                }
                if(pwd == ""){
                    $("#spanErrMsg0").text("请输入卡号密码");
                    sendToPhone("complete",""); 
                    return false;
                }
                CardRechargeFun(cardNo,pwd,"2");        
        
        }
        
        $(document).ready(function(){
            $("#btnCharge").click(function(){
                var cardNo = $("#txtCardNo").val();
                var pwd = $("#txtPwd").val();
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
                CardRechargeFun(cardNo,pwd,"2");
            })        
       
        })

        //继续充值
        continueRecharge = function(){
            window.location.reload();
        }
        //完成充值
        completeRecharge = function(){
            //var returnUrl = $("#hdReturnUrl").val();
            //if(returnUrl==""){
                window.close();
                sendToPhone("complete","");
            //}else{
            //    window.location.href=returnUrl;
            //}
        }       
        
        //卡充值function(obj,cardNo,cardPwd,cardType)
        CardRechargeFun = function(cardNo,cardPwd,cardType){
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
		   			//obj.attr("class","btnUnEnable");
		   			//obj.attr("disabled","false");
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
    
</head>

<body>
<div style=" background:#fafafa; color:#959595;">
  <div class="top">
    <a href="#" class="back"></a>
    <p>帐户充值</p>
  </div>
  <div style=" border-bottom:1px solid #e5e5e5;">
    <div style=" width:320px; margin:0 auto; line-height:27px;">
      <div style="width:116px; float:left;">号码百事通帐号：<br />
        帐 号 余 额：</div>
      <div style="width:204px; float:left;"><%=BestPayAccount %><br />
          <strong><span   id="spanBalance" style="display: none; color: #ff6c00"></span></strong>元   
      </div>
    </div>
  </div>
  <div class="input">
    <div style=" width:320px; margin:0 auto;">
      <div style=" text-align:center; width:60px; float:left; line-height:35px;">
        <ul>
          <li style="padding:9px 0 5px 0;">卡 号：</li>
          <li style="padding:9px 0 5px 0;">密 码：</li>
        </ul>
      </div>
      <div style="width:260px; float:left;">
        <ul>
          <li style="padding:9px 0 5px 0;"><input type="text" name="txtCardNo" id="txtCardNo" style=" height:35px; width:232px; border:1px solid #d5d5d5; font-size:14px; padding:0 5px;" /></li>
          <li style="padding:9px 0 5px 0;"><input type="text" name="txtPwd" id="txtPwd" style=" height:35px; width:232px; border:1px solid #d5d5d5; font-size:14px; padding:0 5px;" /></li>
          <li style="padding:9px 0 5px 0;"><span   id="spanErrMsg0"></span></li>
        </ul>
      </div>
    </div>
  </div>
  <div style=" width:320px; margin:20px auto; line-height:20px;">
    <p>充值提示：单笔充值限额为<font color="#ff6c00"><%=OnceRechargeLimited%>元</font>；当日累计充值限额为 <font color="#ff6c00"><%=DayRechargeLimited%>元</font>；帐户余额上限为<font color="#ff6c00"><%=AccountBalanceLimited %>元</font>。</p>
    <p>&nbsp;</p>
    <p>提示：<br />
    卡中的余额将会全额冲入您的帐户，充值后不可退回消费卡本卡在销售发放时已开具发票，故购物时不在开具购物发票。</p>
  </div>
    <div style=" margin:0 0 50px 0; text-align:center;">
            <a href="#" onclick="javascript:confirmCharge();">
                <img width="196" height="42" src="images/bt_submit.gif"/>
            </a>
    </div>
</div>

   <form id="Form4" runat="server">
        <asp:HiddenField ID="hdCustID" runat="server" />
        <asp:HiddenField ID="hdSPID" runat="server" />
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

</body>
</html>
