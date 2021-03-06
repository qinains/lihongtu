set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go







/*
 * 存储过程dbo.up_Customer_V3_Interface_UnBindPhone
 *
 * 功能描述: 电话解绑
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-7
 * Remark:
 *
 */

ALTER Procedure [dbo].[up_Customer_V3_Interface_UnBindPhone]
(
	@CustID varchar(16),
	@PhoneNum varchar(20),	
	@PhoneClass varchar(1),
	@Result int out,
	@ErrMsg varchar(256) out
)
as
	set @Result = -19999
	set @ErrMsg = ''

	if not exists(select 1 from CustPhone with(nolock) where Phone = @PhoneNum)
	begin
		set @Result = -30005
		set @ErrMsg = '该电话尚未被绑定，不能解绑'
		return
	end

	if not exists ( select 1 From CustInfo with(nolock) Where CustID = @CustID)
		Begin
			set @Result = -20504
			set @ErrMsg = '无此帐号'
			return
		End	
	
	delete from CustPhone where Phone = @PhoneNum and CustID = @CustID

	if(@@Rowcount < 1)
	begin
		set @Result = -30005
		set @ErrMsg = '该电话尚未被绑定，不能解绑'
		return
	end
	
	set @Result = 0







