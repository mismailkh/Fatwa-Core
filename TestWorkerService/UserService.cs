using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkerService
{
    public class UserService : BackgroundService
    {

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            while(stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000,stoppingToken);
            }

        }
        public override Task StopAsync(CancellationToken stoppingToken)
        {
           return  base.StopAsync(stoppingToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
