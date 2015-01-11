create Procedure [dbo].[up_BT_V5_Interface_PwdQuestionQuery]

as
	select QuestionID,Question from PwdQuestion  order by SequenceID


set ANSI_NULLS Off
set QUOTED_IDENTIFIER Off
go
