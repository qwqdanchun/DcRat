@echo off
pushd "%~dp0"
powershell Compress-7Zip "Binaries\Release" -ArchiveFileName "DcRat.zip" -Format Zip
:exit
popd
@echo on
