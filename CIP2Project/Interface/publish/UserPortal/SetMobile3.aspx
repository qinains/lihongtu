<%@ page language="C#" autoeventwireup="true" inherits="SetMobile3, App_Web_setmobile3.aspx.cdcab7d2" masterpagefile="~/MasterPage2.master" enableEventValidation="false" %>
<%@ Register TagPrefix="CIPUserCtrl" TagName="TokenValidate" Src="UserCtrl/ValidateCIPToken.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <CIPUserCtrl:TokenValidate ID="TokenValidate" runat="server"></CIPUserCtrl:TokenValidate>

    <script language="javascript" type="text/javascript" src="JS/setMobile/setMoblie_JS.js"></script>

    <form id="form1" >
            <div style=" width:790px; float:right;">
            <div style=" border:1px solid #dadada;">
                <div style=" background:#f4f4f4; text-align:center; font-size:24px; height:69px; line-height:69px;">认证手机</div>
                <div style=" height:190px; background:url(images/buzhou.jpg) center bottom no-repeat; padding:130px 0 0 400px; font-size:16px; line-height:40px;">
                    <p>
                    电信用户发送到
                  <% 
                      if (SPID.Equals("35433333"))
                      {
                   %>
                    <font color="#e25500">"11811411" </font>
                   <% 
                      }
                      else 
                      { 
                  %>
                    <font color="#e25500">"11811412" </font>
                  <%   
                      }
                  %>  
                    
                   
                    </p>
                    <p>
                    联通或移动 用户发送到
                   
                   <% 
                      if (SPID.Equals("35433333"))
                      {
                   %>
                        <font color="#e25500">"10690007311811"</font>
                   <% 
                      }
                      else 
                      { 
                  %>
                    <font color="#e25500">"10690007311812"</font>
                  <%   
                      }
                  %>  
                    
                
                    </p>
                </div>
                <div style=" padding:0 19px; margin:0 0 20px 0;">
                    <div style=" background:#f4f4f4; padding:0 0 30px 0;">
                        <div style="line-height: 32px;padding: 10px 0 0 30px;">
                          
                                <div style="float: left;text-align: right;width:161px;">认证手机号码：</div>
                              
                                    <input  style="border:1px solid #B5B5B5;height: 30px;padding: 0 5px;width: 256px;" type="text" id="AuthenMobile" name="AuthenMobile"  />
                                    
                                    <label id="AuthenPhoneError" runat="server"  style="color: rgb(255, 0, 0); float: right; padding: 0px 189px 0px 5px; width: 48px; height: 32px; border-left-width: 0px; border-right-width: 0px;"></label>
                               
                           
                        </div>
                        <div style="line-height: 32px;padding: 10px 0 0 30px;">
                            
                                <div style="float: left;text-align: right;width:161px;">认证验证码：</div>
                                
                                    <input  style="border:1px solid #B5B5B5;height: 30px;padding: 0 5px;width: 256px;" type="text" id="AuthenCode" name="AuthenCode" />
                                    <label id="AuthenCodeError" runat="server"   style="color: rgb(255, 0, 0); float: right; padding: 0px 189px 0px 5px; width: 48px; height: 32px; border-left-width: 0px; border-right-width: 0px;"></label>
                            
                        </div>
                    </div>
                </div>
            </div>
            <div style=" text-align:center; margin:20px 0 0 0;">
                <input type="text" id="custidtxt" runat="server" style="display: none;" />
                <input type="text" id="returnurltxt" runat="server" style="display: none;" />
                <input type="text" id="spidtxt" runat="server" style="display: none;" />
                <asp:ImageButton ID="MobileAuthenButton" ImageUrl="images/bt_submit.gif" height="51" width="230" runat="server" OnClick="MobileAuthenButton_Click" />
               
                
            </div>
        </div>
    </form>

</asp:Content>