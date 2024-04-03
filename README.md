Default Windows Service with a monitor that checks Health of the Service. if service is down, will start it.
* Need to install - dotnet add package Microsoft.Extensions.Hosting.WindowsServices
* Need to run with highest privileges (both IDE and cmd)
* Tested on .net core 7
* Once the project is published , create and strat a service using sc commands
  - sc.exe create NameForService "path to published executable" start= auto
  - sc start/stop NameForService
  - sc query NameForService (check service status)

* To verify - WIN + R , type services.msc , locate the NameForService
* Modify Service name in DemoService and HealRebootService for personal use
