


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_CustBasicInfoQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_CustBasicInfoQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程up_Customer_V3_Interface_CustBasicInfoQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-7-30
 * Remark:
 *
 */
 
 Create Procedure dbo.up_Customer_V3_Interface_CustBasicInfoQuery
(
	@SPID varchar(8),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16),
	@OuterID varchar(20) out,
	@Status varchar(2) out,
	@CustType varchar(2) out,
	@CustLevel varchar(1) out,
	@RealName varchar(30) out,
	@UserName varchar(30) out,
	@NickName varchar(30) out,
	@CertificateCode varchar(30) out,
	@CertificateType varchar(1) out,
	@Sex varchar(1) out,
	@Email varchar(100) out,
	@EnterpriseID varchar(2) out,
	@ProvinceID varchar(2) out,
	@AreaID varchar(3) out,
	@Registration datetime out
)
as
	set @Result = -22500
	set @ErrMsg = ''
	set @OuterID = ''
	set @Status = ''
	set @CustType = ''
	set @CustLevel = ''
	set @RealName = ''
	set @UserName = ''
	set @NickName = ''
	set @CertificateCode = ''
	set @CertificateType = ''
	set @Sex = ''
	set @Email = ''
	set @EnterpriseID = ''
	set @ProvinceID = ''
	set @AreaID = ''
	set @Registration = ''

	if not exists ( select 1 From CustInfo Where CustID = @CustID )
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	select @Status=Status, @CustType=CustType, @CustLevel=CustLevel, @RealName=RealName, @UserName=UserName,
	       @NickName=NickName, @CertificateCode=CertificateCode, @CertificateType=CertificateType,
	       @Sex=Sex, @Email=Email,@EnterpriseID=EnterpriseID,@ProvinceID=ProvinceID,@AreaID=
			case(@SPID)
			when '01010101' then  b.RegionCode--若是移百系统则转换为国标
			else
				case left(AreaID,1) 
					when '0' then Right(AreaID,2)
					else AreaID
				end
			end
	,@Registration=CreateTime from CustInfo,RegionCodeArea b where CustID = @CustID
		
	set @Result = 0
	
	
Set QUOTED_IDENTIFIER Off


