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
 
ALTER   Procedure [dbo].[up_BT_V2_Interface_UserAuthV2]
(
	@SPID varchar(8),
	@AuthenName varchar(16),
	@AuthenType varchar(16),
	@Pwd varchar(100),
	@Result int out,
	@ErrMsg varchar(256) out,
	@CustID varchar(16) out,
	@UserAccount varchar(16) out,
	@CustType varchar(2) out,
	@UProvinceID varchar(2) out,
	@SysID varchar(4) out,
	@AreaID varchar(3) out,
	@outerid varchar(16) out
	--@ProvinceID varchar(2) out
) as
	set @CustID = ''
	set @Result = -19999
	set @ErrMsg = ''
	set @AreaID = ''
	set @outerid=''
	set @ProvinceID=''
	declare @TmpPwd varchar(50)
	set @TmpPwd = ''
	declare @AuthStatus varchar(1)
	set @AuthStatus=''
	if(@AuthenType not in ('5','6','7','8','9','10','11'))
	Begin
		--商旅卡直接赋值
		if(@AuthenType <>3 )
			Begin
				select  @CustID = CustID,@AuthStatus = Status From UserAuthenStyle where AuthenName = @AuthenName and AuthenType = @AuthenType 
				if(@@RowCount = 0)
				Begin
					--继续根据用户的联系电话进行匹配
					if( @AuthenType = '2')
						Begin
							select @CustID = CustID , @AuthStatus = Status from custInfo where CustId in (select CustID from custExtend where custContactTel=@AuthenName ) and custType in ('00','01') and EncryptedPassword = @Pwd
							if(@@RowCount = 0)
							Begin
								Set @Result = -20504
								Set @ErrMsg = '手机号密码不符'
								return
							End
						End
					else
						Begin
							Set @Result = -20504
							Set @ErrMsg = '无此用户1'
							return
						End
				End
			End
		else
			Begin
				--select @AuthStatus = Status from UserAuthenStyle where AuthenName = @AuthenName and AuthenType = @AuthenType 
				select @CustID= CustID,@outerid=outerid from custUserInfo where UserAccount = @AuthenName
				if(@CustID='' or @CustID is null)
				Begin
					Set @Result = -20504
					Set @ErrMsg = '无此用户2'
					return
				End
				
				set @AuthStatus = '0'
			end


		if (@AuthStatus<>'0')
		Begin
			Set @Result = -21553
			Set @ErrMsg = '该认用户此认证方式处于暂停状态'
			return
		End
		declare @Status varchar(2)
		Set @Status = ''
		
		select @TmpPwd = EncryptedPassword, @Status=Status,@UProvinceID=ProvinceID, @CustType=CustType from CustInfo where CustID = @CustID 
		if(@CustType !='02' and @CustType !='11')
		Begin
			if(@TmpPwd <> @Pwd)
				Begin
					if( @AuthenType = '2')
						Begin
							select @CustID = CustID , @AuthStatus = Status from custInfo where CustId in (select CustID from custExtend where custContactTel=@AuthenName ) and custType in ('00','01') and EncryptedPassword = @Pwd
							if(@@RowCount = 0)
							Begin
								Set @Result = -20504
								Set @ErrMsg = '手机号密码不符'
								return
							End
						End
					else
					begin
						Set @Result = -215011
						Set @ErrMsg = '密码错误'
						return
					end
				End
		End
		if(@Status <>'00')
		Begin
			Set @Result = -21553
			Set @ErrMsg = '该用户状态不正常'
			return
		End
		
		select @UserAccount = UserAccount,@outerid=outerid from CustUserInfo where CustID = @CustID and status = '00'
		--如果是商旅卡号直接返回结果
		If(@CustType !='02' and @CustType !='11')
			Begin

				Set @Result = 0 
				return
			End
		else 
			Set @AuthenName = @UserAccount
		
	End
	else
	Begin
		select  @CustID = CustID,@AuthStatus = Status From UserAuthenStyle where AuthenName = @AuthenName and AuthenType = @AuthenType 
				if(@@RowCount = 0)
				Begin
					Set @Result = -20504
					Set @ErrMsg = '无此用户3'
					return
				End
		select @Status=Status,@UProvinceID=ProvinceID, @CustType=CustType,@SysID = SysID, @UProvinceID=ProvinceID,@AreaID=AreaID from CustInfo where CustID = @CustID 

		if(@Status <>'00')
		Begin
			Set @Result = -21553
			Set @ErrMsg = '该用户状态不正常'
			return
		End

		select @UserAccount = UserAccount,@outerid=outerid from CustUserInfo where CustID = @CustID and status = '00'
				
	End
	
	
	Set @Result=0	
	
	
	
SET ANSI_NULLS Off






