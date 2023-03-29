using Postgres.Scheduler;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IJobFactory _jobFactory;
    private readonly List<JobMetadata> _jobMetadatas;
    private IScheduler _scheduler;

    public QuartzHostedService(
        ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory,
        IEnumerable<JobMetadata> jobMetadatas)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _jobMetadatas = jobMetadatas.ToList();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        _scheduler.JobFactory = _jobFactory;

        foreach (var jobMetadata in _jobMetadatas)
        {
            var jobDetail = JobBuilder.Create<ProcedureJob>()
                .WithIdentity(jobMetadata.Name, jobMetadata.Id.ToString())
                .UsingJobData("ProcedureName", jobMetadata.Name)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobMetadata.Name}Trigger", jobMetadata.Id.ToString())
                .WithCronSchedule(jobMetadata.CronExpression)
                .Build();

            await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
        }

        await _scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduler.Shutdown(cancellationToken);
    }
}
