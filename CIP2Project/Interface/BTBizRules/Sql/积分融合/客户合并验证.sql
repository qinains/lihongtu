--客户信息平台，积分商城 ,客户合并验证


alter  Procedure [dbo].[up_BT_V2_Interface_IncorporateCustYZ]
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
    @ErrorDescription varchar(256) out  

)
as
   
    Set @Result = 0
    Set @ErrorDescription = ''


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



