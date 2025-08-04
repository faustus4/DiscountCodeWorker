# DiscountWorker

**DiscountWorker** is a .NET 8 Worker Service that processes discount code operations in the background. It uses Entity Framework Core for data persistence, a layered architecture for maintainability, and background services for continuous processing.

## Features

- Background TCP server for processing discount code requests
- Generate and use discount codes via business logic services
- SQL Server data storage with Entity Framework Core
- Clean, extensible architecture

## Project Structure

- `Domain`: Business entities and interfaces
- `Application`: Business logic and service interfaces
- `Infrastructure`: Data access, network communication, and repository implementations
- `DiscountWorker`: Worker entry point and service configuration

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server instance (local or remote)
- (Optional) Visual Studio 2022 or later

## Getting Started

1. **Clone the repository:**
2. **Configure the database connection:**
- Update the `DefaultConnection` string in `appsettings.json` to point to your SQL Server instance.

3. **Run the worker service:**

## Usage

The worker runs as a background service and listens for TCP messages. Messages should be sent in JSON format for discount code operations.

### Message Types

- `Generate` = 0 : Generate new discount codes
- `UseCode` = 1 : Use (redeem) a discount code

### Example: Generate Discount Codes

**Request:**
```json
{
  "Type": 0,
  "Params": {
    "Count": 10,
    "Length": 7
  }
}
```

# Response
```json
{
  "Ressult": true
}
```

### Example: Use a Discount Code

**Request:**
```json
{
  "Type": 1,
  "Params": {
    "Code": "ABC12345"
  }
}
```

# Response
```json
{
  "Ressult": true
}
```

> The TCP server expects each message as a single JSON object. The response will indicate success or failure.

## Extending

- Add new message types by updating the `MessageProcessor` and related DTOs.
- Implement additional business logic in the `Application` layer.

---

**Note:**  
Ensure your SQL Server is running and accessible before starting the worker. For development, you may use SQL Server Express or a Docker container