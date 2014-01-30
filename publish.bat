@echo off
cls
if not exist tools\FAKE\tools\Fake.exe ( 
  src\.nuget\nuget.exe install FAKE -OutputDirectory tools -ExcludeVersion -Prerelease
)
tools\FAKE\tools\Fake.exe build.fsx %* Release "logfile=build-log.xml"
rem tools\FAKE\tools\Fake.exe build.fsx %* Release "nugetkey=invalidaccesskey" "logfile=build-log.xml"
pause
