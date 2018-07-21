Select * FROM [dbo].MqMessages WHERE Phone='79162403666';

SELECT COUNT(*) FROM [dbo].MqMessages ;

truncate table TeleMessages;
truncate table MqMessages;



DELETE FROM TeleMessages;
DELETE from MqMessages;
DBCC CHECKIDENT (MqMessages, RESEED, 0);

