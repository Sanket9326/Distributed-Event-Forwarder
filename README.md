# 🚀 Distributed Event Forwarder

A scalable and fault-tolerant distributed event processing platform inspired by modern asynchronous architectures. The system consumes events from Kafka, applies routing logic, and reliably forwards messages to downstream systems while supporting horizontal scaling, retries, dead-letter queues, idempotency, and distributed locking.

Built with **.NET, Kafka, Redis, Docker, and Background Workers**.

---

## ✨ Features

* Event-driven architecture
* Kafka-based asynchronous messaging
* Background worker consumers
* Message routing engine
* Horizontal scalability
* Fault-tolerant processing
* Retry mechanism with delayed retries
* Dead Letter Queue (DLQ)
* Redis-based distributed locking
* Idempotent message processing
* Duplicate message prevention
* Dockerized deployment
* Extensible message pipeline
* Clean separation of concerns

---

## 🏗 Architecture

```
                +----------------+
                | Event Producer |
                +----------------+
                         |
                         v
                +----------------+
                |     Kafka       |
                |   Main Topic    |
                +----------------+
                         |
                         v
                +----------------------+
                | Distributed Event     |
                | Forwarder Consumer    |
                +----------------------+
                         |
             +-----------+-----------+
             |                       |
             v                       v
     +----------------+      +----------------+
     | Distributed    |      | Idempotency    |
     | Lock (Redis)   |      | Check (Redis)  |
     +----------------+      +----------------+
             |
             v
      +------------------+
      | Message Processor|
      +------------------+
             |
      +------+------+
      |             |
      v             v
 Success          Failure
      |             |
 Commit Offset      |
                    v
           +----------------+
           | Retry Queue     |
           | (Redis Sorted   |
           | Set)            |
           +----------------+
                    |
                    v
           +----------------+
           | Retry Worker    |
           +----------------+
                    |
                    v
                Kafka Topic
                    |
                    v
            Max Retries Exceeded
                    |
                    v
            +----------------+
            | Dead Letter     |
            | Queue (DLQ)     |
            +----------------+
```

---

## 🛠 Tech Stack

| Component            | Technology                |
| -------------------- | ------------------------- |
| Language             | C#                        |
| Framework            | .NET                      |
| Messaging            | Apache Kafka              |
| Cache & Coordination | Redis                     |
| Containerization     | Docker                    |
| Worker Processing    | Background Services       |
| Serialization        | System.Text.Json          |
| Architecture         | Event-Driven Architecture |
| Retry Scheduling     | Redis Sorted Sets         |
| Distributed Locking  | Redis                     |
| Dead Letter Queue    | Kafka                     |

---

## 📂 Project Structure

```
Distributed-Event-Forwarder
│
├── Dtos/
│
├── Services/
│   ├── Implementations/
│   ├── IMessageProcessor.cs
│   ├── IRetryService.cs
│   ├── IDlqService.cs
│   ├── IDistributedLockService.cs
│   ├── IIdempotencyService.cs
│
├── ConsumerWorker/
├── RetryWorker/
│
├── Program.cs
├── Worker.cs
├── appsettings.json
└── Dockerfile
```

---

## ⚙️ How It Works

1. Producers publish events to Kafka topics.
2. Consumer workers read events from Kafka.
3. A Redis-based distributed lock ensures only one consumer processes a message.
4. Idempotency checks prevent duplicate processing.
5. Messages are deserialized and routed.
6. Downstream forwarding operations are executed.
7. On success, offsets are committed and message state is marked completed.
8. On failure, messages are scheduled for retry using Redis Sorted Sets.
9. Retry workers re-publish failed messages to Kafka when their retry time arrives.
10. Messages exceeding the maximum retry count are sent to the Dead Letter Queue (DLQ).

---

## Example Event

```json
{
  "messageId": "12345",
  "eventType": "OrderCreated",
  "payload": {
    "orderId": 1001,
    "customerId": 567
  }
}
```

---

## Reliability Mechanisms

### Retry Processing

Failed messages are not lost. They are stored in Redis with a scheduled retry time and later re-published by a dedicated Retry Worker.

### Dead Letter Queue (DLQ)

Messages that exceed the maximum retry count are sent to a Kafka DLQ topic for investigation and replay.

### Distributed Locking

Redis locks prevent multiple consumers from processing the same message simultaneously in a scaled environment.

### Idempotency

Message states are stored in Redis to ensure duplicate messages are ignored and processing occurs exactly once from the application's perspective.

---

## Running Locally

### Clone Repository

```bash
git clone https://github.com/Sanket9326/Distributed-Event-Forwarder.git
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run
```

---

## Docker

Build image:

```bash
docker build -t distributed-event-forwarder .
```

Run container:

```bash
docker run distributed-event-forwarder
```

---

## Future Enhancements

* Metrics and monitoring
* Prometheus integration
* OpenTelemetry tracing
* Multiple Kafka topics
* Dynamic routing rules
* Dashboard and observability
* Exponential backoff policies
* Batch processing
* Circuit breaker support
* Event replay capability

---

## Design Principles

* Scalability
* Reliability
* Loose Coupling
* Asynchronous Processing
* Extensibility
* High Throughput
* Fault Tolerance
* At-Least-Once Delivery
* Idempotent Processing

---

## Inspiration

This project draws inspiration from modern event-driven systems and distributed messaging architectures used by companies such as:

* Uber
* Netflix
* LinkedIn
* Amazon

---

## License

MIT License
