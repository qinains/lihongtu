USE [besttone]
GO
/****** 对象:  StoredProcedure [dbo].[IncorporateCustYZ_BTForCRM]    脚本日期: 03/13/2009 06:54:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--客户信息平台，CRM客户合并


ALTER  Procedure [dbo].[up_v5_BestTone_IncorporateCustYZ_BTForCRM]
(
       
  --  @SPID varchar(8),	
    @IncorporatedCRMID varchar(16),	
    @SavedCRMID varchar(16),	
    @ProvinceID varchar(2),
--    @IncorporatedCustType varchar(2),	
--    @SavedCustType varchar(2),	

	
    @Result int out,	
    @ErrorDescription varchar(256) out,	
    @IncorporatedCustID varchar(16) out,	
    @SavedCustID varchar(16) out

)
as
   
    DECLARE @CustID  varchar(16) 
    Set @Result = -19999
    Set @ErrorDescription = ''
    set @IncorporatedCustID=''
    set @SavedCustID=''


--select  * from dbo.CustInfo where outerID ='55000040743' and SysId='1301'

if not exists ( select  1 from dbo.CustInfo where outerID =@IncorporatedCRMID and SysId=@ProvinceID+'01'  )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '要被合并的客户卡号不存在！' 

    return 
end 
select  @CustID=CustID from dbo.CustInfo where outerID =@IncorporatedCRMID and SysId=@ProvinceID+'01'  
if not exists ( select  1 from dbo.CustUserInfo where CustID=@CustID )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '要被合并的客户卡号不存在！' 

    return 
end 
set  @IncorporatedCustID=@CustID


if not exists ( select 1 from dbo.CustInfo where  outerID =@SavedCRMID and SysId=@ProvinceID+'01'  )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '合并后的客户卡号不存在！' 
  
    return 
end 
select  @CustID=CustID from dbo.CustInfo where outerID =@SavedCRMID and SysId=@ProvinceID+'01'  

if not exists ( select 1 from dbo.CustUserInfo where CUSTID =@CustID )
begin  
    Set @Result = -22500
    Set @ErrorDescription = '无合并后的用户！' 
  
    return 
end 

set @SavedCustID=@CustID
Set @Result = 0


