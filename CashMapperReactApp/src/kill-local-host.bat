@echo on
REM Get the PIDs of all processes running on port 5175
for /f "tokens=5" %%a in ('netstat -ano ^| findstr ":5175"') do (
    echo Killing process with PID %%a
    taskkill /PID %%a /F
)

echo All processes on port 5175 have been terminated.

pause

