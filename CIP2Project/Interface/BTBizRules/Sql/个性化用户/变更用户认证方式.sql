USE [besttone]
GO
/****** 对象:  StoredProcedure [dbo].[up_BT_V5_Interface_UserAuthenStyleUpload]    脚本日期: 06/04/2009 14:25:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


 
ALTER  Procedure [dbo].[up_v5_BestTone_ChangeUserAuthenStyleCRM]
(
     @ProvinceID varchar(2),
	 @OriginalID varchar(16),  
     @NewID varchar(16),
     @DealType varchar(1),  
     @AuthenRecords text,
     @Result int out,
     @ErrorDescription varchar(256) out,
     @NewCustID varchar(16) out,
     @OriginalCustID varchar(16) out
     
) as
	Declare @SysID varchar(4)
	set @SysID=@ProvinceID+'01'
    set @ErrorDescription=''
    set @Result=0
    
    select @NewCustID=CustID from CustUserInfo with(nolock) where outerid=@OriginalID
    if(@@RowCount=0)
		begin
            Set @Result = -19999
			Set @ErrorDescription = '保存后的CustID不存在！'				
			return
		end
    select @OriginalCustID=CustID from CustUserInfo with(nolock) where outerid=@NewID
    if(@@RowCount=0)
		begin
            Set @Result = -19999
			Set @ErrorDescription = '要变更的省客户ID不存在！'				
			return
		end
    
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
	
	--begin Tran		
	if(@DealType=='1')
	begin
	  update UserAuthenStyle set UserAuthenStyle.CustID=@NewID where UserAuthenStyle.CustID=@OriginalCustID and  UserAuthenStyle.CustID=@OriginalCustID and authenName in (select authenName from @Authen ) and authenType in(select authenType  from @Authen )
	  if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrorDescription = '更新认证信息失败'					
			return
		End
	end
	else if(@DealType=='2')
	begin
       delete from UserAuthenStyle where UserAuthenStyle.CustID=@OriginalCustID and authenName in (select authenName from @Authen ) and authenType in(select authenType  from @Authen )
       if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrorDescription = '注销认证信息失败！'					
			return
		End
	end 
	else
	begin
	   Declare @DealTime DateTime 
	   Set @DealTime = GetDate()
	   if  exists(select 1 from UserAuthenStyle where authenName in (select authenName from @Authen ) and authenType in(select authenType  from @Authen ))
	   begin
	        Set @Result = -19999
			Set @ErrorDescription = '已经存在认证信息！'					
			return
	   end 
	   Insert into UserAuthenStyle( CustID,AuthenName,AuthenType,SourceSPID,Status,Description,DealTime)
			select  @OriginalCustID,AuthenName,AuthenType,@SysID,'0','',@DealTime from @Authen
		if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrorDescription = '插入认证方式表失败！'					
			return
		End
	end
	

	--commit
	return

	

		
