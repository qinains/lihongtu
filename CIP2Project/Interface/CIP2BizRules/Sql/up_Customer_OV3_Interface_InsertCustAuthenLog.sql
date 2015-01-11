set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go











/*
 * 存储过程dbo.up_Customer_OV3_Interface_InsertCustAuthenLog
 *
 * 功能描述: 
 *			 
 *
 * Author: tongb
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-19
 * Remark:
 *
 */

alter  Procedure [dbo].[up_Customer_OV3_Interface_InsertCustAuthenLog]
(
@CustID varchar(16),
@AuthenType varchar(2),
@AuthenName varchar(30),
@LoginType  varchar(1),
@Results    int,
@SPID       varchar(8),
@DealTime   datetime,
@Description varchar(40) 
	
) as
	
begin
 declare @monthen varchar(2)
 select @monthen=month(getdate())
 Declare @ExeSql nvarchar(4000) 
 Set @ExeSql = ' insert into  CustAuthenLog_'+@monthen+ ' (CustID,AuthenType,AuthenName,LoginType,Results,SPID,DealTime,Description) values(''' + @CustID + ''',''' + @AuthenType + ''','''+@LoginType+''','+@Results+','''+@SPID+''','''+@DealTime+''','''+@Description+''')'
 Exec(@ExeSql)      
   
end




