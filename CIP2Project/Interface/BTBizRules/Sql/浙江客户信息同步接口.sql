



USE [BestTone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V2_Interface_InsertUserCommonInfo]    脚本日期: 11/29/2007 16:38:38 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_ReceiveCustInfo]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_ReceiveCustInfo]
GO


/*

说明：



*/


Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go

Create   Procedure [dbo].[up_BT_V2_Interface_ReceiveCustInfo]
(
		@SPID varchar(8),
		@DealType varchar(1),
		@userType varchar(2),
		@userAccount varchar(16),
		@uProvinceID varchar(2),
		@status varchar(2),
		@realName varchar(30),
		--@cardClass varchar(1),
		--@credit varchar(32),
		@registration dateTIme,
		@certificateCode varchar(20),
		@certificateType varchar(2),
		@birthday DateTime,
		@sex varchar(1),
		@custLevel varchar(1),
		@custContactTel varchar(20),
		--@enterpriseID varchar(30),
		@Result Int out,
		@ErrMsg varchar(256) out,
		@oCustID varchar(16) out
)
as
		Set @Result = -1
		Set @ErrMsg = ''

		Declare @CurrentTime dateTime
		Set @CurrentTime = GetDate()
		
		--0：新增	1：修改


		if(@DealType='0')
		Begin
			Declare @AreaCode varchar(3)
			Set @AreaCode = '571'
			Set @UProvinceID = '12'

			declare @OriginalAreaCode varchar(3)
			Set @OriginalAreaCode = right(('0' + @AreaCode),3)
			
			declare @RegistrationSource varchar(2)
			set @RegistrationSource = '06'
			declare @RegistrationType varchar(1)
			set @RegistrationType = '2'

			Declare @InnerCardID varchar(16)
			set @InnerCardID = ''
			
			Declare @ExeSql nvarchar(4000)


			
				
			--校验证件 可为空， 但不能有重复。


			
			if(@CertificateCode!='' and @CertificateType !='')
			Begin
				if exists(select 1 from custInfo where CertificateCode=@CertificateCode and CertificateType=@CertificateType)
				Begin
					set @Result = -30001
					set @ErrMsg = '证件号已存在，不能注册'
					return
				End
			End
		
			Set @InnerCardID = '8600' + @UserAccount
			
			declare @SecquenceID int
			begin tran
				select @SecquenceID = SequenceID  from ParSequenceNumber with(updlock) where SequenceType = '0101'
				Set @oCustID = @SecquenceID+1
				update ParSequenceNumber set SequenceID =  @SecquenceID+1 where SequenceType = '0101'
			commit
		
		
			if exists(select 1 from custUserInfo where InnerCardID =@InnerCardID or UserAccount = @UserAccount or CustID = @oCustID)
			Begin
				set @Result = -30001
				set @ErrMsg = '该用户已存在'
				return
			End
			
			
		--开启事物进行处理


		--------------------------------------------------------------------------------------------------------------
		begin tran
		--select @oCustID,@InnerCardID,@oUserAccount,@UserType,@EncryptPassword,@CertificateType,@CertificateCode,@RealName,@uProvinceID,@OriginalAreaCode,@CustLevel,@RegistrationSource,@RegistrationType,@CurrentTime,'00', @CurrentTime, ''
				
				insert into CustInfo( CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,RealName,
					ProvinceID,AreaID,CustLevel,RegistrationSource,RegistrationType,RegistrationDate,Status,DealTime, EnterPriseID )
					values (@oCustID,@UserAccount,@UserType,'',@CertificateType,@CertificateCode,@RealName,
					@uProvinceID,@OriginalAreaCode,@CustLevel,@RegistrationSource,@RegistrationType,@CurrentTime,'02', @CurrentTime, '')
				
					if(@@Error<>0)
					Begin
						set @Result = -22500
						set @ErrMsg = '插入客户基本信息表时错误'
						Rollback
						return
					End
				
				--插入客户信息扩展表


				insert Into CustExtend(CustID,UserAccount,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,CustContactTel,DealTime)
				Values (@oCustID, @UserAccount,@Sex,@BirthDay,'','','','',@CustContactTel,@CurrentTime)
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
					insert into boundPhone (CustID,UserAccount,boundPhoneNumber,dealTIme)
						values ( @oCustID,@UserAccount,@CustContactTel,@CurrentTime)
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
					RegistrationDate,Status,DealTime )
					values( @oCustID,@InnerCardID,@UserAccount,@SPID,@RegistrationSource, @RegistrationType,@CurrentTime,@Status,@CurrentTime )
				if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '插入客户用户信息表错误'
					Rollback
					return
				End		

				Commit
	 
			
		Set @Result=0
	END
	
	
	if(@DealType = '1')
	Begin
		begin tran
				
			--更新客户信息表



			update CustInfo 
			set RealName=@RealName,ProvinceID = @UProvinceID,
				Status=@Status,DealTime=@CurrentTime, CertificateCode=@CertificateCode,CustLevel=@CustLevel,
				CertificateType=@CertificateType,registrationDate=@registration
			Where UserAccount = @UserAccount

			if(@@Error<>0)
			Begin
				set @Result = -32500
				set @ErrMsg = '修改客户基本信息表时错误'
				Rollback
				return
			End
			
			--更新客户信息扩展表


			update CustExtend
			Set Sex=@Sex,BirthDay=@BirthDay,CustContactTel=@CustContactTel,DealTime=@CurrentTime
			Where UserAccount = @UserAccount
			
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户信息扩展表时错误'
				Rollback
				return
			End
			
			commit
			
			set @Result = 0
	
	End


Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go

