set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



/*
 * 存储过程[dbo].[up_Customer_V3_Interface_CheckCIPTicket]
 *
 * 功能描述: 客户信息平台票据解读接口
 *			 
 *
 * Author: Zhang Ying Jie
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-08-03
 * Remark: BestTone Information Service CO., LTD.
 *
 */
ALTER PROCEDURE [dbo].[up_Customer_V3_Interface_CheckCIPTicket]
	(
     @SPID varchar(50),
     @Ticket varchar(20),
     @CustID varchar(16) out,
     @RealName varchar(50) out,
     @UserName varchar(30) out,
     @NickName varchar(30) out, 
	 @Result int out,
	 @ErrMsg varchar(256) out
)
AS
BEGIN
    Set @Result = -19999
	Set @ErrMsg = ''
	SET NOCOUNT ON;
if not exists ( select 1 From CIPTicket Where Ticket = @Ticket)
    Begin
		set @Result = -30013
		set @ErrMsg = '票据不存在'
		return
	End
BEGIN
SELECT @CustID=CustID,@RealName=RealName,@UserName=UserName,@NickName=NickName 
FROM CIPTicket
Where Ticket = @Ticket
End
BEGIN
INSERT INTO CIPTicketHistory
           ([Ticket]
           ,[GenerateTime]
           ,[SPID]
           ,[CustID]
           ,[RealName]
           ,[UserName]
           ,[NickName]
           ,[Description]
           ,[DealTime])
    SELECT [Ticket]
      ,[GenerateTime]
      ,[SPID]
      ,[CustID]
      ,[RealName]
      ,[UserName]
      ,[NickName]
      ,[Description]
      ,getdate()
  FROM [CIPTicket]
  where Ticket = @Ticket
Delete
FROM CIPTicket
Where Ticket = @Ticket
	if(@@RowCount = 1)
		set @Result = 0		
END					
End

