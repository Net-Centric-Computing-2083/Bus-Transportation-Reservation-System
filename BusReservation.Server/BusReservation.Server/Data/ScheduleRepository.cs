using Microsoft.Data.SqlClient;
using System.Data;
using BusReservation.Server.Models;

namespace BusReservation.Server.Data;

public class ScheduleRepository
{
    private readonly IConfiguration _configuration;

    public ScheduleRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));
    }

    // ---------- GET ALL ----------
    public async Task<List<Schedule>> GetAllAsync()
    {
        var schedules = new List<Schedule>();

        using var connection = GetConnection();

        const string sql = @"
            SELECT
                ScheduleId,
                BusId,
                RouteId,
                DriverId,
                DepartureDate,
                DepartureTime,
                ArrivalTime,
                Fare,
                Status
            FROM Schedules
            ORDER BY DepartureDate, DepartureTime";

        using var command = new SqlCommand(sql, connection);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            schedules.Add(new Schedule
            {
                ScheduleId = reader.GetInt32(reader.GetOrdinal("ScheduleId")),
                BusId = reader.GetInt32(reader.GetOrdinal("BusId")),
                RouteId = reader.GetInt32(reader.GetOrdinal("RouteId")),
                DriverId = reader.GetInt32(reader.GetOrdinal("DriverId")),

                DepartureDate =
                    reader.GetDateTime(reader.GetOrdinal("DepartureDate")),

                DepartureTime =
                    reader.GetTimeSpan(reader.GetOrdinal("DepartureTime")),

                ArrivalTime =
                    reader.IsDBNull(reader.GetOrdinal("ArrivalTime"))
                    ? null
                    : reader.GetTimeSpan(reader.GetOrdinal("ArrivalTime")),

                Fare =
                    reader.GetDecimal(reader.GetOrdinal("Fare")),

                Status =
                    reader.GetString(reader.GetOrdinal("Status"))
            });
        }

        return schedules;
    }

    // ---------- GET BY ID ----------
    public async Task<Schedule?> GetByIdAsync(int scheduleId)
    {
        using var connection = GetConnection();

        const string sql = @"
            SELECT
                ScheduleId,
                BusId,
                RouteId,
                DriverId,
                DepartureDate,
                DepartureTime,
                ArrivalTime,
                Fare,
                Status
            FROM Schedules
            WHERE ScheduleId = @ScheduleId";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@ScheduleId", scheduleId);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Schedule
            {
                ScheduleId = reader.GetInt32(reader.GetOrdinal("ScheduleId")),
                BusId = reader.GetInt32(reader.GetOrdinal("BusId")),
                RouteId = reader.GetInt32(reader.GetOrdinal("RouteId")),
                DriverId = reader.GetInt32(reader.GetOrdinal("DriverId")),

                DepartureDate =
                    reader.GetDateTime(reader.GetOrdinal("DepartureDate")),

                DepartureTime =
                    reader.GetTimeSpan(reader.GetOrdinal("DepartureTime")),

                ArrivalTime =
                    reader.IsDBNull(reader.GetOrdinal("ArrivalTime"))
                    ? null
                    : reader.GetTimeSpan(reader.GetOrdinal("ArrivalTime")),

                Fare =
                    reader.GetDecimal(reader.GetOrdinal("Fare")),

                Status =
                    reader.GetString(reader.GetOrdinal("Status"))
            };
        }

        return null;
    }

    // ---------- CREATE ----------
    public async Task<int> CreateAsync(Schedule schedule)
    {
        using var connection = GetConnection();

        const string sql = @"
            INSERT INTO Schedules
            (
                BusId,
                RouteId,
                DriverId,
                DepartureDate,
                DepartureTime,
                ArrivalTime,
                Fare,
                Status
            )
            VALUES
            (
                @BusId,
                @RouteId,
                @DriverId,
                @DepartureDate,
                @DepartureTime,
                @ArrivalTime,
                @Fare,
                @Status
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@BusId", SqlDbType.Int)
            .Value = schedule.BusId;

        command.Parameters.Add("@RouteId", SqlDbType.Int)
            .Value = schedule.RouteId;

        command.Parameters.Add("@DriverId", SqlDbType.Int)
            .Value = schedule.DriverId;

        command.Parameters.Add("@DepartureDate", SqlDbType.Date)
            .Value = schedule.DepartureDate;

        command.Parameters.Add("@DepartureTime", SqlDbType.Time)
            .Value = schedule.DepartureTime;

        command.Parameters.Add("@ArrivalTime", SqlDbType.Time)
            .Value = schedule.ArrivalTime.HasValue
                ? schedule.ArrivalTime.Value
                : DBNull.Value;

        command.Parameters.Add("@Fare", SqlDbType.Decimal)
            .Value = schedule.Fare;

        command.Parameters.Add("@Status", SqlDbType.VarChar, 20)
            .Value = schedule.Status;

        await connection.OpenAsync();

        return (int)await command.ExecuteScalarAsync();
    }

    // ---------- UPDATE ----------
    public async Task<bool> UpdateAsync(Schedule schedule)
    {
        using var connection = GetConnection();

        const string sql = @"
            UPDATE Schedules
            SET
                BusId = @BusId,
                RouteId = @RouteId,
                DriverId = @DriverId,
                DepartureDate = @DepartureDate,
                DepartureTime = @DepartureTime,
                ArrivalTime = @ArrivalTime,
                Fare = @Fare,
                Status = @Status
            WHERE ScheduleId = @ScheduleId";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@ScheduleId",
            schedule.ScheduleId);

        command.Parameters.AddWithValue("@BusId",
            schedule.BusId);

        command.Parameters.AddWithValue("@RouteId",
            schedule.RouteId);

        command.Parameters.AddWithValue("@DriverId",
            schedule.DriverId);

        command.Parameters.AddWithValue("@DepartureDate",
            schedule.DepartureDate);

        command.Parameters.AddWithValue("@DepartureTime",
            schedule.DepartureTime);

        command.Parameters.Add("@ArrivalTime", SqlDbType.Time)
            .Value = schedule.ArrivalTime.HasValue
                ? schedule.ArrivalTime.Value
                : DBNull.Value;

        command.Parameters.AddWithValue("@Fare",
            schedule.Fare);

        command.Parameters.AddWithValue("@Status",
            schedule.Status);

        await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync() > 0;
    }

    // ---------- DELETE (soft delete: marks as Cancelled) ----------
    public async Task<bool> DeleteAsync(int scheduleId)
    {
        using var connection = GetConnection();

        const string sql = @"
            UPDATE Schedules
            SET Status = 'Cancelled'
            WHERE ScheduleId = @ScheduleId";

        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@ScheduleId", scheduleId);

        await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync() > 0;
    }
}