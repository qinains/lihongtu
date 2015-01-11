set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go






/*
 * �洢����:up_Customer_V3_Interface_FrequentUserUploadQuery
 *
 * ��������: 
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
		set @ErrMsg = '�ÿͻ�������'
		return
	end
	--@FrequentUserIDΪ�գ�@DealType == 0ʱ��������
	if(@FrequentUserID = 0)
	begin
		if(@DealType = 0)
		begin
			
			if exists (select 1 from FrequentUserInfo where CustID = @CustID and FrequentUserName = @RealName and Phone = @Phone)
			begin
				set @Result = -22500
				set @ErrMsg = '���ÿ��Ѿ�����'
				return 
			end
			
			insert into FrequentUserInfo(CustID,FrequentUserName,CertificateType,CertificateCode,Phone,DealTime)
			values(@CustID,@RealName,@CertificateType,@CertificateCode,@Phone,getdate())
			
			set @Result = 0
			return
		end
	end

	--@FrequentUserID��Ϊ�գ�@DealType == 1ʱɾ������
	if(@FrequentUserID <> 0)
	begin

		if not exists ( select 1 From FrequentUserInfo Where SequenceID=@FrequentUserID)
		Begin
			set @Result = -20504
			set @ErrMsg = '�޴����'
			return
		End

		if(@DealType = 1)
		begin
			delete from FrequentUserInfo where SequenceID=@FrequentUserID

			set @Result = 0
			return
		end
	end 

	--@FrequentUserID��Ϊ�գ�@DealType == 2ʱ��������
	if(@FrequentUserID <> 0)
	begin

		if not exists ( select 1 From FrequentUserInfo Where SequenceID=@FrequentUserID)
		Begin
			set @Result = -20504
			set @ErrMsg = '�޴����'
			return
		End

		if(@DealType = 2)
		begin

			if exists (select 1 from FrequentUserInfo where CustID = @CustID and FrequentUserName = @RealName and Phone = @Phone)
			begin
				set @Result = -22500
				set @ErrMsg = '���ÿ��Ѿ�����'
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
		set @ErrMsg = 'δ֪����'
		return
	end
				
Set QUOTED_IDENTIFIER Off




