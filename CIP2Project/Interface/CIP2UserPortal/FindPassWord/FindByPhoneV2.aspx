<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindByPhoneV2.aspx.cs" Inherits="FindByPhone" %>

<%@ Register Src="UserCtrl/Foot.ascx" TagName="Foot" TagPrefix="uc2" %>
<%@ Register Src="UserCtrl/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" rel="stylesheet" href="http://www.114yg.cn/static/css/global.min.css?t=20130922"></link>
   <link href="../css/renzheng.css" rel="stylesheet" type="text/css" />

    
</head>
<body>
    <form id="form1" runat="server">
              <div class=" wrap">
              <div class="top">
                <div style=" float:right; text-align:right; line-height:27px; padding:30px 0 0 0;">
                  <p style=" color:#039;">你好</p>
                  <p style=" color:#c00; font-size:14px;">号码百事通用户信息</p>
                </div>
              </div>
              <div style=" width:790px; margin:0 auto;">
                <div style=" border:1px solid #dadada;">
                  <div style=" background:#f4f4f4; text-align:center; font-size:24px; height:69px; line-height:69px;">认证手机重置密码</div>
                  <div style=" height:190px; background:url(images/buzhou_c.jpg) center bottom no-repeat; padding:130px 0 0 400px; font-size:16px; line-height:40px;">
                    <p>电信用户发送到
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
                    <p>联通或移动 用户发送到
                  
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
                  <div style=" padding:0 23px; margin:0 0 20px 0;">
                    <div style=" background:#f4f4f4; padding:0 0 30px 0;">
                      <div class="main">
                        <ul>
                          <li class="file">认证手机号码：</li>
                          <li class="inputs">
                            <input type="text" name="AuthenPhone"   id="AuthenPhone" />
                          </li>
                          <li class="wrong"> <label id="AuthenPhoneError" runat="server"  ></label></li>
                        </ul>
                      </div>
                      <div class="main">
                        <ul>
                          <li class="file">认证验证码：</li>
                          <li class="inputs">
                            <input type="text" name="AuthenCode"  id="AuthenCode" />
                          </li>
                          <li class="wrong"><label id="AuthenCodeError" runat="server"  ></label></li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </div>
                <div style=" text-align:center; margin:20px 0 0 0;">
                    
                    <asp:ImageButton ID="RestPasswordByPhoneBtn"   ImageUrl="images/bt_submit.gif" height="51" width="230"  runat="server" OnClick="RestPasswordByPhoneBtn_Click" />
                </div>
              </div>
            </div>
        <uc2:Foot ID="Foot1" runat="server" />
        <asp:HiddenField ID="hdReturnUrl" runat="server" />
        <asp:HiddenField ID="hdCustID" runat="server" />
        <asp:HiddenField ID="hdAuthenPhone" runat="server" />
        <asp:HiddenField ID="hdAuthenCode" runat="server" />
    </form>
    
</body>
</html>
