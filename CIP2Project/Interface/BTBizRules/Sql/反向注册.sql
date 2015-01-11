SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO







ALTER      Procedure [dbo].[up_BT_V2_Interface_ReverseUserRegistry]
(
		@ProvinceID varchar(2),
		@SPID	varchar(8),
        @AreaCode varchar(3),
        @RealName varchar(30),
		@CertificateCode  varchar(20),
		@CertificateType  varchar(2),
		@CustContactTel  varchar(20),
		@Result Int out,
		@ErrMsg varchar(256) out,
		@oCustID varchar(16) out,
		@oUserAccount varchar(16) out
)
as

		declare @UProvinceID varchar(2)
    set @UProvinceID=''


		If  exists(select 1 from area where areaid=@AreaCode and provinceid=@ProvinceID)
		begin
			set @UProvinceID=@ProvinceID
		end
		Else if(@CertificateCode is not null and @CertificateCode<>'')
		begin
			declare @tmpAreaCode varchar(4)
			select @tmpAreaCode = AreaCode from RegionCodeArea where left(RegionCode,4) = left(@CertificateCode,4)
			select @UProvinceID = provinceid from area where areaid=@tmpAreaCode
			set @AreaCode = @tmpAreaCode
			
			if(@tmpAreaCode is null or @tmpAreaCode='')
			begin
					set @AreaCode =''
					set @UProvinceID=''
			end
		end

		
		If(@UProvinceID is  null or @UProvinceID='' or @AreaCode is null or @AreaCode='' )
		begin
			set @UProvinceID=@ProvinceID
			select @AreaCode = areaid from  ProvinceCapital where provinceid = @UProvinceid
		end
	
		If not exists(select 1 from area where areaid=@AreaCode and provinceid=@UProvinceID)
		begin
			set @Result = -19998
			set @ErrMsg = '地区信息有误'
			return 
		end
					

		if(@AreaCode ='' or @AreaCode is null)
		Begin
			set @Result = -30010
			set @ErrMsg = '地市码有误'
			return
		End
		-- 如果电话号码不空，根据电话号码排重 李冶 2008-07-31
		if(@CustContactTel is not null and ltrim(rtrim(@CustContactTel))<>'')
         Begin
                if exists (select 1 from custuserinfo u ,custextend e where e.custcontacttel=@CustContactTel and  u.useraccount = e.useraccount and u.custpersontype='1')
				 begin
						set @Result = -30002
						set @ErrMsg = '电话号码已经存在，不能注册'
						return
				 end
				
         End
        ---------------------------------------------------------

		Set @CertificateType = '0'
			
		Set @Result = -1
		Set @ErrMsg = ''
		declare @OriginalAreaCode varchar(3)
		Set @OriginalAreaCode = right(('0' + @AreaCode),3)
	

		if(@UProvinceID='' or @UProvinceID is null)
		Begin
			set @Result = -30011
			set @ErrMsg = '无此区号对应省1'
			return
		End
		
		--生成卡号时，3位数的地市码要在后面补0
		if(len(@AreaCode)=3 and left(@AreaCode,1)='0')
			set @AreaCode=right(@AreaCode,2)+'0'
		
		if(len(@AreaCode)=2)
			set @AreaCode=@AreaCode+'0'
		
		declare @UserType varchar(2)
		set @UserType ='00'


		declare @RegistrationSource varchar(2)
		set @RegistrationSource = '05'
		declare @RegistrationType varchar(1)
		set @RegistrationType = '1'
		declare @CustLevel varchar(1)
		Declare @InnerCardID varchar(16)
		set @InnerCardID = ''
		
		Declare @ExeSql nvarchar(4000)
		Declare @CurrentTime dateTime
		Set @CurrentTime = GetDate()


		Set @RegistrationType = '1'
		--生成卡号
		---------------------------------------------------------------------
		declare @cardSerial	int  --卡的 i~m位


		declare @cardPre	varchar(8)
		declare @i	int
		declare @retainStr char(1)
		set @i = 0
		-- 0~4 间的任一位


	
		

		if( @OriginalAreaCode in ('020'))
			set @retainStr = '7'
		else
			set @retainStr = '6'

		
			
		--校验证件 可为空， 但不能有重复。


		if(@CertificateCode is null or @CertificateCode='')
		begin
			set @CertificateType=''
		end
		
		if(@CertificateCode!='' and @CertificateType !='')
		Begin
			if exists(select 1 from custInfo a, CustUserInfo b where CertificateCode=@CertificateCode and CertificateType=@CertificateType and a.CustID=b.CustID and b.CustPersonType='1' and b.status <> '03' )
			Begin
				set @Result = -30002
				set @ErrMsg = '证件号已存在，不能注册'
				return
			End
		End
	
		--set @oCustID=''
	
		
		
		declare @h int  -- H位取值


		declare @r int	-- 取值结果 0 成功 ；1- 卡号资源已满；其它，失败
		

		while(1=1)
		begin
			--从序列号中取值


			set @h = 3
			set @r =-1
			
			-- 开始获取卡号


			while(1=1)
			Begin
						select @cardSerial=SequenceID+1 from AutoDistribute with(rowlock) where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and htype='1' 
						and hflag=convert(char(1),@h)
						if(@cardSerial>=100000)
						Begin
								set @h=@h+1
								if(@h>7 or @h<3)
								Begin
									set @r=1
									--print @r
									break
								End			
						End
						Else
						Begin			
								update AutoDistribute with(rowlock) 
								set SequenceID=@cardSerial
								where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode
								and htype='1' and hflag=convert(char(1),@h)
								set @retainStr = convert(char(1),@h)
								set @r = 0  --
								break
						End
			End
			--- 结束获取卡号

			If(@r=1)
			Begin
					set @Result = -22500
					set @ErrMsg = '自动分配卡资源已满，分配卡失败'					
					--break
					return 
			End
			
			if(@r=-1)
			Begin
					set @Result = -22501
					set @ErrMsg = '自动分配卡资源异常，分配卡失败'				
					--break
					return 
			End	
			
			-- 是否违反卡保留的规则
			if exists (select 1 from PreservedCardRule where ProvinceID=@UProvinceID and AreaID=@OriginalAreaCode and charindex(PreservedNumber,@cardSerial)>0)
			Begin       
				continue
			End
			--set @password = Convert(varchar(6),convert(decimal,RAND()*1000000))
			
			Declare @cardSerialStr varchar(5)
			set @cardSerialStr = right('00000'+convert(varchar(5),@cardSerial),5)
	
			set @CustLevel = '0'
			set @cardPre = '86'+'0'+@CustLevel+@AreaCode+@retainStr
			if(len(@cardSerialStr)=5 )
			begin
				--取得卡号
				set @oUserAccount=Convert(varchar,@cardPre)+convert(varchar,@cardSerialStr)+'000'
				--对卡号进行位数处理，取9位卡号


	
				declare @SecquenceID int
				begin tran
					select @SecquenceID = SequenceID  from ParSequenceNumber with(updlock) where SequenceType = '0101'
					Set @oCustID = @SecquenceID+1
					update ParSequenceNumber set SequenceID =  @SecquenceID+1 where SequenceType = '0101'
				commit

				set @InnerCardID = @oUserAccount
				set @oUserAccount = left(right(@oUserAccount,12),9)
				
				if exists (select 1 from CustInfo where UserAccount=@oUserAccount or UserAccount = @InnerCardID )
				begin
					continue
				end

				--卡表里是否已经存在


				Set @ExeSql = ' select 1 from BestToneCard_'+@UProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @oUserAccount +'''' -- and InnerCardID='''+ @InnerCardID+ ''''
				Exec(@ExeSql)
				if(@@RowCount>0)
				begin
					continue
				end

				
				--Set @oCustID=@oUserAccount
				break
			end
			else --如果生成的序列号不是8位，重新生成
				continue
		end

	--select @oUserAccount
	if(@InnerCardID='' or len(@InnerCardID)<>16)
	Begin
		set @Result = -30008
		set @ErrMsg = '生成卡号失败'
		return
	End

	
	--开启事物进行处理	--------------------------------------------------------------------------------------------------------------
	begin tran
	--select @oCustID,@InnerCardID,@oUserAccount,@UserType,@EncryptPassword,@CertificateType,@CertificateCode,@RealName,@uProvinceID,@OriginalAreaCode,@CustLevel,@RegistrationSource,@RegistrationType,@CurrentTime,'00', @CurrentTime, ''
			
			insert into CustInfo( CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,RealName,
				ProvinceID,AreaID,CustLevel,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime, EnterPriseID )
				values (@oCustID,@oUserAccount,@UserType,'',@CertificateType,@CertificateCode,@RealName,
				@uProvinceID,@OriginalAreaCode,'3',@RegistrationSource,@RegistrationType,@CurrentTime,'00', @CurrentTime, '')
			
				if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '插入客户基本信息表时错误'
					Rollback
					return
				End
			
			--插入客户信息扩展表			
			insert Into CustExtend(CustID,UserAccount,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,CustContactTel,DealTime)
			Values (@oCustID, @oUserAccount,'2','','','','','',@CustContactTel,@CurrentTime)
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户信息扩展表时错误'
				Rollback
				return
			End
			
			--插入绑定电话表



			if  not Exists(select 1 from boundPhone where boundPhoneNumber=@CustContactTel)
			begin
				insert into boundPhone (CustID,UserAccount,boundPhoneNumber,dealTIme,CustPersonType)
					values ( @oCustID,@oUserAccount,@CustContactTel,@CurrentTime,'1')
				if(@@Error<>0)
				Begin
					Set @ErrMsg = ''
					--set @Result = -22500
					--set @ErrMsg = '插入绑定电话表时错误'
					--Rollback
					--return
				End
			end
			--插入客户用户信息表


			insert into CustUserInfo( CustID,InnerCardID,UserAccount,SPID,RegistrationSource,RegistrationType,
				RegistrationDate,Status,DealTime,CustPersonType )
				values( @oCustID,@InnerCardID,@oUserAccount,@SPID,@RegistrationSource, @RegistrationType,@CurrentTime,'00',@CurrentTime, '1' )
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户用户信息表错误'
				Rollback
				return
			End		
			Insert into CardSendStyle ( CustID, UserAccount, IsPost, DealTime,[Description])
			 values(@oCustID,@oUserAccount,'1',@CurrentTime,null)

			Commit
 
		
	Set @Result=0



Set QUOTED_IDENTIFIER Off








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

