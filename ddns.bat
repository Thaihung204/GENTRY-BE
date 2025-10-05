@echo off
setlocal enabledelayedexpansion

set TOKEN=MkWcWfoH04QmUFcfIdvmQtp-HRuOASpZY2H6tZjY
set ZONE_ID=26cedf45e311cdd387ef740b3c97c0b1

set RECORD1_ID=20be7900f743f7b527a3b76f7eb761bf
set RECORD2_ID=5b6da5de8c8458d3b6b0f8929f42e3df
set RECORD3_ID=c38bf5c693a8110de74984eb1e406ed1
set RECORD4_ID=2af09a18f66e342c3b130795ee64e85a

set RECORD1_NAME=restx.food
set RECORD2_NAME=restx.restx.food
set RECORD3_NAME=www.restx.food
set RECORD4_NAME=ssh.restx.food

for /f "delims=" %%i in ('curl -s https://api.ipify.org') do set IP=%%i
echo IP public hiện tại: !IP!

call :UpdateRecord %RECORD1_ID% %RECORD1_NAME%
call :UpdateRecord %RECORD2_ID% %RECORD2_NAME%
call :UpdateRecord %RECORD3_ID% %RECORD3_NAME%
call :UpdateRecordNoProxy %RECORD4_ID% %RECORD4_NAME%

echo Done.
timeout /t 2 >nul
exit

:UpdateRecord
echo.
echo Updating record: %2
curl -X PUT "https://api.cloudflare.com/client/v4/zones/%ZONE_ID%/dns_records/%1" ^
     -H "Authorization: Bearer %TOKEN%" ^
     -H "Content-Type: application/json" ^
     --data "{\"type\":\"A\",\"name\":\"%2\",\"content\":\"!IP!\",\"ttl\":120,\"proxied\":true}"
echo.
exit /b

:UpdateRecordNoProxy
echo.
echo Updating record: %2
curl -X PUT "https://api.cloudflare.com/client/v4/zones/%ZONE_ID%/dns_records/%1" ^
     -H "Authorization: Bearer %TOKEN%" ^
     -H "Content-Type: application/json" ^
     --data "{\"type\":\"A\",\"name\":\"%2\",\"content\":\"!IP!\",\"ttl\":120,\"proxied\":false}"
echo.
exit /b
