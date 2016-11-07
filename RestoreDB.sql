--RESTORE FILELISTONLY FROM DISK='C:\Users\nando\Downloads\agmsolutions_net_site_635950573935620093.bak'

RESTORE DATABASE AGM_dev FROM DISK='C:\Users\nando\Downloads\agmsolutions_net_site_635950573935620093.bak'
WITH 
   MOVE 'agmsolutions_net_site' TO 'C:\Users\nando\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\AGM_dev.mdf',
   MOVE 'agmsolutions_net_site_log' TO 'C:\Users\nando\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\AGM_dev.ldf'

USE AGM_dev

Create login dev with password='dev';
Create user dev for login dev;
Grant Execute to dev;

EXEC sp_addrolemember N'db_datareader', N'dev' 
EXEC sp_addrolemember N'db_datawriter', N'dev'
exec sp_addrolemember 'db_owner', 'dev'