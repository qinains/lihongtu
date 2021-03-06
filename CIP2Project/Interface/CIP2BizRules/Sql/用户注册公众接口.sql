set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


/*
 * 存储过程: up_Customer_OV3_Interface_UserInfoRegistry
 *
 * 功能描述: 客户注册接口(soap)
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-09-01
 * Remark:
 *
 */




Create Procedure [dbo].[up_Customer_OV3_Interface_UserInfoRegistry]
(
		@SPID	varchar(8),
		@UserType varchar(2),
        @UserAccount varchar(16),
        @Password varchar(6),
        @EncryptPassword varchar(50),
        @CustID varchar(16),
        @UProvinceID varchar(2),
        @AreaCode varchar(6),
        @Status varchar(2),
        @RealName varchar(30),
		@CertificateCode  varchar(30),
		@CertificateType  varchar(1),
		@Birthday  varchar(19),
		@Sex  varchar(1),
		@CustLevel  varchar(1),
		@EduLevel  varchar(1),
		@Favorite  varchar(256),
		@IncomeLevel  varchar(1),
		@Email  varchar(100),
		@RegistrationSource varchar(2),
		@UserName varchar(30),
		@Result Int out,
		@ErrMsg varchar(256) out,
		@oCustID varchar(16) out,
		@oUserAccount varchar(16) out
)
as

	Set @Result = -1
	Set @ErrMsg = ''
	Set @oUserAccount = ''
	--客户类型全部替换为42
	set @UserType = '42'
	
	--判断用户名是否为空，如果不为空验证用户是否存在
	
	--根据国标获取区号
	if(len(@AreaCode)=6 or @AreaCode=0)
	Begin
		select @AreaCode = AreaCode from RegionCodeArea where RegionCode = @AreaCode
		if(@@Rowcount<1)
		Begin
			Set @Result = -30008
			set @ErrMsg = '地区国标码有误'
			return
		End
	End

	if(len(@AreaCode)=2)
		Set @AreaCode='0'+@AreaCode
	--根据区号获取省代码
	select @UProvinceID = ProvinceID from area where areaID = @AreaCode

	if(@@rowcount =0)
	Begin
		Set @Result = -30008
		set @ErrMsg = '地区码有误'
		return
	End
	
	if(@UserType <> '41' and @UserType <> '42')
	begin
		Set @Result = -22500
		set @ErrMsg = '客户类型不合法:' + @UserType
		return
	end

	Declare	@RegistrationType varchar(1) --1:自动分配，0：卡注册
	Declare @OriginalAreaCode varchar(3)
	Set @RegistrationType = '0'
	if(@RegistrationSource='05')
		Set @RegistrationSource = '08'
		
	if(@RegistrationSource='06')
		Set @RegistrationSource = '09'


	Set @OriginalAreaCode = right(('0' + @AreaCode),3)
	
	--数据校验
	--------------------------------------------------------------------------------------------------------

	
	-- 获取 @CustID 的取值



	declare @SecquenceID int
	begin tran
		select @SecquenceID = SequenceID  from ParSequenceNumber with(updlock) where SequenceType = '0101'
		Set @CustID = @SecquenceID+1
		update ParSequenceNumber set SequenceID =  @SecquenceID+1 where SequenceType = '0101'
	commit


	
	--------------------------------------------------------------------------------------------------------------
	--校验CustID
	if exists (select 1 from CustInfo where CustID=@CustID)
	Begin
		set @Result = -30001
		set @ErrMsg = '客户id已存在' + @CustID
		return
	End

	--校验证件 可为空， 但不能有重复。
	
	if(@CertificateCode!='' and @CertificateType !='')
	Begin
		if exists(select 1 from CustInfo with(nolock) where CertificateCode=@CertificateCode and CertificateType=@CertificateType and CustID=@CustID and status = '00' and CustType=@UserType )
		Begin
			set @Result = -30001
			set @ErrMsg = '证件号已存在，不能注册'
			return
		End
	End
	
	--没有无级别卡概念，只有水晶卡概念
	if(@CustLevel='0')
		Set @CustLevel = '3'
			
	--开启事物进行处理

		

	--------------------------------------------------------------------------------------------------------------
	begin tran
	
			
	--插入客户信息表

	
			
			insert into CustInfo( CustID,ProvinceID,AreaID,CustType,VoicePwd,CertificateType,CertificateCode,RealName,
				CustLevel,Sex,RegistrationSource,UserName,Status,Email,EmailClass,SourceSPID,DealTime,CreateTime )
				values (@CustID,@uProvinceID,@OriginalAreaCode,@UserType,@EncryptPassword,@CertificateType,@CertificateCode,@RealName,
				@CustLevel,@Sex,@RegistrationSource,@UserName,@Status,@Email,1,@SPID,getdate(),getdate())
			
				if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '插入客户基本信息表时错误'
					Rollback
					return
				End
				
						

	
			--插入客户信息扩展表


			insert Into CustExtendInfo(CustID,ProvinceID,BirthDay,EduLevel,InComeLevel,Favorite,Createtime,DealTime)
			Values (@CustID, @uProvinceID,@BirthDay,@EduLevel,@InComeLevel,@Favorite,getdate(),getdate())
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户信息扩展表时错误'
				Rollback
				return
			End
			
			
			--插入客户商旅卡信息表
			declare @Result_TourCard int
			declare @ErrMsg_TourCard varchar(256)
			declare @sCardID varchar(16)
			Set @sCardID = ''
			exec up_Customer_OV3_Interface_UserRegistryV2 @CustID,@UserAccount,@UProvinceID,@AreaCode,1,@CustLevel,'01','1',@oUserAccount output,@sCardID output,@Result_TourCard output,@ErrMsg_TourCard output
			if(@Result_TourCard <> 0)
				Begin
					set @Result = -22500
					set @ErrMsg = '商旅卡建卡错误'
					rollback
					return
				End
				


	Commit

	set @oCustID = @CustID
		
	Set @Result=0


	
SET ANSI_NULLS Off




