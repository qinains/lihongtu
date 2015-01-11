<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="CustInfoManager_CustInfoEdit, App_Web_ac68v7gr" enableEventValidation="false" %>

<%@ Register Src="../UserCtrl/ValidateCIPToken.ascx" TagName="ValidateCIPToken" TagPrefix="uc1" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript" src="../ModelJS/Date/WdatePicker.js"></script>
    <script language="javascript" type="text/javascript" src="../ModelJS/jquery-1.3.1.js"></script>

    <uc1:ValidateCIPToken ID="ValidateCIPToken1" runat="server" />
    
    <div class="ca">
        <h3>
            设置客户个人信息
        </h3>
    </div>
    <div class="cb">
        <table class="tableA">
            <tr>
                <th>
                    用户名：</th>
                <td>
                    <asp:TextBox ID="txtUserName" class="texti" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    姓&nbsp;&nbsp;&nbsp;名：</th>
                <td>
                    <asp:TextBox ID="txtRealName" class="texti" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    昵&nbsp;&nbsp;&nbsp;称：</th>
                <td>
                    <asp:TextBox ID="txtNickName" class="texti" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    性&nbsp;&nbsp;&nbsp;别：</th>
                <td>
                    <asp:DropDownList ID="DDLSex" runat="server">
                        <asp:ListItem Value="2">未知</asp:ListItem>
                        <asp:ListItem Value="1">男</asp:ListItem>
                        <asp:ListItem Value="0">女</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <th>
                    出生日期：</th>
                <td>
                    <asp:TextBox ID="txtBirthday" class="texti" onclick="WdatePicker();" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    省&nbsp;&nbsp;&nbsp;份：</th>
                <td>
                    <asp:DropDownList ID="DDLProvinceList" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <th>
                    地&nbsp;&nbsp;&nbsp;区：</th>
                <td>
                    <asp:HiddenField ID="hdAreaID" runat="server" />
                    <asp:DropDownList ID="DDLAreaList" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <th>
                    证件类型：</th>
                <td>
                    <asp:DropDownList ID="DDLCertificateType" runat="server">
                        <asp:ListItem Value="">未知</asp:ListItem>
                        <asp:ListItem Value="0">身份证</asp:ListItem>
                        <asp:ListItem Value="1">士兵证</asp:ListItem>
                        <asp:ListItem Value="2">军官证</asp:ListItem>
                        <asp:ListItem Value="3">护照</asp:ListItem>
                        <asp:ListItem Value="4">保留</asp:ListItem>
                        <asp:ListItem Value="5">台胞证</asp:ListItem>
                        <asp:ListItem Value="6">港澳通行证</asp:ListItem>
                        <asp:ListItem Value="7">国际海员证</asp:ListItem>
                        <asp:ListItem Value="9">其它</asp:ListItem>
                        <asp:ListItem Value="10">部队干部离休证</asp:ListItem>
                        <asp:ListItem Value="11">工商营业执照</asp:ListItem>
                        <asp:ListItem Value="12">单位证明</asp:ListItem>
                        <asp:ListItem Value="13">驾驶证</asp:ListItem>
                        <asp:ListItem Value="14">学生证</asp:ListItem>
                        <asp:ListItem Value="15">教师证</asp:ListItem>
                        <asp:ListItem Value="16">户口本/居住证</asp:ListItem>
                        <asp:ListItem Value="17">老人证</asp:ListItem>
                        <asp:ListItem Value="18">组织机构代码证</asp:ListItem>
                        <asp:ListItem Value="19">工作证</asp:ListItem>
                        <asp:ListItem Value="20">暂住证</asp:ListItem>
                        <asp:ListItem Value="21">电信识别编码</asp:ListItem>
                        <asp:ListItem Value="22">集团客户标识码</asp:ListItem>
                        <asp:ListItem Value="23">VIP卡</asp:ListItem>
                        <asp:ListItem Value="24">警官证</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    证件号码：</th>
                <td>
                    <asp:TextBox ID="txtCertificateCode" class="texti" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <th>
                    文化水平：</th>
                <td>
                    <asp:DropDownList ID="DDLEdueLevel" runat="server">
                        <asp:ListItem Value="">未知</asp:ListItem>
                        <asp:ListItem Value="1">小学</asp:ListItem>
                        <asp:ListItem Value="2">初中</asp:ListItem>
                        <asp:ListItem Value="3">高中/中专</asp:ListItem>
                        <asp:ListItem Value="4">大学/专科</asp:ListItem>
                        <asp:ListItem Value="5">研究生及以上</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <th>
                    收入水平：</th>
                <td>
                    <asp:DropDownList ID="DDLIncomeLevel" runat="server">
                        <asp:ListItem Value="">未知</asp:ListItem>
                        <asp:ListItem Value="0"><=1000</asp:ListItem>
                        <asp:ListItem Value="1">1000~3000</asp:ListItem>
                        <asp:ListItem Value="2">3000~5000</asp:ListItem>
                        <asp:ListItem Value="3">5000~8000</asp:ListItem>
                        <asp:ListItem Value="4">8000~20000</asp:ListItem>
                        <asp:ListItem Value="5">20000以上</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="subtn">
                        <span class="btn btnA"><span>
                            <img id="loadImg" src="../Images/Loading.gif" style="position:absolute; display:none" />
                            <button id="btnSave" type="button" onclick="SaveFun();">
                                保 存</button></span></span></div>
                </td>
            </tr>
        </table>
    </div>
    <div style="display:none">
    <asp:Button ID="btnSaveServer" runat="server" Text="" />
    </div>
    <asp:HiddenField ID="hdSPID" runat="server" />
    <asp:HiddenField ID="hdCustID" runat="server" />
    <script type="text/javascript">
        $(document).ready(function(){
            //BindAreaDDL();
            $("#ctl00_ContentPlaceHolder1_DDLProvinceList").bind("change",BindAreaDDL);
        })
        
        BindAreaDDL = function(){
            var provinceid = $("#<%=DDLProvinceList.ClientID %>").val();
            $("#<%=DDLAreaList.ClientID %> > option").remove();
            var time = new Date();
            $.ajax({
                type:"post",
                url:"HttpHandler/GetInfoHandler.ashx",
                dataType:"JSON",
                data:{ProvinceID:provinceid,Type:"GetAreaList",time:time.getTime()},
                success:function(data,textStatus){
                    var dataJson = eval("("+data+")");
                    if(dataJson.result == "true"){
                        $.each(dataJson.areainfo,function(index,item){
                            $("#<%=DDLAreaList.ClientID %>").append("<option value='"+item.AreaID+"'>"+item.AreaName+"</option>");
                        })
                    }
                },
                error:function(){
                    
                }
            })
        }
        
        SaveFun = function(){
            var spid = $("#<%=hdSPID.ClientID %>").val();
            var custid = $("#<%=hdCustID.ClientID %>").val();
            var username = $("#<%=txtUserName.ClientID %>").val();
            var realname = $("#<%=txtRealName.ClientID %>").val();
            var nickname = $("#<%=txtNickName.ClientID %>").val();
            var sex = $("#<%=DDLSex.ClientID %>").val();
            var birthday = $("#<%=txtBirthday.ClientID %>").val();
            var provinceid = $("#<%=DDLProvinceList.ClientID %>").val();
            var areaid = $("#<%=DDLAreaList.ClientID %>").val();
            var certificatetype = $("#<%=DDLCertificateType.ClientID %>").val();
            var certificatecode = $("#<%=txtCertificateCode.ClientID %>").val();
            var edulevel = $("#<%=DDLEdueLevel.ClientID %>").val();
            var incomelevel = $("#<%=DDLIncomeLevel.ClientID %>").val();
            var time = new Date();
            $.ajax({
                type:"post",
                url:"HttpHandler/SaveInfoHandler.ashx",
                dataType:"JSON",
                data:{SPID:spid,CustID:custid,UserName:username,RealName:realname,NickName:nickname,Sex:sex,Birthday:birthday,ProvinceID:provinceid,
                        AreaID:areaid,CertificateType:certificatetype,CertificateCode:certificatecode,EduLevel:edulevel,IncomeLevel:incomelevel,time:time.getTime(),Type:"SaveCustInfo"},
                beforeSend: function(XMLHttpRequest) {
                    $("#loadImg").css("display", "block");
                    $("#btnSave").css("display","none");
                },
                complete: function(XMLHttpRequest, textStatus) {
                    $("#loadImg").css("display", "none");
                    $("#btnSave").css("display","block");
                },
                success:function(data,textStatus){
                    var dataJson = eval("("+data+")");
                    if(dataJson.result == "true"){
                        $("#<%=btnSaveServer.ClientID %>").click();
                    }else{
                        alert(dataJson.info);
                    }
                },
                error:function(){
                    alert("信息保存失败");
                }
            })
        }
    </script>
</asp:Content>
