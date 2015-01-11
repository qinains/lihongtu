<%@ page language="C#" autoeventwireup="true" inherits="UserAccount_DetailQuery, App_Web_detailquery.aspx.74181853" enableEventValidation="false" %>

<%@ Register Src="../UserCtrl/CommonHead.ascx" TagName="CommonHead" TagPrefix="uc1" %>
<%@ Register Src="../UserCtrl/CommonFoot.ascx" TagName="CommonFoot" TagPrefix="uc2" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="../UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>交易明细</title>
    <meta name="description" content="" />
    <meta name="keywords" content="机票 国内机票 特价机票 旅行度假 商旅管理 号码百事通" />
    <link rel="stylesheet" href="http://static.118114.cn/css/base.css" />
    <link href="~/UserAccount/css/myhome.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .liC{margin:4px;clear:both;height:37px;border-bottom:1px solid #e0e0e0}
        .liC li{float:left;margin-right:-1px;width:102px;height:37px;line-height:37px;text-align:center;color:#666; background:url(images/icon1.jpg) no-repeat 0 3px;cursor:pointer}
        .liC li.vazn{ background:url(images/icon1.jpg) no-repeat;color:#f60;font-weight:700}
        .butA{margin:0 auto;background:url(images/but_1.png) no-repeat!important;width:66px;height:21px;line-height:21px;color:#39c;font-size:12px}
        .blue,a.blue:visited{color:#39c!important;text-decoration:none;font-weight:700}
        a.blue:hover{text-decoration:underline}
        .orange,a.orange:visited{color:#f60!important;text-decoration:none;font-weight:700}
        a.orange:hover{text-decoration:underline}
        html body{background:#fff}
    </style>

    <script type="text/javascript" src="scripts/tab.js"></script>
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
    <script language="javascript" type="text/javascript" src="../ModelJS/Date/WdatePicker.js"></script>

</head>
<body>
    <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
    <uc1:CommonHead ID="CommonHead1" runat="server"></uc1:CommonHead>
    <form id="form1" runat="server">
        <div class="air_boxB clfx" style="width: 790px; padding: 0 10px 10px; margin: 10px auto">
            <ul class="liC">
                <li class="vazn" onclick="tabconAlt(0,'E',8);" id="tabE0">近三个月交易</li>
                <li onclick="tabconAlt(1,'E',8);" id="tabE1">三个月前交易</li>
            </ul>
            <div id="conE0">
                <div class="details">
                    <dl class="dlist">
                        <%--<dt><span style="width: 100px">日期</span><span style="width: 216px">交易摘要</span><span style="width: 79px">交易金额(元)</span><span style="width: 158px">商户名称</span><span style="width: 119px">交易类型</span><span style="width: 79px">状态</span></dt>--%>
                        <dt><span style="width: 158px">流水号</span><span style="width: 275px">交易摘要</span><span style="width: 98px">交易金额(元)</span><span style="width: 100px">订单日期</span><span style="width: 60px">交易类型</span><span style="width: 60px">订单状态</span></dt>
                        <dd id="ddRecordListOne" style="height: 400px; overflow-y: auto; display: none">
                            
                        </dd>
                        <dd id="ddNoRecordOne" style="text-align: center; display: none; padding-top:10px; padding-bottom:10px; letter-spacing:10px">
                            没有交易记录
                        </dd>
                        <dd id="ddWaitQueryOne" style="text-align: center; display: block; padding-top:10px; padding-bottom:10px">
                            <img src="../images/large-loading.gif" />
                        </dd>
                    </dl>
                </div>
            </div>
            <div id="conE1" style="display: none">
                <div class="details">
                    <dl class="dlist">
                        <dt><span style="width: 158px">流水号</span><span style="width: 275px">交易摘要</span><span style="width: 98px">交易金额(元)</span><span style="width: 100px">订单日期</span><span style="width: 60px">交易类型</span><span style="width: 60px">订单状态</span></dt>
                        <dd id="ddRecordListTwo" style="height: 400px; overflow-y: auto; display: none">
                            
                        </dd>
                        <dd id="ddNoRecordTwo" style="text-align: center; display: none; padding-top:10px; padding-bottom:10px; letter-spacing:10px">
                            没有交易记录
                        </dd>
                        <dd id="ddWaitQueryTwo" style="text-align: center; display: block; padding-top:10px; padding-bottom:10px">
                            <img src="../images/large-loading.gif" />
                        </dd>
                    </dl>
                </div>
            </div>
            <p class="mt10">提示：系统仅显示您两年之内的交易明细，更早的交易明细不再显示。</p>
           <p style="margin:5px 0">如对交易记录有疑问，请拨打号码百事通账户服务热线：<strong style="color:#39c">4008-011-888</strong>。（号码百事通账户合作伙伴：天翼电子商务有限公司）</p> 
        </div>
        <asp:HiddenField ID="hdStartDate" runat="server" />
        <asp:HiddenField ID="hdEndDate" runat="server" />
        <asp:HiddenField ID="hdStatus" runat="server" />
        <asp:HiddenField ID="hdHeadFoot" runat="server" />
    </form>
    <uc2:CommonFoot ID="CommonFoot1" runat="server"></uc2:CommonFoot>

    <script type="text/javascript">
        $(document).ready(function(){
            //设置头和尾是否显示
            var isNeed = $("#hdHeadFoot").val();
            if(isNeed=="0"){
                $("#divHead,#divFoot").css("display","none");
            }else{
                $("#divHead,#divFoot").css("display","block");
            }
            
            getThreeMonthsHistoryDetails();
            getThreeMonthsBeforeHistoryDetails();
//            $("#tabE1").click(function(){
//                var status = $("#hdStatus").val();
//                if(status !="1"){
//                    getHistoryDetails();
//                }
//            })
        })
        getThreeMonthsHistoryDetails = function(){
            var time = new Date();
            var account = <%=BestPayAccount %>;
            $.ajax({
                type:"post",
                url:"ajax/QueryInfoAjax.aspx",
                dataType:"JSON",
                data:{Type:"GetThreeMonthsDetails",BestPayAccount:account,time:time.getTime()},
                beforeSend: function(XMLHttpRequest) {
		   			$("#ddNoRecordOne").css("display","none");
		   			$("#ddWaitQueryOne").css("display","block");
                },
                complete: function(XMLHttpRequest, textStatus) {
                    $("#ddWaitQueryOne").css("display","none");
                },
                success:function(data,textStatus){
                    
                    var dataJson = eval("("+data+")");
                    if(dataJson.result=="true"){
                        var html = "";
                        $.each(dataJson.txninfo,function(index,item){
                            html += "<p>";
                            html += "<span style=\"width:158px\">"+item["AcceptSeqNO"]+"</span>";
                            html += "<span style=\"width:275px\">"+item["TxnDscpt"]+"</span>";
                            html += "<span class=\"orange\" style=\"width:98px\">"+item["TxnAmount"]+"</span>";
                            html += "<span style=\"width:100px\">"+item["TxnTime"]+"</span>";
                            //html += "<span style=\"width:158px\">"+item["MerchantName"]+"</span>";
                            html += "<span style=\"width:60px\">"+item["TxnType"]+"</span>";
                            html += "<span style=\"width:60px\">"+item["CancelFlag"]+"</span>";
                            html += "</p>";                             
                        })
                        $("#ddRecordListOne").append(html).children("p:odd").css("background","#eee");
                        $("#ddRecordListOne").css("display","block");
                    }else if(dataJson.result=="NoData"){
                        $("#ddNoRecordOne").css("display","block");
                    }else{
                        $("#ddNoRecordOne").css("display","block");
                    }
                    $("#hdStatus").val("1");
                },
                error:function(data,textStatus){
                    $("#ddNoRecordOne").css("display","block");
                }
            })
        }
        getThreeMonthsBeforeHistoryDetails = function(){
            var time = new Date();
            var account = <%=BestPayAccount %>;
            $.ajax({
                type:"post",
                url:"ajax/QueryInfoAjax.aspx",
                dataType:"JSON",
                data:{Type:"GetThreeMonthsBeforeDetails",BestPayAccount:account,BestPayAccount:account,time:time.getTime()},
                beforeSend: function(XMLHttpRequest) {
		   			$("#ddNoRecordTwo").css("display","none");
		   			$("#ddWaitQueryTwo").css("display","block");
                },
                complete: function(XMLHttpRequest, textStatus) {
                    $("#ddWaitQueryTwo").css("display","none");
                },
                success:function(data,textStatus){
                    var dataJson = eval("("+data+")");
                    if(dataJson.result=="true"){
                        var html = "";
                        $.each(dataJson.txninfo,function(index,item){
                            html += "<p>";
                            html += "<span style=\"width:158px\">"+item["AcceptSeqNO"]+"</span>";
                            html += "<span style=\"width:275px\">"+item["TxnDscpt"]+"</span>";
                            html += "<span class=\"orange\" style=\"width:98px\">"+item["TxnAmount"]+"</span>";
                            html += "<span style=\"width:100px\">"+item["TxnTime"]+"</span>";
                            //html += "<span style=\"width:158px\">"+item["MerchantName"]+"</span>";
                            html += "<span style=\"width:60px\">"+item["TxnType"]+"</span>";
                            html += "<span style=\"width:60px\">"+item["CancelFlag"]+"</span>";
                            html += "</p>";                        
                        })
                        $("#ddRecordListTwo").append(html).children("p:odd").css("background","#eee");
                        $("#ddRecordListTwo").css("display","block");
                    }else if(dataJson.result=="NoData"){
                        $("#ddNoRecordTwo").css("display","block");
                    }else{
                        $("#ddNoRecordTwo").css("display","block");
                    }
                    $("#hdStatus").val("1");
                },
                error:function(data,textStatus){
                    $("#ddNoRecordTwo").css("display","block");
                }
            })
        }
    </script>

</body>
</html>
