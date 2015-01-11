set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go














/*
 * 存储过程:up_Customer_V3_Interface_IsExistsAuthEmail
 *
 * 功能描述: 
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-11-04
 * Remark:
 *
 */
 
 alter Procedure [dbo].[up_Customer_V3_Interface_IsExistsAuthEmail]
(
	@Result int out,
	@CustID varchar(16),
	@ErrMsg varchar(255) out
)
as
	set @Result = -22500
	set @ErrMsg = ''

	if not exists ( select 1 From CustInfo Where CustID = @CustID and EmailClass <> '1')
	Begin
		set @Result = -20504
		set @ErrMsg = ''
		return
	End

	set @Result = 0
	set @ErrMsg = '该用户已经存在认证邮箱,不能修改'
	
Set QUOTED_IDENTIFIER Off













