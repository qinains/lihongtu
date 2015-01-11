alter Procedure [dbo].[up_BT_V5_Interface_InsertAnswer]
(
	@CustID varchar(16),
	@QuestionID int,
    @Answer varchar(100),
    @Result int out,
    @ErrorDescription varchar(256) out
)
as
     if  not exists (select 1 from CustInfo where CustID=@custID )
    begin
      set @Result=-39999
      set @ErrorDescription='CustID不存在！'
      return 
    end
    if  exists (select 1 from Answer where CustID=@custID and QuestionID=@QuestionID)
    begin
      set @Result=-19999
      set @ErrorDescription='此用户的密码答案已经存在！'
      return 
    end

	insert into Answer(CustID,QuestionID,Answer,DealTime)
       values(@CustID,@QuestionID,@Answer,getdate())
    If( @@error <> 0)
	begin
		set @Result=-29999
        set @ErrorDescription='数据添加不成功！'
     
        return  											
	end
    set @Result=0
    set @ErrorDescription='成功！'


set ANSI_NULLS Off
set QUOTED_IDENTIFIER Off
go

