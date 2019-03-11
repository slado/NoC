USE [master]
GO

CREATE DATABASE [NoC] 
GO


USE [NoC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vouchers](
	[VoucherId] [int] NOT NULL
) ON [PRIMARY]

GO

with randowvalues
    as(
       select 1 id, CAST(RAND(CHECKSUM(NEWID()))*10000000 as int) randomnumber
	   --select 1 id, RAND(CHECKSUM(NEWID()))*100 randomnumber
        union  all
        select id + 1, CAST(RAND(CHECKSUM(NEWID()))*10000000 as int)  randomnumber
		--select id + 1, RAND(CHECKSUM(NEWID()))*100  randomnumber
        from randowvalues
        where 
          id < 1000000
      )
 
 
 insert into Vouchers (VoucherId) 
    select randomnumber
    from randowvalues
    OPTION(MAXRECURSION 0)
	
GO

