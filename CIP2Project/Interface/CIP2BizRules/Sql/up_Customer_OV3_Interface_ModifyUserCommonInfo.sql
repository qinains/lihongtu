set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go












-- =============================================
-- Author:		<lihongtu>
-- Create date: <2009-08-05>
-- Description:	<客户信息修改-公众>
-- =============================================

ALTER PROCEDURE [dbo].[up_Customer_OV3_Interface_ModifyUserCommonInfo] 
		@SPID varchar(8),
		@UserType varchar(2),
        @UserAccount varchar(16),
        @Password varchar(50),
        @CustID varchar(16),
        @UProvinceID varchar(2),
        @AreaCode varchar(3),
        @Status varchar(2),
        @RealName varchar(30),
		@CertificateCode  varchar(20),
		@CertificateType  varchar(1),
		@Birthday  varchar(19),
		@Sex  varchar(1),
		@CustLevel  varchar(1),
		@EduLevel  varchar(1),
		@Favorite  varchar(256),
		@IncomeLevel  varchar(1),
		@Email  varchar(100),
		@PaymentAccountType  varchar(2),
		@PaymentAccount  varchar(50),
		--@EnterpriseID varchar(30),
		@PaymentAccountPassword  varchar(50),
		@CustContactTel  varchar(20),
		@IsPost varchar(2),
		@BoundPhoneRecords Text,
		@AddressRecords Text,
		@Result Int out,
		@ErrMsg varchar(256) out
	
AS
BEGIN
	SET NOCOUNT ON;

	--如果是9位的政企客户卡则替换为16位卡号
	If(len(@UserAccount)=9 and substring(@UserAccount,4,1)='1')
	Begin
		Set @UserAccount = '8600'+ @UserAccount + '000'
	End

	Declare @PhoneDoc int
	Declare @AddressDoc int


	Set @Result = -1
	Set @ErrMsg = ''

	select @UserType = custType,@UProvinceID = ProvinceID from CustInfo where CustID = @CustID
	if(@@rowcount<1)
	begin
			Set @Result = -1
			Set @ErrMsg = '该客户不存在'
			return
	end
	--if not exists (select 1 from CustInfo where CustID = @CustID)
	--	begin
	--		Set @Result = -1
	--		Set @ErrMsg = '该客户不存在'
	--		return
	--	end

	-- 创建临时表	-- 电话帮定关系表	
	Declare @BoundPhone Table
	(
		Phone varchar(20)
	)

	-- 创建临时表	-- 地址信息表	
	Declare @Address Table
	(
		LinkMan varchar(20),
		ContactTel varchar(20),
		Address varchar(200),
		Zipcode varchar(6),
		Email	varchar(100)
	)

	if( @BoundPhoneRecords is not null )
	Begin
		-- 解析请求数据
		-- 解析绑定电话列表关系
		EXECUTE sp_xml_preparedocument @PhoneDoc OUTPUT, @BoundPhoneRecords


		Insert Into @BoundPhone( Phone )
		SELECT Phone	
			FROM OpenXML( @PhoneDoc, '/ROOT/BoundPhoneRecord' , 2 ) 
			With( Phone	Varchar(20) )

		select * from @BoundPhone
	End

	if(@AddressRecords is not null )
	Begin

		-- 解析请求数据
		-- 解析地址列表关系
		EXECUTE sp_xml_preparedocument @AddressDoc OUTPUT, @AddressRecords

		Insert Into @Address( LinkMan, ContactTel, Address, Zipcode, Email  )
			SELECT 	isnull(LinkMan,''), isnull(ContectTel,''), isnull(Address,''), isnull(Zipcode,''),isnull(Email,'')
				FROM OpenXML( @AddressDoc, '/ROOT/AddressRecord', 2) 
				With( LinkMan varchar(20),ContectTel varchar(20),Address varchar(200),Zipcode varchar(6),Email	varchar(100) )
		
		select * from @Address
	
	End

	
	Declare @CurrentTime dateTime
	Set @CurrentTime = GetDate()
	


	--Declare @tmpAreaCode varchar(3)
	--if(len(@AreaCode)=2)
	--	set @tmpAreaCode='0'+@AreaCode,2
		
	
	--数据校验
	--------------------------------------------------------------------------------------------------------
	--判断绑定电话是否超过５个
	select 1 from @boundPhone 
	if(@@RowCount>5)
	Begin
		set @Result = -22500
		set @ErrMsg = '绑定电话不能超过５门'
		Rollback
		return
	End
	

	--校验证件 可为空， 但不能有重复。
	
	if(@CertificateCode!='' and @CertificateType !='')
	Begin
		if exists(select 1 from custInfo where CertificateCode=@CertificateCode and CertificateType=@CertificateType and CustID != @CustID) 
		Begin
			set @Result = -30001
			set @ErrMsg = '证件号已存在，不能修改'
			return
		End
	End
	
	--开启事务进行处理	--------------------------------------------------------------------------------------------------------------
	begin tran
	
			
	--更新客户信息表	
	update CustInfo 
	set RealName=@RealName,ProvinceID=@uProvinceID,AreaID=@AreaCode,Email = @Email,
		Status=@Status,DealTime=@CurrentTime,--, 
		--EnterpriseID = @EnterpriseID, 
		CertificateCode=@CertificateCode,--不允许修改CustID,UserAccount,CertificateCode;
		CertificateType=@CertificateType
	Where CustID = @CustID 
	--and UserAccount = @UserAccount

	if(@@Error<>0)
	Begin
		set @Result = -32500
		set @ErrMsg = '修改客户基本信息表时错误'
		Rollback
		return
	End
	
	--插入客户信息扩展表	--CustID=@CustID,UserAccount=@UserAccount,Sex=@Sex,,Email=@Email,CustContactTel=@CustContactTel,
	
	If not exists(select 1 from Favorites where FavoritesID=@Favorite)
		Begin
		set @Result = -32500
		set @ErrMsg = '爱好在主表不存在'
		Rollback
		return
	End

	If exists(select 1 from CustExtendInfo where CustID = @CustID) 
	Begin
		update CustExtendInfo 
		Set ProvinceID=@uProvinceID,BirthDay=@BirthDay,EduLevel=@EduLevel,Favorite=@Favorite,InComeLevel=@InComeLevel,DealTime=@CurrentTime
		Where CustID = @CustID 
	End
	else
	Begin
		insert into CustExtendInfo (CustID,ProvinceID,BirthDay,EduLevel,IncomeLevel,Favorite,Createtime,DealTime)
		values (@CustID,@uProvinceID,@BirthDay,@EduLevel,@IncomeLevel,@Favorite,GetDate(),GetDate())
	End


	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '插入客户信息扩展表时错误'
		Rollback
		return
	End
	
