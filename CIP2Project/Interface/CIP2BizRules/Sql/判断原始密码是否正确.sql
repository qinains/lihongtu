/****** 对象:  StoredProcedure [dbo].[up_Customer_V3_Interface_OldPwdIsRight]     ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





/*
 * 存储过程: dbo.up_Customer_V3_Interface_OldPwdIsRight
 *
 * 功能描述: 判断原始密码是否正确
 *			 
 *
 * Author: zhoutao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-19
 * Remark:
 *
 */

 
 Create  Procedure [dbo].[up_Customer_V3_Interface_OldPwdIsRight]
(
	@CustID varchar(16),
	@OldPwd varchar(128),
	@PwdType varchar(1),
	@Result int output,
	@ErrMsg varchar(256) output
)
as
	set @Result = -19999

	if(@PwdType = '1')
	begin
		if not exists ( select 1 From CustInfo with(nolock) Where CustID=@CustID and VoicePwd=@OldPwd )
		Begin
			set @Result = -20504
			set @ErrMsg = '原始密码错误'
			return
		End	
	end

	if(@PwdType = '2')
	begin
		if not exists ( select 1 From CustInfo with(nolock) Where CustID=@CustID and WebPwd=@OldPwd )
		Begin
			set @Result = -20504
			set @ErrMsg = '原始密码错误'
			return
		End	
	end			

	Set @Result =0

Set QUOTED_IDENTIFIER Off



