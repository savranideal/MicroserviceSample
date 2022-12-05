@echo off
for /f "delims=" %%f in ('dir bin /s /b /ad') do (
	rd "%%f" /Q/S 
	echo "Cleaned %%f"
)
for /f "delims=" %%f in ('dir obj /s /b /ad') do (
	rd "%%f" /Q/S
	echo "Cleaned %%f"
)
pause
