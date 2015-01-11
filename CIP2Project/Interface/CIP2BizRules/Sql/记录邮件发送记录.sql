

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[up_Customer_V3_Interface_InsertEmailSendHistory]') and 

OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[up_Customer_V3_Interface_InsertEmailSendHistory]
GO



Set QUOTED_IDENTIFIER On
go
Set ANSI_NULLS On
go


/*
 * 存储过程up_Customer_V3_Interface_InsertEmailSendHistory
 *
 * 功能描述: 邮件发送成功后记入邮件发送历史表
 *			 
 *
 * Author: 苑峰
 * Company: Linkage Technology CO., LTD.
 * Create: 2010-0303
 * Remark:
 *
 */
 
Create proc [dbo].[up_Customer_V3_Interface_InsertEmailSendHistory]
(
	@CustID	Varchar(16),
	@OPType	varchar(1),
	@Message Text,
	@AuthenCode varchar(6),
	@Result	int,
	@Email Varchar(100),	
	@SubjectName Varchar(100),
	@Description	varchar(40)
)
as

	declare @DealTime DATETIME
	Set @DealTime = getdate()
	declare @SequenceID bigint
	declare @ProvinceID	varchar(2)
	select @ProvinceID = ProvinceID from Custinfo with(nolock) where custID = @CustID
	if(@@Rowcount <1 ) --如果找不到该客户则CUstID 可能为UserName
	begin
		select @CustID = CustID, @ProvinceID = ProvinceID from Custinfo with(nolock) where UserName = @CustID
	end
	
	
	
	begin tran
	--插入原始表获取当前@SequenceID
	insert into SendEmailRecord
		(CustID,ProvinceID,OPType,Message,AuthenCode,Result,Email,SubjectName,DealTime,Description,NotifyCount) 
	values
		(@CustID,@ProvinceID,@OPType,@Message,@AuthenCode,@Result,@Email,@SubjectName,@DealTime,@Description,1) 
	--取出当前@SequenceID
	Set @SequenceID = @@Identity  

	--插入历史记录表
	INSERT INTO SendEmailRecordHistory
	(
		SequenceID,
		CustID,
		ProvinceID,
		OPType,
		Message,
		AuthenCode,
		Result,
		Email,
		SubjectName,
		DealTime,				
		Description,
		NotifyCount
	)
	VALUES
	(
		@SequenceID,
		@CustID,
		@ProvinceID,
		@OPType,
		@Message,
		@AuthenCode,
		@Result,
		@Email,	
		@SubjectName,
		getdate(),
		@Description,
		1
	)

	delete from SendEmailRecord where SequenceID = SequenceID

commit