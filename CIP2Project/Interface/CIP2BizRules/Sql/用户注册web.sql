


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_UserRegistryV2Web]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_UserRegistryV2Web]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程:up_Customer_V3_Interface_UserRegistryV2Web
 *
 * 功能描述:用户注册web接口
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

	--根据区号获取省代码
	select @Province = ProvinceID from area where areaID = @Area

	if(@@rowcount =0)
	Begin
		Set @Result = -30008
		set @ErrMsg = '地区码有误'
		return
	End
	--------------------------------------------------------------------------------------------------------

	
	-- 获取 @CustID 的取值

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
		set @ErrMsg = '客户id帐号已存在' + @tempCustID 
		return
	End

	--校验证件 可为空， 但不能有重复。
	if(@Certno <> '' and @CertificateType <> '')
	Begin
		if exists(select 1 from custInfo with(nolock) where CertificateCode=@Certno and CertificateType=@CertificateType and status = '00' and CustType='42' )
		Begin
			set @Result = -30001
			set @ErrMsg = '证件号已存在，不能注册'
			return
		End
	End
	
	--用户级别----临时???????
	declare @CustLevel varchar(1)
		Set @CustLevel = '0'
		
		
	begin tran
	
	--插入客户信息表
	if(@EmailState = '' and @Email <> '')
	begin
		insert into CustInfo(CustID,ProvinceID,AreaID,CustType,WebPwd,CertificateType,CertificateCode,UserName,RealName,NickName,
							CustLevel,Sex,RegistrationSource,Status,Email,EmailClass,SourceSPID,DealTime,CreateTime)
		values(@tempCustID,@Province,@Area,'42',@Password,@CertificateType,@Certno,@UserName,
			  @FullName,@NickName,@CustLevel,@Sex,2,'00',@Email,1,@SPID,getdate(),getdate())
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '插入客户基本信息表时错误'
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
			set @ErrMsg = '插入客户基本信息表时错误'
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
			set @ErrMsg = '插入客户基本信息表时错误'
			rollback
			return
		End	
	end
	--插入客户扩展信息表
	insert into CustExtendInfo(CustID,ProvinceID,BirthDay,EduLevel,IncomeLevel,Createtime,DealTime)
	values(@tempCustID,@Province,@Birthday,@EduLevel,@IncomeLevel,getdate(),getdate())
	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '插入客户扩展信息表时错误'
		rollback
		return
	End	
	
	--插入客户手机表
	if(@PhoneState = '' and @Telephone <> '')
	begin
		insert into CustPhone(CustID,CustType,ProvinceID,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
		values(@tempCustID,'42',@Province,@Telephone,'2','1',@SPID,getdate())
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户手机表时错误'
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
				set @ErrMsg = '插入客户手机表时错误'
				rollback
				return
			End	
	end

	commit	


	set @CustID = @tempCustID;
	set @Result = 0;
	set @ErrMsg = '注册成功'
		
	Set QUOTED_IDENTIFIER Off
	go
	Set ANSI_NULLS Off
	go
