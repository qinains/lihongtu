set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go













/*
 * 存储过程dbo.up_Customer_OV3_Interface_InsertCustAuthenLog
 *
 * 功能描述: 
 *			 
 *
 * Author: tongb
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-19
 * Remark:
 *
 */



alter  Procedure [dbo].[up_Customer_OV3_Interface_InsertSSOCRMLog]
(
@SSOSPID varchar(8),
@SSOCustID varchar(16),
@SSOOuterID varchar(16),
@CRMCustID varchar(16),
@CRMOuterID varchar(16),
@ProvinceID varchar(2),
@Result int ,
@Description varchar(256),
@authenName varchar(30),
@authenType varchar(2)

	
) as
	
begin


insert into SSOCRMLog(SSOSPID,SSOCustID,SSOOuterID,CRMCustID,CRMOuterID,Dealtime,ProvinceID,Result,Description,authenName,authenType)
values(@SSOSPID,@SSOCustID,@SSOOuterID,@CRMCustID,@CRMOuterID,getdate(),@ProvinceID,@Result,@Description,@authenName,@authenType)
   
   
end






