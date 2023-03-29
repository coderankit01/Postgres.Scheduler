using System;
using System.Collections.Generic;
using System.Text;

namespace Postgres.Scheduler
{
    public class ProcedureConfig
    {
        public string Name { get; set; }
        public string CronExpression { get; set; }
    }

}
