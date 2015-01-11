set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go











/*
 * 存储过程: up_Customer_OV3_Interface_CustProvinceRelationQuery
 *
 * 功能描述: 客户注册接口(soap)
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-10-29
 * Remark:
 *
 */
 
 create Procedure [dbo].[up_Customer_OV3_Interface_CustProvinceRelationQuery]
(
	@Outerid varchar(16),
	@ProvinceID varchar(2),
	@Result int out,
	@ErrorDescription varchar(256) out,
	@CustID varchar(16) out,
	@SID varchar(16) out,
	@CustAccount varchar(16) out

)
as

	SET @Result = -22500
	SEt @ErrorDescription = '' 
	

	if not exists ( select 1 From CustInfo Where OuterID = @Outerid  and ProvinceID = @ProvinceID)
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








