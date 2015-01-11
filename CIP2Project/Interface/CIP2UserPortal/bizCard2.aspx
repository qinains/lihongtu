<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" 
CodeFile="bizCard2.aspx.cs" Inherits="bizCard2" Title="Untitled Page" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

<script language="javascript" type="text/jscript">
function selpro(i)
        {
            $("#ctl00_ContentPlaceHolder1_stext").attr("value",i);
         
           $.get("default_ajax.aspx",{
           pid:$("#ctl00_ContentPlaceHolder1_stext").attr("value"),
           typeId:1,
           Now:Math.random()
           },result)
           
           
           $.get("default_ajax.aspx",{
           pid:$("#ctl00_ContentPlaceHolder1_stext").attr("value"),
           typeId:2,
           Now:Math.random()
           },result2)
        }
        
        function result(k)
       {
         document.getElementById('ctl00_ContentPlaceHolder1_areaInfoList').options.length=0;
         var area =k;
         var count= area.split("count")[1];
         var selectcity = document.getElementById('ctl00_ContentPlaceHolder1_areaInfoList');
         var selectcityid=document.getElementById('ctl00_ContentPlaceHolder1_areaid');         
          for(var i=0;i<count;i++)
          {
            var city=area.split("count"+count+"count")[i];
            selectcity[i] = new Option(city);
            selectcity[i].value=i;
          }
       }
       
       function result2(k)
       {
        document.getElementById('ctl00_ContentPlaceHolder1_areaid').options.length=0;
         var area =k;
         var count= area.split("count")[1];
         var selectcity = document.getElementById('ctl00_ContentPlaceHolder1_areaInfoList');
         var selectcityid=document.getElementById('ctl00_ContentPlaceHolder1_areaid');         
          for(var i=0;i<count;i++)
          {
            var city=area.split("count"+count+"count")[i];
            selectcityid[i] = new Option(city);
            selectcityid[i].value=city;
            $("#ctl00_ContentPlaceHolder1_resulttxt").attr("value",selectcityid[0].value);
          }
       }
        
        
        function selcity(i)
        {
           document.getElementById("ctl00_ContentPlaceHolder1_areaid").options[i].selected=true;
        $("#ctl00_ContentPlaceHolder1_resulttxt").attr("value", $("#ctl00_ContentPlaceHolder1_areaid").attr("value"));
        }



function FlowChange(i) {
          $("#F_IdTxt").attr("value", i);

            $.get("", {
             pid:$("#p_txt").attr("value"),
                typeId: 1,
                Now: Math.random()
            }, Fresult);
        }

        function Fresult(k) {
           alert(k);
        }

</script>
 <div class="ca">
			<h3>申请商旅卡</h3>
		</div>
		<div class="cb">
			<table class="tableA">
				<tr>
					<th><label>省份：</label></th>
					<td>
					  <%-- <select id="pid"  runat="server" onchange="sselChange(this.value);" ></select>
						<input type="text" id="stxt" />--%>
						 <select id="proInfoList" runat="server" onchange="selpro(this.value)"></select>
						 	<input type="text" id="stext" runat="server" style="display:none;"/>
					</td>
				</tr>
				<tr>
					<th><label>城市：</label></th>
					<td>
						<%--<select id="areaInfoList">
						</select>--%>
						<select id="areaInfoList" runat="server"  onchange="selcity(this.value)" ></select>
						<select id="areaid" runat="server" style="display:none;" ></select>
					
						<input type="text" id="resulttxt" runat="server" style="display:none;" />
					</td>
				</tr>
			</table>
			<div class="subtn"><span class="btn btnA"><span><button type="button" runat="server" id="Button1" onserverclick="Button1_ServerClick">申请</button>
               <button type="button" runat="server" id="Button3" onserverclick="Button2_Click">返回</button>
               </span></span></div>
		</div>
		
		

</asp:Content>



