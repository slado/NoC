-- Sample database
-- replace the path to the database backup file with the proper one

USE [master]
RESTORE DATABASE [NoC] FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQL\MSSQL\Backup\NoC.bak' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 5

GO
