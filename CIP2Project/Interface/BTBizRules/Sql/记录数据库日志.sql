

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_WriteDataLog]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_WriteDataLog]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_V2_Interface_WriteDataLog
 *
 * 功能描述: 
 *			 
 *
 * Author:
 * Company: Linkage Technology CO., LTD.
 * Create: 2007-8-21
 * Remark:
 *
 */
 
 Create Procedure dbo.up_BT_V2_Interface_WriteDataLog
(
	@SPID varchar(8),
	@CustID varchar(16),
	@UserAccount varchar(16),
	@Result int,
	@ErrorDescription varchar(256),
	@PhoneNum varchar(20),
	@InterfaceName varchar(48)
)
as

	Declare @CurrentTime DateTime
	Set @CurrentTime = GetDate()
	
	Insert Into UserExtionInfo
		(SPID, CustID,UserAccount,Result,ErrorDescription,DealTime,PhoneNumber,InterfaceName)
	values(@SPID, @CustID,@UserAccount,@Result,@ErrorDescription,@CurrentTime,@PhoneNum,@InterfaceName)
	
	
	
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
