# 🚀 Distributed Event Forwarder

A scalable and fault-tolerant distributed event processing platform inspired by modern asynchronous architectures. The system consumes events from Kafka, applies routing rules, enforces downstream rate limits, and reliably forwards messages while supporting retries, dead-letter queues, idempotency, and distributed locking.

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
* Dockerized deployment
* Extensible message pipeline

---

# 🏗 Architecture

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

Consumes messages from Kafka, performs idempotency checks, acquires distributed locks, and processes events.

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
| Architecture         | Event-Driven Architecture |

---

# Project Structure

```
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

# Future Enhancements

* Prometheus metrics
* OpenTelemetry tracing
* Batch processing
* Dynamic routing configuration
* Exponential backoff
* Circuit breaker support
* Dashboard and observability
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