--	update PaymentAccount 
--	Set --AccountType=@paymentAccountType,
--		--AccountNumber=@PaymentAccount,AccountPassword=@PaymentAccountPassword
--		PaymentAccount = @PaymentAccount
--	Where CustID = @CustID 
	--and UserAccount = @UserAccount
--	
--	if(@@Error<>0)
--	Begin
--		set @Result = -22500
--		set @ErrMsg = '插入支付帐号表时错误'
--		Rollback
--		return
--	End

	--更改绑定电话表	--插入绑定电话表	--先删除再插入
	
	if(@SPID='35000004' or @SPID='35700091' or @SPID='35000001' or @SPID='35123456')	
	Begin
		delete from custphone where CustID = @CustID 
		insert into custphone (custID,custType,ProvinceID,Phone,PhoneType,PhoneClass,SourceSPID,DealTime) 
		values (@CustID,@UserType,@UProvinceID,@CustContactTel,'2','2',@SPID,GetDate())
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '插入绑定电话表时错误'
			Rollback
			return
		End


	End

	--delete from BoundPhone where CustID = @CustID and UserAccount = @UserAccount    --remark by lht 
	--Insert Into BoundPhone(CustID,UserAccount,BoundPhoneNumber,DealTime, CustPersonType)  --remark by lht 
	--	 Select @CustID,@UserAccount,Phone,@CurrentTime, '1' from @BoundPhone where Phone not in (select BoundPhoneNumber from BoundPhone  where CustPersonType='1')  --remark by lht 
	
	--if(@@Error<>0)   remark by lht 
	--Begin  remark by lht 
	--	Set @ErrMsg = '' remark by lht 
		--set @Result = -22500
		--set @ErrMsg = '插入绑定电话表时错误'
		--Rollback
		--return
	--End  remark by lht 
	--先删除再插入
/*
	--电话号码是否已被绑定
	if exists (select 1 from BoundPhone where BoundPhoneNumber in (select Phone from @BoundPhone where UserAccount<>@UserAccount))
	Begin
		set @Result = -30003
		set @ErrMsg = '电话已被绑定'
		Rollback
		return
	End

	delete from BoundPhone where  CustID = @CustID and UserAccount = @UserAccount
		
	Insert Into BoundPhone(CustID,UserAccount,BoundPhoneNumber,DealTime)
			 Select @CustID,@UserAccount,Phone,@CurrentTime from @BoundPhone
	
	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '插入绑定电话表时错误'
		Rollback
		return
	End
*/
	
	-- 更改发行方式
--	update CardSendStyle set IsPost = @IsPost where  CustID = @CustID and UserAccount = @UserAccount
--	if(@@Error<>0)
--		Begin
--			set @Result = -22500
--			set @ErrMsg = '更改发行方式时错误'
--			Rollback
--			return
--		End

	--更改联系信息表	--先删除再插入
	--如果传来的联系信息为空，则不更改
	if(@AddressRecords is not null)
	Begin
		--Delete From ContactInfo where  CustID = @CustID and UserAccount = @UserAccount
		--Insert Into ContactInfo(CustID,UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,DealTime)
		--	 Select @CustID,@UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,@CurrentTime
		--	 from @Address
		delete from AddressInfo where CustID = @CustID
		insert into AddressInfo (CustID,AddressProvince,Address,ZipCode,Type,DealTime)
		select @CustID,@UProvinceID,Address,Zipcode,'99',getdate() from @Address
		--values (@CustID,@UProvinceID,@AddressRecords,'','00',getdate())
		--SequenceID,
		--CustID,
		--AddressProvince
		--AddressArea
		--ZipCode
		--Type
		--DealTime		
		

		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '插入联系信息表时错误'
			Rollback
			return
		End
	End
	
	
	Commit
	set @Result=0
	set @ErrMsg='客户信息修改成功'
	
END














