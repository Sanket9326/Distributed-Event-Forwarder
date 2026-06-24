using Prometheus;

namespace UForwarderConsumer.Metrics
{
    public static class MetricsRegistry
    {
        public static readonly Counter MessagesProcessed =
            Prometheus.Metrics.CreateCounter(
                "messages_processed_total",
                "Total messages processed");

        public static readonly Counter MessagesFailed =
            Prometheus.Metrics.CreateCounter(
                "messages_failed_total",
                "Total messages failed");

        public static readonly Counter MessagesRetried =
            Prometheus.Metrics.CreateCounter(
                "messages_retried_total",
                "Total messages retried");

        public static readonly Counter MessagesDlq =
            Prometheus.Metrics.CreateCounter(
                "messages_dlq_total",
                "Total messages sent to DLQ");

        public static void Success() => MessagesProcessed.Inc();

        public static void Failure() => MessagesFailed.Inc();

        public static void Retry() => MessagesRetried.Inc();

        public static void Dlq() => MessagesDlq.Inc();
    }
}
