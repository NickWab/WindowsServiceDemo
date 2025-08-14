# .NET Core Windows Service with Health Monitor

This project demonstrates a .NET Core Windows Service (`DemoService`) and a separate monitoring application (`HealRebootService`) that ensures the service remains running. If the `DemoService` stops for any reason, the `HealRebootService` will automatically restart it.

## Components

### 1. DemoService

A sample .NET Core application configured to run as a Windows Service. It's a simple worker that logs a message every second. The service name is configurable in its `appsettings.json` file.

### 2. HealRebootService

A console application that acts as a watchdog for the `DemoService`. It continuously checks the status of the `DemoService` and will automatically start it if it is not in the 'Running' state.

## How to Use

### Step 1: Configuration

Before running, ensure the `ServiceName` property in both configuration files matches:
- `DemoService/DemoService/DemoService/appsettings.json`
- `HealRebootService/HealRebootService/appsettings.json`

By default, it is set to `DemoServiceApp`.

```json
{
  "ServiceName": "DemoServiceApp"
}
```

### Step 2: Build the Solution

Build both projects using the .NET CLI:
```sh
dotnet build
```

### Step 3: Install and Run DemoService

1.  **Publish the service:**
    ```sh
    dotnet publish DemoService/DemoService/DemoService.sln -c Release -o ./publish/DemoService
    ```

2.  **Install the service:** Open a terminal **with Administrator privileges** and use the `sc.exe` command to create the Windows Service. Make sure to use the full path to the published executable.
    ```sh
    sc.exe create DemoServiceApp binPath="C:\full\path\to\your\project\publish\DemoService\DemoService.exe" start=auto
    ```
    *   Replace `DemoServiceApp` if you changed the service name.
    *   Replace the `binPath` with the absolute path to your published `DemoService.exe`.

3.  **Manage the service:**
    *   **Start:** `sc.exe start DemoServiceApp`
    *   **Stop:** `sc.exe stop DemoServiceApp`
    *   **Query Status:** `sc.exe query DemoServiceApp`
    *   You can also manage the service through the Windows Services GUI (`services.msc`).

### Step 4: Run the HealRebootService Monitor

1.  **Run the monitor:** Open another terminal **with Administrator privileges** and run the `HealRebootService` executable directly.
    ```sh
    dotnet run --project HealRebootService/HealRebootService/HealRebootService.csproj
    ```
    Alternatively, you can run the published executable:
    ```sh
    # First, publish the monitor
    dotnet publish HealRebootService/HealRebootService.sln -c Release -o ./publish/HealRebootService

    # Then, run it
    ./publish/HealRebootService/HealRebootService.exe
    ```

The `HealRebootService` console window will remain open, logging the status of the `DemoService` every minute and restarting it if necessary.

## Prerequisites

*   .NET 7 SDK (or newer)
*   Windows Operating System
*   Administrator privileges (for installing and monitoring the service)
