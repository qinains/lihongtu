USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_OV3_Interface_CustInfoUpload]    脚本日期: 10/30/2009 16:16:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[up_BT_OV3_Interface_CustInfoUpload] (
 	 @SysID varchar(8),
 	 @ID varchar(16),  
 	 @UserType varchar(2),  
 	 @UserAccount varchar(16),  
 	 @CustLevel varchar(1),
      @RealName varchar(20),  
      @ContactTel varchar(20),  
      @Address varchar(200), 
      @ZipCode varchar(6),  
      @CertificateCode varchar(20),
      @CertificateType varchar(1),  
      @AreaID varchar(3),  
      @Sex varchar(1),  
      @Email varchar(50),  
      @dealType varchar(1),
      @AuthenRecords text,
      @Result int out,
      @ErrMsg varchar(256) out,
      @CustID varchar(16) out
    --  @CustID_OUT varchar(16) out
      
 ) as
--Declare @CustID varchar(16)
	Declare @DealTime DateTime 
	Set @DealTime = GetDate()
    set @CustID=''
declare @NCustID varchar(16)
    set @NCustID=''
    Declare @ProvinceID varchar(2)
    set @ProvinceID=left(@SysID,2)
  --  set @CustID_OUT=''
	Declare @AuthenDoc int

	Declare @Authen Table
	(
		AuthenName varchar(20),
    	AuthenType varchar(2),
        SType Varchar(2)       
	)

	if( @AuthenRecords is not null )
	Begin
		-- 解析请求数据
		-- 解析绑定电话列表关系
		EXECUTE sp_xml_preparedocument @AuthenDoc OUTPUT, @AuthenRecords


		Insert Into @Authen( AuthenName, AuthenType ,SType)
		SELECT AuthenName, AuthenType , 
			CASE AuthenType 
				WHEN '9' THEN '1' 
				WHEN '10' THEN '3' 
				WHEN '7' THEN '2'
                else '' 			
			END
			FROM OpenXML( @AuthenDoc, '/ROOT/AuthenRecord' , 2 ) 
			With( AuthenName	Varchar(20), AuthenType varchar(2) )
	End
		
if(@CertificateType is null or @CertificateType='')
begin
   set @CertificateType='0'
