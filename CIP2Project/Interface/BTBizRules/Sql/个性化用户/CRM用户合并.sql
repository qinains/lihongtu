--CRM用户合并

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
    Set @ErrorDescription = '要被合并的客户卡号不存在！'  
    return 
end 
if not exists ( select 1 from dbo.CustInfo where CUSTID =@SavedCustID  )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '合并后的客户卡号不存在！'   
    return 
end 
if not exists ( select 1 from dbo.CustUserInfo where CUSTID =@SavedCustID )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '无合并后的用户！'   
    return 
end 
if not exists ( select 1 from dbo.CustUserInfo where CUSTID =@IncorporatedCustID )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '无要被合并的客户！'   
    return 
end 

begin tran

--备份表CustUserInfo数据到CustUserInfoHistory
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
 

--修改表CustUserInfo，将多个的帐户归并到同一个上,
update dbo.CustUserInfo set CustID=@SavedCustID, OuterID= @SavedCRMID,UserAccount=@SavedAccount,
CustType=@SavedCustType where CustID=@IncorporatedCustID 
If( @@error <> 0)
begin
	rollback
	return						
end


--将要删除的数据备份到历史记录表中
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

--删除多余的数据

delete from CustInfo where CustID=@IncorporatedCustID 
If( @@error <> 0)
begin
	rollback  
	return									
end

--备份客户扩展信息，将数据插入到表CustExtendHistory
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


--删除客户扩展信息表

delete from CustExtend  where  CustID=@IncorporatedCustID
If( @@error <> 0)
begin
        rollback	
        return	
end	



commit		
