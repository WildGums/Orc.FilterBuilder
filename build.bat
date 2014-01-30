@echo off
cls
if not exist tools\FAKE\tools\Fake.exe ( 
  src\.nuget\nuget.exe install FAKE -OutputDirectory tools -ExcludeVersion -Prerelease
)
tools\FAKE\tools\Fake.exe build.fsx %* "logfile=build-log.xml"
pause
