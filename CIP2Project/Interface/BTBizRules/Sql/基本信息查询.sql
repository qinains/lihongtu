USE [BestTone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V2_Interface_BasicInfoQuery]    脚本日期: 03/27/2008 11:19:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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
 
 ALTER Procedure [dbo].[up_BT_V2_Interface_BasicInfoQuery]
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
	if( len(@UserAccount)=9)
		Set @UserAccountColumn = 'UserAccount'
	else 
		Set @UserAccountColumn = 'InnerCardID'
		
	--根据关键信息查询出UserAccount,和CustID
	Declare @sqlStr nvarchar(4000)
	
	set @sqlStr = 'select CustID, UserAccount,RealName,CertificateCode,CertificateType, status from CustInfo, CustUserInfo where 1=1 '
	
	if( @UserAccount != '')
		set @sqlStr = @sqlStr+ ' and ' + @UserAccountColumn + '=''' +@UserAccount+ ''''
	if( @CertificateCode != '')
		set @sqlStr = @sqlStr+ ' and CertificateCode=''' +@CertificateCode+ ''''
	if( @CertificateCode != '' and @CertificateType != '')
		set @sqlStr = @sqlStr+ ' and CertificateCode=''' + @CertificateCode + ''''+ ' and CertificateType=''' + @CertificateType + ''''
	if( @PhoneNum != '')
		set @sqlStr = @sqlStr+ ' and UserAccount in (select UserAccount from BoundPhone where BoundPhoneNumber=''' + @PhoneNum + ''')'
	if( @RealName != '')
		set @sqlStr = @sqlStr+ ' and RealName=''' + @RealName + ''''
	
	exec sp_executesql @sqlStr

/*
	--检查证件号码是否已被注册

	if( @CertificateCode <>'' and @CertificateType <> '')
	begin
		if exists(select 1 from CustInfo where CertificateCode=@CertificateCode and CertificateType=@CertificateType  )
		begin
			set @Result = -30002
			set @ErrMsg = '该证件已被注册。'
			return 
		end
	end
*/
			


Set QUOTED_IDENTIFIER Off
