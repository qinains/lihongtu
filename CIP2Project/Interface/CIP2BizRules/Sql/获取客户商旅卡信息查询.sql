

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_CustTourCardIDQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_CustTourCardIDQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_Customer_V3_Interface_CustTourCardIDQuery
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
 
 Create Procedure dbo.up_Customer_V3_Interface_CustTourCardIDQuery
(

	@CustID varchar(16)
)
as

	select CardID,CardType,InnerCardID from CustTourCard with(nolock) where CustID = @CustID
	
	
Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go