
/****** 对象:  StoredProcedure [dbo].[up_BT_ov3_Interface_IncorporateCust_BTForCRM2]   
 脚本日期: 08/25/2009 10:28:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CRM用户合并

create  Procedure [dbo].[up_BT_ov3_Interface_IncorporateCust_BTForCRM2]
(
    @ProvinceID varchar(2),
    @IncorporatedCustID varchar(16),

    @SavedCustID varchar(16),
	

    @OutProvinceID varchar(2) out ,	
    @Result int out,	
    @ErrorDescription varchar(256) out--,
   -- @CustID varchar(16) out
)
as

set @OutProvinceID=@ProvinceID
set @Result=-199999
set @ErrorDescription='未知错误'

select 1 from CustInfo with(nolock) where CUSTID =@IncorporatedCustID and Status='00'
if(@@RowCount = 0)
begin  
    Set @Result = -22500
    Set @ErrorDescription = '要被合并的客户卡号不存在或状态有误！'  
    return 
end 
select 1 from dbo.CustInfo where CUSTID =@SavedCustID and Status='00'
if(@@RowCount = 0)
begin  
    Set @Result = -22500
    Set @ErrorDescription = '合并后的客户卡号不存在或状态有误！'   
    return 
end 


begin tran

--备份表CustUserInfo数据到CustInfoHistory
insert into CustInfoHistory
(CustID,CustType,VoicePwd,WebPwd
,CertificateType,CertificateCode,RealName,ProvinceID,AreaID,
CustLevel,Sex,BirthDay,EduLevel,Favorites,InComeLevel,Email,NickName,UserName,
RegistrationSource,RegistrationDate,Status,DealTime,
IncorporateCustID,ReasonType,OuterID,HistoryDescription)
select c.CustID,c.CustType,c.VoicePwd,c.WebPwd,
c.CertificateType,c.CertificateCode,c.RealName,c.ProvinceID,c.AreaID,
c.CustLevel,c.Sex,e.BirthDay,e.EduLevel,e.Favorite,e.IncomeLevel,c.Email,c.NickName,c.UserName,
c.RegistrationSource,c.CreateTime,c.Status,c.DealTime,@SavedCustID,'1',c.OuterID,''
from custinfo c with(nolock),CustExtendInfo e with(nolock) 
where c.custid=@IncorporatedCustID and c.custid=e.custid
If( @@error <> 0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '备份数据出错！'       
	rollback		
	return 		
end
 
update CustAuthenInfo set CustID=@SavedCustID where CustID=@IncorporatedCustID
If( @@error <> 0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '更新认证信息出错！'       
	rollback  
	return									
end 

update CustPhone set CustID=@SavedCustID where CustID=@IncorporatedCustID
If( @@error <> 0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '更新电话认证信息出错！'       
	rollback  
	return									
end 

insert into PaymentAccountHistory(CustID,ProvinceID,SouceSPID,InstitutionID,PaymentAccount,InstitutionType,Status,Description)
select CustID,ProvinceID,SouceSPID,InstitutionID,PaymentAccount,InstitutionType,Status,Description
from PaymentAccount with(nolock) where custid=@IncorporatedCustID
If( @@error <> 0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '备份客户支付账号信息出错！'       
	rollback  
	return									
end 

update PaymentAccount set CustID=@SavedCustID where CustID=@IncorporatedCustID
If( @@error <> 0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '更新客户支付账号信息出错！'       
	rollback  
	return									
end 

declare @Email varchar(256)
declare @UserName varchar(30)
declare @EmailClass int 
declare @CustType varchar(2)
declare @CreateTime DateTime
declare @Dealtime DateTime

select @Email=email,@UserName=UserName,@EmailClass=EmailClass,@CreateTime=CreateTime,@Dealtime=Dealtime from custinfo with(nolock) where  CustID=@IncorporatedCustID
 if(@@RowCount>0)      
    begin     
      if(@UserName is not null and @UserName<>'' )
      begin
          insert into CustAuthenInfo(CustID,CustType,ProvinceID,AuthenName,Authentype,CreateTime,Dealtime)
            values( @SavedCustID,@CustType,@ProvinceID,@UserName,'1',@CreateTime,@Dealtime)
          If( @@error <> 0)
			begin
				Set @Result = -22500
				Set @ErrorDescription = '更新用户认证方式出错！'       
				rollback  
				return									
			end 
      end
      if(@EmailClass=2 and @Email is not null and @Email<>'')
      begin
         insert into CustAuthenInfo(CustID,CustType,ProvinceID,AuthenName,Authentype,CreateTime,Dealtime)
            values( @SavedCustID,@CustType,@ProvinceID,@Email,'4',@CreateTime,@Dealtime)
          If( @@error <> 0)
			begin
				Set @Result = -22500
				Set @ErrorDescription = '更新邮件认证方式出错！'       
				rollback  
				return									
			end 
      end
    end

update CustTourCard set custID=@SavedCustID where CustID=@IncorporatedCustID
if(@@error<>0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '更新卡信息出错！'       
	rollback  
	return				
end

--删除多余的数据
delete from CustExtendInfo where CustID=@IncorporatedCustID
If( @@error <> 0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '删除扩展信息出错！'       
	rollback  
	return									
end 
delete from AddressInfo  where CustID=@IncorporatedCustID
If( @@error <> 0)
begin
    Set @Result = -22500
    Set @ErrorDescription = '删除地址信息出错！'       
	rollback  
	return									
end 
delete from CustInfo where CustID=@IncorporatedCustID 
If( @@error <> 0)
begin
     Set @Result = -22500
    Set @ErrorDescription = '删除用户信息出错！'       
	rollback  
	return									
end


set @Result=0
set @ErrorDescription=''

commit		
