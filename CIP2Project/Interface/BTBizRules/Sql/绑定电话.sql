USE [BestTone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V2_Interface_BindPhoneNum]    脚本日期: 03/27/2008 11:43:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/*
 * 存储过程dbo.up_BT_V2_Interface_BindPhoneNum
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-12-27
 * Remark:
 *
 */
 
create Procedure [dbo].[up_BT_V2_Interface_BindPhoneNum]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@UserAccount varchar(16),
	@PhoneNum varchar(20),
	@Result int out,
	@ErrMsg varchar(256) out

)
as
	set @Result = -22500
	set @ErrMsg = ''

	if not exists ( select 1 From CustInfo Where CustID = @CustID and UserAccount = @UserAccount)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	if exists( select 1 from BoundPhone where BoundPhoneNumber = @PhoneNum)
	Begin
		set @Result = -30003
		set @ErrMsg = '该电话已经被其他用户绑定，不允许重复绑定'
		return
	End
	
	select 1 from BoundPhone where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = -30004
		set @ErrMsg = '该用户超过绑定电话个数限制'
		return
	End
	
	Declare @CurrentTime dateTime
	set @CurrentTime = getDate()
	
	declare @CustPersonType varchar(1)
	Set @CustPersonType = ''
	
	select @CustPersonType = CustPersonType from CustUserInfo where UserAccount = @UserAccount or InnerCardID = @UserAccount
	
	--插入绑定关系表

	insert Into BoundPhone( CustID,UserAccount,CustPersonType,BoundPhoneNumber,DealTime)
	values( @CustID,@UserAccount,@CustPersonType,@PhoneNum,@CurrentTime)
	
	if(@@RowCount = 1)
		set @Result = 0

Set QUOTED_IDENTIFIER Off
Set ANSI_NULLS Off
