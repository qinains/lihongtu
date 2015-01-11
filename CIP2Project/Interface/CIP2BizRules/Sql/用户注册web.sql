


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_UserRegistryV2Web]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_UserRegistryV2Web]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * �洢����:up_Customer_V3_Interface_UserRegistryV2Web
 *
 * ��������:�û�ע��web�ӿ�
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-18
 * Remark:
 *
 */
 
 Create Procedure dbo.up_Customer_V3_Interface_UserRegistryV2Web
(
	@SPID varchar(8),
	@UserName varchar(30),
	@FullName varchar(50),
	@Password varchar(128),
	@Telephone varchar(20),
	@PhoneState varchar(1),
	@Email varchar(256),
	@EmailState varchar(1),
	@NickName varchar(30),
	@CertificateType varchar(1),
	@Certno varchar(30),
	@Sex varchar(1),
	@Birthday datetime,
	@EduLevel varchar(2),
	@IncomeLevel varchar(1),
	@Province varchar(2),
	@Area varchar(3),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16) out

)
as

	set @Result = -22500
	set @ErrMsg = ''
	set @CustID = ''

	if(len(@Area)=2)
		Set @Area='0'+@Area

	--�������Ż�ȡʡ����
	select @Province = ProvinceID from area where areaID = @Area

	if(@@rowcount =0)
	Begin
		Set @Result = -30008
		set @ErrMsg = '����������'
		return
	End
	--------------------------------------------------------------------------------------------------------

	
	-- ��ȡ @CustID ��ȡֵ

	declare @tempCustID varchar(16)
	declare @SecquenceID int
	begin tran
		select @SecquenceID = SequenceID  from ParSequenceNumber with(nolock) where SequenceType = '0101'
		Set @tempCustID = @SecquenceID+1
		update ParSequenceNumber set SequenceID =  @SecquenceID+1 where SequenceType = '0101'
	commit


	--------------------------------------------------------------------------------------------------------------
	if exists (select 1 from CustInfo where CustID=@tempCustID)
	Begin
		set @Result = -30001
		set @ErrMsg = '�ͻ�id�ʺ��Ѵ���' + @tempCustID 
		return
	End

	--У��֤�� ��Ϊ�գ� ���������ظ���
	if(@Certno <> '' and @CertificateType <> '')
	Begin
		if exists(select 1 from custInfo with(nolock) where CertificateCode=@Certno and CertificateType=@CertificateType and status = '00' and CustType='42' )
		Begin
			set @Result = -30001
			set @ErrMsg = '֤�����Ѵ��ڣ�����ע��'
			return
		End
	End
	
	--�û�����----��ʱ???????
	declare @CustLevel varchar(1)
		Set @CustLevel = '0'
		
		
	begin tran
	
	--����ͻ���Ϣ��
	if(@EmailState = '' and @Email <> '')
	begin
		insert into CustInfo(CustID,ProvinceID,AreaID,CustType,WebPwd,CertificateType,CertificateCode,UserName,RealName,NickName,
							CustLevel,Sex,RegistrationSource,Status,Email,EmailClass,SourceSPID,DealTime,CreateTime)
		values(@tempCustID,@Province,@Area,'42',@Password,@CertificateType,@Certno,@UserName,
			  @FullName,@NickName,@CustLevel,@Sex,2,'00',@Email,1,@SPID,getdate(),getdate())
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '����ͻ�������Ϣ��ʱ����'
			rollback
			return
		End	
	end
	if(@EmailState = '' and @Email = '')
	begin
		insert into CustInfo(CustID,ProvinceID,AreaID,CustType,WebPwd,CertificateType,CertificateCode,UserName,RealName,NickName,
							CustLevel,Sex,RegistrationSource,Status,SourceSPID,DealTime,CreateTime)
		values(@tempCustID,@Province,@Area,'42',@Password,@CertificateType,@Certno,@UserName,
			  @FullName,@NickName,@CustLevel,@Sex,2,'00',@SPID,getdate(),getdate())
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '����ͻ�������Ϣ��ʱ����'
			rollback
			return
		End	
	end
	if(@EmailState = '0')
	begin
		insert into CustInfo(CustID,ProvinceID,AreaID,CustType,WebPwd,CertificateType,CertificateCode,UserName,RealName,NickName,
							CustLevel,Sex,RegistrationSource,Status,Email,EmailClass,SourceSPID,DealTime,CreateTime)
		values(@tempCustID,@Province,@Area,'42',@Password,@CertificateType,@Certno,@UserName,
			  @FullName,@NickName,@CustLevel,@Sex,2,'00',@Email,2,@SPID,getdate(),getdate())
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '����ͻ�������Ϣ��ʱ����'
			rollback
			return
		End	
	end
	--����ͻ���չ��Ϣ��
	insert into CustExtendInfo(CustID,ProvinceID,BirthDay,EduLevel,IncomeLevel,Createtime,DealTime)
	values(@tempCustID,@Province,@Birthday,@EduLevel,@IncomeLevel,getdate(),getdate())
	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '����ͻ���չ��Ϣ��ʱ����'
		rollback
		return
	End	
	
	--����ͻ��ֻ���
	if(@PhoneState = '' and @Telephone <> '')
	begin
		insert into CustPhone(CustID,CustType,ProvinceID,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
		values(@tempCustID,'42',@Province,@Telephone,'2','1',@SPID,getdate())
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '����ͻ��ֻ���ʱ����'
				rollback
				return
			End	
	end
	
	if(@PhoneState = '0')
	begin
		insert into CustPhone(CustID,CustType,ProvinceID,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
		values(@tempCustID,'42',@Province,@Telephone,'2','2',@SPID,getdate())
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '����ͻ��ֻ���ʱ����'
				rollback
				return
			End	
	end

	commit	


	set @CustID = @tempCustID;
	set @Result = 0;
	set @ErrMsg = 'ע��ɹ�'
		
	Set QUOTED_IDENTIFIER Off
	go
	Set ANSI_NULLS Off
	go
