  function MessageShow()
    {
        if ($("#txtMobilePhone").attr("value").length==11)
        {
           $.get("../SSO/Ajax/Login_Ajax.aspx",{
           mobile:$("#txtMobilePhone").attr("value"),
           typeId:1,
           Now:Math.random()
           },ResultMessage)
           $("#txtPmYz").show();
        }
    }
    function ResultMessage()
    {
        
    }
    
    
    function TableShow()
    {
        $("#TabToggle").toggle();
    }
    
    function SelValue(i)
    {
        $("#txtCardType").attr("value",i);
    }
    
    function SelSexValue(i)
    {
        
        $("#txtSex").attr("value",i);
    }
    
     function SelCardValue(i)
    {
        
        $("#txtCard").attr("value",i);
    }
    
    function AddLogin()
    {
           $.get("../SSO/Ajax/Login_Ajax.aspx",{
           name:$("#txtName").attr("value"),
           pwd:$("#txtPwd").attr("value"),
           mobile:$("#txtMobilePhone").attr("value"),
           sex:$("#txtSex").attr("value"),
           pmyz:$("#txtPmYz").attr("value"),
           yanzhen:$("#txtYanZhen").attr("value"),
           cardtype:$("#txtCardType").attr("value"),
           cardnum:$("#txtCardNum").attr("value"),
           birthday:$("#txtBirthday").attr("value"),
           email:$("#txtMail").attr("value"),
           card:$("#txtCard").attr("value"),
           address:$("#txtAddress").attr("value"),
           postcard:$("#txtPostCard").attr("value"),
           typeId:2,
           Now:Math.random()
           },Result)
    }
    
    function Result(k)
    {
       alert(k);
    }
    
    
    function geturl()
    {
        document.location=""+$("#servertextUrl").attr("value")+"";
    }