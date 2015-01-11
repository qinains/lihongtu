USE [BestTone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V2_Interface_ModifyUserCommonInfo]    脚本日期: 03/27/2008 11:45:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




/*
 * 存储过程dbo.up_BT_V2_Interface_ModifyUserCommonInfo
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-4-17
 * Remark:
 *
 */
 
ALTER   Procedure [dbo].[up_BT_V2_Interface_ModifyUserCommonInfo]
(
		@UserType varchar(2),
        @UserAccount varchar(16),
        @Password varchar(50),
        @CustID varchar(16),
        @UProvinceID varchar(2),
        @AreaCode varchar(3),
        @Status varchar(2),
        @RealName varchar(30),
		@CertificateCode  varchar(20),
		@CertificateType  varchar(2),
		@Birthday  varchar(19),
		@Sex  varchar(1),
		@CustLevel  varchar(1),
		@EduLevel  varchar(1),
		@Favorite  varchar(256),
		@IncomeLevel  varchar(1),
		@Email  varchar(100),
		@PaymentAccountType  varchar(2),
		@PaymentAccount  varchar(50),
		@EnterpriseID varchar(30),
		@PaymentAccountPassword  varchar(50),
		@CustContactTel  varchar(20),
		@IsPost varchar(2),
		@BoundPhoneRecords Text,
		@AddressRecords Text,
		@Result Int out,
		@ErrMsg varchar(256) out
)
as

	--如果是9位的政企客户卡则替换为16位卡号

	If(len(@UserAccount)=9 and substring(@UserAccount,4,1)='1')
	Begin
		Set @UserAccount = '8600'+ @UserAccount + '000'
	End

	Declare @PhoneDoc int
	Declare @AddressDoc int


	Set @Result = -1
	Set @ErrMsg = ''

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

	declare @CustPersonType varchar(1)
	Set @CustPersonType = ''
	select @CustPersonType = CustPersonType from CustUserInfo where UserAccount = @UserAccount or InnerCardID = @UserAccount
	
	if(@CertificateCode!='' and @CertificateType !='')
	Begin
		if exists(select 1 from custInfo a, CustUserInfo b where CertificateCode=@CertificateCode and CertificateType=@CertificateType and a.CustID=b.CustID and b.CustPersonType='1' )
		Begin
			set @Result = -30001
			set @ErrMsg = '证件号已存在，不能注册'
			return
		End
	End
	
	--开启事物进行处理	--------------------------------------------------------------------------------------------------------------
	begin tran
	
			
	--更新客户信息表	
	update CustInfo 
	set RealName=@RealName,ProvinceID=@uProvinceID,AreaID=@AreaCode,
		Status=@Status,DealTime=@CurrentTime, EnterpriseID = @EnterpriseID, CertificateCode=@CertificateCode,
		CertificateType=@CertificateType
	Where CustID = @CustID and UserAccount = @UserAccount

	if(@@Error<>0)
	Begin
		set @Result = -32500
		set @ErrMsg = '修改客户基本信息表时错误'
		Rollback
		return
	End
	
	--插入客户信息扩展表	
	update CustExtend
	Set CustID=@CustID,UserAccount=@UserAccount,Sex=@Sex,BirthDay=@BirthDay,EduLevel=@EduLevel,Favorites=@Favorite,InComeLevel=@InComeLevel,Email=@Email,CustContactTel=@CustContactTel,DealTime=@CurrentTime
	Where CustID = @CustID and UserAccount = @UserAccount
	
	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '插入客户信息扩展表时错误'
		Rollback
		return
	End
	
	update PaymentAccount 
	Set AccountType=@paymentAccountType,AccountNumber=@PaymentAccount,AccountPassword=@PaymentAccountPassword
	Where CustID = @CustID and UserAccount = @UserAccount
	
	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '插入支付帐号表时错误'
		Rollback
		return
	End

	--更改绑定电话表	--插入绑定电话表
	--先删除再插入
	
	
	delete from BoundPhone where CustID = @CustID and UserAccount = @UserAccount
	Insert Into BoundPhone(CustID,UserAccount,BoundPhoneNumber,DealTime, CustPersonType)
		 Select @CustID,@UserAccount,Phone,@CurrentTime, '1' from @BoundPhone where Phone not in (select BoundPhoneNumber from BoundPhone  where CustPersonType='1')
	
	if(@@Error<>0)
	Begin
		Set @ErrMsg = ''
		--set @Result = -22500
		--set @ErrMsg = '插入绑定电话表时错误'
		--Rollback
		--return
	End
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
	update CardSendStyle set IsPost = @IsPost where  CustID = @CustID and UserAccount = @UserAccount
	if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '更改发行方式时错误'
			Rollback
			return
		End

	--更改联系信息表	--先删除再插入
	--如果传来的联系信息为空，则不更改
	if(@AddressRecords is not null)
	Begin
		Delete From ContactInfo where  CustID = @CustID and UserAccount = @UserAccount
		Insert Into ContactInfo(CustID,UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,DealTime)
			 Select @CustID,@UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,@CurrentTime
			 from @Address
		
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
	


