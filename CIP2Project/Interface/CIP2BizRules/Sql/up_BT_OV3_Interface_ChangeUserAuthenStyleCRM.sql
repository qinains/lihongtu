USE [CIP2]
GO
/****** ����:  StoredProcedure [dbo].[up_BT_OV3_Interface_ChangeUserAuthenStyleCRM]    �ű�����: 11/19/2009 15:17:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


 
ALTER  Procedure [dbo].[up_BT_OV3_Interface_ChangeUserAuthenStyleCRM]
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
	Declare @SysID varchar(8)
    Declare @CustType varchar(2)

    Declare @UserName varchar(30)
    Declare @Email varchar(256)
	set @SysID=@ProvinceID+'999999'
    set @ErrorDescription=''
    set @Result=0
   
    select @NewCustID=CustID,@CustType=CustType from CustInfo with(nolock) where outerid=@OriginalID and SourceSPID=@SysID
    if(@@RowCount=0)
		begin
            Set @Result = -19999
			Set @ErrorDescription = '������CustID�����ڣ�'				
			return
		end
     



    select @OriginalCustID=CustID from CustInfo with(nolock) where outerid=@NewID and SourceSPID=@SysID
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
    	AuthenType varchar(2),
        SType Varchar(2)       
	)

	if( @AuthenRecords is not null )
	Begin
		-- ������������
		-- �����󶨵绰�б��ϵ
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
	
	--begin Tran		
	if(@DealType='1')
	begin

--        select 1 from custphone c with(nolock), @Authen a where CustID=@OriginalCustID and a.SType=c.PhoneType and a.AuthenName=c.phone  
--        if(@@RowCount=0)
--        begin            
--            
--			select 1 from CustAuthenInfo c with(nolock), @Authen a where a.SType='' and CustID=@OriginalCustID and a.AuthenType=c.authenType and a.AuthenName=c.AuthenName  					
--			if(@@RowCount=0)
--			begin            
--				Set @Result = -19999
--				Set @ErrorDescription = 'Ҫ�޸ĵ绰��Ϣ������'
--				return
--			end 
--        end 
       begin Tran	
      update CustPhone set CustPhone.custid=@NewCustID from  @Authen a where CustID=@OriginalCustID and a.SType=CustPhone.PhoneType and a.AuthenName=CustPhone.phone  
       if(@@RowCount>0)
        begin            
			Set @Result = -19999
			Set @ErrorDescription = '�޸ĵ绰��Ϣʧ��'
            rollback 					
			return
        end 
	  update CustAuthenInfo set CustAuthenInfo.CustID=@NewCustID from  @Authen a where CustID=@OriginalCustID and a.AuthenType in('5','6','8','11','12','13','14','15') and CustAuthenInfo.Authentype=a.Authentype and a.AuthenName=CustAuthenInfo.AuthenName
       if(@@RowCount>0)
        begin            
			Set @Result = -19999
			Set @ErrorDescription = '�޸���֤��Ϣʧ��'
            rollback 					
			return
        end 
       commit
	end
	else if(@DealType='2')
	begin
        begin Tran	
       select top 1 @UserName=AuthenName from @Authen where SType='' and AuthenType='1'
        if(@@RowCount>0)
        begin            
			update CustInfo set  UserName=null where custid=@OriginalCustID and UserName=@UserName
			if(@@error<>0)
			begin
			  Set @Result = -19995
			  Set @ErrorDescription = 'ע���û�����Ϣ����'
			  rollback  
			  return 
			end
        end

        select top 1 @Email=AuthenName from @Authen where SType='' and AuthenType='4'
        if(@@RowCount>0)
        begin      
			update CustInfo set  Email=@Email,EmailClass='2' where  custid=@OriginalCustID and Email = @Email
			if(@@error<>0)
			begin
			  Set @Result = -19997
			  Set @ErrorDescription = 'ע��Email��Ϣ����'
			  rollback  
			  return 
			end
        end


        delete from CustPhone from @Authen a where CustPhone.custid=@OriginalCustID and  a.SType<>'' and a.SType=CustPhone.PhoneType and a.AuthenName=CustPhone.phone 
        if(@@error<>0)
        begin
          Set @Result = -19996
          Set @ErrorDescription = 'ע���绰��Ϣ����'
          rollback  
          return 
        end

        delete from CustAuthenInfo from @Authen a where CustAuthenInfo.custid=@OriginalCustID and  a.SType='' and a.authenType=CustAuthenInfo.authenType and a.AuthenName=CustAuthenInfo.AuthenName 
       if(@@error<>0)
        begin
          Set @Result = -19994
          Set @ErrorDescription = 'ע����֤��Ϣ����'
          rollback  
          return 
        end

       
        commit
	end 
	else if(@DealType='3')
	begin
    -----�����֤��Ϣ
    Declare @DealTime DateTime 
	Set @DealTime = GetDate()
      begin Tran	
      select 1 from custphone c with(nolock),@Authen a where a.SType<>'' and a.SType=c.PhoneType and a.AuthenName=c.phone  
      if(@@RowCount>0)
        begin
           Set @Result = -19998
          Set @ErrorDescription = '�绰��Ϣ�Ѵ��ڣ�'
          rollback  
          return 
        end 

      insert into CustPhone(CustID,ProvinceID,CustType,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
       select @OriginalCustID,@ProvinceID,@CustType,AuthenName,SType,'2',@SysID,@DealTime from @Authen where SType<>'' 
       if(@@error<>0)
        begin
          Set @Result = -19997
          Set @ErrorDescription = '����绰��Ϣ����'
          rollback  
          return 
        end
       
        select top 1 @UserName=AuthenName from @Authen where SType='' and AuthenType='1'
        if(@@RowCount>0)
        begin
            select 1 from CustInfo where UserName = @UserName
            if(@@error<>0)
            begin
              Set @Result = -19996
			  Set @ErrorDescription = '�û����Ѿ����ڣ�'
			  rollback  
			  return 
            end 
			update CustInfo set  UserName=@UserName where custid=@OriginalCustID 
			if(@@error<>0)
			begin
			  Set @Result = -19995
			  Set @ErrorDescription = '�����û�����Ϣ����'
			  rollback  
			  return 
			end
        end

        select top 1 @Email=AuthenName from @Authen where SType='' and AuthenType='4'
        if(@@RowCount>0)
        begin   
            select 1 from CustInfo where Email = @Email
            if(@@error<>0)
            begin
              Set @Result = -19996
			  Set @ErrorDescription = 'Email�Ѿ����ڣ�'
			  rollback  
			  return 
            end     
			update CustInfo set  Email=@Email,EmailClass='2' where custid=@OriginalCustID 
			if(@@error<>0)
			begin
			  Set @Result = -19997
			  Set @ErrorDescription = '����Email��Ϣ����'
			  rollback  
			  return 
			end
        end
        
        select 1 from CustAuthenInfo  c with(nolock),@Authen a 
        where a.AuthenType in('5','6','8','11','12','13','14','15') and c.Authentype=a.Authentype and a.AuthenName=c.AuthenName
        if(@@error<>0)
            begin
              Set @Result = -19996
			  Set @ErrorDescription = '��֤��Ϣ�Ѿ����ڣ�'
			  rollback  
			  return 
            end 
        insert into  CustAuthenInfo(CustID,ProvinceID,CustType,AuthenName,AuthenType,CreateTime,Dealtime)
        select @OriginalCustID,@ProvinceID,@CustType,AuthenName,AuthenType,@DealTime,@DealTime from @Authen a 
        where a.AuthenType in('5','6','8','11','12','13','14','15') 
        if(@@error<>0)
			begin
			  Set @Result = -19997
			  Set @ErrorDescription = '������֤��Ϣ��Ϣ����'
			  rollback  
			  return 
			end
           
        commit

	
end
   
 Set @Result = 0
Set @ErrorDescription = ''
	return

	

		


