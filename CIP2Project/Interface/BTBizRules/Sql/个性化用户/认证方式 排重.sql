set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



/*
 * 存储过程[dbo].[up_Customer_OV3_Interface_AuthStyleQueryByAuthenName]
 *
 * 功能描述: 认证方式查询接口
 *			 
 *
 * Author: Zhang Ying Jie
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-11
 * Remark: BestTone Information Service CO., LTD.
 *
 */
 
ALTER   Procedure [dbo].[up_Customer_OV3_Interface_AuthStyleQueryByAuthenName]
(
	@SPID varchar(8),
	@AuthenName varchar(48),
	@AuthenType varchar(1),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16) out,
	@UserAccount varchar(16) out
) as
	Set @Result = -19999
	Set @CustID = ''
    --用户名
    if(@AuthenType='1')
		Begin
		 select @CustID = a.CustID from CustInfo a where a.status='00' and a.UserName=@AuthenName
		End
    --手机
    else if(@AuthenType='2')
		Begin
		 select @CustID = a.CustID from CustPhone a where a.phonetype='2' and a.phoneclass='2' and a.phone=@AuthenName
		End
    --商旅卡号
    else if(@AuthenType='3')
		Begin
		 select @CustID = a.CustID  from CustTourCard a where a.status='0' and a.CardID=@AuthenName
		End
    --Email
    else if(@AuthenType='4')
		Begin
		 select @CustID = a.CustID from CustInfo a where a.status='00' and a.EmailClass=2 and a.Email=@AuthenName
		End
    else 
        Begin
         select @CustID = a.CustID from  CustAuthenInfo a where a.AuthenName=@AuthenName and a.AuthenType=@AuthenType  
        End
	if(@CustID = '')
    Begin
		Set @Result = -20504
		Set @ErrMsg = '无此用户名'
		return
	End

	
	select @UserAccount=a.CardID from CustTourCard a where a.status='0' and a.CustID = @CustID 
	Set @Result = 0
	
	
SET ANSI_NULLS Off





