<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="setCustInfo1, App_Web_setcustinfo.aspx.cdcab7d2" title="Untitled Page" enableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript" src="JS/setCustInfo/setCustInfo_JS.js"></script>
    <script language="javascript" type="text/javascript" src="JS/ProandArea/ProandArea.js"></script>
    <script language="javascript" type="text/javascript" src="ModelJS/Date/WdatePicker.js"></script>
    <script language="javascript" type="text/javascript">
   
    
      function selvalue1()
      {
         var certificate= $("#ctl00_ContentPlaceHolder1_certificateSel").val();  
         if (certificate=="")
         {
            document.getElementById("ctl00_ContentPlaceHolder1_certnoL").style.display ="none";  
            document.getElementById("ctl00_ContentPlaceHolder1_certnotxt").style.display ="none";   
            document.getElementById("ctl00_ContentPlaceHolder1_certnotxt").value="";      
         }
         else
         {
            document.getElementById("ctl00_ContentPlaceHolder1_certnoL").style.display ="block";
            document.getElementById("ctl00_ContentPlaceHolder1_certnotxt").style.display ="block";
         } 
         document.getElementById("ctl00_ContentPlaceHolder1_certificatetxt").value=certificate;
      }
  
    </script>

   <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>
    <div class="ca">
        <h3>
            设置客户个人信息</h3>
    </div>
    <div class="cb">
        <dl>
            
            <dt>
                <label for="username">
                    真实姓名：</label></dt><dd><input type="text" value="" id="realnametxt" class="texti"
                        runat="server" onblur="sel(1);" /><span id="err_username" class="remark"></span>
                        <label id="reannameLable" style="display: none">
                        </label>
                    </dd>
            <dt>
                <label for="fullname">
                    昵 称：</label>
            </dt>
            <dd>
                <input type="text" value="" id="nicknametxt" class="texti" runat="server" onblur="sel(2);" /><span
                    id="err_fullname" class="remark"></span><label id="nicknameLable" style="display: none">
                    </label>
            </dd>
        </dl>
        <dl>
            <dt>
                <label>
                    证件类型：</label>
            </dt>
            <dd>
                <select id="certificateSel" onchange="selvalue1();" runat="server">
                    <option value="">未知</option>
                    <option value="0">身份证</option>
                    <option value="1">士兵证</option>
                    <option value="2">军官证</option>
                    <option value="3">护照</option>
                    <option value="4">保留</option>
                    <option value="5">台胞证</option>
                    <option value="6">港澳通行证</option>
                    <option value="7">国际海员证</option>
                    <option value="9">其它</option>
                    <option value="10">部队干部离休证</option>
                    <option value="11">工商营业执照</option>
                    <option value="12">单位证明</option>
                    <option value="13">驾驶证</option>
                    <option value="14">学生证</option>
                    <option value="15">教师证</option>
                    <option value="16">户口本/居住证</option>
                    <option value="17">老人证</option>
                    <option value="18">组织机构代码证</option>
                    <option value="19">工作证</option>
                    <option value="20">暂住证</option>
                    <option value="21">电信识别编码</option>
                    <option value="22">集团客户标识码</option>
                    <option value="23">VIP卡</option>
                    <option value="24">警官证</option>
                </select>
                <input type="text" id="certificatetxt" value="0" runat="server" style="display: none;" />
                <%--<input type="text" id="Text1" value="0" runat="server"  />--%>
            </dd>
            <dt>
                <label for="certno" id="certnoL" runat="Server">
                    证件号：</label>
            </dt>
            <dd>
                <input type="text" id="certnotxt" class="texti" runat="server" onblur="sel(3);" />
                <label id="certnoLable" style="display: none">
                </label>
            </dd>
            
            <dt>
                <label>
                    性别：</label>
            </dt>
            <dd>
                <select id="sexSel" onchange="selvalue(this.value,2);" runat="server">
                    <option value="2" selected="selected">未知</option>
                    <option value="1">男</option>
                    <option value="0">女</option>
                </select>
                <input type="text" id="sextxt" value="1" runat="server" style="display: none;" />
            </dd>
            <dt>
                <label for="birthday">
                    生日：</label>
            </dt>
            <dd>
                <input type="text" id="birthdaytxt" class="texti" onclick="WdatePicker();" readonly="readonly"
                    runat="server" /><label id="birthdayLable" style="display: none;">
                        生日不能为空</label>
            </dd>
            <dt>
                <label>
                    文化程度：</label>
            </dt>
            <dd>
                <select id="EduSel" onchange="selvalue(this.value,3);" runat="server">
                    <option value="" selected="selected">未知</option>
                    <option value="1">小学</option>
                    <option value="2">初中</option>
                    <option value="3">高中/中专</option>
                    <option value="4">大学/专科</option>
                    <option value="5">研究生及以上</option>
                </select>
                <input type="text" id="Edutxt" value="1" runat="server" style="display: none;" />
            </dd>
            <dt>
                <label>
                    收入水平：</label>
            </dt>
            <dd>
                <select id="IncomeSel" onchange="selvalue(this.value,4);" runat="server">
                    <option value="" selected="selected">未知</option>
                    <option value="0"><=1000</option>
                    <option value="1">1000~3000</option>
                    <option value="2">3000~5000</option>
                    <option value="3">5000~8000</option>
                    <option value="4">8000~20000</option>
                    <option value="5">20000以上</option>
                </select>
                <input type="text" id="Incometxt" value="0" runat="server" style="display: none;" />
            </dd>
            <dt>
                <label>
                    所属省：</label>
            </dt>
            <dd>
                <select id="proInfoList" runat="server" onchange="selpro(this.value)">
                </select>
                <input type="text" id="stext" runat="server" style="display: none;" />
            </dd>
            <dt>
                <label>
                    所属城市：</label>
            </dt>
            <dd>
                <select id="areaInfoList" runat="server" onchange="selcity(this.value,$('#ctl00_ContentPlaceHolder1_caozuotxt').attr('value'))">
                </select>
                <select id="areaid" runat="server" style="display: none;">
                </select>
                <input type="text" id="resulttxt" runat="server" style="display: none;" />
                <input type="text" id="caozuotxt" runat="server" style="display: none;" value="1" />
            </dd>
        </dl>
        <div class="subtn">
            <span class="btn btnA"><span>
                <button type="button" onclick="UpdateCust();">
                    确定</button></span></span></div>
    </div>
    <input type="text" id="custidtxt" runat="server" style="display: none;" />
    <input type="text" id="spidtxt" runat="server" style="display: none;" />
    <input type="button" id="urlRedirectButton" style="display: none;" runat="server"
        onserverclick="urlRedirectButton_ServerClick" />
 </asp:Content>
