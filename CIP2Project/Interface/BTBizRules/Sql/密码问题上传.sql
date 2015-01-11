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
      set @ErrorDescription='CustID�����ڣ�'
      return 
    end
    if  exists (select 1 from Answer where CustID=@custID and QuestionID=@QuestionID)
    begin
      set @Result=-19999
      set @ErrorDescription='���û���������Ѿ����ڣ�'
      return 
    end

	insert into Answer(CustID,QuestionID,Answer,DealTime)
       values(@CustID,@QuestionID,@Answer,getdate())
    If( @@error <> 0)
	begin
		set @Result=-29999
        set @ErrorDescription='������Ӳ��ɹ���'
     
        return  											
	end
    set @Result=0
    set @ErrorDescription='�ɹ���'


set ANSI_NULLS Off
set QUOTED_IDENTIFIER Off
go

