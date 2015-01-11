if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_GetBoundPhone]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_GetBoundPhone]
GO




set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


/*
 * 存储过程dbo.up_BT_V2_Interface_GetBoundPhone
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-12-26
 * Remark:
 *
 */
 
create Procedure [dbo].[up_BT_V2_Interface_GetBoundPhone]
(
	@CustID varchar(16),
	@UserAccount varchar(16)
)
as
	select top 5 BoundPhoneNumber from BoundPhone where CustID = @CustID and UserAccount = @UserAccount and  order by DealTime


set ANSI_NULLS Off
set QUOTED_IDENTIFIER Off
go

