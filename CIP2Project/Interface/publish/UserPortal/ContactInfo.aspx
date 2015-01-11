<%@ page language="C#" autoeventwireup="true" inherits="ContactInfo, App_Web_contactinfo.aspx.cdcab7d2" enableEventValidation="false" %>
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="~/UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta name="keywords" content="上海房产,上海供求信息,上海餐饮,上海美食,休闲娱乐,黄页,号码百事通,114,118114" />
    <meta name="description" content="号码百事通:本地生活信息平台,免费为您提供餐饮预定,美食搜索,休闲娱乐,房产信息,供求信息等生活服务信息.吃喝玩乐尽你搜,你的生活好帮手." /> 

    <link rel="stylesheet" href="JS/jquery-ui-1.10.4.custom/development-bundle/themes/base/jquery.ui.all.css"/>
    <link rel="stylesheet" href="JS/jquery-ui-1.10.4.custom/development-bundle/demos/demos.css" /> 
    <link href="JS/jquery-ui-1.10.4.custom/css/ui-lightness/jquery-ui-1.10.4.custom.css" rel="stylesheet"/>

    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/js/jquery-1.10.2.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/js/jquery-ui-1.10.4.custom.js"></script> 
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.core.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.widget.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.mouse.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.button.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.draggable.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.position.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.resizable.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.button.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.dialog.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-1.10.4.custom/development-bundle/ui/jquery.ui.effect.js"></script>
    <style type="text/css">  
        body { font-size: 10px; }  
        label, input { display:block; }  
        input.text { margin-bottom:12px; width:95%; padding: .4em; }  
        div#address-contain table { margin: 1em 0; border-collapse: collapse; width: 100%; }  
        div#address-contain table td, div#address-contain table th { border: 1px solid #eee; padding: .6em 4px; text-align: left; } 

    </style>  
 
</head>
<body>
 <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
 <div class="demo">  
  
<div id="dialog-form" title="创建/修改地址信息"  >  
    <form id="addresssForm" action="">  
    <fieldset>  
        <label for="address">省/地市/区县</label>
        <table>
            <tr>
                <td>
                      <select name="province" id="province">
                             <option value="0">请选择</option>
                            <option value="110000">北京</option>
                            <option value="120000">天津</option>
                            <option value="130000">河北</option>
                            <option value="140000">山西</option>
                            <option value="150000">内蒙古</option>
                            <option value="210000">辽宁</option>
                            <option value="220000">吉林</option>
                            <option value="230000">黑龙江</option>
                            <option value="310000">上海</option>
                            <option value="320000">江苏</option>
                            <option value="330000">浙江</option>
                            <option value="340000">安徽</option>
                            <option value="350000">福建</option>
                            <option value="360000">江西</option>
                            <option value="370000">山东</option>
                            <option value="410000">河南</option>
                            <option value="420000">湖北</option>
                            <option value="430000">湖南</option>
                            <option value="440000">广东</option>
                            <option value="450000">广西</option>
                            <option value="460000">海南</option>
                            <option value="500000">重庆</option>
                            <option value="510000">四川</option>
                            <option value="520000">贵州</option>
                            <option value="530000">云南</option>
                            <option value="540000">西藏</option>
                            <option value="610000">陕西</option>
                            <option value="620000">甘肃</option>
                            <option value="630000">青海</option>
                            <option value="640000">宁夏</option>
                            <option value="650000">新疆</option>                       
                      </select>  
                      省 
                  </td>
                <td>  
                       <select name="city" id="city">
                           
                      </select>
                     市                  
                </td>
                <td>  
                      <select name="district" id="district">
                            
                      </select>
                     区                   
                </td> 
            </tr>
        </table>
        <p></p>
        <label for="address">地址</label>  
        <input type="text" name="address" id="address" class="text"  />  
        <label for="zipcode">邮编</label>  
        <input type="text" name="zipcode" id="zipcode" class="text" />  
        <label for="Type">类型</label>
        <select name="addressType" id="addressType">
	            <option value="00">帐单地址</option>
	            <option value="01" selected="selected">家庭地址</option>
	            <option value="02">单位地址</option>
	            <option value="03">服务地址</option>
	            <option value="04">送票地址</option>
	            <option value="99">其它地址</option>
        </select>
        <p></p>
        <label for="OtherType"></label>
        <input type="text" name="otherType" id="otherType" value="" class="text"  style="display: none;"/>  
       <label for="contactPhone">联系电话</label>  
        <input type="text" name="contactPhone" id="contactPhone" value="" class="text" />  
        <label for="relationPerson">联系人</label>
        <input type="text" name="relationPerson" id="relationPerson" value="" class="text" />
        <label for="fixedPhone">固定电话</label>
        <input type="text" name="fixedPhone" id="fixedPhone" value="" class="text" />
        <input type="hidden" name="rowindex" id="rowindex" value=""/>  
        <input type="hidden" name="addid" id="addid" value=""/>  
        <input type="text" id="custidtxt" runat="server" style="display: none;" />
        <input type="text" id="spidtxt" runat="server" style="display: none;" />
    </fieldset>  
    </form>  
