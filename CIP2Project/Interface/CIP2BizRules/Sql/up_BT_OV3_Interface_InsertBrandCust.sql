USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_OV3_Interface_InsertBrandCust]    脚本日期: 10/22/2009 10:34:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[up_BT_OV3_Interface_InsertBrandCust] (
 	@UProvinceID varchar(2),
 	@ID varchar(16),
 	@CustType varchar(2),
 	@CardID varchar(16),
 	@RealName varchar(20),
 	@CustLevel varchar(1),
 	@AreaID varchar(3),
 	@Address varchar(256),
 	@ZipCode varchar(6),
 	@Sex varchar(1),
 	@CertificateType varchar(1),
 	@CertificateCode varchar(20),
 	@TelCode2 varchar(20),
 	@Email varchar(50),
 	@Phone varchar(20),
 	@PHSNumber varchar(20),
 	@Mobile varchar(20),
 	@ADSLAccount varchar(50),
 	@RegDate DateTime,
 	@RegistrationSource char(2),
 	@RegistrationType char(1),
 	@OUserAccount varchar(16) out,
 	@OCustID varchar(16) out, 
 	@Result int out,
 	@ErrMsg varchar(256) out
 ) as
Set @Result = -1

		Declare @CustID varchar(16)
		Declare @CurrentTime dateTime
		set @CurrentTime = GetDate()

        set @CustID=''
        Declare @SysID varchar(8)
        set @SysID=@UProvinceID+'999999'

		if(@AreaID ='')
			Begin
				set @Result = -22500
				set @ErrMsg = '区号不能为空'
				return
			End
		else 
		Begin
			 if(len(@AreaID)=4 )      
				set @AreaID=right(@AreaID,3)
		end
		

		if Exists (select 1 from CustInfo with(nolock) where Outerid=@ID and SourceSPID=@SysID )
		Begin
			set @Result = -22500
			set @ErrMsg = '帐号重复'
			return
		End
        if(@CertificateCode is not null and @CertificateCode<>'')
begin
        select 1  from CustInfo with(nolock) where CertificateType=@CertificateType and CertificateCode=@CertificateCode and CustType not in('41','42','43','50')
        if(@@RowCount>0)
		Begin           
			set @Result = -22500
			set @ErrMsg = '身份证号重复'
			return
		End		
end
		Begin Tran
			--获取@CustID
			declare @SecquenceID int
			begin tran
				select @SecquenceID = SequenceID  from ParSequenceNumber with(updlock) where SequenceType = '0101'
				Set @CustID = @SecquenceID+1
				update ParSequenceNumber set SequenceID =  @SecquenceID+1 where SequenceType = '0101'
			commit
			
			insert into CustInfo(CustID,ProvinceID,AreaID,CustType,CertificateType,CertificateCode,RealName,CustLevel,
                  Sex,RegistrationSource,Status,Email,EmailClass,SourceSPID,OuterID,DealTime,CreateTime )
				values (@CustID,@UProvinceID,@AreaID,@CustType,@CertificateType,@CertificateCode,@RealName,@CustLevel,
                  @Sex,@RegistrationSource,'00',@Email,'1',@SysID,@ID,@CurrentTime,@CurrentTime)			
				if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '插入客户基本信息表时错误'
					Rollback
					return
				End

           insert into CustExtendInfo(CustID,ProvinceID,Createtime,DealTime)
           values(@CustID,@UProvinceID,@CurrentTime,@CurrentTime)

			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户地址信息时错误'
				Rollback
				return
			End
			
			--插入客户地址信息
			Insert Into AddressInfo(CustID,Address,ZipCode,[Type],DealTime)
				 values( @CustID,@Address,@ZipCode,'99',@CurrentTime)			
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '插入客户地址信息时错误'
				Rollback
				return
			End
			
			if(@TelCode2<>'' or @TelCode2 is not null)
            begin
               insert into CustPhone(CustID,ProvinceID,CustType,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
                 values(@CustID,@UProvinceID,@CustType,@TelCode2,'4','1',@SysID,@CurrentTime)
               if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '保存联系电话时错误'
					Rollback
					return
				End
            end 

		Commit

		Set @oCustID = @CustID
		Set @oUserAccount =@CardID
		Set @Result = 0
