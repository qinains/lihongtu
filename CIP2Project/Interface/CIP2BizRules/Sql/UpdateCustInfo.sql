set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go







ALTER proc [dbo].[up_Customer_V3_Interface_UpdateCustInfo]
(
@CustID varchar(16),
@SqlResult int out,
@ErrMsg varchar(256) out,
@ProvinceID varchar(2),
@AreaID varchar(3),
@CertificateType char(1),
@CertificateCode varchar(20),
@RealName varchar(50),
@Sex char(1),
@NickName varchar(30),
@DealTime datetime,
@BirthDay datetime,
@EduLevel varchar(2),
@IncomeLevel varchar(1)
)
as

BEGIN
 if exists(select 1 from custinfo,CustExtendInfo where custinfo.CustID=@CustId and custinfo.CustID=CustExtendInfo.CustID)
	begin
	 SET @SqlResult=0
	  BEGIN TRANSACTION UpdateCustInfoByCustID
		DECLARE @errorSun INT --定义错误计数器
		SET @errorSun=0 --没错为0
		 --事务操作SQL语句
		update custinfo set 
		ProvinceID=@ProvinceID,
		AreaID=@AreaID,
		CertificateType=@CertificateType,
		CertificateCode=@CertificateCode,
		RealName=@RealName,
		Sex=@Sex,
		NickName=@NickName,
		DealTime=@DealTime
		 where
		 CustID=@CustID

		SET @errorSun=@errorSun+@@ERROR --累计是否有错
		--事务操作SQL语句
		update CustExtendInfo set 
		ProvinceID=@ProvinceID,
		BirthDay=@BirthDay,
		EduLevel=@EduLevel,
		IncomeLevel=@IncomeLevel,
		DealTime=@DealTime 
		where
		CustID=@CustID

		SET @errorSun=@errorSun+@@ERROR --累计是否有错
		IF @errorSun<>0 BEGIN PRINT '有错误，回滚'
		ROLLBACK TRANSACTION--事务回滚语句
		END ELSE BEGIN PRINT '成功，提交'
		COMMIT TRANSACTION--事务提交语句

	 END
	end
else
	begin
	 set @SqlResult=-30008
	 set @ErrMsg='帐号不存在'
	end
END


