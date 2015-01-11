<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BatchAccountCharge.aspx.cs" Inherits="UserAccount_BatchAccountCharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>批量充值</title>
   	<link type="text/css" href="css/jquery-ui-1.8.16.custom.css" rel="stylesheet" />
	<script type="text/javascript" src="scripts/jquery-1.6.2.min.js"></script>
	<script type="text/javascript" src="scripts/jquery-ui-1.8.16.custom.min.js"></script> 
	<style type="text/css">
		.ui-widget-header {
	    	background: url("images/ui-bg_gloss-wave_35_f6a828_500x100.png") repeat-x scroll 50% 50% #9999cc;
	    	border: 1px solid #9999cc;
	    	color: #FFFFFF;
	    	font-weight: bold;
		}
	
		div#progressbar{width:300;height:20;}
	
	</style>
	<script type="text/javascript">
	
		var PrograssBarMaxValue = 160;
		var value = 0;
		var timer ;
		var startcard = "123";
		function ajaxWorking()
		{
			var time = new Date();
			$.ajax({
				url:"testajax.jsp",
				type:"post",
               	dataType:"JSON",
               	data:{'cardno':startcard,'time':time.getTime()},
				success:function(data,textStatus){
					var dataJson = eval("("+data+")");
					//处理返回的json数据
				},
				complete:function(XMLHttpRequest,textStatus ){
				},
				error:function(XMLHttpRequest,textStatus,errorThrown){
				}
			});			
		
		}
		
		$(function(){

			$("#start").show();

			$("#start").toggle(	
								function(){
											$("#start").val("stop") ;
											timer = setInterval(function(){
													ajaxWorking();
													$("div#progressbar").progressbar("value",value);
													value ++;
													if(value>PrograssBarMaxValue) clearInterval(timer);
												},100);	  //每隔十分之一秒执行一次											
											
										  }
											,
								function(){ 
											$("#start").val("start") ;
											clearInterval(timer);
										  }
			);

			
			$("div#progressbar").progressbar({
				max: '160', 
				textFormat: 'fraction', 
				change:function(event){
					var value = $("div#progressbar").progressbar("value");
					$("#percent").html(value+"/"+PrograssBarMaxValue)
					if(value==PrograssBarMaxValue){
						location.reload(true);
						return;
						//$("#start").hide();
					}
				}
			
			});
		
			$("#percent").html("0/"+PrograssBarMaxValue);
		
			//事件发生的顺序keydown-->keyup-->keypress
			$("#startno").keyup(function(){

				$("#startcardno").html($("#startno").val()).css("font-size","30px");
			});
			
			$("#endno").keyup(function(){

				$("#endcardno").html($("#endno").val()).css("font-size","30px");
			});				
		
		})
	
	</script>	
	
	
	
	
</head>
<body>
    <form id="form1" runat="server">
    
    	卡号:<input type="text" id="startno" size="16"/>至
		<input type="text" id="endno" size="16"/>	
    
    </form>
   
	<div id="startcardno"></div>
	<div id="endcardno"></div>
	<table>
		<tr><td>
		  	<div id="progressbar"></div>
		</td><td>
		    <div id="percent"></div>
		</td></tr>
	</table>
  
  	<div>
    	<input id="start" type="button" value="start" />
  	</div>
    
</body>
</html>
