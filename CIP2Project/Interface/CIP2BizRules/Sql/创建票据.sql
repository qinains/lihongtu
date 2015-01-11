set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

/*
 * 存储过程[dbo].[up_Customer_V3_Interface_InsertCIPTicket]
 *
 * 功能描述: 创建票据信息
 *			 
 *
 * Author: Zhang Ying Jie
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-03
 * Remark: BestTone Information Service CO., LTD.
 *
 */
ALTER PROCEDURE [dbo].[up_Customer_V3_Interface_InsertCIPTicket]
	(
     @Ticket varchar(20),
     @SPID varchar(50),
     @CustID varchar(16),
     @RealName varchar(50),
     @UserName varchar(30),
     @NickName varchar(30), 
     @Description varchar(256),
	 @Result int out,
	 @ErrMsg varchar(40) out
)
AS
BEGIN
    Set @Result = -19999
	Set @ErrMsg = ''
	SET NOCOUNT ON;
    INSERT INTO [CIPTicket]
           ([Ticket]
           ,[GenerateTime]
           ,[SPID]
           ,[CustID]
           ,[RealName]
           ,[UserName]
           ,[NickName]
           ,[Description])
     VALUES
           (@Ticket
           ,getdate()
           ,@SPID
           ,@CustID
           ,@RealName
           ,@UserName
           ,@NickName
           ,@Description)
	if(@@RowCount = 1)
		set @Result = 0							
End










