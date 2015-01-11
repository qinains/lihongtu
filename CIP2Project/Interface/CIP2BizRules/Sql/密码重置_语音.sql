set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go








/*
 * �洢����dbo.up_BT_OV3_Interface_PasswordReset
 *
 * ��������: 
 *			 
 *
 * Author: Yuan Feng
 * Company: Linkage Technology CO., LTD.
 * Create: 2009-8-17
 * Remark:
 *
 */
 
ALTER Procedure [dbo].[up_Customer_OV3_Interface_PasswordReset]
(
	@SPID varchar(8),
	@CustID varchar(16),
	@UserAccount varchar(16),
	@NewPassword varchar(50),
	@Result int out,
	@ErrMsg varchar(256) out,
	@ContactTel varchar(20) out,
	@Email varchar(256) out,
	@RealName varchar(30) out
)
as
	set @Result = -22500
	set @ErrMsg = ''

	if(len(@UserAccount) = 16)
	Begin
		Set @UserAccount = SubString(@UserAccount,5,9)
	End
	
	declare @Status varchar(2)
	
	select @CustID = CustID, @Status = Status from CustTourCard  with(nolock) where CardID = @UserAccount
	if(@@Rowcount = 1)
	set @Result = 0


	if(@Status <> '00')
	Begin
		set @Result = -20504
		set @ErrMsg = '�û�״̬������'
		return
	End
	
	update CustInfo set VoicePwd = @NewPassword where CustID=@CustID
	if(@@Rowcount = 1)
		Begin
		set @Result = 0
		set @ErrMsg = '�����������óɹ�'
	End
	Declare @EmailClass Int
	--ֻ����֤�������֤�ֻ����Ի�ȡ����
	select @Email=Email, @EmailClass= EmailClass, @RealName = RealName from CustInfo where CustID = @CustID
	if(@EmailClass <> 1)
	Begin
		set @Email = ''
	End
	select @ContactTel=Phone from CustPhone with(nolock) where CustID = @CustID and phoneclass = 2

Set QUOTED_IDENTIFIER Off






