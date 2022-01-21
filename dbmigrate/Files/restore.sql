USE [master]
RESTORE DATABASE [WideWorldImporters] 
	FROM  DISK = N'C:\temp\WideWorldImporters-Full.bak' 
	WITH  FILE = 1,  
		MOVE N'WWI_Primary' TO N'C:\temp\DATA\WideWorldImporters.mdf',  
		MOVE N'WWI_UserData' TO N'C:\temp\DATA\WideWorldImporters_UserData.ndf',  
		MOVE N'WWI_Log' TO N'C:\temp\DATA\WideWorldImporters.ldf',  
		MOVE N'WWI_InMemory_Data_1' TO N'C:\temp\DATA\WideWorldImporters_InMemory_Data_1',  
	NOUNLOAD,  STATS = 5
GO