if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_BindPhoneQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_BindPhoneQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go




/*
 * 存储过程dbo.up_BT_V2_Interface_BindPhoneQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-03-28
 * Remark:
 *
 */
 
Create Procedure [dbo].[up_BT_V2_Interface_BindPhoneQuery]
(
	@SPID varchar(8),
	@PhoneNum varchar(20),
	@Result int out,
	@ErrMsg varchar(40) out,
	@CustID varchar(16) out,
	@UserAccount varchar(16) out,
	@RealName varchar(30) out
)
as
	Set @Result = -19999
	Set @ErrMsg = ''
	
	Set @CustID = ''
	Set @UserAccount = ''
	select @CustID=CustID, @UserAccount=UserAccount from BoundPhone where BoundPhoneNumber = @PhoneNum and CustPersonType='1'
	if(@CustID = '')
	Begin
		Set @Result = 1
		Set @ErrMsg = '未绑定'
		return
	End
	
	select @RealName = RealName from CustInfo where CustID = @CustID
	Set @Result =0
	
	