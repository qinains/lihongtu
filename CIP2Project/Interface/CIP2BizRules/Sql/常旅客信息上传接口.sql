set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go






/*
 * 存储过程:up_Customer_V3_Interface_FrequentUserUploadQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-05
 * Remark:
 *
 */
 
 ALTER Procedure [dbo].[up_Customer_V3_Interface_FrequentUserUploadQuery]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@Result int out,
	@ErrMsg varchar(256) out,
	@FrequentUserID BigInt,
	@DealType varchar(1),
	@RealName varchar(16),
	@CertificateCode varchar(20),
	@CertificateType varchar(1),
	@Phone varchar(20),
	@ExtendField varchar(256)	
)
as
	set @Result = -22500
	set @ErrMsg = ''


	if not exists (select 1 from CustInfo where CustID = @CustID)
	begin
		set @Result = -22500
		set @ErrMsg = '该客户不存在'
		return
	end
	--@FrequentUserID为空，@DealType == 0时插入数据
	if(@FrequentUserID = 0)
	begin
		if(@DealType = 0)
		begin
			
			if exists (select 1 from FrequentUserInfo where CustID = @CustID and FrequentUserName = @RealName and Phone = @Phone)
			begin
				set @Result = -22500
				set @ErrMsg = '该旅客已经存在'
				return 
			end
			
			insert into FrequentUserInfo(CustID,FrequentUserName,CertificateType,CertificateCode,Phone,DealTime)
			values(@CustID,@RealName,@CertificateType,@CertificateCode,@Phone,getdate())
			
			set @Result = 0
			return
		end
	end

	--@FrequentUserID不为空，@DealType == 1时删除数据
	if(@FrequentUserID <> 0)
	begin

		if not exists ( select 1 From FrequentUserInfo Where SequenceID=@FrequentUserID)
		Begin
			set @Result = -20504
			set @ErrMsg = '无此序号'
			return
		End

		if(@DealType = 1)
		begin
			delete from FrequentUserInfo where SequenceID=@FrequentUserID

			set @Result = 0
			return
		end
	end 

	--@FrequentUserID不为空，@DealType == 2时更新数据
	if(@FrequentUserID <> 0)
	begin

		if not exists ( select 1 From FrequentUserInfo Where SequenceID=@FrequentUserID)
		Begin
			set @Result = -20504
			set @ErrMsg = '无此序号'
			return
		End

		if(@DealType = 2)
		begin

			if exists (select 1 from FrequentUserInfo where CustID = @CustID and FrequentUserName = @RealName and Phone = @Phone)
			begin
				set @Result = -22500
				set @ErrMsg = '该旅客已经存在'
				return 
			end

			update  FrequentUserInfo 
			set CustID=@CustID,
				FrequentUserName=@RealName,
				CertificateType=@CertificateType,
				CertificateCode=@CertificateCode,
				Phone=@Phone,
				DealTime=getdate()
			where SequenceID=@FrequentUserID
			
			set @Result = 0
			return

		end
	end
	
	if(@@Rowcount <> 1)
	begin
		set @Result = -19999
		set @ErrMsg = '未知错误'
		return
	end
				
Set QUOTED_IDENTIFIER Off




