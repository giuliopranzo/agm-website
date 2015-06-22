EXECUTE sp_addrolemember @rolename = N'db_ddladmin', @membername = N'agmsolutions_net_user';


GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'agmsolutions_net_user';


GO
EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'agmsolutions_net_user';

