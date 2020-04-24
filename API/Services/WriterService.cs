using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelExample.Services
{
    public class WriterService : BackgroundService
    {
        private readonly Batcher _batcher;
        private string _filePath;

        public WriterService(Batcher batcher)
        {
            _batcher = batcher;
            _filePath = @"Measurements.txt";

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var batchOfItems = await _batcher.ReadBatchAsync(5, stoppingToken);

                var average = batchOfItems.Average(item => item.Value);
                var range = string.Join(',', batchOfItems.Select(item => item.MeasurementID));
                var fileLine = $"{range}    {average}{System.Environment.NewLine}";

                await File.AppendAllTextAsync(_filePath, fileLine);
            }

        }
    }
}
