# 🚀 Distributed Event Forwarder

A scalable distributed event forwarding platform inspired by modern asynchronous architectures. The system consumes events from Kafka, applies routing logic, and forwards messages to downstream consumers while supporting horizontal scaling and fault tolerance.

Built with **.NET, Kafka, Redis, Docker, and Background Workers**.

---

## ✨ Features

* Event-driven architecture
* Kafka-based asynchronous messaging
* Message routing engine
* Background worker consumers
* Horizontal scalability
* Fault-tolerant processing
* Dockerized deployment
* Extensible message pipeline
* Clean separation of concerns
* Producer-consumer architecture

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
                |     Topic       |
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
      +-------------+         +-------------+
      | Routing     |         | Message     |
      | Engine      |         | Processor   |
      +-------------+         +-------------+
             |
             v
      +------------------+
      | Downstream        |
      | Consumers         |
      +------------------+
```

---

## 🛠 Tech Stack

| Component         | Technology                |
| ----------------- | ------------------------- |
| Language          | C#                        |
| Framework         | .NET                      |
| Messaging         | Apache Kafka              |
| Cache             | Redis                     |
| Containerization  | Docker                    |
| Worker Processing | Background Services       |
| Serialization     | System.Text.Json          |
| Architecture      | Event-Driven Architecture |

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
│   ├── IRouteEngine.cs
│
├── Program.cs
├── Worker.cs
├── appsettings.json
└── Dockerfile
```

---

## ⚙️ How It Works

1. Producers publish events to Kafka topics.
2. Background workers consume events.
3. Messages are deserialized.
4. Routing logic determines where events should be forwarded.
5. Message processors execute forwarding operations.
6. Multiple consumers can be scaled horizontally.

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

* Retry policies
* Dead Letter Queue (DLQ)
* Redis-based deduplication
* Metrics and monitoring
* Prometheus integration
* OpenTelemetry tracing
* Multiple Kafka topics
* Dynamic routing rules
* Exactly-once processing
* Dashboard and observability

---

## Design Principles

* Scalability
* Reliability
* Loose Coupling
* Asynchronous Processing
* Extensibility
* High Throughput
* Fault Tolerance

---

## Inspiration

This project draws inspiration from modern event-driven systems and distributed messaging architectures used by companies such as Uber, Netflix, LinkedIn, and Amazon.

---

## License

MIT License
