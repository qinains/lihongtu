set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



/*
 * 存储过程dbo.up_BT_V2_Interface_BasicInfoQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-7-5
 * Remark:
 *
 */
 
create Procedure [dbo].[up_BT_OV3_Interface_BasicInfoQuery]
(
	@ProvinceID varchar(2),
	@SPID varchar(8),
	@UserAccount varchar(16),
	@PhoneNum varchar(20),
	@CertificateCode varchar(20),
	@CertificateType varchar(2),
	@RealName varchar(30)
)
as

	Declare @UserAccountColumn varchar(12)
	if( len(@UserAccount)=16)
		Set @UserAccount = substring(@UserAccount,5,9)

		
	--根据关键信息查询出UserAccount,和CustID
	Declare @sqlStr nvarchar(4000)
	
	set @sqlStr = 'select a.CustID, b.CardID UserAccount,RealName,CertificateCode,CertificateType, a.status from CustInfo a with(nolock) left join CustTourCard b with(nolock) on a.CustID=b.CustID  where 1=1 '
	
	if( @UserAccount != '')
		set @sqlStr = @sqlStr+ ' and b.CardID=''' +@UserAccount+ ''' and b.Status=''00'''
	if( @CertificateCode != '')
		set @sqlStr = @sqlStr+ ' and CertificateCode=''' +@CertificateCode+ ''''
	if( @CertificateCode != '' and @CertificateType != '')
		set @sqlStr = @sqlStr+ ' and CertificateCode=''' + @CertificateCode + ''''+ ' and CertificateType=''' + @CertificateType + ''''
	if( @PhoneNum != '')
		set @sqlStr = @sqlStr+ ' and a.CustID in (select CustID from CustPhone where Phone=''' + @PhoneNum + ''')'
	if( @RealName != '')
		set @sqlStr = @sqlStr+ ' and RealName=''' + @RealName + ''''
	
		Set @sqlStr = @sqlStr + ' and a.CustType not in (''11'',''12'',''13'',''14'',''20'',''30'')  '
	exec sp_executesql @sqlStr
		


Set QUOTED_IDENTIFIER Off

