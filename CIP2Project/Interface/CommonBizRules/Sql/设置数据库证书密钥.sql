

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_BT_V3_Interface_InsertCerFile]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_BT_V3_Interface_InsertCerFile]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程dbo.up_BT_V3_Interface_InsertCerFile
 *
 * 功能描述: 
 *			 
 *
 * Author: 
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-5
 * Remark:
 *
 */

 
 Create Procedure dbo.up_BT_V3_Interface_InsertCerFile
(
	@SPID varchar(8),
	@UserName varchar(100),
	@CerPassword varchar(100),
	@btCer image,
	@FilePath varchar(512),
	@CerType int 
)
as
	declare @SequenceID bigint

	select @SequenceID=SequenceID from CAInfo where SPID=@SPID and CerType=@CerType and status=0
	--如果存在则更新，否则插入
	if(@@RowCount >0)
		Begin
			begin tran
				Update	CAInfo Set status=1 where SequenceID = @SequenceID
				Insert into CAInfo 
					(SPID, CerInfo, FilePath, CerType, CerPassword, CerUserName, Status)
				values (@SPID, @btCer, @FilePath, @CerType, @CerPassword, @UserName, 0)
			commit
		End
	else 
		Begin
				Insert into CAInfo 
					(SPID, CerInfo, FilePath, CerType, CerPassword, CerUserName, Status)
				values(@SPID, @btCer, @FilePath, @CerType, @CerPassword, @UserName, 0)
		End
	