end 
	
	if(@dealType='0') --新增
	Begin
		Declare @OUserAccount varchar(16)
	--	declare @OCustID varchar(16)
		
		begin Tran



		Exec up_BT_OV3_Interface_InsertBrandCust @ProvinceID, @ID,@UserType,@UserAccount,@RealName,@CustLevel,@AreaID,@Address,@ZipCode,
			@Sex,@CertificateType,@CertificateCode,@ContactTel,@Email,'','','','',@DealTime,	'06','2',@OUserAccount out,@NCustID out,@Result out,@ErrMsg out
		if(@@Error<>0 or @Result!=0)
		Begin
			Rollback			
			return
		End

		select  top 1 @ContactTel=AuthenName from @Authen  where AuthenType ='1' 
        if(@@RowCount>0)
         begin
           select 1  from custinfo with(nolock) where UserName=@ContactTel
           if(@@RowCount>0)
           begin
              Set @Result = -19999
              Set @ErrMsg = '用户名已经存在:'+@ContactTel
              Rollback
              return
           end 
           update CustInfo set UserName=@ContactTel where CustID=@NCustID
           if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入用户名认证方式失败：' + @ContactTel
              Rollback
              return
           end
         end

        select  top 1 @ContactTel=AuthenName from @Authen  where AuthenType ='4' 
        if(@@RowCount>0)
         begin
           select  1  from custinfo with(nolock) where Email=@ContactTel and EmailClass='2'
           if(@@RowCount>0)
           begin
              Set @Result = -19999
              Set @ErrMsg = 'Email已经存在:'+@ContactTel
              Rollback
              return
           end 
           update CustInfo set Email=@ContactTel,EmailClass='2' where CustID=@NCustID
           if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入Email认证方式失败：' + @ContactTel
              Rollback
              return
           end
         end
        
        select 1 from CustAuthenInfo c with(nolock) ,@Authen a   where 
          a.AuthenType in('5','6','8','11','12','13','14','15') and c.AuthenName=a.AuthenName and c.AuthenType=a.AuthenType
        if(@@RowCount>0)
        begin
              Set @Result = -19999
              Set @ErrMsg = '卡认证方式已经存在:'
              Rollback
              return
        end 
        Insert into CustAuthenInfo(CustID,CustType,ProvinceID,AuthenName,AuthenType,CreateTime,Dealtime)
            select @NCustID,@UserType,@ProvinceID,authenName,AuthenType,@DealTime ,@DealTime from @Authen 
            where AuthenType in('5','6','8','11','12','13','14','15') 
        if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入卡认证方式失败：' 
              Rollback
              return
           end


       select  1 from CustPhone c with(nolock) ,@Authen a where 
           c.Phone=a.AuthenName and c.PhoneType=a.SType
        if(@@RowCount>0)
        begin
              Set @Result = -19999
              Set @ErrMsg = '电话认证方式已经存在:'

              Rollback
              return
        end 
        Insert into CustPhone(CustID,ProvinceID,CustType,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
            select @NCustID,@ProvinceID,@UserType,authenName,SType,'2',@SysID,@DealTime from @Authen 
            where SType <>''
        if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入电话认证方式失败：' 
              Rollback
              return
           end
 set @CustID=@NCustID
		commit
		return
	End
	
	
	--修改信息
	--------------------------------------------------------------------

	Select @NCustID = CustID from CUstInfo with(nolock) where Outerid=@ID and SourceSPID=@SysID 
		if(@@RowCount=0)
		Begin
			Set @Result = -19999
			Set @ErrMsg = '无此客户'
			return
		End

   if(@CertificateCode is not null and @CertificateCode<>'')
begin
     select 1  from CustInfo with(nolock) where CertificateType=@CertificateType and CertificateCode=@CertificateCode and CustType not in('41','42','43','50') and CustID<>@NCustID 
        if(@@RowCount>0)
		Begin           
			set @Result = -22500
			set @ErrMsg = '身份证号重复'
			return
		End	
end	

	begin Tran	 
		update CustInfo Set CertificateType =@CertificateType,CertificateCode=@CertificateCode,RealName=@RealName,ProvinceID=@ProvinceID,
		CustLevel=@CustLevel,Sex=@Sex,Email=@Email,CustType=@UserType,EmailClass='1',UserName='',DealTime=@DealTime
		where CustID = @NCustID			
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '更新客户基本信息表时错误'
			Rollback
			return
		End

		--更新客户地址信息
		update AddressInfo  Set Address=@Address,ZipCode=@ZipCode
		where  CustID = @NCustID

			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新客户地址信息时错误'
				Rollback
				return
			End

      update CustExtendInfo set ProvinceID=@ProvinceID,DealTime=@DealTime
		where  CustID = @NCustID
			if(@@Error<>0)
			Begin
				set @Result = -22500
				set @ErrMsg = '更新客户扩展信息时错误'
				Rollback
				return
			End

--删除原认证信息
      delete from CustAuthenInfo where CustID=@NCustID
		if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrMsg = '删除客户验证信息失败'
			Rollback			
			return
		End
      delete from CustPhone where CustID=@NCustID
		if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrMsg = '删除客户电话信息失败'
			Rollback			
			return
		End

--重新写入认证信息


			if(@ContactTel<>'' or @ContactTel is not null)
            begin
               insert into CustPhone(CustID,ProvinceID,CustType,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
                 values(@NCustID,@ProvinceID,@UserType,@ContactTel,'4','1',@SysID,@DealTime)
               if(@@Error<>0)
				Begin
					set @Result = -22500
					set @ErrMsg = '保存联系电话时错误'
					Rollback
					return
				End
            end 


	 	select  top 1 @ContactTel=AuthenName from @Authen  where AuthenType ='1' 
        if(@@RowCount>0)
         begin
           select 1  from custinfo with(nolock) where UserName=@ContactTel
           if(@@RowCount>0)
           begin
              Set @Result = -19999
              Set @ErrMsg = '用户名已经存在:'+@ContactTel
              Rollback
              return
           end 
           update CustInfo set UserName=@ContactTel where CustID=@NCustID
           if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入用户名认证方式失败：' + @ContactTel
              Rollback
              return
           end
         end

        select  top 1 @ContactTel=AuthenName from @Authen  where AuthenType ='4' 
        if(@@RowCount>0)
         begin
           select  1  from custinfo with(nolock) where Email=@ContactTel and EmailClass='2'
           if(@@RowCount>0)
           begin
              Set @Result = -19999
              Set @ErrMsg = 'Email已经存在:'+@ContactTel
              Rollback
              return
           end 
           update CustInfo set Email=@ContactTel,EmailClass='2' where Email=@NCustID
           if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入Email认证方式失败：' + @ContactTel
              Rollback
              return
           end
         end
        
        select 1 from CustAuthenInfo c with(nolock) ,@Authen a   where 
          a.AuthenType in('5','6','8','11','12','13','14','15') and c.AuthenName=a.AuthenName and c.AuthenType=a.AuthenType
        if(@@RowCount>0)
        begin
              Set @Result = -19999
              Set @ErrMsg = '卡认证方式已经存在:'
              Rollback
              return
        end 
        Insert into CustAuthenInfo(CustID,CustType,ProvinceID,AuthenName,AuthenType,CreateTime,Dealtime)
            select @NCustID,@UserType,@ProvinceID,authenName,AuthenType,@DealTime ,@DealTime from @Authen 
            where AuthenType in('5','6','8','11','12','13','14','15') 
        if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入卡认证方式失败：' 
              Rollback
              return
           end


       select  1 from CustPhone c with(nolock) ,@Authen a where 
           c.Phone=a.AuthenName and c.PhoneType=a.SType
        if(@@RowCount>0)
        begin
              Set @Result = -19999
              Set @ErrMsg = '电话认证方式已经存在:'
              Rollback
              return
        end 
        Insert into CustPhone(CustID,ProvinceID,CustType,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
            select @NCustID,@ProvinceID,@UserType,authenName,SType,'2',@SysID,@DealTime from @Authen 
            where SType <>''
        if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入电话认证方式失败：' 
              Rollback
              return
           end


        set @CustID=@NCustID
		Set @Result = 0

commit
