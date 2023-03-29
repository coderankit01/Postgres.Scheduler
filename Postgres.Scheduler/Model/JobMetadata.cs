using System;
using System.Collections.Generic;
using System.Text;

namespace Postgres.Scheduler
{
    public class JobMetadata
    {
        public JobMetadata(Guid id, string name, string cronExpression)
        {
            Id = id;
            Name = name;
            CronExpression = cronExpression;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string CronExpression { get; }
    }
}
