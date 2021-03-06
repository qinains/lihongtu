set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




/*
 * 存储过程dbo.up_Customer_OV3_Interface_GetBoundPhone
 *
 * 功能描述: 获取用户绑定的号码列表(公众)
 *			 
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-12
 * Remark:
 *
 */
 
ALTER Procedure [dbo].[up_Customer_OV3_Interface_GetBoundPhone]
(
	@CustID varchar(16),
	@UserAccount varchar(16),
	@Result int out,
	@ErrMsg varchar(256) out
)
as

	if not exists(select 1 Phone from CustTourCard with(nolock) where CustID = @CustID and (CardID = @UserAccount or InnerCardID = @UserAccount))
	Begin
		set @Result = -20504
		set @ErrMsg = '无此用户'
		return
	End

	if not exists(select 1 Phone from CustInfo with(nolock) where CustID = @CustID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	select top 5 Phone BoundPhoneNumber
	from CustPhone with(nolock)
	where CustID = @CustID
	order by DealTime

set ANSI_NULLS Off
set QUOTED_IDENTIFIER Off


