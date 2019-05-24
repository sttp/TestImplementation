use CtpAuth
GO

ALTER Assembly SqlServerAuthorizationCLR from 'P:\Database\CtpAuth\SqlServerAuthorizationCLR.dll' WITH PERMISSION_SET = UNSAFE;
GO

use CtpAuth
GO
DROP PROCEDURE dbo.[Sign]
GO
DROP PROCEDURE dbo.[OpenCertificate]
GO
IF EXISTS (select * from sys.assemblies WHERE NAME = 'SqlServerAuthorizationCLR')
BEGIN
    drop Assembly SqlServerAuthorizationCLR;
END;
GO
Create Assembly SqlServerAuthorizationCLR from 'P:\Database\CtpAuth\SqlServerAuthorizationCLR.dll' WITH PERMISSION_SET = UNSAFE;
GO

CREATE PROCEDURE [dbo].[Sign]
    @ephemeralCertificate varbinary(max) OUTPUT,
    @signingCertificate varbinary(max),
	@contentsQuery nvarchar(max)
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SqlServerAuthorizationCLR].[SqlServerAuthorizationCLR.SignEphemeralCertificate].[Sign]
GO

CREATE PROCEDURE [dbo].[OpenCertificate]
    @data varbinary(max) OUTPUT,
    @thumbprint varbinary(max) OUTPUT,
    @contents nvarchar(max) OUTPUT,
    @certificatePath nvarchar(max)
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [SqlServerAuthorizationCLR].[SqlServerAuthorizationCLR.SignEphemeralCertificate].[OpenCertificate]
GO

-------------------------------------
