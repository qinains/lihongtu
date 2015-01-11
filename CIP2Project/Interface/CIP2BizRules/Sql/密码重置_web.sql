set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go








/*
 * 存储过程dbo.up_Customer_OV3_Interface_WebPasswordReset
 *
 * 功能描述: 
 *			 
 *
 * Author: lihongtu
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-17
 * Remark:
 *
 */
 
 ALTER Procedure [dbo].[up_Customer_OV3_Interface_WebPasswordReset]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@UserAccount varchar(16),
	@NewPassword varchar(50),
	@Result int out,
	@ErrMsg varchar(256) out,
	@ContactTel varchar(20) out,
	--@ProvinceID varchar(2) out,
	@Email varchar(256) out,
	@RealName varchar(30) out


)
as
	set @Result = -22500
	set @ErrMsg = ''
	update CustInfo set WebPwd = @NewPassword where CustID=@CustID --and UserAccount = @UserAccount
	
	if(@@Rowcount = 1)
	Begin
		set @Result = 0
		set @ErrMsg = 'web密码重置成功'
	End
	else
	Begin
		set @Result = -20504
		set @ErrMsg = '用户不存在'
		return
	End
	
	
	--select @ContactTel=CustContactTel , @Email=Email from CustExtend where CustID = @CustID
	--select @ProvinceID = provinceID from CustInfo where CustID = @CustID
	--select @CardID = CardID from CustTourCard where  CustID = @CustID
	Declare @EmailClass Int
	--只有认证邮箱和认证手机可以获取密码
	select @Email=Email, @EmailClass= EmailClass, @RealName = RealName from CustInfo where CustID = @CustID
	if(@EmailClass <> 1)
	Begin
		set @Email = ''
	End
	select @ContactTel=Phone from CustPhone with(nolock) where CustID = @CustID and phoneclass = 2


Set QUOTED_IDENTIFIER Off








