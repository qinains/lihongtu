set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




/*
 * 存储过程dbo.up_Customer_V3_Interface_AuthenPhoneChange
 *
 * 功能描述: 认证电话变更
 *
 * Author: liuchunli
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-10
 * Remark:
 *
 */

ALTER Procedure [dbo].[up_Customer_V3_Interface_AuthenPhoneChange]
(
	@CustID Varchar(16),
	@AuthenPhone Varchar(20),
	@Result int out,
	@ErrMsg varchar(256) out
)
as
	declare @CustType varchar(2)
	set @CustType = ''
	Set @Result = -19999
	Set @ErrMsg = ''

	if not exists ( select 1 From CustPhone with(nolock) Where CustID = @CustID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End	

	select @CustType = CustType from CustInfo with(nolock) where CustID = @CustID

	if exists ( select 1 From CustPhone with(nolock) Where Phone = @AuthenPhone and CustType = @CustType and PhoneClass != '1')
	Begin
		set @Result = -30003
		set @ErrMsg = '该电话已经被用户绑定为认证电话，不允许重复绑定'
		return
	End	
	
	update CustPhone set Phone = @AuthenPhone, PhoneClass = '2' where CustID = @CustID

	if(@@Error<>0 )
	begin
		set @Result = -19999
		set @ErrMsg = '认证电话更新失败'
		return
	end
	
	set @Result = 0




