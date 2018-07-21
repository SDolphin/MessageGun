Select * FROM [dbo].MqMessages WHERE Phone='79162403666';

Select * FROM [dbo].TeleMessages WHERE Sended = 'False';

SELECT COUNT(*) FROM [dbo].MqMessages ;   
SELECT COUNT(*) FROM [dbo].TeleMessages ;


truncate table TeleMessages;
truncate table MqMessages;



DELETE FROM TeleMessages;
DELETE from MqMessages;
DBCC CHECKIDENT (MqMessages, RESEED, 0);

