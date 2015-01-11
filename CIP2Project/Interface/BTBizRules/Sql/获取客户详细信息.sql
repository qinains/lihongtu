if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V2_Interface_GetCustDetailInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V2_Interface_GetCustDetailInfo]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


/*
 * 存储过程[dbo].[up_BT_V2_Interface_GetCustDetailInfo]
 *
 * 功能描述: 获取客户详细信息，			 
 *
 * Author: Li Ye
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-01-02
 * Remark:
 *
 */
create proc [dbo].[up_BT_V2_Interface_GetCustDetailInfo]
(
 @CustID varchar(16)
)

AS
Begin
	select CustInfo.UserAccount,RealName,Email,CustContactTel from CustInfo,CustExtend where CustInfo.CustID = CustExtend.CustID and CustInfo.CustID = @CustID

end

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

