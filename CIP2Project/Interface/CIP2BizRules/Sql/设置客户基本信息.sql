set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go















/*
 * 存储过程:up_Customer_V3_Interface_UpdateCustinfo_1
 *
 * 功能描述: 
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-11-04
 * Remark:
 *
 */
 
 ALTER Procedure [dbo].[up_Customer_V3_Interface_UpdateCustinfo_1]
(
	@SPID varchar(8),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16),
	@RealName varchar(30),
	@CertificateCode varchar(30),
	@CertificateType varchar(1),
	@Sex varchar(1),
	@Email varchar(100)
)
as
	set @Result = -22500
	set @ErrMsg = ''

	if not exists ( select 1 From CustInfo Where CustID = @CustID )
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	if(@RealName = '')
		begin
			select @RealName=RealName from CustInfo Where CustID = @CustID 
		end

	if(@CertificateCode = '')
		begin
			select @CertificateCode=CertificateCode from CustInfo Where CustID = @CustID 
		end

	if(@CertificateType = '')
		begin
			select @CertificateType=CertificateType from CustInfo Where CustID = @CustID 
		end

	if(@Sex = '')
	begin
		select @Sex=Sex from CustInfo Where CustID = @CustID 
	end

	if(@Email = '')
	begin
		select @Email=Email from CustInfo where CustID = @CustID
	end

	update CustInfo set  RealName = @RealName,
	CertificateCode = @CertificateCode,
	CertificateType = @CertificateType,
	Sex = @Sex,
	Email = @Email
	where CustID = @CustID

	if(@@Error <> 0)
	Begin
		set @Result = -22500
		set @ErrMsg = '更新客户基本信息表时错误'
		rollback
		return
	End	

	set @Result = 0
	
	
Set QUOTED_IDENTIFIER Off













