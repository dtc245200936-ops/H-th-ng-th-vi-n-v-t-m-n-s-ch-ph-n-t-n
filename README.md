# Hệ thống Thư viện và Đặt mượn sách phân tán (Đề tài 20)

Hệ thống cho phép người dùng tra cứu sách, gửi yêu cầu đặt mượn và theo dõi trạng thái xử lý bất đồng bộ qua RabbitMQ.

## Kiến trúc hệ thống
`Client` -> `Library.API` -> `RabbitMQ` -> `Reservation.Worker` -> `PostgreSQL`

## Công nghệ sử dụng
- **Nền tảng:** C# / .NET 10 (ASP.NET Core Minimal API, Worker Service)
- **Message Broker:** RabbitMQ
- **Cơ sở dữ liệu:** PostgreSQL
- **Triển khai:** Docker & Docker Compose