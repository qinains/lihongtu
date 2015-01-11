set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go





ALTER   Procedure [dbo].[up_BT_V2_Interface_PersonalUserNameUserRegistry]
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
		@EnterPriseID varchar(30),
		@PaymentAccountPassword  varchar(50),
		@CustContactTel  varchar(20),
		@RegistrationSource varchar(2),
		@IsPost varchar(2),
		@BoundPhoneRecords Text,
		@AddressRecords Text,
		@Result Int out,
		@ErrMsg varchar(256) out,
		@oCustID varchar(16) out,
		@oUserAccount varchar(16) out
)
as

	Set @Result = -1
	Set @ErrMsg = ''

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
	--insert into tmpTrace values('11provinceID-'+@UProvinceID+'areaCode'+@AreaCode)

	if(@@rowcount =0)
	Begin
		Set @Result = -30008
		set @ErrMsg = '地区码有误'
		return
	End

	Declare @PhoneDoc int
	Declare @AddressDoc int
	Declare	@RegistrationType varchar(1) --1:自动分配，0：卡注册
	Declare @OriginalAreaCode varchar(3)
	Declare @InnerCardID varchar(16)
	Declare @IsHaveCard varchar(1)
	Set @RegistrationType = '0'
	if(@RegistrationSource='05')
		Set @RegistrationSource = '08'
		
	if(@RegistrationSource='06')
		Set @RegistrationSource = '09'

	Declare @Phone varchar(20)
	Declare @UserName varchar(48)
	Set @Phone =@PaymentAccount
	Set @UserName =  @EnterPriseID

	Set @OriginalAreaCode = right(('0' + @AreaCode),3)
	
	--生成卡号时，3位数的地市码要在后面补0
	if(left(@AreaCode,1)='0')
		set @AreaCode=right(@AreaCode,2)+'0'
	
	-- 创建临时表




	-- 电话帮定关系表




	Declare @BoundPhone Table
	(
		Phone varchar(20)
	)

	-- 创建临时表




	-- 联系信息表




	Declare @Address Table
	(
		 LinkMan varchar(20),
    	ContactTel varchar(20),
     		Address varchar(200),
     		Zipcode varchar(6),
		Email	varchar(100),
		Type	varchar(2)
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
		

	End

	
	Declare @CurrentTime dateTime
	Set @CurrentTime = GetDate()
	


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
	
		declare @cardSerial	int  --卡的 i~m位




		declare @cardPre	varchar(8)
		declare @i	int
		declare @retainStr char(1)
		Declare @ExeSql Nvarchar(2046)
	-------------------------------------------------------------------------------------------------------------
	-- 生成卡号
		declare @h int  -- H位取值




		declare @r int	-- 取值结果 0 成功 ；1- 卡号资源已满；其它，失败
		declare @hflag char(1)
		set @hflag = ''
		
		while(1=1)
		begin
			set @r =-1
			
			-- 开始获取卡号




			while(1=1)
			Begin
						select @hflag = hflag,@cardSerial= SequenceID+1 from autodistribute with(rowlock) where  ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and htype='1' and status='0'
						set @h = convert(int,@hflag)
						if(@h <3 or @h>7)
						Begin
								set @r=1
								break								
						End
						if(@cardSerial >= 100000)
						Begin								
								set @h = @h+1
								if(@h >=3 and @h<=7)
								Begin
									begin tran
											update autodistribute set status='1' where  ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and htype='1' and hflag=@hflag
											If( @@error <> 0)
											begin
												rollback												
											end
											update autodistribute Set status='0' where  ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and htype='1' and hflag=convert(char(1),@h)
											If( @@error <> 0)
											begin
												rollback												
											end
									commit		
									continue							
								End
								Else								
								Begin
									set @r=1
									break				
								End						
						End -- End For if(@cardSerial >= 100000)
						Else
						Begin								
							update AutoDistribute with(rowlock) 
								set SequenceID=@cardSerial
								where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode
								and htype='1' and hflag=@hflag
								set @retainStr = @hflag								
								set @r = 0  -- 成功						
							 break
						End
			End
			--- 结束获取卡号

			If(@r=1)
			Begin
					set @Result = -22500
					set @ErrMsg = '自动分配卡资源已满，分配卡失败'					
					return 
			End
			
			if(@r=-1)
			Begin
					set @Result = -22501
					set @ErrMsg = '自动分配卡资源异常，分配卡失败'				
					return 
			End	

			if exists (select 1 from PreservedCardRule where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and charindex(PreservedNumber,@cardSerial)>0)
			Begin       
				continue
			End
			--set @password = Convert(varchar(6),convert(decimal,RAND()*1000000))
			
			set @CustLevel = '0'		
			set @cardPre = '86'+'0'+@CustLevel+@AreaCode+@retainStr
			Declare @cardSerialStr varchar(5)
			set @cardSerialStr = right('00000'+convert(varchar(5),@cardSerial),5)

			if(len(@cardSerialStr)=5 )
			begin
				--取得卡号
				set @UserAccount=Convert(varchar,@cardPre)+convert(varchar,@cardSerialStr)+'000'

				--对卡号进行位数处理，取9位卡号




	
				--set @CustID = @oUserAccount
				set @InnerCardID = @UserAccount
				set @UserAccount = left(right(@UserAccount,12),9)
				
				if exists (select 1 from CustUserInfo where UserAccount=@UserAccount and InnerCardID=@InnerCardID)
				begin
					continue
				end


				--卡表里是否已经存在




				Set @ExeSql = ' select 1 from BestToneCard_'+@UProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @UserAccount +''' and InnerCardID='''+ @InnerCardID+ ''''
				Exec(@ExeSql)
				if(@@RowCount>0)
				begin
					continue
				end

				

				break
			end
			else --如果生成的序列号不是8位，重新生成
				continue

		

	End
	
	

	
	
	
	--------------------------------------------------------------------------------------------------------------
	--校验CustID,UserAccount
	if exists (select 1 from CustInfo where UserAccount=@UserAccount)
	Begin
		set @Result = -30001
		set @ErrMsg = '客户id或用户帐号已存在' + @CustID +'...' + @UserAccount
		return
	End

	--校验证件 可为空， 但不能有重复。 ？？？？是否需要？
	
	if(@CertificateCode!='' and @CertificateType !='')
	Begin
		if exists(select 1 from custInfo a, CustUserInfo b where CertificateCode=@CertificateCode and CertificateType=@CertificateType and a.CustID=b.CustID and b.CustPersonType='1' and b.status = '00' )
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



	
			
			insert into CustInfo( CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,RealName,
				ProvinceID,AreaID,CustLevel,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime, EnterPriseID )
				values (@CustID,@UserAccount,@UserType,@EncryptPassword,@CertificateType,@CertificateCode,@RealName,
				@uProvinceID,@OriginalAreaCode,@CustLevel,@RegistrationSource,@RegistrationType,@CurrentTime,@Status, @CurrentTime, '')
			
				if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '插入客户基本信息表时错误'
					Rollback
					return
				End
				
						

	
			--插入客户信息扩展表




			insert Into CustExtend(CustID,UserAccount,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,CustContactTel,DealTime)
			Values (@CustID, @UserAccount,@Sex,@BirthDay,@EduLevel,@Favorite,@InComeLevel,@Email,@CustContactTel,@CurrentTime)
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户信息扩展表时错误'
				Rollback
				return
			End

			--插入支付帐号表




			insert into PaymentAccount(CustID,UserAccount,AccountType,AccountNumber,AccountPassword,Status,DealTime)
			Values( @CustID,@UserAccount,@PaymentAccountType,@PaymentAccount,@PaymentAccountPassword,'00',@CurrentTime)
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入支付帐号表时错误'
				Rollback
				return
			End
			
			
			--插入绑定电话表



		
			Insert Into BoundPhone(CustID,UserAccount,BoundPhoneNumber,DealTime, CustPersonType)
				 Select @CustID,@UserAccount,Phone,@CurrentTime,'1' from @BoundPhone where Phone not in (select BoundPhoneNumber from BoundPhone)
			
			if(@@Error<>0)
			Begin
				Set @ErrMsg = ''
				--set @Result = -22500
				--set @ErrMsg = '插入绑定电话表时错误'
				--Rollback
				--return
			End
			

			--插入联系信息表




			Insert Into ContactInfo(CustID,UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,Type,DealTime)
				 Select @CustID,@UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,Type,@CurrentTime
				 from @Address
			
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入联系信息表时错误'
				Rollback
				return
			End
			
			
/*
			--插入 插入发行方式表




			insert into CardSendStyle (CustID,UserAccount,IsPost,DealTime,Description)
				 values(@CustID,@UserAccount,@IsPost,@CurrentTime,null)
			if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '插入发行方式表失败'
					Rollback
					return
				End
*/	
			--插入客户用户信息表




			insert into CustUserInfo( CustID,InnerCardID,UserAccount,SPID,RegistrationSource,RegistrationType,
				RegistrationDate,Status,DealTime,IsHaveCard, CustPersonType )
				values( @CustID,@InnerCardID,@UserAccount,@SPID,@RegistrationSource, @RegistrationType,@CurrentTime,'00',@CurrentTime,'7','1' )
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户用户信息表错误'
				Rollback
				return
			End				
			
			--插入客户绑定方式和表 UserAuthenStyle
			Declare @AuthenType varchar(1)
			Declare @AuthenName varchar(48)
			if(@RegistrationSource = '08') --手机注册
				Begin
					Set @AuthenType = '2'
					Set @AuthenName=@Phone
				End
			Else if(@RegistrationSource = '09') --用户名注册




				Begin
					Set @AuthenType = '1'
					Set @AuthenName= @UserName
				End
			--排重
			if exists (select 1 from UserAuthenStyle where AuthenName=@AuthenName and AuthenType=@AuthenType and status = '0')
			Begin
				set @Result = -22500
				set @ErrMsg = '手机或用户名已被注册或绑定，不能重复绑定'
				Rollback
				return
			End		
			
			--插入绑定方式表	
			Insert into UserAuthenStyle(CustID,AuthenName,AuthenType,SourceSPID,DealTime,Status,Description )
				values(@CustID,@AuthenName,@AuthenType,@SPID,@CurrentTime,'0',null)
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入绑定方式表错误'
				Rollback
				return
			End		


	Commit

	set @oCustID = @CustID
	
	set @oUserAccount =  @UserAccount
		
	Set @Result=0


	
SET ANSI_NULLS Off


