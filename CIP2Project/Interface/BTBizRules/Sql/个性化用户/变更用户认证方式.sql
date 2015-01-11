USE [besttone]
GO
/****** ����:  StoredProcedure [dbo].[up_BT_V5_Interface_UserAuthenStyleUpload]    �ű�����: 06/04/2009 14:25:11 ******/
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
			Set @ErrorDescription = '������CustID�����ڣ�'				
			return
		end
    select @OriginalCustID=CustID from CustUserInfo with(nolock) where outerid=@NewID
    if(@@RowCount=0)
		begin
            Set @Result = -19999
			Set @ErrorDescription = 'Ҫ�����ʡ�ͻ�ID�����ڣ�'				
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
		-- ������������
		-- �����󶨵绰�б��ϵ
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
			Set @ErrorDescription = '������֤��Ϣʧ��'					
			return
		End
	end
	else if(@DealType=='2')
	begin
       delete from UserAuthenStyle where UserAuthenStyle.CustID=@OriginalCustID and authenName in (select authenName from @Authen ) and authenType in(select authenType  from @Authen )
       if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrorDescription = 'ע����֤��Ϣʧ�ܣ�'					
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
			Set @ErrorDescription = '�Ѿ�������֤��Ϣ��'					
			return
	   end 
	   Insert into UserAuthenStyle( CustID,AuthenName,AuthenType,SourceSPID,Status,Description,DealTime)
			select  @OriginalCustID,AuthenName,AuthenType,@SysID,'0','',@DealTime from @Authen
		if(@@Error<>0 )
		Begin
			Set @Result = -19999
			Set @ErrorDescription = '������֤��ʽ��ʧ�ܣ�'					
			return
		End
	end
	

	--commit
	return

	

		
