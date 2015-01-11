

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_Interface_ConvertCertificateCode]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_Interface_ConvertCertificateCode]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_Interface_ConvertCertificateCode
 *
 * 功能描述: 新身份证转化为旧身份证
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-5-23
 * Remark:
 *
 */
 
 Create Procedure dbo.up_BT_Interface_ConvertCertificateCode
(
	@NewCertificateCode varchar(18),
	@OldCertificateCode varchar(16) output
)
AS
	Set @OldCertificateCode = ''
	
	if(len(@NewCertificateCode) != 18)
		return
		
	Declare @PreSix char(6)
	Declare @EndTen char(13)
	--取前六位
	set @PreSix = left(@NewCertificateCode,6)
	--取后10位
	set @EndTen = right(@NewCertificateCode,10)
	--去掉末尾一位
	Set @OldCertificateCode = left( (@PreSix +	@EndTen), 15)
	
	

	

Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go