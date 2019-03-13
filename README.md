
![Don't Kill Bill](img/KillBill_banner.jpg "Don't Kill Bill")


## Datapac and YOU @ Night of Chances

# Quick start
**Wifi: NOC**
**Heslo:FIITnoc2019**

## 1. Git clone
```
git clone https://github.com/slado/NoC.git
```

## 2. Vytvor databazu
Vzorovu databazu si restorni z backupu NoC.bak. Restore pojde v SQL Server 2016 a vyssom
[restore_db.sql](restore_db.sql)

```sql
USE [master]
RESTORE DATABASE [NoC] FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQL\MSSQL\Backup\NoC.bak' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 5

GO
```
Uz vydane vouchre su v tabulke Vouchers. 

## 3. Code
Kod daj do foldra src

## 4. Odovzdaj riesenie
Jednoducho. Vytvor branch, commit a push. BranchName zvol take, ktore identifikuje teba alebo tvoj team.

```
git checkout -b BranchName
git add .
git commit -m "popis commitu"
git push origin BranchName
```

# Ako vznikla vzorova databaza
Script, ktory
* vytvori databazu
* vytvori tabulku Vouchers
* naplni ju demo datami

[prepare_data.sql](prepare_data.sql)


```sql
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
        union  all
        select id + 1, CAST(RAND(CHECKSUM(NEWID()))*10000000 as int)  randomnumber
        from randowvalues
        where 
          id < 1000000
      )

 insert into Vouchers (VoucherId) 
    select randomnumber
    from randowvalues
    OPTION(MAXRECURSION 0)
GO
```

