set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


 
ALTER  Procedure [dbo].[up_BT_V2_Interface_CustInfoUpload]
(
	 @SysID varchar(4),
	 @ID varchar(16),  
	 @UserType varchar(2),  
	 @UserAccount varchar(16),  
	 @CustLevel varchar(1),
     @RealName varchar(20),  
     @ContactTel varchar(20),  
     @Address varchar(200), 
     @ZipCode varchar(6),  
     @CertificateCode varchar(20),
     @CertificateType varchar(2),  
     @AreaID varchar(3),  
     @Sex varchar(1),  
     @Email varchar(50),  
     @dealType varchar(1),
     @AuthenRecords text,
     @Result int out,
     @ErrMsg varchar(256) out,
     @CustID varchar(16) out 
     
) as
	Declare @CustID varchar(16)
	Declare @DealTime DateTime 
	Set @DealTime = GetDate()

	Declare @AuthenDoc int

	Declare @Authen Table
	(
		 AuthenName varchar(20),
    	AuthenType varchar(2)
	)

	if( @AuthenRecords is not null )
	Begin
		-- 解析请求数据
		-- 解析绑定电话列表关系
		EXECUTE sp_xml_preparedocument @AuthenDoc OUTPUT, @AuthenRecords

		Insert Into @Authen( AuthenName, AuthenType )
		SELECT AuthenName, AuthenType 	
			FROM OpenXML( @AuthenDoc, '/ROOT/AuthenRecord' , 2 ) 
			With( AuthenName	Varchar(20), AuthenType varchar(2) )
	End
	
	
	if(@dealType='0') --新增
	Begin
		Declare @OUserAccount varchar(16)
		declare @OCustID varchar(16)
		begin Tran
		Exec up_BT_V2_Interface_InsertBrandCust left(@SysID),@ID,@UserType,@UserAccount,@RealName,@CustLevel,@AreaID,@Address,@ZipCode,
			@Sex,@CertificateType,@CertificateCode,@ContactTel,@Email,'','','','',@DealTime,	'06','2',@OUserAccount out,@OCustID out,@Result out,@ErrMsg out
		if(@@Error<>0 or @Result!=0)
		Begin
			Rollback			
			return
		End

			
		Insert into UserAuthenStyle( CustID,AuthenName,AuthenType,SourceSPID,Status,Description,DealTime)
			select  @OCustID,AuthenName,AuthenType,@SysID,'0','',@DealTime from @Authen
		if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrMsg = '插入认证方式表失败'
			Rollback			
			return
		End

		commit
		return
	End
	
	
	--修改信息
	--------------------------------------------------------------------
	Select @CustID = CustID from CUstInfo where UserAccount=@UserAccount and CustType = @UserType 
		if(@@RowCount=0)
		Begin
			Set @Result = -19999
			Set @ErrMsg = '无此客户'
			return
		End
begin Tran
		update CustInfo Set CertificateType =@CertificateType,CertificateCode=@CertificateCode,RealName=@RealName,
		CustLevel=@CustLevel
		where CustID = @CustID
			
				if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '更新客户基本信息表时错误'
					Rollback
					return
				End

		--更新客户信息扩展表


		update CustExtend Set Sex=@Sex,Email=@Email 
		where  CustID = @CustID

			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新客户信息扩展表时错误'
				Rollback
				return
			End
			
		Set @Result = 0
		
commit