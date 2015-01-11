set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go





/*
 * 存储过程up_Customer_OV3_Interface_CustPwdAuth
 *
 * 功能描述: 3.1.10 客户密码认证接口
 *			 
 *
 * Author: tongbo
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-11
 * Remark:
 *@authenClass :1,CustAuthenInfo;2,CustPhone ;
 */

ALTER Procedure [dbo].[up_Customer_OV3_Interface_CustPwdAuth]
(
@SPID varchar(8),
@CustID varchar(16),
@Pwd varchar(128),
@PwdType varchar(1),

@Result int out,
@ErrMsg varchar(256) out,
@NewCustID varchar(16) out,
@AreaID varchar(3) out,
@SysID varchar(4) out,
@AuthenName varchar(16) out,
@custType varchar(2) out,
@authenType varchar(2) out
)
as
begin
    Declare @status as varchar(2)

    set @Result = -22500
	set @ErrMsg = ''
	set @NewCustID = ''
    set @AreaID=''
    set @SysID=''
    set @AuthenName=''
    set @custType=''
    set @authenType=''
    

    select @areaid=areaid, @custType=custType,@status=status,@SysID=SourceSPID From CustInfo with(nolock) Where CustID=@CustID 
    if(@@RowCount = 0)
	Begin
		Set @Result = -20504
		Set @ErrMsg = '无此用户1'
		return
	End

   	if(@Status <>'00')
	Begin
		Set @Result = -21553
		Set @ErrMsg = '该用户状态不正常'
		return
	End
   
if(@custType in ('41','42'))
begin  
   if(@PwdType='2')
	   begin
         if not exists ( select 1 From CustInfo with(nolock)  Where CustID=@CustID and WebPwd=@Pwd )
			 Begin
                set @Result = -20503
				set @ErrMsg = 'WEB密码不正确'
				return
             end 
	   end
   else if(@PwdType='1')
	   begin
         if not exists ( select 1 From CustInfo with(nolock)  Where CustID=@CustID and VoicePwd=@Pwd  )
			 Begin
                set @Result = -20503
				set @ErrMsg = '语音密码不正确'
				return
             end 
	   end 
   else
       begin
          if not exists ( select 1 From CustInfo  with(nolock) Where CustID=@CustID and (VoicePwd=@Pwd or WebPwd=@Pwd) )
             Begin
                set @Result = -20503
				set @ErrMsg = '密码不正确'
				return
             end 
       end 
    set @Result = 0
	set @ErrMsg = ''
	set @NewCustID = @CustID
    return 
end
else
begin
     --用户名
    select top 1 @AuthenName=UserName,@AuthenType='1' from CustInfo a where  a.CustID=@CustID and UserName is not null and UserName<>''
    if(@@RowCount > 0)
    begin
        set @Result = 0
    	set @ErrMsg = '用户名存在'
        set @NewCustID = @CustID 
		return
    end 
    --手机
    Declare @AuthenType1 varchar(2) 
    select @AuthenType1=a.phonetype,@AuthenName=a.phone from CustPhone a where  a.CustID=@CustID and a.phoneclass<>'1'               
    if(@@RowCount > 0)
    begin
		if(@AuthenType1 ='1')
		   set @AuthenType='9'
		else if(@AuthenType1='3')
		   set @AuthenType='10'
		else 
		   set @AuthenType='7'
		set @NewCustID = @CustID 
        set @Result = 0
    	set @ErrMsg = '手机存在'
		return
	end	
   

    --商旅卡号
    select @AuthenType='3', @AuthenName=a.CardID from CustTourCard a  with(nolock) where   a.CustID=@CustID and a.status='00' 
    if(@@RowCount > 0)
    begin
        set @Result = 0
    	set @ErrMsg = '商旅卡号存在'
        set @NewCustID = @CustID 
		return
    end 
  
    --Email
    select @AuthenType='4', @AuthenName=a.Email from CustInfo a  with(nolock) where a.CustID=@CustID and a.status='00' and a.EmailClass='2'
    if(@@RowCount > 0)
    begin
        
        set @Result = 0
    	set @ErrMsg = 'Email存在'
        set @NewCustID = @CustID 
		return
    end 
    else
    begin
		select @AuthenType='4',@AuthenName=AuthenName from CustAuthenInfo with(nolock) where CustID=@CustID and AuthenType='4'
        if(@@RowCount > 0)
        begin                 
			set @Result = 0
    		set @ErrMsg = 'Email存在'
			set @NewCustID = @CustID 
			return
		end 
    end
	 --卡  
     select @AuthenName=a.AuthenName , @AuthenType=a.AuthenType   from  CustAuthenInfo a with(nolock) where  a.CustID =@CustID 
     if(@@RowCount > 0)
     begin
        set @Result = 0
    	set @ErrMsg = '卡存在'
        set @NewCustID = @CustID 
		return
     end 

	 if(@AuthenType1='1') 
		begin
			select @AuthenName=AuthenName from CustAuthenInfo with(nolock) where CustID=@CustID and AuthenType='1'
			if(@@RowCount > 0)
			begin                 
				set @Result = 0
    			set @ErrMsg = '用户名存在'
				set @NewCustID = @CustID 
				set @AuthenType='1'
				return
			end 
		end
	 if(@AuthenType1='4') 
		begin
			select @AuthenName=AuthenName from CustAuthenInfo with(nolock) where CustID=@CustID and AuthenType='4'
			if(@@RowCount > 0)
			begin                 
				set @Result = 0
    			set @ErrMsg = 'Email存在'
				set @NewCustID = @CustID 
				set @AuthenType='4'
				return
			end 
		end
	set @Result = -20501 
	set @ErrMsg = '用户认证失败 '
	
end 

   
end







