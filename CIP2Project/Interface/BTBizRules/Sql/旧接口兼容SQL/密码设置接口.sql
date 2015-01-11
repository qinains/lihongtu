set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




/*
 * 存储过程dbo.up_BT_OV3_Interface_PasswordReset
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-17
 * Remark:
 *
 */
 
alter Procedure [dbo].[up_BT_OV3_Interface_PasswordReset]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@UserAccount varchar(16),
	@NewPassword varchar(50),
	@Result int out,
	@ErrMsg varchar(256) out,
	@ContactTel varchar(20) out,
	@Email varchar(256) out,
	@RealName varchar(30) out
)
as
	set @Result = -22500
	set @ErrMsg = ''

	if(len(@UserAccount) = 16)
	Begin
		Set @UserAccount = SubString(@UserAccount,5,9)
	End
	
	declare @Status varchar(2)
	
	select @CustID = CustID, @Status = Status from CustTourCard  with(nolock) where CardID = @UserAccount
	if(@@Rowcount = 1)
		set @Result = 0
	else
	Begin
		set @Result = -20504
		set @ErrMsg = '用户不存在'
		return
	End

	if(@Status <> '00')
	Begin
		set @Result = -20504
		set @ErrMsg = '用户状态不正常'
		return
	End
	
	update CustInfo set VoicePwd = @NewPassword where CustID=@CustID
	Declare @EmailClass Int
	--只有认证邮箱和认证手机可以获取密码
	select @Email=Email, @EmailClass= EmailClass, @RealName = RealName from CustInfo where CustID = @CustID
	if(@EmailClass <> 1)
	Begin
		set @Email = ''
	End
	select @ContactTel=Phone from CustPhone with(nolock) where CustID = @CustID

Set QUOTED_IDENTIFIER Off


