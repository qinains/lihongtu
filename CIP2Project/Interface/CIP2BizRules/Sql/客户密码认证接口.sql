set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
/*
 * 存储过程up_Customer_OV3_Interface_CustPwdAuth
 *
 * 功能描述: 3.1.10 客户密码认证接口
 *			 
 *
 * Author: tongbo
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-11
 * Remark:
 *
 */

ALTER Procedure [dbo].[up_Customer_OV3_Interface_CustPwdAuth]
(
@SPID varchar(8),
@CustID varchar(16),
@Pwd varchar(128),
@PwdType varchar(1),
@Result int out,
@ErrMsg varchar(256) out,
@NewCustID varchar(16) out
)
as

begin
    set @Result = -22500
	set @ErrMsg = ''
	set @NewCustID = ''

	if not exists ( select 1 From CustInfo with(nolock) Where CustID=@CustID )
	Begin
		set @Result = -20504
		set @ErrMsg = '无此CustID'
		return
	End

   if(@PwdType='2')
	   begin
         if not exists ( select 1 From CustInfo with(nolock) Where CustID=@CustID and WebPwd=@Pwd )
			 Begin
                set @Result = -20503
				set @ErrMsg = 'WEB密码不正确'
				return
             end 
	   end
   else if(@PwdType='1')
	   begin
         if not exists ( select 1 From CustInfo with(nolock) Where CustID=@CustID and VoicePwd=@Pwd  )
			 Begin
                set @Result = -20503
				set @ErrMsg = '语音密码不正确'
				return
             end 
	   end 
   else
       begin
          if not exists ( select 1 From CustInfo with(nolock) Where CustID=@CustID and (VoicePwd=@Pwd or WebPwd=@Pwd) )
             Begin
                set @Result = -20503
				set @ErrMsg = '密码不正确'
				return
             end 
       end 

    set @Result = 0
	set @ErrMsg = ''
	set @NewCustID = @CustID
end


