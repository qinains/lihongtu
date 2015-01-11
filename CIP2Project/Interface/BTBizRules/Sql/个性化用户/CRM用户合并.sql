--CRM�û��ϲ�

alter  Procedure [dbo].[up_V5_bestTone_IncorporateCust_BTForCRM]
(
    @ProvinceID varchar(2),	
    @IncorporatedCustID varchar(16),
	@IncorporatedCRMID varchar(16),	
    @SavedCustID varchar(16),
	@SavedCRMID  varchar(16),
	@IncorporatedCustType varchar(2),
	@SavedCustType varchar(2),
    @SavedAccount  varchar(16),	

    @OutProvinceID varchar(2) out ,	
    @Result int out,	
    @ErrorDescription varchar(256) out,
    @CustID varchar(16) out
)
as
set @OutProvinceID=@ProvinceID
if not exists ( select 1 from dbo.CustInfo where CUSTID =@IncorporatedCustID  )
begin  
    Set @Result = -22500
    Set @ErrorDescription = 'Ҫ���ϲ��Ŀͻ����Ų����ڣ�'  
    return 
end 
if not exists ( select 1 from dbo.CustInfo where CUSTID =@SavedCustID  )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '�ϲ���Ŀͻ����Ų����ڣ�'   
    return 
end 
if not exists ( select 1 from dbo.CustUserInfo where CUSTID =@SavedCustID )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '�޺ϲ�����û���'   
    return 
end 
if not exists ( select 1 from dbo.CustUserInfo where CUSTID =@IncorporatedCustID )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '��Ҫ���ϲ��Ŀͻ���'   
    return 
end 

begin tran

--���ݱ�CustUserInfo���ݵ�CustUserInfoHistory
insert into CustUserInfoHistory
(CustID,SPID,InnerCardID,UserAccount,CustPersonType,
RegistrationSource,RegistrationType,RegistrationDate,[Status],IsHaveCard,
DealTime,[Description],Reason,HistoryDescription,SysID,OuterID)
 select CustID,SPID,InnerCardID,UserAccount,
CustPersonType,RegistrationSource,RegistrationType,RegistrationDate,
[Status],'',DealTime,[Description],'','',SysID,OuterID from CustUserInfo  where CustID=@IncorporatedCustID
If( @@error <> 0)
begin

	rollback		
	return 		
end
 

--�޸ı�CustUserInfo����������ʻ��鲢��ͬһ����,
update dbo.CustUserInfo set CustID=@SavedCustID, OuterID= @SavedCRMID,UserAccount=@SavedAccount,
CustType=@SavedCustType where CustID=@IncorporatedCustID 
If( @@error <> 0)
begin
	rollback
	return						
end


--��Ҫɾ�������ݱ��ݵ���ʷ��¼����
insert into CustInfoHistory(CustID,UserAccount,CustType,EncryptedPassword,CertificateType,
CertificateCode,RealName,ProvinceID,AreaID,CustLevel,EnterpriseID,RegistrationSource,
RegistrationType,RegistrationDate,[Status],DealTime,IncorporateCustID,IncorporateUserAccount,
Reason,HistoryDescription)
select CustID,UserAccount,CustType,EncryptedPassword,CertificateType,CertificateCode,
RealName,ProvinceID,AreaID,CustLevel,EnterpriseID,RegistrationSource,RegistrationType,
RegistrationDate,Status,getdate(),@IncorporatedCRMID,@IncorporatedCustID,'',''
from dbo.CustInfo where CustID=@IncorporatedCustID 
If( @@error <> 0)
begin
	rollback  
	return 									
end

--ɾ�����������

delete from CustInfo where CustID=@IncorporatedCustID 
If( @@error <> 0)
begin
	rollback  
	return									
end

--���ݿͻ���չ��Ϣ�������ݲ��뵽��CustExtendHistory
insert into CustExtendHistory(CustID,UserAccount,Sex,BirthDay,EduLevel,
Favorites,InComeLevel,Email,CustContactTel,DealTime,Reason,HistoryDescription) 
select CustID,UserAccount,Sex,BirthDay,EduLevel,
Favorites,InComeLevel,Email,CustContactTel,DealTime,'',''
from CustExtend  where CustID=@IncorporatedCustID
If( @@error <> 0)
begin
	rollback   
	return 										
end


--ɾ���ͻ���չ��Ϣ��

delete from CustExtend  where  CustID=@IncorporatedCustID
If( @@error <> 0)
begin
        rollback	
        return	
end	



commit		
