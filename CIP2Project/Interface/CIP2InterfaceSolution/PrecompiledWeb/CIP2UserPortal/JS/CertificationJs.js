function SubMobile()
{
    
   $.get("../Certification/Ajax/Certification_Ajax.aspx",{
   MobileNum:$("#txtMobile").attr("value"),
   typeId:2,
   Now:Math.random()
   },Result)
    $("#txtMobileYanZhen").show();


   
}

function Result(k)
{
    alert(k);
}

function MobileYanZhengShow()
{
  if ($("#txtMobile").attr("value").length==11)
  {
  $.get("../Certification/Ajax/Certification_Ajax.aspx",{
   MobileNum:$("#txtMobile").attr("value"),
   typeId:1,
   Now:Math.random()
   },Result)
    $("#txtMobileYanZhen").show();
  } 
 
}