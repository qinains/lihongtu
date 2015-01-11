<%@ page language="C#" autoeventwireup="true" inherits="UserAccount_BankRechargeForm, App_Web_bankrechargeform.aspx.74181853" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>
</head>
<body>
    <!--https://webpaywg.bestpay.com.cn/payWeb.action  新-->
    <!--http://webpay.bestpay.com.cn/payWeb.do 老-->  
    <form action="https://webpaywg.bestpay.com.cn/payWeb.action" method="post" name="orderForm"
        id="orderForm">
            <input type="hidden" name="MERCHANTID" value="<%=MERCHANTID %>" /> 
            <input type="hidden" name="SUBMERCHANTID" value="<%=SUBMERCHANTID %>" /> 
			<input type="hidden" name="ORDERSEQ" value="<%=ORDERSEQ %>"/>
			<input type="hidden" name="ORDERREQTRANSEQ" value="<%=ORDERREQTRANSEQ %>"/> 
			<input type="hidden" name="ORDERDATE" value="<%=ORDERDATE %>"/>
			<input type="hidden" name="ORDERAMOUNT" value="<%=ORDERAMOUNT %>"/> 
			<input type="hidden" name="PRODUCTAMOUNT" value="<%=PRODUCTAMOUNT %>" /> 
			<input type="hidden" name="ATTACHAMOUNT" value="<%=ATTACHAMOUNT %>"/> 
			<input type="hidden" name="CURTYPE" value="RMB" /> 
			<input type="hidden" name="ENCODETYPE" value="<%=ENCODETYPE %>"/>
			<input type="hidden" name="MERCHANTURL" value="<%=MERCHANTURL %>" /> 
			<input type="hidden" name="BACKMERCHANTURL" value="<%=BACKMERCHANTURL %>" /> 
			<input type="hidden" name="ATTACH" value="无" /> 
			<input type="hidden" name="BUSICODE" value="0001" /> 
			<input type="hidden" name="TMNUM" value="无" /> 
			<input type="hidden" name="CUSTOMERID" value="" /> 
			<input type="hidden" name="PRODUCTID" value="" /> 
			<input type="hidden" name="PRODUCTDESC" value="" />
			<input type="hidden" name="MAC" value="<%=MAC %>"/>
    </form>
    <script type="text/javascript">
        $(document).ready(function(){
            $("#orderForm").submit();
        })
    </script>
</body>
</html>
