using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Postgres.Scheduler
{
    public class ProcedureJob : IJob
    {
        private readonly ILogger<ProcedureJob> _logger;
        private readonly string _connectionString;

        public ProcedureJob(ILogger<ProcedureJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("ConnectionStrings");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var procedureName = context.JobDetail.JobDataMap.GetString("ProcedureName");

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var command = new NpgsqlCommand(procedureName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    await command.ExecuteNonQueryAsync();

                    _logger.LogInformation($"Executed procedure '{procedureName}'");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing procedure '{procedureName}'");
            }
        }
    }

}
