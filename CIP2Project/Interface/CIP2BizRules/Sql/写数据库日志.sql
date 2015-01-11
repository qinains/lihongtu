

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_WriteDataCustAuthenLog]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_WriteDataCustAuthenLog]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程: dbo.up_Customer_V3_Interface_WriteDataCustAuthenLog
 *
 * 功能描述: 写数据库日志
 *			 
 *
 * Author: zhou tao
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-28
 * Remark:
 *
 */
 
create procedure [dbo].[up_Customer_V3_Interface_WriteDataCustAuthenLog] (
 
 	@SPID varchar(8),
 	@CustID varchar(16),
	@ProvinceID varchar(2),
 	@AuthenType varchar(2),
 	@AuthenName varchar(48),
 	@LoginType varchar(2),
 	@Results int,
 	@Description varchar(256)
 ) as
 
  declare	@monthen varchar(2)
declare @DealTime dateTIme 
set @DealTime =getdate()
 select @monthen=right(100 + Month(@DealTime),2)
 
 declare @CustType varchar(2)
 
 select @CustType = CustType from CustInfo with(nolock) where CustID=@CustID
 
 Declare @ExeSql nvarchar(4000) 
 Set @ExeSql = ' insert into  CustAuthenLog_'+@monthen+ ' (CustID,CustType,ProvinceID,AuthenType,AuthenName,LoginType,Result,SPID,DealTime,Description) values(''' + @CustID + ''',''' + @CustType + ''',''' +@ProvinceID + ''','''+ @AuthenType + ''','''+@AuthenName + ''','''+ @LoginType+''','+convert(varchar,@Results)+','''+@SPID+''','''+convert(varchar,@DealTime)+''','''+@Description+''')'
--select  @ExeSql    
   
Exec(@ExeSql)      
   
	
Set QUOTED_IDENTIFIER Off
go
Set ANSI_NULLS Off
go