set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go







/*
 * 存储过程dbo.up_Customer_OV3_Interface_CheckUserPassword
 *
 * 功能描述: 
 *			 
 *
 * Author: lihongtu
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-10
 * Remark:
 *
 */

 
 ALTER Procedure [dbo].[up_Customer_OV3_Interface_ValidUserPassword]
(
	@CustID varchar(16),
	@EncryptedPassword varchar(50),
	@Result int output

)
as
	set @Result = -19999

	declare @ErrMsg varchar(100)
	set @ErrMsg = ''
	

	Declare @sqlStr nvarchar(4000)
	declare @WebPwd varchar(50)
	declare @VoicePwd varchar(50)
	set @WebPwd = ''
	set @VoicePwd = ''

	set @sqlStr = 'select top 1 @WebPwd=WebPwd,@VoicePwd=VoicePwd  from CustInfo where  CustID='''+@CustID+''' '

	exec sp_executesql @sqlStr,N' @WebPwd varchar(50) output,@VoicePwd varchar(50) output', @WebPwd output,@VoicePwd output

	if(@@RowCount != 1)
	Begin
		set @Result = -20504
		set @ErrMsg = '用户信息不存在,无此帐号'
		return
	End
	
	if(@WebPwd!=@EncryptedPassword and @VoicePwd!=@EncryptedPassword)
	Begin
		set @Result = -20504
		set @ErrMsg = '密码不正确'
		return
	End
	else
	Begin
		set @Result = 0
		set @ErrMsg = '密码正确'
		return
	End

Set QUOTED_IDENTIFIER Off








