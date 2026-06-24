# 🚀 Distributed Event Forwarder

A scalable and fault-tolerant distributed event processing platform inspired by modern asynchronous architectures. The system consumes events from Kafka, applies routing rules, enforces downstream rate limits, and reliably forwards messages while supporting retries, dead-letter queues, idempotency, distributed locking, and observability.

Built with **.NET, Kafka, Redis, Docker, and Background Workers**.

---

# ✨ Features

* Event-driven architecture
* Kafka-based asynchronous messaging
* Background worker consumers
* Dynamic routing engine
* Horizontal scalability
* Redis-based distributed locking
* Idempotent message processing
* Duplicate message prevention
* Retry mechanism with delayed retries
* Dead Letter Queue (DLQ)
* Redis Sorted Set retry scheduling
* Token Bucket rate limiting
* Automatic token refill worker
* Atomic Redis Lua scripts
* Fault-tolerant processing
* Prometheus metrics integration
* Real-time observability
* Dockerized deployment
* Extensible message pipeline

---

# 🏗 Architecture

```text
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
              +----------+----------+
              |                     |
              v                     v
      +----------------+   +----------------+
      | Distributed    |   | Idempotency    |
      | Lock (Redis)   |   | Check (Redis)  |
      +----------------+   +----------------+
                         |
                         v
                +-------------------+
                | Message Processor |
                +-------------------+
                         |
                         v
                +-------------------+
                | Token Bucket Rate |
                | Limiter (Redis)   |
                +-------------------+
                         |
            +------------+------------+
            |                         |
            v                         v
      Tokens Available          Tokens Exhausted
            |                         |
            |                         v
            |                 Retry Queue (Redis)
            |                         |
            |                  Retry Worker
            |                         |
            |                     Kafka Topic
            |
            v
     Downstream Systems
 (Inventory, Email, etc.)
            |
            v
         Success
            |
            v
      Commit Offset

After Max Retries
            |
            v
      Kafka Dead Letter Queue
```

---

# ⚙ Components

## Consumer Worker

Consumes messages from Kafka, performs idempotency checks, acquires distributed locks, processes events, and exposes Prometheus metrics.

## Retry Worker

Schedules delayed retries using Redis Sorted Sets and republishes messages back to Kafka.

## Token Refiller Worker

Refills token buckets periodically to enforce downstream service limits.

## Rate Limiter

Uses Redis and Lua scripts to atomically consume tokens for multiple destinations.

---

# 🛠 Tech Stack

| Component            | Technology                |
| -------------------- | ------------------------- |
| Language             | C#                        |
| Framework            | .NET                      |
| Messaging            | Apache Kafka              |
| Cache & Coordination | Redis                     |
| Retry Scheduling     | Redis Sorted Sets         |
| Distributed Locking  | Redis                     |
| Rate Limiting        | Token Bucket Algorithm    |
| Atomic Operations    | Redis Lua Scripts         |
| Worker Processing    | Background Services       |
| Serialization        | System.Text.Json          |
| Containerization     | Docker                    |
| Monitoring           | Prometheus                |
| Architecture         | Event-Driven Architecture |

---

# Project Structure

```text
Distributed-Event-Forwarder

├── UForwarderConsumer
│
├── RetryWorker
│
├── TokenRefiller
│
├── Dtos
│
├── Services
│     ├── DlqService
│     ├── RetryService
│     ├── IdempotencyService
│     ├── RateLimitingService
│     ├── DistributedLockService
│     ├── MessageProcessingService
│     └── RoutineEngine
│
├── Workers
│
├── Dockerfile
│
├── prometheus.yml
│
└── docker-compose
```

---

# Reliability Mechanisms

### Idempotency

Prevents duplicate processing.

### Distributed Locking

Ensures only one consumer processes a message.

### Retry Queue

Failed messages are scheduled in Redis Sorted Sets and retried later.

### Dead Letter Queue

Messages exceeding the retry limit are forwarded to Kafka DLQ topics.

### Token Bucket Rate Limiting

Prevents downstream systems from being overwhelmed.

### Automatic Token Refill

Dedicated worker replenishes tokens periodically.

### At-Least-Once Delivery

Kafka + retries guarantee reliable delivery.

---

# 📊 Observability

The platform exposes Prometheus-compatible metrics for operational monitoring and visibility.

### Available Metrics

* messages_processed_total
* messages_failed_total
* messages_retried_total
* messages_dlq_total

### Monitoring Flow

```text
UForwarder
    ↓
Prometheus
    ↓
Grafana
```

Prometheus continuously scrapes application metrics while Grafana can be used to build real-time dashboards and visualize system behavior.

---

# Future Enhancements

* OpenTelemetry tracing
* Batch processing
* Dynamic routing configuration
* Exponential backoff
* Circuit breaker support
* Grafana dashboards
* Event replay capability
* Multi-topic support
* Multiple consumer replicas
* Kubernetes deployment

---

# Design Principles

* Scalability
* Fault Tolerance
* Reliability
* Loose Coupling
* Extensibility
* High Throughput
* At-Least-Once Delivery
* Idempotent Processing
* Backpressure Control
* Asynchronous Processing

---

# Inspiration

Inspired by distributed event-driven architectures used by:

* Uber
* Netflix
* LinkedIn
* Amazon
* Stripe
