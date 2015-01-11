


 
alter  Procedure [dbo].[up_BT_OV3_Interface_UserAuthenStyleUpload]
(
	 @ProvinceID varchar(2),
	 @ID varchar(16),  
     @AreaID varchar(3),  
     @AuthenRecords text,
     @Result int out,
     @ErrMsg varchar(256) out,
     @CustID varchar(18) out 
     
) as
	Declare @DealTime DateTime 
	Set @DealTime = GetDate()

    Declare @SysID varchar(4)
    set @SysID=@ProvinceID+'01'

    set @ErrMsg=''
    set @Result=0

    Declare @UserType varchar(2)

    select @CustID=custid,@UserType=CustType from CustInfo  with(nolock)
     where  outerid =@ID and  SourceSPID=@SysID
		if(@@RowCount=0)
		begin
            Set @Result = -19999
			Set @ErrMsg = '无此客户'				
			return
		end

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
	declare @ContactTel  varchar(20)


	begin Tran	

   update CustInfo Set Email='',EmailClass='',DealTime=@DealTime
		where CustID = @CustID and 	EmailClass='2'
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '更新客户基本信息表时错误'
			Rollback
			return
		End
   update CustInfo Set UserName='',DealTime=@DealTime
		where CustID = @CustID 
		if(@@Error<>0)
		Begin
			set @Result = -22500
			set @ErrMsg = '更新客户基本信息表时错误'
			Rollback
			return
		End

	--删除原认证信息
      delete from CustAuthenInfo where CustID=@CustID
		if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrMsg = '删除客户验证信息失败'
			Rollback			
			return
		End
      delete from CustPhone where CustID=@CustID
		if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrMsg = '删除客户电话信息失败'
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
           update CustInfo set UserName=@ContactTel where CustID=@CustID
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
           update CustInfo set Email=@ContactTel,EmailClass='2' where Email=@CustID
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
            select @CustID,@UserType,@ProvinceID,authenName,AuthenType,@DealTime ,@DealTime from @Authen 
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
            select @CustID,@ProvinceID,@UserType,authenName,SType,'2',@SysID,@DealTime from @Authen 
            where SType <>''
        if(@@Error<>0 )
           begin
              Set @Result = -19999
              Set @ErrMsg = '插入电话认证方式失败：' 
              Rollback
              return
           end

		commit

	

		
