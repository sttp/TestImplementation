@echo off
CD %~dp0

:: Executables
set dn=C:\Program Files\dotnet\dotnet.exe
set oc=%userprofile%\.nuget\packages\opencover\4.6.519\tools\OpenCover.Console.exe
set rg=%userprofile%\.nuget\packages\reportgenerator\3.0.2\tools\ReportGenerator.exe

:: Set Up Directories
if EXIST Test\Results (rd /S /Q Test\Results)
mkdir Test\Results

if EXIST Test\Reports (rd /S /Q Test\Reports)
mkdir Test\Reports

IF NOT EXIST Test\History mkdir Test\Reports

:: Restore Packages
dotnet restore SttpPrototype.sln
::dotnet build SttpPrototype.sln

:: Build and Run Tests, Currently each framework overwrites the previous test results (netstandard20,net40,net45,net46)
"%oc%" -oldstyle -register:user -target:"%dn%" -targetargs:"test --logger:trx;LogFileName=\"%~dp0\Test\Results\Sttp.Tests.trx\" /p:DebugType=full \"%~dp0\Sttp.Tests\Sttp.Tests.csproj\"" -filter:"+[*]* -[*]*Tests*" -excludebyattribute:"*.ExcludeFromCodeCoverage*" -output:"%~dp0\Test\Results\coverage-opencover-sttp.xml"

:: Generate and Launch Report
"%rg%" -reports:"%~dp0\Test\Results\coverage-opencover-sttp.xml" -targetdir:"%~dp0\Test\Reports" -historydir:"%~dp0\Test\History
start "report" "%~dp0\Test\Reports\index.htm"

:: done