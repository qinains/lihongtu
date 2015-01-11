if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_InsertCustInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_InsertCustInfo]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO









/*
*SPID 代表不同的商户导入的数据，每次需要重新设置
*1，CustID 为空，UserAccount为空：新客户登记	  CustID和UserAccount 均自动分配。 
*2，CustID 为空，UserAccount不为空：客户拿卡登记  自动分配CustID
*select right('0000000'+cast(5 as varchar),5)
*/

Create Procedure [dbo].[up_BT_V2_InsertCustInfo]
(
 @RealName varchar(30),
 @UserAccount varchar(16),
 @EncryedPassword varchar(50),
 @CustType  varchar(2),
 @CustomType varchar(1), -- 0政企 1公众
 @CustLevel varchar(1),
 @AreaID    varchar(3),
 @Sex       varchar(1),
 @CertificateType  varchar(2),
 @CertificateCode  varchar(20),
 @BirthDay  datetime,
 @MobilePhone      varchar(20),
 @MobileUserDate   datetime,
 @TelCode1         varchar(20),
 @TelCode1Date     datetime,
 @TelCode2         varchar(20),
 @TelCode2Date     datetime,
 @Email            varchar(100),
 @EduLevel         varchar(2),
 @IncomLevel       varchar(2),
 @Fav              varchar(256),
 @HCode            varchar(1),
 @RegType		   varchar(1),
 @RegSrc           varchar(2),
 @RegDate          datetime,
 @SPID			   varchar(8),
 @OUserAccount     varchar(16)   out,
 @OCustID		   varchar(16)   out,
 @Result           int           out,
 @ErrMsg           varchar(1024) out
)
-- liye
-- 
AS
BEGIN

	declare @Valid1			int
	declare @Valid2			int
	declare @InnerCardID     varchar(16)
	
	-- 处理areaid
	declare @BTAreaID         varchar(3)  -- 2位区号后面加0   250
	declare @OriginalAreaCode varchar(3)  -- 2位区号前面加0   025
	declare @CustID			  varchar(16)
    declare @ProvinceID		  varchar(2)
    
    set @Valid1 = 0
    set @Valid2 = 0
    set @InnerCardID = ''
    
	set @BTAreaID = ''
	set @OriginalAreaCode = ''
	set @ProvinceID = ''
	set @Result = 0
	set @ErrMsg = ''

    if (len(@AreaID) = 2)
		begin
			set @BTAreaID = @AreaID + '0'
			set @OriginalAreaCode = '0'+@AreaID
		end
    else if(len(@AreaID) = 3 )
		begin
			set @BTAreaID = @AreaID
			set @OriginalAreaCode = @AreaID
		end 

    select @ProvinceID = ProvinceID from Area  where AreaID = @OriginalAreaCode
    if(@ProvinceID ='' or @ProvinceID = null)
    Begin
		Set @Result = -30001
		set @ErrMsg = '当前AreaID无效，无法检索到ProvinceID'
		return
    End
    --set  @IncomLevel = '51'
	
	BEGIN tran
	-- 插入基本信息

	--------------------------------------------------------------------------------------------------------
	if(@UserAccount != '')
	Begin
		Declare @ExeSql nvarchar(4000)
		Declare @BatchNumber int
		Declare @tmpStatus varChar(2)
		Declare @tmpAreaCode varchar(3)
		Declare @CardExpireTime DateTime
		Declare @CurrentTime	DateTime
		Set @tmpAreaCode = SubString(@UserAccount,5,3)
		Set @BatchNumber = 0
		Set @tmpStatus = 'kk'
		Set @CurrentTime = getdate()
		
		/* 不进行区号校验

		if(@tmpAreaCode != @OriginalAreaCode)
		Begin
			Set @Result = -31008
			set @ErrMsg = '帐号中的地市码与传入地市码不符'
			return
		End
		*/

		Set @ExeSql = ' select @BatchNumber=BatchNumber, @tmpStatus=Status, @CardExpireTime=ExpireDate from BestToneCard_'+@ProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @UserAccount + ''''

		exec sp_executesql @ExeSql,N' @BatchNumber int output, @tmpStatus varchar(2) output, @CardExpireTime dateTime output', @BatchNumber output,@tmpStatus output, @CardExpireTime output
		if( @@rowcount <> 0 )
		Begin
			Set @Result = -31001
			set @ErrMsg = '卡号已经存在，不能导入'
			Rollback
			return
		End
		/*
		if(@tmpStatus = '01')
		Begin
			Set @Result = -30001
			set @ErrMsg = '此卡已使用'
			rollback
			return
		End
		
		if( @CardExpireTime < @CurrentTime )
		Begin
			Set @Result = -21528
			set @ErrMsg = '此卡已过期'
			rollback
			return
		End

		if(@tmpStatus = '02')
		Begin
			Set @Result = -21528
			set @ErrMsg = '此卡已冻结'
			rollback
			return
		End
		
		if(@tmpStatus = '03')
		Begin
			Set @Result = -21528
			set @ErrMsg = '此卡已废弃'
			rollback
			return
		End
		
		if(@BatchNumber =0)
		Begin
			Set @Result = -30008
			set @ErrMsg = '无此卡'
			rollback
			return
		End
		
		declare @tmpBatchStatus varchar(2)
		set @tmpBatchStatus = 'kk'
		Select @tmpBatchStatus =BatchStatus from BestToneCardBatch where BatchNumber=@BatchNumber

		if(@tmpBatchStatus = 'kk')
		Begin
			Set @Result = -30008
			set @ErrMsg = '无此卡批次'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '00')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡未激活，不能注册'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '02')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡已冻结，不能注册'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '03')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡已废弃，不能注册'
			rollback
			return
		End
		
		if(@tmpBatchStatus = '04')
		Begin
			Set @Result = -30008
			set @ErrMsg = '该批次卡已删除，不能注册'
			rollback
			return
		End
        */
		
		select @CustID = SequenceID+1 from ParSequenceNumber with(updlock)where SequenceType = '0101'
		update ParSequenceNumber set SequenceID = @CustID where SequenceType = '0101'
		if ( @@error <> 0)
		begin
			set @Result = -22500
			set @ErrMsg = '自动分配CustID失败'
			rollback
			return
		end

		
		set @OCustID = @CustID
		set @OUserAccount = @UserAccount
		set @InnerCardID = '8600'+@UserAccount
	End
	

	--如果没卡，则自动为客户生成卡
	---------------------------------------------------------------------------------------------------
	if(@UserAccount ='')
	Begin
		--Set @RegistrationType = '1'
		
		declare @cardSerial	int  --卡的 i~m位		
		declare @cardPre	varchar(8)
		declare @i	int
		declare @retainStr char(1)
		set @i = 0
		set @retainStr = '1'     -- H 位置 1 
		-- 0~4 间的任一位
	
		--根据SPID和status来判断是属于个人通信助理客户还是商旅卡用户以及h位的取值
		--根据SPID来判断h位的取值,目前只有政企客户
		/*
		if(@SPID='35000001')
			Begin
				set @retainStr = '1'
			End
		else
			Begin
				set @Result = -20000
				set @ErrMsg = '不是有效的spid'
				return
			End		
		*/

		set @CustID=''
				
		set @cardPre = '86'+'0'+'0'+@BTAreaID+@retainStr
		
		while(1=1)
		begin
			select @cardSerial=SequenceID+1 from AutoDistributeSequence with(updlock) where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode
			if (@cardSerial =100000)
			Begin
					set @Result = -22500
					set @ErrMsg = '自动分配卡资源已满，分配卡失败'
					rollback
					return
			End
			update AutoDistributeSequence 
			set SequenceID=@cardSerial
			where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode

			select @CustID = SequenceID+1 from ParSequenceNumber with(updlock)where SequenceType = '0101'
			update ParSequenceNumber set SequenceID = @CustID where SequenceType = '0101'
			if ( @@error <> 0)
			begin
					set @Result = -22500
					set @ErrMsg = '自动分配CustID失败'
					rollback
					return
			end
			
			if exists (select 1 from PreservedCardRule where ProvinceID=@ProvinceID and AreaID=@OriginalAreaCode and charindex(PreservedNumber,@cardSerial)>0)
			Begin       
				continue
			End
			--set @password = Convert(varchar(6),convert(decimal,RAND()*1000000))
			
			if(len(@cardSerial)=5 )
			begin
				--取得卡号
				set @InnerCardID = Convert(varchar,@cardPre)+convert(varchar,@cardSerial)+'000'
                set @UserAccount = @BTAreaID+@retainStr+convert(varchar,@cardSerial)

				if exists (select 1 from CustInfo where UserAccount=@UserAccount)
				begin
					continue
				end

				--卡表里是否已经存在				
				Set @ExeSql = ' select 1 from BestToneCard_'+@ProvinceID+'_'+@OriginalAreaCode + ' where CardID=''' + @UserAccount + ''''
				Exec(@ExeSql)
				if(@@RowCount>0)
				begin
					continue
				end			
				--Set @CustID=@UserAccount
				set @OCustID = @CustID
				set @OUserAccount = @UserAccount
				break
			end
			else --如果生成的序列号不是8位，重新生成
				continue
		end
	End
	
	if(len(@UserAccount) < 9 or len(@UserAccount)>16)
	Begin
		set @Result = -30008
		set @ErrMsg = '生成卡号失败'
		rollback
		return
	End

	--校验CustID,UserAccount
	if exists (select 1 from CustInfo where CustID = @CustID or UserAccount=@UserAccount)
	Begin
		set @Result = -30001
		set @ErrMsg = '客户id或用户帐号已存在' 
		rollback
		return
	End
	

	-- 校验CustUserInfo表
	if exists (select 1 from CustUserInfo  where CustID = @CustID or UserAccount=@UserAccount or InnerCardID = @InnerCardID)
	begin
		set @Result = -30001
		set @ErrMsg = '客户id或用户帐号在CustUserInfo中已存在' 
		rollback
		return
	end

	-- 校验证件号码@CertificateType
	if ( @CertificateType <> '' and @CertificateType is not null)
	begin
		if exists (select 1 from CustInfo where CertificateType = @CertificateType and CertificateCode = @CertificateCode)
		begin
			set @Result = -22500
			set @ErrMsg = '当前证件号码已经注册，不能在注册'
			Rollback
			return
		end
	end
	

	if exists (select 1 from CustInfo where RealName = @RealName)
	begin
	   set @Valid1=1
	end
	if exists (select 1 from CustExtend where CustContactTel = @MobilePhone)
	begin
		set @Valid2=1
	end
	
	if( @Valid1=1 and @Valid2=1)
	begin
		set @Result = -22500
		set @ErrMsg = '当前姓名手机号码在数据库中已经存在'
		Rollback
		return
	end	

	--插入客户信息表	
	insert into CustInfo( CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,RealName,
		ProvinceID,AreaID,CustLevel,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime )
		values (@CustID,@UserAccount,@CustType,@EncryedPassword,@CertificateType,@CertificateCode,@RealName,
		@ProvinceID,@OriginalAreaCode,@CustLevel,@RegSrc,@RegType,getdate(),'00', getdate())

	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '插入CustInfo时错误'
		Rollback
		return
	End
	-- 插入客户用户信息表
	------------------------------------------------------------------------------------------------------------------------
	if not exists (select 1 from CustUserInfo where UserAccount=@UserAccount)
	begin
		insert into CustUserInfo(CustID,SPID,InnerCardID,UserAccount,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime)
							values(@CustID,@SPID,@InnerCardID,@UserAccount,@RegSrc,@RegType,getdate(),'00',getdate())
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '插入CustUserInfo时错误'
			Rollback
			return
		End
	end
	else
	begin
		set @Result = -30001
		set @ErrMsg = 'UserAccount在 CustUserInfo表中已经存在'
		Rollback
		return
	end

	-- 插入扩展信息
	------------------------------------------------------------------------------------------------------------------------------------------
	if exists (select 1 from CustExtend where CustID = @CustID)
	begin
		set @Result = -22500
		set @ErrMsg = '无法插入客户扩展信息表，CustID已经存在'
		Rollback
		return
	end
	
	insert into CustExtend(CustID,UserAccount,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,CustContactTel,DealTime)
	      values(@CustID,@UserAccount,@Sex,@BirthDay,@EduLevel,@Fav,@IncomLevel,@Email,@MobilePhone,getdate())
	      
	if(@@Error<>0)
	Begin
		set @Result = -22500
		set @ErrMsg = '插入CustInfo时错误'
		Rollback
		return
	End
	

	
			
	-- 插入电话号码绑定信息 不论邦定电话是否成功，对客户注册（上面两步）不再回退
	-----------------------------------------------------------------------------------------------------------------------------------------------
	--Declare @BoundErrMsg varchar(1024)
	Declare @BoundDate datetime
	--set @BoundErrMsg = ''
	--set @BoundDate = getdate()
	
	if not exists ( select 1 From CustInfo Where CustID = @CustID and UserAccount = @UserAccount)
	Begin
		set @Result = 1
		set @ErrMsg = ' 插入绑定电话失败，无此帐号'
		commit
		return 
	End
	
	select 1 from BoundPhone where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = 1
		set @ErrMsg = ' 该用户超过绑定电话个数限制'
		commit
		return
	End
	
	---  bound mobilephone------------------
	IF(@MobilePhone <> '' and @MobilePhone is not null)
	BEGIN
	if exists( select 1 from BoundPhone where BoundPhoneNumber = @MobilePhone)
	Begin
		select @BoundDate = DealTime from BoundPhone where BoundPhoneNumber = @MobilePhone
		if( @MobileUserDate > @BoundDate)
		begin
			update BoundPhone set CustID=@CustID,UserAccount=@UserAccount,DealTime=getdate() where BoundPhoneNumber = @MobilePhone
			if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' 更新绑定手机号码错误'				
				End
		end	
		else
		begin
			set @Result = 1
			set @ErrMsg = @ErrMsg+' 手机号码已被使用 '	
		end		
	End
	else
	Begin
		insert into BoundPhone (CustID,UserAccount,BoundPhoneNumber,DealTime) values(@CustID,@UserAccount,@MobilePhone,getdate())
		if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' 绑定手机号码错误'				
				End
	End
	END  -- for BEGIN
	--- End bound mobilephone
	
	--- Bound @TelCode1-----
	IF(@TelCode1 <> '' and @TelCode1 is not null)
	BEGIN
	select 1 from BoundPhone where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = 1
		set @ErrMsg = @ErrMsg+ ' 该用户超过绑定电话个数限制,不能绑定电话1'
		commit
		return
	End
	
	if exists (select 1 from BoundPhone where BoundPhoneNumber = @TelCode1)
	Begin
		select @BoundDate = DealTime from BoundPhone where BoundPhoneNumber = @TelCode1
		if( @TelCode1Date > @BoundDate)
		begin
			update BoundPhone set CustID=@CustID,UserAccount=@UserAccount,DealTime=getdate() where BoundPhoneNumber = @TelCode1
			if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+'  更新绑定电话号码1错误'				
				End
		end	
		else
		begin
			set @Result = 1
			set @ErrMsg = @ErrMsg+' 电话号码1已被使用 '	
		end				
	End
	else
	Begin
		insert into BoundPhone (CustID,UserAccount,BoundPhoneNumber,DealTime) values(@CustID,@UserAccount,@TelCode1,getdate())
		if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' 绑定电话号码1错误'				
				End
	
	End
	END
	--- End Bound @TelCode1-----------------------

	--- Bound TelCode2-----------------------------
	IF (@TelCode2 <>'' AND @TelCode2 is not null)
	BEGIN
	select 1 from BoundPhone where CustID = @CustID
	if(@@RowCount>=5)
	Begin
		set @Result = 1
		set @ErrMsg = @ErrMsg+ ' 该用户超过绑定电话个数限制,不能绑定电话2'
		commit
		return
	End
	
	if exists (select 1 from BoundPhone where BoundPhoneNumber = @TelCode2)
	Begin
		select @BoundDate = DealTime from BoundPhone where BoundPhoneNumber = @TelCode2
		if( @TelCode2Date > @BoundDate)
		begin
			update BoundPhone set CustID=@CustID,UserAccount=@UserAccount,DealTime=getdate() where BoundPhoneNumber = @TelCode2
			if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+' 更新绑定电话号码2错误'				
				End
		end	
		else
		begin
			set @Result = 1
			set @ErrMsg = @ErrMsg+' 电话号码2已被使用 '	
		end				
	End
	else
	Begin
		insert into BoundPhone (CustID,UserAccount,BoundPhoneNumber,DealTime) values(@CustID,@UserAccount,@TelCode2,getdate())
		if(@@Error<>0)
				Begin
					set @Result = 1
					set @ErrMsg = @ErrMsg+ ' 绑定电话号码2错误'				
				End
	
	End
	END
	---End Bound TelCode2 ------------------------


	Commit  -- End For Tran
END








GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

