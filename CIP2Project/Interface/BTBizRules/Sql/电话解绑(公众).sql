USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_Customer_OV3_Interface_UnBindPhone]    脚本日期: 10/13/2009 14:36:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[up_Customer_OV3_Interface_UnBindPhone] (
 	@PhoneNum varchar(20),
 	@Result int out,
 	@ErrMsg varchar(256) out,
 	@CustID varchar(16) out,
 	@UserAccount varchar(16) out
 ) as
set @Result = -22500
	set @ErrMsg = ''
	set @CustID = ''
	set @UserAccount = ''
	select @CustID=CustID from CustPhone with(nolock) where Phone = @PhoneNum
	if(@@Rowcount < 1)
	begin
		set @Result = -30005
		set @ErrMsg = '该电话尚未被绑定，不能解绑'
		return
	end
		
	if not exists ( select 1 From CustInfo with(nolock) Where CustID = @CustID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此客户'
		return
	End	

	if exists ( select 1 From CustTourCard with(nolock) Where CustID = @CustID and [Status] = '00')
	Begin
		select @UserAccount = CardID from CustTourCard where CustID = @CustID and [Status] = '00'
	End

	delete from CustPhone where Phone = @PhoneNum and CustID = @CustID

	if(@@Rowcount < 1)
	begin
		set @Result = -30005
		set @ErrMsg = '该电话尚未被该客户绑定，不能解绑'
		return
	end
	
	set @Result = 0
	
	
Set QUOTED_IDENTIFIER Off