</div>  
  
  
<div id="address-contain">  
    <h1>已存在地址:</h1>  
    <table id="addressInfo">  
        <thead>  
            <tr class="ui-widget-header ">  
                <th>省</th>
                <th>市</th>
                <th>区</th>  
                <th>地址</th>  
                <th>邮编</th>
                <th>类型</th>  
                <th>电话</th>
                <th>联系人</th>
                <th>固话</th>
                <th style="width:12em;">操作</th>  
            </tr>  
        </thead>  
        <tbody>  
            
        </tbody>  
    </table>  
</div>  
<button id="create-address">添加新地址</button>  
  
</div><!-- End demo -->  
<div id="debug">  
</div>  
    <script type="text/javascript">
    
        $(document).ready(function(){
            $("#addressType").change(function(){
                   var p = $(this).children('option:selected').val();
                   if(p=='99'){
                        $("#otherType").show();
                   } else{
                        $("#otherType").hide();
                   }
            });
        
            var arrHiddenID = new Array();
            var time = new Date();       
            $.ajax({
                type:"post",
                url:"HttpHandler/ContactInfoHandler.ashx",
                dataType:"JSON",
                data:{CustID:$("#custidtxt").val(),Type:"GetAddressInfoByID",time:time.getTime()},
                beforeSend: function(XMLHttpRequest) {

                },
                complete: function(XMLHttpRequest, textStatus) {

                },                        
                success:function(data,textStatus){
                    $.each(data.info,function(index,item){
                         if(data.result=="true"){ 
                               var addType = "";
                               if(item.Type=='00'){
                                    addType = "帐单地址";
                               }else if(item.Type=='01'){
                                    addType ="家庭地址";
                               }else if(item.Type=='02'){
                                    addType ="单位地址";
                               }else if(item.Type=='03'){
                                    addType ="服务地址";
                               }else if(item.Type=='04'){
                                    addType ="送票地址";
                               }else{
                                     addType = item.OtherType;
                               }   
                               $( "#addressInfo tbody" ).append( "<tr>" +  
                                "<td>" +item.Province + "</td>" +   
                                "<td>" +item.City + "</td>" + 
                                "<td>" +item.District + "</td>" + 
                                "<td>" +item.Address + "</td>" +   
                                "<td>" +item.ZipCode + "</td>" +   
                                "<td>" +addType + "</td>" +  
                                "<td>" +item.Phone + "</td>" +   
                                "<td>" +item.RelationPerson + "</td>" +   
                                "<td>" +item.FixedPhone + "</td>" + 
                                '<td><button value='+item.ID+' provinceCode='+item.ProvinceCode+' cityCode='+item.CityCode+' districtCode='+item.DistrictCode+' class="EditButton">编辑</button><button value='+item.ID+' class="DeleteButton" >删除</button></td>'+  
                                "</tr>" );   
                       }
                    });  
                   bindEditEvent();   
                },
                error:function(){
                    alert('ajax error!');
                }
            });   
                    
            var address = $( "#address" ),  
            zipcode = $("#zipcode"), 
            province = $("#province"),
            city = $("#city"), 
            areaCode =  $("#district"), 
            addressType = $("#addressType"), 
            otherType = $( "#otherType" ),  
            contactPhone = $( "#contactPhone" ),
            relationPerson = $("#relationPerson"),
            fixedPhone = $("#fixedPhone"),  
            rowindex = $( "#rowindex" ),
            addid = $("#addid")   

            allFields = $( [] ).add( address ).add(zipcode).add(province).add(city).add(areaCode).add(addressType).add( otherType ).add( contactPhone ).add(relationPerson).add(fixedPhone).add( rowindex ).add(addid);  

            $( "#dialog-form" ).dialog({  
                autoOpen: false,  
                height: 480,  
                width: 452,  
                modal: true,  
                buttons: {  
                    "确定": function() {  
                        if (rowindex.val()==""){//新增  
                        //ajax() -begin
                                $.ajax({
                                        type:"post",
                                        url:"HttpHandler/ContactInfoHandler.ashx",
                                        dataType:"JSON",
                                        data:{CustID:$("#custidtxt").val(),AddressID:"",Address:address.val(),ZipCode:zipcode.val(),AreaCode:areaCode.val(),AddRessType:addressType.val(),OtherType:otherType.val(),ContactPhone:contactPhone.val(),RelationPerson:relationPerson.val(),FixedPhone:fixedPhone.val(),Type:"SaveAddressInfo",time:time.getTime()},
                                        beforeSend: function(XMLHttpRequest) {

                                        },
                                        complete: function(XMLHttpRequest, textStatus) {
                   
                                        },                        
                                        success:function(data,textStatus){
                                             alert(data.msg);
                                             $.each(data.info,function(index,item){
                                                 if(data.result=="true"){ 
                                                       //var addType = "";
                                                       //if(item.Type=='00'){
                                                       //     addType = "帐单地址";
                                                       //}else if(item.Type=='01'){
                                                       //     addType ="家庭地址";
                                                       //}else if(item.Type=='02'){
                                                       //     addType ="单位地址";
                                                       //}else if(item.Type=='03'){
                                                       //     addType ="服务地址";
                                                       //}else if(item.Type=='04'){
                                                       //     addType ="送票地址";
                                                       //}else{
                                                       //      addType = item.OtherType;
                                                       //}    
                                                 
                                                       //$( "#addressInfo tbody" ).append( "<tr>" +  
                                                       //  "<td>" +item.Province + "</td>" +   
                                                       // "<td>" +item.City + "</td>" + 
                                                       // "<td>" +item.District + "</td>" +                                                       
                                                       // "<td>" +item.Address + "</td>" +   
                                                       // "<td>" +addType + "</td>" +  
                                                       // "<td>" +item.Phone + "</td>" +   
                                                       // "<td>" +item.RelationPerson + "</td>" +   
                                                       // "<td>" +item.FixedPhone + "</td>" +                                                        
                                                       // '<td><button value='+item.ID+' provinceCode='+item.ProvinceCode+' cityCode='+item.CityCode+' districtCode='+item.DistrictCode+' class="EditButton">编辑</button><button value='+item.ID+' class="DeleteButton" >删除</button></td>'+  
                                                       // "</tr>" );                                                  
                                                        window.location.reload();
                                               }
                                            });  
                                           bindEditEvent();   
                                        },
                                        error:function(){
                                            alert('ajax error!');
                                        }
                                    });                           
                        // -end
   
                            bindEditEvent();  
                        }  
                        else{//修改  
                            var idx = rowindex.val();  
                            var tr = $("#addressInfo>tbody>tr").eq(idx);  
                            //$("#debug").text(tr.html());  
                            //ajax() -begin
                                $.ajax({
                                        type:"post",
                                        url:"HttpHandler/ContactInfoHandler.ashx",
                                        dataType:"JSON",
                                        data:{CustID:$("#custidtxt").val(),AddressID:addid.val(),Address:address.val(),ZipCode:zipcode.val(),AreaCode:areaCode.val(),AddressType:addressType.val(),OtherType:otherType.val(),ContactPhone:contactPhone.val(),RelationPerson:relationPerson.val(),FixedPhone:fixedPhone.val(),Type:"SaveAddressInfo",time:time.getTime()},
                                        beforeSend: function(XMLHttpRequest) {

                                        },
                                        complete: function(XMLHttpRequest, textStatus) {
                   
                                        },                        
                                        success:function(data,textStatus){
                                                alert(data.msg);
                                                //tr.children().eq(0).text(province.val());
                                                //tr.children().eq(1).text(city.val());
                                                //tr.children().eq(2).text(areaCode.val());
                                                //tr.children().eq(3).text(address.val());  
                                                //tr.children().eq(4).text(zipcode.val());  
                                                //tr.children().eq(5).text(addresstype.val());  
                                                //tr.children().eq(6).text(contactPhone.val());  
                                                //tr.children().eq(7).text(relationPerson.val());
                                                //tr.children().eq(8).text(fixedPhone.val());
                                                window.location.reload();
                                        },
                                        error:function(){
                                            alert('ajax error!');
                                        }
                                    });                           
                            // -end

                        }  
                        $( this ).dialog( "close" );  
                    },  
                    "取消": function() {  
                        $( this ).dialog( "close" );  
                    }  
                },  
                close: function() {  
                    //allFields.val( "" ).removeClass( "ui-state-error" );  
                    ;  
                }  
            });  
            
            function bindEditEvent(){  
                //绑定Edit按钮的单击事件  
                $(".EditButton").click(function(){  
                        var b = $(this);  
                        var tr = b.parents("tr");  
                        var tds = tr.children();  
                        //设置初始值  
                        //alert(b.attr("provinceCode")+"|"+b.attr("cityCode")+"|"+b.attr("districtCode"));
                        $("#province option[value='"+b.attr("provinceCode")+"']").attr("selected","selected");
                        $("#province").change();
                        $("#city option[value='"+b.attr("cityCode")+"']").attr("selected","selected");
                        $("#city").change();
                        $("#district option[value='"+b.attr("districtCode")+"']").attr("selected","selected"); 
                        address.val(tds.eq(3).text());  
                        zipcode.val(tds.eq(4).text());
                        var optionval = "01";
                        if(tds.eq(5).text()=="帐单地址"){
                             optionval = "00";
                        }else if(tds.eq(5).text()=="家庭地址"){
                             optionval = "01";
                        }else if(tds.eq(5).text()=="单位地址"){
                             optionval = "02";
                        }else if(tds.eq(5).text()=="服务地址"){
                             optionval = "03";
                        }else if(tds.eq(5).text()=="送票地址"){
                             optionval = "04";
                        }else{
                             optionval = "99";
                        } 
                        $("select[name=addressType] option[value='"+optionval+"']").attr("selected","selected");
                        if(optionval=="99"){
                            $("#otherType").show();
                            otherType.val(tds.eq(6).text()); 
                        }else{
                            $("#otherType").hide();
                        }
                        contactPhone.val(tds.eq(6).text());  
                        relationPerson.val(tds.eq(7).text());
                        fixedPhone.val(tds.eq(8).text());  
                        var trs = b.parents("tbody").children();  
                        //设置行号，以行号为标识，进行修改。  
                        rowindex.val(trs.index(tr));  
                        //alert(b.val());  // b.val() 就是 按钮上的值 
                        $("#addid").val(b.val());
                        //打开对话框  
                        $( "#dialog-form" ).dialog( "open" );  
                });  
                  
                  
               $("#province").change(function(){
                    //ajax begin
                    $("#city").empty();
                    $("#district").empty();     
                    $.ajax({
                        async: false, 
                        type:"post",
                        url:"HttpHandler/ContactInfoHandler.ashx",
                        dataType:"JSON",
                        data:{ProvinceID:$("#province").val(),Type:"GetCities",time:time.getTime()},
                        beforeSend: function(XMLHttpRequest) {

                        },
                        complete: function(XMLHttpRequest, textStatus) {
   
                        },                        
                        success:function(data,textStatus){
                            $("#city").prepend("<option value='0'>请选择</option>");
                            $.each(data.info,function(index,item){
                                 if(data.result=="true"){ 
                                       $("#city").append("<option value='"+item.Code+"'>"+item.Name+"</option>");
                               }
                            });  
                        },
                        error:function(){
                            alert('ajax error!');
                        }
                    });                     
                    //ajax end            
              
               });
                
               $("#city").change(function(){
                    //ajax begin
                    $("#district").empty();    
                    $.ajax({
                        async: false,  
                        type:"post",
                        url:"HttpHandler/ContactInfoHandler.ashx",
                        dataType:"JSON",
                        data:{CityID:$("#city").val(),Type:"GetDistircts",time:time.getTime()},
                        beforeSend: function(XMLHttpRequest) {

                        },
                        complete: function(XMLHttpRequest, textStatus) {
   
                        },                        
                        success:function(data,textStatus){
                            $("#district").prepend("<option value='0'>请选择</option>");
                            $.each(data.info,function(index,item){
                                 if(data.result=="true"){ 
                                       $("#district").append("<option value='"+item.Code+"'>"+item.Name+"</option>");
                                 }
                            });  
                        },
                        error:function(){
                            alert('ajax error!');
                        }
                    });                     
                    //ajax end            
              
               }); 
    
                  
                //绑定Delete按钮的单击事件  
                $(".DeleteButton").click(function(){  
                    var b = $(this);  
                    var tr = $(this).parents("tr");  
                    //ajax() -begin
                    $.ajax({
                            type:"post",
                            url:"HttpHandler/ContactInfoHandler.ashx",
                            dataType:"JSON",
                            data:{CustID:$("#custidtxt").val(),AddressID:b.val(),Type:"DeleteAddressInfo",time:time.getTime()},
                            beforeSend: function(XMLHttpRequest) {

                            },
                            complete: function(XMLHttpRequest, textStatus) {
       
                            },                        
                            success:function(data,textStatus){
                                   alert(data.msg);
                                   tr.remove();   
                            },
                            error:function(){
                                alert('ajax error!');
                            }
                        });                           
                    // -end 
                    
                });  
            };  
              
            bindEditEvent();  
              
            $( "#create-address" )  
                .button()  
                .click(function() {  
                    //清空表单域  
                    allFields.each(function(idx){  
                        this.value="";  
                    });  
                    $("#province").empty();
                    $("#city").empty();
                    $("#district").empty();
                    $("select[name=addressType] option[value='01']").attr("selected","selected");

                    //ajax begin
                          $.ajax({
                                    type:"post",
                                    url:"HttpHandler/ContactInfoHandler.ashx",
                                    dataType:"JSON",
                                    data:{Type:"GetProvinces",time:time.getTime()},
                                    beforeSend: function(XMLHttpRequest) {

                                    },
                                    complete: function(XMLHttpRequest, textStatus) {
               
                                    },                        
                                    success:function(data,textStatus){

                                        $("#province").prepend("<option value='0'>请选择</option>");
                                        $.each(data.info,function(index,item){
                                             if(data.result=="true"){ 
                                                   $("#province").append("<option value='"+item.Code+"'>"+item.Name+"</option>");
                                           }
                                        });  
                                    },
                                    error:function(){
                                        alert('ajax error!');
                                    }
                                });                
                    //ajax end
                    $( "#dialog-form" ).dialog( "open" );  
                });  
                  
                    
        });  
        
       
    </script>

</body>
</html>
