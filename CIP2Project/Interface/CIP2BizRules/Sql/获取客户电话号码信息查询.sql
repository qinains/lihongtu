

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_CustPhoneQuery]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_CustPhoneQuery]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * �洢����dbo.up_Customer_V3_Interface_CustPhoneQuery
 *
 * ��������: ��ȡ�ͻ��绰������Ϣ��ѯ
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-7-30
 * Remark:
 *
 */
 
 Create Procedure dbo.up_Customer_V3_Interface_CustPhoneQuery
(

	@CustID varchar(16)
)
as

	select Phone,PhoneClass from CustPhone with(nolock) where CustID = @CustID
	
	
Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go