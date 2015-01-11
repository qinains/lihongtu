/*********************************************************************************************************
 *     描述: 省市联动（普通版本）
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 赵锐
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 *********************************************************************************************************/
 
function selpro(i)
{
   if (document.getElementById('caozuotxt')!=null){
      $("#caozuotxt").attr("value","2");
   }
   $("#stext").attr("value",i);
   $.get("Ajax/ProandArea/ProandArea_ajax.aspx",{
       pid:$("#stext").attr("value"),
       typeId:1,
       Now:Math.random()},
       result
   )
   
   $.get("Ajax/ProandArea/ProandArea_ajax.aspx",{
       pid:$("#stext").attr("value"),
       typeId:2,
       Now:Math.random()
       },
       result2
   )
}
        
function result(k)
{
     document.getElementById('areaInfoList').options.length=0;
     var area =k;
     var count= area.split("count")[1];
     var selectcity = document.getElementById('areaInfoList');
     var selectcityid=document.getElementById('areaid');         
     for(var i=0;i<count;i++){
        var city=area.split("count"+count+"count")[i];
        selectcity[i] = new Option(city);
        selectcity[i].value=i;
     }
}
       
function result2(k)
{
    document.getElementById('areaid').options.length=0;
    var area =k;
    var count= area.split("count")[1];
    var selectcity = document.getElementById('areaInfoList');
    var selectcityid=document.getElementById('areaid');         
    for(var i=0;i<count;i++){
       var city=area.split("count"+count+"count")[i];
       selectcityid[i] = new Option(city);
       selectcityid[i].value=city;
       $("#resulttxt").attr("value",selectcityid[0].value);
    }
}
        
function selcity(i,j){
    if (j==1){
       $("#areaid").attr("value",i);
       $("#resulttxt").attr("value",i);
    }else{
        document.getElementById("areaid").options[i].selected=true;
        $("#resulttxt").attr("value", $("#areaid").attr("value")); 
    }                    
}