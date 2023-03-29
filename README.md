# Postgres.Scheduler using Quartz

Postgres scheduler is a .net core console application tool to schedule function, procedures and execute it in a specific time.

It is very easy to use just you need to download and configure procedures in appsetting.json.

Internally it does use the quartz to schedule each postgres jobs.

How to run ?
1) Add/change procedure name and cron time in the appsetting.json
2) change connection string
3) That's It

How to deploy in window

1) Publish the code and run postgres.scheduler.exe directly or deploy as window service
2) To deploy as a window service window service, follow the command.<br/>
   ``` sc create postgres-scheduler binPath= "C:\path\to\publish\postgres.scheduler.exe" start= auto ```

How to deploy in ubuntu

1) Publish code and copy content to the server and create a systemctl service.
