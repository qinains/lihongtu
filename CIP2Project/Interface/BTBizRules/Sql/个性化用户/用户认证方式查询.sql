set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



/*
 * 存储过程[dbo].[up_Customer_OV3_Interface_UserAuthStyleQuery]
 *
 * 功能描述: 用户认证方式查询接口
 *			 
 *
 * Author: Zhang Ying Jie
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-03
 * Remark: BestTone Information Service CO., LTD.
 *
 */

ALTER   procedure  [dbo].[up_Customer_OV3_Interface_UserAuthStyleQuery]
(
@SPID varchar(8),
@UserAccount varchar(16),
@Result int out,
@ErrMsg varchar(256) out
)

AS
BEGIN

SET @Result = 0
SEt @ErrMsg = ''

declare @CustID varchar(16)
select @CustID = a.CustID  from CustTourCard a where a.status='0' and a.CardID=@UserAccount
if(@CustID = '')
Begin
	Set @Result = -20504
	Set @ErrMsg = '用户帐号不存在'
	return
End

 --用户名
 select a.CustID,a.UserName as AuthenName,'1' as AuthenType from CustInfo a where a.status='00' and a.Custid=@CustID
 union 
 --手机
 select a.CustID,a.phone as AuthenName,'2' as AuthenType from CustPhone a where a.phonetype='2' and a.phoneclass='2' and a.Custid=@CustID
 union
 --商旅卡号
 select a.CustID,a.CardID as AuthenName ,'3' as AuthenType from CustTourCard a where a.status='0' and  a.Custid=@CustID
 union
 --Email
 select a.CustID,a.Email as AuthenName,'4' as AuthenType from CustInfo a where a.status='00' and a.EmailClass=2 and a.Custid=@CustID
 union
 select a.CustID ,a.AuthenName, a.AuthenType from  CustAuthenInfo a where a.Custid=@CustID

END







