if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_ScoreDetailInfoQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_ScoreDetailInfoQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_V2_Interface_ScoreDetailInfoQuery
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-12-27
 * Remark:
 *
 */
 
 Create Procedure dbo.up_BT_V2_Interface_ScoreDetailInfoQuery
(
	@CustID varchar(16),
	@UserAccount varchar(16),
	@StartTime dateTime,
	@EndTime dateTime
)
as

	
	select SPID,CustID,UserAccount,Score,ScoreType,EffectiveTime,ExpireTime,UploadTime,Description
	from ScoreInfo
	where CustID = @CustID and UserAccount = @UserAccount  and EffectiveTime between @StartTime and @EndTime 
	order by UploadTime




Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go
