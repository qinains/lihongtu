set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go








/*
 * 存储过程dbo.up_BT_V2_Interface_UserAuthV2
 *
 * 功能描述: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2008-8-31
 * Remark:
 *
 */
 
alter  Procedure [dbo].[up_BT_V2_Interface_UserAuthV2]
(
	@SPID varchar(8),
	@AuthenName varchar(16),
	@AuthenType varchar(16),
	@Pwd varchar(100),
    @authenClass varchar(1) ,--1,CustAuthenInfo;2,CustPhone ;3,CustTourCard ;4,CustInfo

	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16) out,
	@UserAccount varchar(16) out,
	@CustType varchar(2) out,
	@UProvinceID varchar(2) out,
	@SysID varchar(4) out,
	@AreaID varchar(3) out
) as
	set @CustID = ''
	set @Result = -19999
    set @UserAccount=''
	set @ErrMsg = ''
	set @AreaID = ''
	declare @TmpPwd1 varchar(50)
    declare @TmpPwd2 varchar(50)
	set @TmpPwd1 = ''
	set @TmpPwd2 = ''
	declare @AuthStatus varchar(2)
	set @AuthStatus=''
    declare @Status as varchar(2)
		

      if(@authenClass='1')
      begin 
        select @CustID=custID from CustAuthenInfo with(nolock) where AuthenName=@AuthenName and AuthenType=@AuthenType
			if(@@RowCount = 0)
				Begin
					Set @Result = -20504
					Set @ErrMsg = '无此用户1'
					return
				End
            set @UserAccount=@AuthenName
      end 
      else if(@authenClass='2')
      begin
         declare @phoneClass as varchar(1)
         select @CustID=custID ,@phoneClass=@phoneClass from CustPhone with(nolock) where Phone=@AuthenName and PhoneType=@AuthenType 
			if(@@RowCount = 0)
				Begin
					Set @Result = -20504
					Set @ErrMsg = '无此用户2'
					return
				End
            if(@phoneClass='1')
               Begin
					Set @Result = -20504
					Set @ErrMsg = '认证电话为一般电话！'
					return
				End
      end
      else if (@authenClass='3')
      begin
         select @CustID=custID  from custinfo with(nolock) where Email=@AuthenName and EmailClass='2'
          if(@@RowCount = 0)
			Begin
				Set @Result = -20504
				Set @ErrMsg = '无此用户5'
				return
			End   
      end
      else
      begin        
         select @CustID=custID ,@AuthStatus=status,@UserAccount=CardID from CustTourCard with(nolock) where CardID=@AuthenName or InnerCardID=@AuthenName
         if(@@RowCount = 0)
			Begin
				Set @Result = -20504
				Set @ErrMsg = '无此用户3'
				return
			End   
        if(@AuthStatus<>'00') 
            Begin
				Set @Result = -21553
				Set @ErrMsg = '该认用户卡处于暂停状态'
			return
		End     
      end 

      select @CustType=CustType,@UProvinceID=ProvinceID,@AreaID=AreaID,@SysID=SourceSPID,@Status=Status,@TmpPwd1=VoicePwd,@TmpPwd2=WebPwd from  custinfo with(nolock) where custID=@CustID
      if(@@RowCount = 0)
			Begin
				Set @Result = -20504
				Set @ErrMsg = '无此用户4'
				return
			End  

      if(@Status <>'00')
		Begin
			Set @Result = -21553
			Set @ErrMsg = '该用户状态不正常'
			return
		End
		
    
     if(@SysID is null and @CustType in ('41','42'))
     begin
       if(@TmpPwd1 <> @Pwd or @TmpPwd2 <> @Pwd)
		Begin
			Set @Result = -21501
			Set @ErrMsg = '密码错误'
			return
		End      
     end

-----------------------------------------------------

	Set @Result=0	
	
	
	
SET ANSI_NULLS Off

