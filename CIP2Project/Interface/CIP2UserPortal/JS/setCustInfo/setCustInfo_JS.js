/*********************************************************************************************************
 *     描述: 客户信息修改JS脚本
 * 开发平台: Windows XP + Microsoft SQL Server 2005
 * 开发语言: C#
 * 开发工具: Microsoft Visual Studio.Net 2005
 *     作者: 赵锐
 * 联系方式: 
 *     公司: 联创科技(南京)股份有限公司
 * 创建日期: 2009-07-31
 *********************************************************************************************************/

function selpro(i,j){
    if (j==1){
        $("#protxt").attr("value",i);
    }else{
        $("#areatxt").attr("value",i);
    }
}

function selvalue(i,j){
    if (j==1){
        $("#ctl00_ContentPlaceHolder1_certificatetxt").attr("value",i);
    }else if (j==2){
        $("#ctl00_ContentPlaceHolder1_sextxt").attr("value",i);
    }else if (j==3){
        $("#ctl00_ContentPlaceHolder1_Edutxt").attr("value",i);
    }else if (j==4){
        $("#ctl00_ContentPlaceHolder1_Incometxt").attr("value",i);
    }
}


function UpdateCust(){
    //真实姓名
    var realname=$("#ctl00_ContentPlaceHolder1_realnametxt").attr("value");
    if (realname==null){
        $("#reannameLable").html("客户姓名不能为空");
        $("#reannameLable").show();
        return;
    }else{
        $("#reannameLable").hide();
    }
    //证件号
    var crlen=$("#ctl00_ContentPlaceHolder1_certnotxt").attr("value");
    var cardid=$("#ctl00_ContentPlaceHolder1_certnotxt").attr("value");
    if ($("#ctl00_ContentPlaceHolder1_certnotxt").attr("value")==null){
        cerid="";
    }else{
        cerid=$("#ctl00_ContentPlaceHolder1_certnotxt").attr("value");
    }
    //昵称
    var nname=$("#ctl00_ContentPlaceHolder1_nicknametxt").attr("value");
    if (nname==null){
        nname="";
    }else{
        nname=$("#ctl00_ContentPlaceHolder1_nicknametxt").attr("value");
    }
    //生日
    if ($("#ctl00_ContentPlaceHolder1_birthdaytxt").attr("value")==null){
        bbirthday="";
    }else{
        bbirthday=$("#ctl00_ContentPlaceHolder1_birthdaytxt").attr("value");
    }
    //提交保存
    $.get("Ajax/CustInfo/setCustInfo_ajax.aspx",
        {
            custid:$("#ctl00_ContentPlaceHolder1_custidtxt").attr("value"),
            realname:$("#ctl00_ContentPlaceHolder1_realnametxt").attr("value"),
            nickname:nname,
            certificate:$("#ctl00_ContentPlaceHolder1_certificatetxt").attr("value"),
            certno:cerid,
            sex:$("#ctl00_ContentPlaceHolder1_sextxt").attr("value"),
            birthday:bbirthday,
            Edu:$("#ctl00_ContentPlaceHolder1_Edutxt").attr("value"),
            Income:$("#ctl00_ContentPlaceHolder1_Incometxt").attr("value"),
            pro:$("#ctl00_ContentPlaceHolder1_stext").attr("value"),
            area:$("#ctl00_ContentPlaceHolder1_resulttxt").attr("value"),
            spid:$("#ctl00_ContentPlaceHolder1_spidtxt").attr("value"),
            typeId:1,
            Now:Math.random()
        },Result);
}

function Result(k){
    if (k==0){
       $("#ctl00_ContentPlaceHolder1_urlRedirectButton").click();
    }else if (k==1){
        $("#reannameLable").html('请输入客户姓名');
        return;
    }
}

function sel(i){
    if (i==1){
        if ($("#ctl00_ContentPlaceHolder1_realnametxt").attr("value")==null){
            $("#reannameLable").html("请输入客户姓名");
            $("#reannameLable").show();
            return;
        }else{
             $("#reannameLable").hide();
        }
    }
}