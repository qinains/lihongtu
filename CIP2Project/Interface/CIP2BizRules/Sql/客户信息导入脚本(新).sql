--客户信息导入脚本
------------------------------------------------------------------------------------------------------------
--数据导入成功按照步骤执行
--第一步执行---start
declare @temTableRecordCount bigint   --记录临时表记录总数
declare @parSequenceNumber bigint     --目前系统最大CustID
select @temTableRecordCount=count(1) from 临时数据表  --tmp1为导入临时表
select @temTableRecordCount as '临时数据表记录总数'
select @parSequenceNumber=SequenceID from parSequenceNumber where sequenceType = '0101'
select @parSequenceNumber as '未增加前系统最大CustID'
set @parSequenceNumber = @parSequenceNumber + @temTableRecordCount
update parSequenceNumber set SequenceID=@parSequenceNumber where sequenceType = '0101'
select @parSequenceNumber as '增加后系统最大CustID'
select * from parSequenceNumber  --查看parSequenceNumber表SequenceID列是否已被修改
--第一步执行--end
------------------------------------------------------------------------------------------------------------

--第二步执行--start
--修改临时数据表结构(用第一步的‘增加后系统最大CustID值’替换'初始值')
ALTER   TABLE   临时数据表  
  add   [CustID]   [bigint]   IDENTITY   (25920591,   1)   NOT   NULL  
GO
--第二步执行--end

------------------------------------------------------------------------------------------------------------

--第三步执行--start
--数据校验
select * from 临时数据表 where len(custAccount)>16 or len(CustType)>2
or len(CertificateType)>1 or len(RealName)>100 or datalength(CertificateCode)>30
or len(Sex)>1 or len(contactTel)>13 or len(areaID)>3 or len(custLevel)>1
--如果存在错误数据1.存入数据错误表（数据错误表命名规则ImportData_地区_日期_DateError）
--1.
select * into ImportData_gs_20090915_DateError
from 临时数据表 where len(custAccount)>16 or len(CustType)>2
or len(CertificateType)>1 or len(RealName)>100 or len(zipcode)>6 or datalength(CertificateCode)>30
or len(Sex)>1 or len(contactTel)>13 or len(areaID)>3 or len(custLevel)>1
--2.查出错误数据及原因
select * from  临时数据表  where  len(RealName)>30 --41
select * from  临时数据表  where  len(custAccount)>16 
select * from  临时数据表  where  custType not in (select CustType from ParCustType)
select * from  临时数据表  where  len(CertificateType)>1 --14
select * from  临时数据表  where  datalength(CertificateCode)>30
select * from  临时数据表  where  sex not in ('0','1','2') --63
select * from  临时数据表  where  len(contactTel)>13 --17
select * from  临时数据表  where  len(areaID)>3 --
select * from  临时数据表  where  custLevel not in ('0','1','2','3','4')--55
select * from    where  len(zipcode)>6--6
--3.删除临时数据表数据错误记录
delete from 临时数据表
where outerID in (select outerID from ImportData_gs_20090915_DateError)

--第三步执行--end

------------------------------------------------------------------------------------------------------------
--第四步执行--start(重复数据错误表命名规则--ImportData_地区_日期_RepeatUserAccount) 
--查出重复的数据并存入重复数据错误表
select * into ImportData_gs_20090915_RepeatUserAccount from 临时数据表
where outerID in (select outerID from custinfo)
--如果查出数据，执行第五步，没有执行第六步
--第四步执行--end

------------------------------------------------------------------------------------------------------------
--第五步执行--start
--册除临时数据表重复数据记录
delete from 临时数据表
where outerID in (select outerID from ImportData_gs_20090915_RepeatUserAccount)
--第五步执行--end

------------------------------------------------------------------------------------------------------------
--第六步执行--start
--修改临时数据表字段长度
alter Table 临时数据表 alter column outerID varchar(30)
alter Table 临时数据表 alter column custType varchar(2)
alter Table 临时数据表 alter column custAccount varchar(16)
alter Table 临时数据表 alter column realName varchar(100)
alter Table 临时数据表 alter column custLevel varchar(1)
alter Table 临时数据表 alter column areaID varchar(3)
alter Table 临时数据表 alter column address varchar(256)
alter Table 临时数据表 alter column zipCode varchar(6)
alter Table 临时数据表 alter column sex varchar(1)
alter Table 临时数据表 alter column certificateType varchar(1)
alter Table 临时数据表 alter column CertificateCode varchar(30)
alter Table 临时数据表 alter column contactTel varchar(20)
alter Table 临时数据表 alter column email varchar(256)
--第六步执行--end

------------------------------------------------------------------------------------------------------------

--第七步执行--start
--正式导入语句
declare @UProvinceID varchar(2)
declare @SPID  varchar(8)
Set @UProvinceID = '14'  ---------------------导入时须按情况修改------------------
Set @SPID = @UProvinceID + '999999'
declare	@RegistrationSource char(2)
declare	@RegistrationType char(1)
set @RegistrationSource = '07'
set @RegistrationType = '2'
declare @AddressType varchar(2)
set @AddressType= '00' --账单地址
declare @status varchar(2)
set @status = '00'  --客户状态
declare @EmailClass int
declare @PhoneClass varchar(1)
declare @PhoneType varchar(1)
set @EmailClass = 1 --邮件级别
set @PhoneClass = '1' --电话级别
set @PhoneType = '4'  --电话类型
declare	@RegDate DateTime
set @RegDate = getDate()

--1.插入客户信息表
insert into CIP2..CustInfo(CustID,ProvinceID,AreaID,CustType,CertificateType,CertificateCode,RealName,
CustLevel,Sex,RegistrationSource,Status,Email,EmailClass,SourceSPID,OuterID,DealTime,CreateTime)
select CustID,@UProvinceID,areaID,custType,certificateType,certificateCode,realName,
custLevel,sex,@RegistrationSource,@status,email,@EmailClass,@SPID,outerID,@RegDate,@RegDate  
from 临时数据表


--2.插入客户地址信息表
insert into CIP2..AddressInfo(CustID,AddressProvince,AddressArea,Address,ZipCode,Type,DealTime) 
select CustID,@UProvinceID,areaID,address,zipCode,@AddressType,@RegDate  
from 临时数据表

--3.插入客户电话信息表
insert into CIP2..CustPhone(CustID,ProvinceID,CustType,Phone,PhoneType,PhoneClass,SourceSPID,Dealtime)
select CustID,@UProvinceID,custType,contactTel,@PhoneType,@PhoneClass,@SPID,@RegDate 
from 临时数据表

--第七步执行--end

------------------------------------------------------------------------------------------------------------
--第八步执行---start
--导出对应关系(对应关系表命名规则--ImportData_地区_日期_Relation)
select CustID, OuterID, AreaID into ImportData_gs_20090323_Relation 
from CIP2..custInfo with(nolock) 
where Dealtime>'2009-05-09 04:00:01' and SourceSPID = @SPID   ----处理时间为导入数据时间
--第八步执行--end


--更新性别
update 临时数据表 set sex = '2' where  sex not in ('0','1','2')