

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_CustExtendInfoQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_CustExtendInfoQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程up_Customer_V3_Interface_CustExtendInfoQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-7-30
 * Remark:
 *
 */
 
 Create Procedure dbo.up_Customer_V3_Interface_CustExtendInfoQuery
(
	@SPID varchar(8),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16),
	@Birthday datetime out,
	@EduLevel varchar(1) out,
	@Favorite varchar(256) out,
	@IncomeLevel varchar(1) out
)
as
	set @Result = -22500
	set @ErrMsg = ''
	set @Birthday = ''
	set @EduLevel = ''
	set @Favorite = ''
	set @IncomeLevel = ''


	if not exists ( select 1 From CustExtendInfo Where CustID = @CustID )
	Begin
		set @Result = -20504
		set @ErrMsg = '无此帐号'
		return
	End

	select @BirthDay=BirthDay, @EduLevel=EduLevel, @Favorite=Favorite, @IncomeLevel=IncomeLevel from CustExtendInfo with(nolock) where CustID = @CustID

	if(@@Rowcount <> 1)
	begin
		set @Result = -19999
		set @ErrMsg = '未知错误'
		return
	end
		
	set @Result = 0
	
	
Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go
