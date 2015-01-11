USE [CIP2]
GO
/****** 对象:  StoredProcedure [dbo].[up_Customer_OV3_Interface_UserAuthV2]    脚本日期: 10/15/2009 15:28:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















/*
 * 存储过程dbo.up_BT_V2_Interface_UserAuthV2
 *
 * 功能描述: 
 *			 
 *
 * Author: tongbo
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-7-28
 * Remark:
 *
 */

ALTER  Procedure [dbo].[up_Customer_OV3_Interface_UserAuthV2]
(
	@SPID varchar(8),
	@AuthenName varchar(256),
	@AuthenType varchar(2),
	@Pwd varchar(100),
  

	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16) out,
	@UserAccount varchar(16) out,
	@CustType varchar(2) out,
	@UProvinceID varchar(2) out,
	@SysID varchar(4) out,
    @outerid varchar(30) out,
	@AreaID varchar(3) out,
    @RealName varchar(20) out,
	@UserName varchar(20) out,
	@NickName varchar(20) out

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
	declare @TmpPwd1 varchar(50)
    declare @TmpPwd2 varchar(50)
	set @TmpPwd1 = ''
	set @TmpPwd2 = ''
	declare @AuthStatus varchar(2)
	set @AuthStatus=''
    declare @Status as varchar(2)
declare @AuthenType1 varchar(2)
		

      if(@AuthenType in('5','6','8','11','12','13','14','15'))
      begin 
        select @CustID=custID from CustAuthenInfo with(nolock) where AuthenName=@AuthenName and AuthenType=@AuthenType
			if(@@RowCount = 0)
				Begin
					Set @Result = -20504
					Set @ErrMsg = '认证：无此用户1'
					return
				End
            set @UserAccount=@AuthenName
      end 
      else if(@AuthenType in('2','7','9','10'))
      begin
         declare @phoneClass as varchar(1)
         if(@AuthenType='9')
           set @AuthenType1='1'
         else if(@AuthenType='10')
            set @AuthenType1='3'
         else
           set @AuthenType1='2'
         select @CustID=custID ,@phoneClass=@phoneClass from CustPhone with(nolock) where Phone=@AuthenName and PhoneType=@AuthenType1 
			if(@@RowCount = 0)
				Begin
					Set @Result = -20504
					Set @ErrMsg = '电话：无此用户2'
                    return
				End
            if(@phoneClass='1')
               Begin
					Set @Result = -20504
					Set @ErrMsg = '认证电话为一般电话！'
					return
				End
      end
      else if (@AuthenType='4')
      begin       
         select @CustID=custID  from custinfo with(nolock) where Email=@AuthenName and EmailClass='2'
         if(@@RowCount = 0)
			Begin
				Set @Result = -20504
				Set @ErrMsg = 'Email：无此用户5'
                select @CustID=custID from CustAuthenInfo with(nolock) where AuthenName=@AuthenName and AuthenType=@AuthenType
				if(@@RowCount = 0)
                    begin
                        Set @Result = -20504
				        Set @ErrMsg = 'Email：无此用户51'
						return
					end              
				
		    End   
      end
      else if (@AuthenType='1')
      begin    
          select @CustID=custID  from custinfo with(nolock) where UserName=@AuthenName 
         if(@@RowCount = 0)
			Begin
				Set @Result = -20504
				Set @ErrMsg = 'UserName：无此用户6'
			    select @CustID=custID from CustAuthenInfo with(nolock) where AuthenName=@AuthenName and AuthenType=@AuthenType
				if(@@RowCount = 0)
                    begin
                        Set @Result = -20504
				        Set @ErrMsg = 'UserName：无此用户51'
						return
					end            
			End   
       end       
      else
      begin        
         select @CustID=custID ,@AuthStatus=status,@UserAccount=CardID from CustTourCard with(nolock) where CardID=@AuthenName or InnerCardID=@AuthenName
         if(@@RowCount = 0)
			Begin
				Set @Result = -20504
				Set @ErrMsg = '商旅卡：无此用户3'
				return
			End   
        if(@AuthStatus<>'00') 
            Begin
				Set @Result = -21553
				Set @ErrMsg = '该认用户卡处于暂停状态'
			return
		End     
      end 

      select @CustType=CustType,@UProvinceID=ProvinceID,@AreaID=AreaID,@SysID=SourceSPID,@Status=Status,@TmpPwd1=VoicePwd,@TmpPwd2=WebPwd,
			@RealName=RealName,@NickName=NickName,@UserName=UserName,@outerid=outerid from  custinfo with(nolock) where custID=@CustID
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
		
      if((@Pwd is null or @Pwd='') )
		Begin
			Set @Result = -21501
			Set @ErrMsg = '密码不能为null'

			return
		End 

     select @UserAccount=CardID from CustTourCard with(nolock) where CustID=@CustID

     if( @CustType in ('41','42','50'))
     begin
         select 1 from custinfo with(nolock)  where custID=@CustID and (VoicePwd=@Pwd or WebPwd=@Pwd)
        if(@@RowCount = 0)
		Begin          
			Set @Result = -21501
			Set @ErrMsg = '密码错误'
			return
		End  
 
     end

-----------------------------------------------------

	Set @Result=0	
	
	
	












