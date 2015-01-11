set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go











/*
 * �洢����:up_Customer_V3_Interface_AddressInfoUploadQuery
 *
 * ��������: 
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-03
 * Remark:
 *
 */
 
 ALTER Procedure [dbo].[up_Customer_V3_Interface_AddressInfoUploadQuery]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@Result int out,
	@ErrMsg varchar(256) out,
	@AddressID Bigint,
	@Address varchar(256),
	@Zipcode varchar(6),
	@Type varchar(20),
	@DealType varchar(1),
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

	--@AddressIDΪ�գ�@DealType == 0ʱ��������
	if(@AddressID = '')
	begin
		if(@DealType = 0)
		begin
			
			if exists (select 1 from AddressInfo where CustID =@CustID and Address = @Address)
				begin
					set @Result = -22500
					set @ErrMsg = '�õ�ַ�Ѿ�����'
					return
				end
			insert into AddressInfo(CustID,Address,ZipCode,"Type",DealTime)
			values(@CustID,@Address,@Zipcode,@Type,getdate())
			
			set @Result = 0
			return
		end
	end

	--@AddressID��Ϊ�գ�@DealType == 1ʱɾ������
	if(@AddressID <> '')
	begin

		if not exists ( select 1 From AddressInfo Where SequenceID=@AddressID)
		Begin
			set @Result = -20504
			set @ErrMsg = '�޴����'
			return
		End

		if(@DealType = 1)
		begin
			delete from AddressInfo where SequenceID=@AddressID

			set @Result = 0
			return
		end
	end 

	--@AddressID��Ϊ�գ�@DealType == 2ʱ��������
	if(@AddressID <> '')
	begin

		if not exists ( select 1 From AddressInfo Where SequenceID=@AddressID)
		Begin
			set @Result = -20504
			set @ErrMsg = '�޴����'
			return
		End

		if(@DealType = 2)
		begin

			if exists (select 1 from AddressInfo where CustID =@CustID and Address = @Address and Zipcode=@Zipcode)
				begin
					set @Result = -22500
					set @ErrMsg = '�õ�ַ�Ѿ�����'
					return
				end

			update  AddressInfo 
			set Address=@Address,
				Zipcode=@Zipcode,
				"Type"=@Type,
				CustID=@CustID,
				DealTime=getdate()
			where SequenceID=@AddressID
			
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









