set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
/*
 * 存储过程dbo.up_BT_V2_Interface_BindPhoneQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-13
 * Remark:
 *
 */
 
alter Procedure [dbo].[up_BT_OV3_Interface_BindPhoneQuery]
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
	Set @ErrMsg =''
	
	Set @CustID = ''
	Set @UserAccount = ''
	select @CustID=CustID from CustPhone  with(nolock) where Phone = @PhoneNum and CustType='42' and PhoneClass=2
	if(@CustID = '')
	Begin
		Set @Result = 1
		Set @ErrMsg = '未绑定'
		return
	End
	
	select @RealName = RealName from CustInfo  with(nolock)  where CustID = @CustID
	select @UserAccount = CardID from CustTourCard where CustID = @CustID
	Set @Result =0
	
	
