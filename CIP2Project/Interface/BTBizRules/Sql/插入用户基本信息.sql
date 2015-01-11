set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go














ALTER       Procedure [dbo].[up_BT_V2_Interface_InsertUserCommonInfo]
(
		@SPID	varchar(8),
		@UserType varchar(2),
        @UserAccount varchar(16),
        @Password varchar(6),
        @EncryptPassword varchar(50),
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

	if(@AreaCode ='')
	Begin
			Set @Result = -30001
			set @ErrMsg = '卡号中地市码有误'
			return
	End

	if(len(@AreaCode)=6 or @AreaCode=0)
	Begin
		select @AreaCode = AreaCode from RegionCodeArea where RegionCode = @AreaCode
		if(@@Rowcount<1)
		Begin
			Set @Result = -30001
			set @ErrMsg = '地区国标码有误'
			return
		End
	End

	--根据区号获取省代码
	select @UProvinceID = ProvinceID from area where areaID = @AreaCode
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
	
	--如果是9位则拼成16位卡号
	if(len(@UserAccount)=9)
		set @InnerCardID = '8600'+@UserAccount+'000'
	else if(len(@UserAccount)!=16 and @UserAccount!='')
		Begin
			set @Result = -30008
			set @ErrMsg = '卡号位数有误'
			return
		End


	Set @OriginalAreaCode = right(('0' + @AreaCode),3)
	
	--生成卡号时，3位数的地市码要在后面补0
	if(len(@AreaCode)=3 and left(@AreaCode,1)='0')
		set @AreaCode=right(@AreaCode,2)+'0'
	
	if(len(@AreaCode)=2)
		set @AreaCode=@AreaCode+'0'
	
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

		Insert Into @Address( LinkMan, ContactTel, Address, Zipcode, Email, Type )
			SELECT 	isnull(LinkMan,''), isnull(ContectTel,''), isnull(Address,''), isnull(Zipcode,''),isnull(Email,''),isnull(Type,'')
				FROM OpenXML( @AddressDoc, '/ROOT/AddressRecord', 2) 
				With( LinkMan varchar(20),ContectTel varchar(20),Address varchar(200),Zipcode varchar(6),Email	varchar(100), Type varchar(2) )
		

	End

	
	Declare @CurrentTime dateTime
	Set @CurrentTime = GetDate()
	


	--数据校验
	--------------------------------------------------------------------------------------------------------
	
	--校验联系电话
	if(@CustContactTel='')
	Begin
			Set @Result = -30001
			set @ErrMsg = '联系电话不能为空'
			return
	End

	if exists(select 1 from custExtend a with(nolock) , CustUserInfo b with(nolock)  where a.CustContactTel=@CustContactTel and a.CustID=b.CustID and b.CustPersonType='1' and b.status = '00' )
	Begin
			Set @Result = -30001
			set @ErrMsg = '联系电话已存在，不能注册'
			return
	End 	

	--校验卡号
	if(@UserAccount != '')
	Begin
		Declare @ExeSql nvarchar(4000)
		Declare @BatchNumber int
		Declare @tmpStatus varChar(2)
		Declare @CardExpireTime DateTime
		Set @BatchNumber = 0
		Set @tmpStatus = 'kk'
		
		/* 不进行区号校验
		if(@tmpAreaCode != @OriginalAreaCode)
		Begin
			Set @Result = -31008
			set @ErrMsg = '帐号中的地市码与传入地市码不符'
			return
		End
		*/
		
		--校验卡号是否已经注册成用户。
		select 1 from CustUserInfo with(nolock)  where UserAccount = @UserAccount
		if(@@RowCount =1)
		Begin
			Set @Result = -30001
			set @ErrMsg = '此卡已激活，已成为用户'
			return
		End

		
		--根据地市号找出省代码
		Declare @CardUProvinceID varchar(2)
		declare @CardAreaCode varchar(3)
		Set @CardAreaCode = left(@UserAccount,3)
		--北京，上海等地的地市码为 250，须转换为025
		if exists( select 1 from area where areaID = '0'+left(@CardAreaCode,2))
			set @CardAreaCode = '0' + left(@CardAreaCode,2)
				
		select @CardUProvinceID=ProvinceID from area where AreaID = @CardAreaCode
		if(@@RowCount !=1)
		Begin
			Set @Result = -30001
			set @ErrMsg = '卡号中地市码有误'
			return
		End

		--如果是统一接口平台，则将用户的地市信息替换为卡号中的地市信息
		if(@SPID = 35000002)
		Begin
			Set @UProvinceID = @CardUProvinceID
			Set @AreaCode = @CardAreaCode
		End

		--判断卡是否有效
		Set @ExeSql = ' select @BatchNumber=BatchNumber, @tmpStatus=Status, @CardExpireTime=ExpireDate from BestToneCard_'+@CardUProvinceID+'_'+@CardAreaCode + ' where CardID=''' + @UserAccount + ''''

		exec sp_executesql @ExeSql,N' @BatchNumber int output, @tmpStatus varchar(2) output, @CardExpireTime dateTime output', @BatchNumber output,@tmpStatus output, @CardExpireTime output

		if(@tmpStatus = '01')
		Begin
			Set @Result = -30001
			set @ErrMsg = '此卡已使用'
			return
		End
		
		if( @CardExpireTime < @CurrentTime )
		Begin
			Set @Result = -21528
			set @ErrMsg = '此卡已过期'
			return
		End

		if(@tmpStatus = '02')
		Begin
			Set @Result = -21528
			set @ErrMsg = '此卡已冻结'
			return
		End
		
		if(@tmpStatus = '03')
		Begin
			Set @Result = -21528
			set @ErrMsg = '此卡已废弃'
			return
		End
		
		if(@BatchNumber =0)
		Begin
			Set @Result = -30008
			set @ErrMsg = '无此卡'
			return
		End
		
		--卡级别从批次表中取
		--set @CustLevel = ''
		declare @tmpBatchStatus varchar(2)
		set @tmpBatchStatus = 'kk'
		Select @tmpBatchStatus =BatchStatus ,@CustLevel=CardLevel from BestToneCardBatch where BatchNumber=@BatchNumber
		
		if(@CustLevel='')
			set @CustLevel = '3'

		if(@tmpBatchStatus = 'kk')
		Begin
			Set @Result = -30008
			set @ErrMsg = '无此卡批次'
			return
		End
		
		if(@tmpBatchStatus = '00')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡未激活，不能注册'
			return
		End
		
		if(@tmpBatchStatus = '02')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡已冻结，不能注册'
			return
		End
		
		if(@tmpBatchStatus = '03')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡已废弃，不能注册'
			return
		End
		
		if(@tmpBatchStatus = '04')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡已删除，不能注册'
			return
		End

		--验证激活码,换卡不用验证
		if(@CustID = '')
		Begin
			Set @ExeSql = ' select 1 from BestToneCard_'+@CardUProvinceID+'_'+@CardAreaCode + ' where CardID=''' + @UserAccount + ''' and Password = ''' + @Password + ''''

			exec(@ExeSql)
			if(@@Rowcount !=1)
			Begin
				Set @Result = -30008
				set @ErrMsg = '激活码有误'
				return
			End
		End

		Set @IsHaveCard = '6'

		--Set @CustID = @UserAccount
	End
	

	
/*	--电话号码是否已被绑定
	if exists (select 1 from BoundPhone where BoundPhoneNumber in (select Phone from @BoundPhone))
	Begin
		set @Result = -30003
		set @ErrMsg = '电话已被绑定'
		return
	End
*/	
	--如果没卡，则自动为客户生成卡
	---------------------------------------------------------------------------------------------------
	if(@UserAccount ='')
	Begin
		Set @RegistrationType = '1'
		
		declare @cardSerial	int  --卡的 i~m位
		declare @cardPre	varchar(8)
		declare @i	int
		declare @retainStr char(1)
		set @i = 0
		-- 0~4 间的任一位
	
		--根据SPID和status来判断是属于个人通信助理客户还是商旅卡用户以及h位的取值
		/*
		--根据SPID来判断h位的取值,目前只有政企客户
		if(@SPID='35000001')
			Begin
				set @retainStr = '1'
			End
		else
			Begin
				set @retainStr = '3'
			End
		
		if(@UProvinceID = '12' )
			set @retainStr = '7'
		*/
		--set @CustID=''
		

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
						select @hflag = hflag,@cardSerial= SequenceID+1 from autodistribute where  ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and htype='1' and status='0'
						set @h = convert(int,@hflag)
						if(@h <3 )
						Begin
								set @r=1
								break								
						End
						if(@cardSerial >= 100000)
						Begin								
								set @h = @h+1
								if(@h >=3 and @h<=9)
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
							update AutoDistribute
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
				
				if exists (select 1 from CustUserInfo with(nolock) where UserAccount=@UserAccount or InnerCardID=@InnerCardID)
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

				
				--Set @CustID=@UserAccount
				break
			end
			else --如果生成的序列号不是8位，重新生成
				continue

		end

			
			if(@CustID='')
				Set @IsHaveCard = '1'
			else
				Set @IsHaveCard = '2'
	End
	
	if(@InnerCardID='' or len(@InnerCardID)<>16)
	Begin
		set @Result = -30008
		set @ErrMsg = '生成卡号失败'
		return
	End
	
	-- 设定 @CustID 的取值
	Declare @IsNewCust int --0:新客户， 1： 老客户
	Set @IsNewCust = 1

	if(@CustID ='')
	Begin
		--Set @CustID = @InnerCardID
		declare @SecquenceID int
		begin tran
			select @SecquenceID = SequenceID  from ParSequenceNumber where SequenceType = '0101'
			Set @CustID = @SecquenceID+1
			update ParSequenceNumber set SequenceID =  @SecquenceID+1 where SequenceType = '0101'
		commit
		Set @IsNewCust = 0
	End
/*
	if(@CustID='' or len(@CustID)<>16)
	Begin
		set @Result = -30001
		set @ErrMsg = '生成CustID号失败'
		return
	End
	*/
	--------------------------------------------------------------------------------------------------------------
	--校验CustID,UserAccount
	if exists (select 1 from CustInfo with(nolock) where UserAccount=@UserAccount )
	Begin
		set @Result = -30001
		set @ErrMsg = '客户id或用户帐号已存在' + @CustID +'...' + @UserAccount + ',,' + convert(varchar,@i)
		return
	End

	--校验证件 可为空， 但不能有重复。
	
	if(@CertificateCode!='' and @CertificateType !='')
	Begin
		if exists(select 1 from custInfo a with(nolock) , CustUserInfo b with(nolock)  where CertificateCode=@CertificateCode and CertificateType=@CertificateType and a.CustID=b.CustID and b.CustPersonType='1' and b.status = '00' )
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
	
	if (@IsNewCust=0)	--如果是新客户
		Begin
			
			insert into CustInfo( CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,RealName,
				ProvinceID,AreaID,CustLevel,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime, EnterPriseID )
				values (@CustID,@UserAccount,@UserType,@EncryptPassword,@CertificateType,@CertificateCode,@RealName,
				@uProvinceID,@OriginalAreaCode,@CustLevel,@RegistrationSource,@RegistrationType,@CurrentTime,@Status, @CurrentTime, @EnterPriseID)
			
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
				 Select @CustID,@UserAccount,Phone,@CurrentTime,'1' from @BoundPhone where Phone not in (select BoundPhoneNumber from BoundPhone where CustPersonType='1')
			
			if(@@Error<>0)
			Begin
				Set @ErrMsg = ''
				--set @Result = -22500
				--set @ErrMsg = '插入绑定电话表时错误'
				--Rollback
				--return
			End
			 --插入联系电话到绑定电话表
			if not exists(select 1 from BoundPhone where BoundPhoneNumber=@CustContactTel and CustPersonType='1')
			Begin     
				Insert Into BoundPhone(CustID,UserAccount,BoundPhoneNumber,DealTime,CustPersonType)  
					values(@CustID,@UserAccount,@CustContactTel,@CurrentTime,'1')    
			 End
			--插入联系信息表

			Insert Into ContactInfo(CustID,UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,DealTime, Type)
				 Select @CustID,@UserAccount,LinkMan,ContactTel,Address,ZipCode,EMail,@CurrentTime, Type
				 from @Address
			
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入联系信息表时错误'
				Rollback
				return
			End
			
			--更新卡状态
			if(@RegistrationType= '0' )
			Begin
				Set @ExeSql = ' UPdate BestToneCard_'+@UProvinceID+'_'+@OriginalAreaCode + ' Set Status=''01'' where CardID=''' + @UserAccount + ''''
				Exec (@ExeSql)
				if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '更新卡状态时错误'
					Rollback
					return
				End
			End

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
			
			--插入客户用户信息表
			insert into CustUserInfo( CustID,InnerCardID,UserAccount,SPID,RegistrationSource,RegistrationType,
				RegistrationDate,Status,DealTime,IsHaveCard, CustPersonType )
				values( @CustID,@InnerCardID,@UserAccount,@SPID,@RegistrationSource, @RegistrationType,@CurrentTime,'00',@CurrentTime,@IsHaveCard,'1' )
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户用户信息表错误'
				Rollback
				return
			End				
		End
	else --如果是老客户
		Begin
			--更新客户用户信息表原记录
			update CustUserInfo set Status='03' where UserAccount = (select UserAccount from CustInfo where CustID = @CustID)
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新客户用户信息表错误'
				Rollback
				return
			End	
			
			--插入客户用户信息表
			insert into CustUserInfo( CustID,InnerCardID,UserAccount,SPID,RegistrationSource,RegistrationType,
				RegistrationDate,Status,DealTime,IsHaveCard )
				select @CustID,@InnerCardID,@UserAccount,@SPID,@RegistrationSource, @RegistrationType,@CurrentTime,'00',@CurrentTime, @IsHaveCard from CustInfo a, CustExtend b where a.custID *= b.custID and a.CustID = @CustID
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户用户信息表错误'
				Rollback
				return
			End		
			


			--更新客户表
			Update CustInfo Set UserAccount = @UserAccount, status='00' where CustID = @CustID
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新客户基本信息表时错误'
				Rollback
				return
			End
			--更新客户扩展表
			update CustExtend Set UserAccount = @UserAccount where CustID = @CustID
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新客户扩展表时错误'
				Rollback
				return
			End
			--更新绑定电话表
			update BoundPhone	Set UserAccount = @UserAccount where CustID = @CustID
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新绑定电话表时错误'
				Rollback
				return
			End
			--更新联系信息表
			update ContactInfo Set UserAccount = @UserAccount where CustID = @CustID
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新联系信息表时错误'
				Rollback
				return
			End
					
		End

	Commit

	set @oCustID = @CustID
	
	set @oUserAccount =  @UserAccount
		
	Set @Result=0



Set QUOTED_IDENTIFIER Off
















