using ChannelExample.ViewModels;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelExample.Services
{
    public class Batcher
    {
        private readonly Channel<MeasurementViewModel> _channel;

        public Batcher()
        {
            _channel = Channel.CreateUnbounded<MeasurementViewModel>(new UnboundedChannelOptions()
            {
                SingleWriter = false,
                SingleReader = true
            });
        }

        public void AddMeasurement(MeasurementViewModel measurement)
        {
            if (!_channel.Writer.TryWrite(measurement))
            {
                throw new System.Exception("Failed to write measurement to channel");
            }
        }

        public async Task<MeasurementViewModel[]> ReadBatchAsync(int batchSize, System.Threading.CancellationToken stoppingToken)
        {
            var result = new MeasurementViewModel[batchSize];

            for (int i = 0; i < batchSize; i++)
            {
                try
                {
                    result[i] = await _channel.Reader.ReadAsync(stoppingToken);
                }
                catch (System.OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return result;
                }
            }

            return result;
        }
    }
}
