/****** 对象:  StoredProcedure [dbo].[up_Customer_V3_Interface_IsExistUser]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/*
 * 存储过程dbo.up_Customer_V3_Interface_IsExistUser
 *
 * 功能描述: 注册用户验证
 *			 
 *
 * Author: zhoutao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-18
 * Remark:
 *
 */

 
 Create  Procedure [dbo].[up_Customer_V3_Interface_IsExistUser]
(
	@username varchar(30),
	@Result int output
)
as
	set @Result = -19999
	declare @ErrMsg varchar(256)

	if  exists ( select 1 From CustInfo with(nolock) Where UserName = @username )
	Begin
		set @Result = -20504
		set @ErrMsg = '用户信息已存在,不能注册'
		return
	End	
				

	Set @Result =0
	
Set QUOTED_IDENTIFIER Off



