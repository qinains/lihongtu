set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
/*
 * 存储过程[dbo].[up_Customer_V3_Interface_InsertPwdResetLog]
 *
 * 功能描述: 创建客户密码找回记录日志
 *			 
 *
 * Author: Zhang Ying Jie
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-20
 * Remark: BestTone Information Service CO., LTD.
 *
 */
ALTER PROCEDURE [dbo].[up_Customer_V3_Interface_InsertPwdResetLog]
	(
     @CustID varchar(16),
     @CustType varchar(2),
     @PwdType varchar(1),
     @OPType varchar(1),
     @AuthenNumber varchar(100),
     @Result int, 
     @SPID varchar(8), 
     @IPAddress varchar(15),
     @Description  varchar(40),
	 @ResultOut int out,
	 @ErrMsg varchar(40) out
)
AS
BEGIN
    Set @ResultOut= -19999
	Set @ErrMsg = ''
	SET NOCOUNT ON;
INSERT INTO [PwdResetLog]
           ([CustID]
           ,[CustType]
           ,[PwdType]
           ,[OPType]
           ,[AuthenNumber]
           ,[Result]
           ,[SPID]
           ,[IPAddress]
           ,[DealTime]
           ,[Description])
     VALUES
           (@CustID
           ,@CustType
           ,@PwdType
           ,@OPType
           ,@AuthenNumber
           ,@Result
           ,@SPID
           ,@IPAddress
           ,getdate()
           ,@Description)
	if(@@RowCount = 1)
		set @ResultOut = 0							
End