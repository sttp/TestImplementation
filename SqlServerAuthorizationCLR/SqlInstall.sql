use CtpAuth
GO

ALTER Assembly SqlServerAuthorizationCLR from 'P:\Database\CtpAuth\SqlServerAuthorizationCLR.dll' WITH PERMISSION_SET = UNSAFE;
GO

use CtpAuth
GO
DROP PROCEDURE dbo.[Sign]
GO
DROP PROCEDURE dbo.[GetTrustedEndpoints]
GO
IF EXISTS (select * from sys.assemblies WHERE NAME = 'SqlServerAuthorizationCLR')
BEGIN
    drop Assembly SqlServerAuthorizationCLR;
END;
GO
Create Assembly SqlServerAuthorizationCLR from 'P:\Database\CtpAuth\SqlServerAuthorizationCLR.dll' WITH PERMISSION_SET = UNSAFE;
GO

CREATE PROCEDURE [dbo].[Sign]
    @ticket varbinary(max) OUTPUT,
    @certificateThumbprint nvarchar(max) OUTPUT,
	@signature varbinary(max) OUTPUT,
	@certificatePath nvarchar(max),
	@ticketQuery nvarchar(max)
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SqlServerAuthorizationCLR].[SqlServerAuthorizationCLR.SignTicket].[Sign]
GO

CREATE PROCEDURE [dbo].[GetTrustedEndpoints]
    @configPath nvarchar(max),
	@hashBits int
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SqlServerAuthorizationCLR].[SqlServerAuthorizationCLR.SignTicket].[GetTrustedEndpoints]
GO

-------------------------------------
