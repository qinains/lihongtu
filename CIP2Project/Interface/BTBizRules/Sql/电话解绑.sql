


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_UnBindPhone]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_UnBindPhone]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_V2_Interface_UnBindPhone
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
 
 Create Procedure dbo.up_BT_V2_Interface_UnBindPhone
(
	@PhoneNum varchar(20),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16) out,
	@UserAccount varchar(16) out
)
as
	set @Result = -22500
	set @ErrMsg = ''
	set @CustID = ''
	set @UserAccount = ''
	select @CustID=CustID, @UserAccount=UserAccount from BoundPhone where BoundPhoneNumber = @PhoneNum
	if(@@Rowcount != 1)
	begin
		set @Result = -30005
		set @ErrMsg = '该电话尚未被绑定，不能解绑'
		return
	end
	
	
	delete from BoundPhone where BoundPhoneNumber = @PhoneNum

	if(@@Rowcount != 1)
	begin
		set @Result = -30005
		set @ErrMsg = '该电话尚未被绑定，不能解绑'
		return
	end
	
	set @Result = 0
	
	
Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go