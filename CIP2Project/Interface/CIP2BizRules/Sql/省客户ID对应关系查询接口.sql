

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_CustProvinceRelationQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_CustProvinceRelationQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程up_Customer_V3_Interface_CustProvinceRelationQuery
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
 
 Create Procedure dbo.up_Customer_V3_Interface_CustProvinceRelationQuery
(
	@SPID varchar(8),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16) out,
	@OuterID varchar(20),
	@ProvinceID varchar(2)
)
as
	set @Result = -22500
	set @ErrMsg = ''
	set @CustID = ''

	if not exists ( select 1 From CustInfo Where OuterID = @OuterID  and ProvinceID = @ProvinceID)
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	select  @CustID=CustID from CustInfo where OuterID = @OuterID  and ProvinceID = @ProvinceID

	if(@@Rowcount <> 1)
	begin
		set @Result = -19999
		set @ErrMsg = '未知错误'
		return
	end
		
	set @Result = 0
	
	
Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go
