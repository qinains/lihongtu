set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



/*
 * 存储过程dbo.up_BT_V2_Interface_PasswordReset
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-4-17
 * Remark:
 *
 */
 
 ALTER Procedure [dbo].[up_BT_V2_Interface_PasswordReset]
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

	select @CustID = CustID from CustUserInfo where UserAccount = @UserAccount
	if(@@Rowcount = 1)
		set @Result = 0
	else
	Begin
		set @Result = -20504
		set @ErrMsg = '用户不存在'
		return
	End
	
	update CustInfo set EncryptedPassword = @NewPassword where CustID=@CustID
	

	
	select @ContactTel=CustContactTel , @Email=Email from CustExtend where CustID = @CustID
	
	select @RealName = RealName from CustInfo where CustID = @CustID
	
Set QUOTED_IDENTIFIER Off

