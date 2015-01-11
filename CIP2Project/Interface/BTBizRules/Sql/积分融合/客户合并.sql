--客户信息平台，积分商城 ,客户合并

--drop Procedure [dbo].[up_BT_V2_Interface_IncorporateCust]
alter  Procedure [dbo].[up_BT_V2_Interface_IncorporateCust]
(
        --SPID	请求方在认证鉴权系统登记的SPID	String	8
    @SPID varchar(8),
	--IncorporatedCustID	被合并的用户卡号 CUSTID (b)
    @IncorporatedCustID varchar(16),
	--SavedCustID	合并后的客户ID	CUSTID  (a+b)
    @SavedCustID varchar(16),
	--ExtendField	保留字段	String	
    @ExtendField varchar(16),

	--Result	0:成功	-22500：系统内部错误	
    @Result int out,	
	--ErrorDescription	错误描述	String	256
    @ErrorDescription varchar(256) out,
	--SavedCustID	合并后的客户ID	String	16
    @SavedCustID_out varchar(16) out,
	--SavedUserAccount	合并后的用户帐号	String	16
    @SavedUserAccount varchar(16) out

)
as
   
    Set @Result = 0
    Set @ErrorDescription = ''
    set @SavedCustID_out=''
    set @SavedUserAccount=''

Declare @SavedUser_CustID varchar(16)
Declare @Incorporated_CustID varchar(16)
--Declare @UserAccount varchar(16)



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




set @SavedUser_CustID=@SavedCustID 
set @Incorporated_CustID=@IncorporatedCustID 

--备份表CustUserInfo数据到CustUserInfoHistory
insert into CustUserInfoHistory
(CustID,SPID,InnerCardID,UserAccount,CustPersonType,
RegistrationSource,RegistrationType,RegistrationDate,[Status],IsHaveCard,
DealTime,[Description],Reason,HistoryDescription,SysID,OuterID)

 select CustID,SPID,InnerCardID,UserAccount,
CustPersonType,RegistrationSource,RegistrationType,RegistrationDate,
[Status],'',DealTime,[Description],'','',SysID,OuterID from CustUserInfo  where CustID=@Incorporated_CustID
If( @@error <> 0)
begin
	rollback		
	return 		
end



--修改表CustUserInfo，将多个的帐户归并到同一个上,
update dbo.CustUserInfo set CustID=@SavedUser_CustID  where CustID=@Incorporated_CustID 
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
RegistrationDate,Status,getdate(),@SavedUser_CustID,@SavedCustID,'',''
from dbo.CustInfo where CustID=@Incorporated_CustID 
If( @@error <> 0)
begin
	rollback  
	return 									
end


--删除多余的数据


delete from CustInfo where CustID=@Incorporated_CustID 
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
from CustExtend  where CustID=@Incorporated_CustID
If( @@error <> 0)
begin
	rollback   
	return 										
end




--删除客户扩展信息表


delete from CustExtend  where CustID= @Incorporated_CustID 
If( @@error <> 0)
begin
        rollback	
        return	
end											


set @SavedCustID_out=@SavedUser_CustID
set @SavedUserAccount=@SavedUser_CustID

commit		
