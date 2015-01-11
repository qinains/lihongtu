USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_Customer_OV3_Interface_MUserAuthV2]    脚本日期: 11/02/2009 15:03:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




ALTER procedure [dbo].[up_Customer_OV3_Interface_MUserAuthV2] (
 	@SPID varchar(8),
 	@AuthenName varchar(256),
 	@AuthenType varchar(2),
 	@AuthenRecords text,  
    @ProvinceID varchar(2),
 	@Result int out,
 	@ErrMsg varchar(256) out,
 	@CustID varchar(16) out,
 	@UserAccount varchar(16) out,
 	@CustType varchar(2) out,
 	@UProvinceID varchar(2) out,
 	@SysID varchar(8) out,
     @outerid varchar(30) out,
 	@AreaID varchar(3) out,
     @RealName varchar(20) out,
 	@UserName varchar(20) out,
 	@NickName varchar(20) out,
    @dealType varchar(1) out
 
 ) as
set @CustID = ''
	set @Result = -19999
    set @UserAccount=''
	set @ErrMsg = ''
	set @AreaID = ''
    set @RealName=''
    set @UserName=''
    set @NickName=''
    set @outerid=''
    set @SysID=@ProvinceID+'999999'
	declare @TmpPwd1 varchar(50)
    declare @TmpPwd2 varchar(50)
	set @TmpPwd1 = ''
	set @TmpPwd2 = ''
	declare @AuthStatus varchar(2)
	set @AuthStatus=''
    declare @Status as varchar(2)
    declare @AuthenType1 varchar(2)

    declare @DealTime DateTime
    set @DealTime=getdate()
		



	Declare @AuthenDoc int

	Declare @Authen Table
	(
		AuthenName varchar(20),
    	AuthenType varchar(2),
        areaid Varchar(3)       
	)

	if( @AuthenRecords is not null )
	Begin
		-- 解析请求数据
		-- 解析绑定电话列表关系
		EXECUTE sp_xml_preparedocument @AuthenDoc OUTPUT, @AuthenRecords


		Insert Into @Authen( AuthenName, AuthenType ,areaid)
		SELECT AuthenName, AuthenType , areaid
			
			FROM OpenXML( @AuthenDoc, '/ROOT/AuthenRecord' , 3 ) 
			With( AuthenName	Varchar(20), AuthenType varchar(2),areaid varchar(3) )
	End
   else
    begin
       Set @Result = -20504
		     Set @ErrMsg = '没有用户信息1！'
			 return
    end 


   select top 1 @AreaID=areaid,@outerid=AuthenName from  @Authen where AuthenType='0'
  if(@@RowCount = 0)
  begin
    Set @Result = -20504
		     Set @ErrMsg = '没有用户信息2！'
			 return
  end 

   select top 1 @CustID=custid from custinfo c with(nolock)
     where SourceSPID=@SysID and  c.OuterID= @outerid
     if(@@RowCount = 0)
       begin
         set @dealType='0'
	     Exec up_BT_OV3_Interface_InsertBrandCust @ProvinceID, @outerid,'90','',@AuthenName,'5',@AreaID,'','',
			'2','0',null,'','','','','','',@DealTime,	'06','2',@UserAccount out ,@CustID out,@Result out,@ErrMsg out
		  if(@@error <> 0)
		  Begin			
             Set @Result = -20504
		     Set @ErrMsg = '内部错误1！'
			 return
		  End 
       end 
      else
      begin  
        set @dealType='1'  
        update custinfo set AreaID=@AreaID,RealName=@AuthenName
         where custID=@CustID and len(RealName)>8
         if(@@error<>0)
          begin
            Set @Result = -20504
		    Set @ErrMsg = '内部错误2！'
			return
          end 
      end
-----------------------------------------------------

      select @CustType=CustType,@UProvinceID=ProvinceID,@AreaID=AreaID,@SysID=SourceSPID,@Status=Status,@TmpPwd1=VoicePwd,@TmpPwd2=WebPwd,
			@RealName=RealName,@NickName=NickName,@UserName=UserName,@outerid=outerid from  custinfo with(nolock) where custID=@CustID
      if(@@RowCount = 0)
			Begin
				Set @Result = -20504
				Set @ErrMsg = '无此用户4'+@CustID
				return
			End  

    
	Set @Result=0


 