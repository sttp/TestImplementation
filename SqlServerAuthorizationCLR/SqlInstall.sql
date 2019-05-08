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
	@signature varbinary(max) OUTPUT,
	@configPath nvarchar(max),
	@validFrom [datetime2](7),
	@validTo [datetime2](7),
	@loginName [nvarchar](max),
	@rolesQuery [nvarchar](max),
	@approvedPublicKey [nvarchar](max)
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SqlServerAuthorizationCLR].[SqlServerAuthorizationCLR.SignTicket].[Sign]
GO

CREATE PROCEDURE [dbo].[GetTrustedEndpoints]
    @configPath nvarchar(max)
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SqlServerAuthorizationCLR].[SqlServerAuthorizationCLR.SignTicket].[GetTrustedEndpoints]
GO

-------------------------------------
