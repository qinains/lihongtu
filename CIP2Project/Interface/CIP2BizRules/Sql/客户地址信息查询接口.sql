

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_AddressInfoQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_AddressInfoQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程up_Customer_V3_Interface_AddressInfoQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-03
 * Remark:
 *
 */
 
 Create Procedure dbo.up_Customer_V3_Interface_AddressInfoQuery
(
	@SPID varchar(8),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16)
)
as
	set @Result = -22500
	set @ErrMsg = ''

	if not exists ( select 1 From AddressInfo Where CustID = @CustID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无地址信息'
		return
	End

	select SequenceID,Address,ZipCode,"Type" From AddressInfo with(nolock) Where CustID = @CustID

	set @Result = 0
	
	
Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go
