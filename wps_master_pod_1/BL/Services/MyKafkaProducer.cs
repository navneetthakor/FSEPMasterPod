using Confluent.Kafka;
using Newtonsoft.Json;
using wps_master_pod_1.Modal;

namespace wps_master_pod_1.BL.Services
{
    public class MyKafkaProducer
    {
        private static ProducerConfig config { get; } = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            AllowAutoCreateTopics = true,
            Acks = Acks.All
        };
        public static void NotifyAdmin(string? message)
        {

            using (
            IProducer<Null, String> producer = new ProducerBuilder<Null, string>(MyKafkaProducer.config).Build())
            {
                try
                {
                    producer.Produce("Admin-Notifier",
                        new Message<Null, string> { Value = message});
                    Console.WriteLine($"kafka message is sent to ADMIN.");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                };

                producer.Flush(TimeSpan.FromSeconds(10));
            }


        }
    }
}
