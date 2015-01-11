set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go











/*
 * �洢����: up_Customer_V3_Interface_UserRegistryV2
 *
 * ��������: �ͻ�ע��ӿ�(soap)
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-07
 * Remark:
 *
 */
 
 ALTER Procedure [dbo].[up_Customer_V3_Interface_UserRegistryV2]
(
	@SPID varchar(8),
	@UserType varchar(2),
	@IsNeedTourCard varchar(1),
	@CardID varchar(16),
	@Password varchar(50),
	@UProvinceID varchar(2),
	@AreaCode varchar(6),
	@RealName varchar(50),
	@UserName varchar(50),
	@AuthenPhone varchar(15),
	@ContactTel varchar(15),
	@CertificateCode varchar(30),
	@CertificateType varchar(1),
	@Sex varchar(1),
	@Result int out,
	@ErrMsg varchar(256) out,
	@oCustID varchar(16) out,
	@oTourCardID varchar(9) out,
	@sCardID varchar(16) out

)
as

	set @Result = -22500
	set @ErrMsg = ''
	set @oCustID = ''
	set @oTourCardID = ''
	set @sCardID = ''
	--�ͻ�����ȫ���滻Ϊ42
	set @UserType = '42'

	if(len(@AreaCode)=2)
		Set @AreaCode='0'+@AreaCode

	if(len(@AreaCode)=6 or @AreaCode=0)
	Begin
		select @AreaCode = AreaCode from RegionCodeArea where RegionCode = @AreaCode
		if(@@Rowcount<1)
		Begin
			Set @Result = -30001
			set @ErrMsg = '��������������'
			return
		End
	End

	--�������Ż�ȡʡ����
	select @UProvinceID = ProvinceID from area where areaID = @AreaCode

	if(@@rowcount =0)
	Begin
		Set @Result = -30008
		set @ErrMsg = '����������'
		return
	End
	--------------------------------------------------------------------------------------------------------
	
	if(@UserType <> '41' and @UserType <> '42')
	begin
		set @Result = -22500
		set @ErrMsg = '�ͻ����Ͳ��Ϸ�:' + @UserType 
		return
	end
	
	
	-- ��ȡ @CustID ��ȡֵ

	declare @CustID varchar(16)
	declare @SecquenceID int
	begin tran
		select @SecquenceID = SequenceID  from ParSequenceNumber with(nolock) where SequenceType = '0101'
		Set @CustID = @SecquenceID+1
		update ParSequenceNumber set SequenceID =  @SecquenceID+1 where SequenceType = '0101'
	commit


	--------------------------------------------------------------------------------------------------------------
	if exists (select 1 from CustInfo where CustID=@CustID)
	Begin
		set @Result = -30001
		set @ErrMsg = '�ͻ�id�ʺ��Ѵ���' + @CustID 
		return
	End

	--У��֤�� ��Ϊ�գ� ���������ظ���
	if(@CertificateCode <> '' and @CertificateType <> '')
	Begin
		if exists(select 1 from custInfo with(nolock) where CertificateCode=@CertificateCode and CertificateType=@CertificateType and status = '00' and CustType=@UserType )
		Begin
			set @Result = -30001
			set @ErrMsg = '֤�����Ѵ��ڣ�����ע��'
			return
		End
	End
	
	--�û�����----������Ա
	declare @CustLevel varchar(1)
		Set @CustLevel = '3'
		
		
	begin tran
	
	--����ͻ���Ϣ�� 
	insert into CustInfo(CustID,ProvinceID,AreaID,CustType,VoicePwd,CertificateType,CertificateCode,
						RealName,UserName,CustLevel,Sex,RegistrationSource,Status,SourceSPID,DealTime,CreateTime)
	values(@CustID,@UProvinceID,@AreaCode,@UserType,@Password,@CertificateType,@CertificateCode,
		  @RealName,@UserName,@CustLevel,@Sex,1,'00',@SPID,getdate(),getdate())
	if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '����ͻ�������Ϣ��ʱ����'
			rollback
			return
		End	

    --��֤�绰��
	declare @Result_Phone int
	declare @ErrMsg_phone varchar(256)

	if(@AuthenPhone <> '')
	begin
		exec up_Customer_V3_Interface_PhoneSet @SPID,@CustID,@AuthenPhone,'2','2',@Result_Phone output,@ErrMsg_phone output
		
		if(@Result_Phone <> 0)
			Begin
				set @Result = -22500
				set @ErrMsg = '��֤�绰�󶨴���,'+@ErrMsg_phone
				rollback
				return
			End
	end

	
	--��ϵ�绰��
	if(@ContactTel <> '')
	begin
		if(@AuthenPhone <> @ContactTel)
			Begin
				declare @Result_Phone1 int
				declare @ErrMsg_phone1 varchar(256)
				exec up_Customer_V3_Interface_PhoneSet @SPID,@CustID,@ContactTel,'1','1',@Result_Phone1 output,@ErrMsg_phone1 output
		
				if(@Result_Phone1 <> 0)
					Begin
						set @Result = -22500
						set @ErrMsg = '��ϵ�绰�󶨴���'
						rollback
						return
					End
			End
	end
	
	--��Ҫ���ÿ�
	if(@IsNeedTourCard = '0')
		Begin
			declare @Result_TourCard int
			declare @ErrMsg_TourCard varchar(256)
			exec up_Customer_OV3_Interface_UserRegistryV2 @CustID,@CardID,@UProvinceID,@AreaCode,1,@CustLevel,'01','1',@oTourCardID output,@sCardID output,@Result_TourCard output,@ErrMsg_TourCard output
			if(@Result_TourCard <> 0)
				Begin
					set @Result = -22500
					set @ErrMsg = '���ÿ���������'
					rollback
					return
				End
		End

	commit

	set @oCustID = @CustID;
	set @Result = 0;
		
	Set QUOTED_IDENTIFIER Off









