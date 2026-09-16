using Microsoft.Data.SqlClient;
using BusReservation.API.Models;

namespace BusReservation.API.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public List<Driver> GetAllDrivers()
        {
            List<Driver> drivers = new List<Driver>();

            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);

                string query = "SELECT DriverId, DriverName, LicenseNumber, Phone, Address, Status FROM Drivers";

                using SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    drivers.Add(new Driver
                    {
                        DriverId = Convert.ToInt32(reader["DriverId"]),
                        DriverName = reader["DriverName"].ToString() ?? "",
                        LicenseNumber = reader["LicenseNumber"].ToString() ?? "",
                        Phone = reader["Phone"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        Status = reader["Status"].ToString() ?? "Available"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

            return drivers;
        }

        public Driver? GetDriverById(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);

                string query = @"
                    SELECT DriverId, DriverName, LicenseNumber, Phone, Address, Status
                    FROM Drivers
                    WHERE DriverId = @DriverId";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverId", id);

                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Driver
                    {
                        DriverId = Convert.ToInt32(reader["DriverId"]),
                        DriverName = reader["DriverName"].ToString() ?? "",
                        LicenseNumber = reader["LicenseNumber"].ToString() ?? "",
                        Phone = reader["Phone"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        Status = reader["Status"].ToString() ?? "Available"
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        public int AddDriver(Driver driver)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);

                string query = @"
                    INSERT INTO Drivers
                    (DriverName, LicenseNumber, Phone, Address, Status)
                    VALUES
                    (@DriverName, @LicenseNumber, @Phone, @Address, @Status)";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverName", driver.DriverName);
                command.Parameters.AddWithValue("@LicenseNumber", driver.LicenseNumber);
                command.Parameters.AddWithValue("@Phone", driver.Phone);
                command.Parameters.AddWithValue("@Address", driver.Address);
                command.Parameters.AddWithValue("@Status", driver.Status);

                connection.Open();

                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateDriver(Driver driver)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);

                string query = @"
                    UPDATE Drivers
                    SET DriverName = @DriverName,
                        LicenseNumber = @LicenseNumber,
                        Phone = @Phone,
                        Address = @Address,
                        Status = @Status
                    WHERE DriverId = @DriverId";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverId", driver.DriverId);
                command.Parameters.AddWithValue("@DriverName", driver.DriverName);
                command.Parameters.AddWithValue("@LicenseNumber", driver.LicenseNumber);
                command.Parameters.AddWithValue("@Phone", driver.Phone);
                command.Parameters.AddWithValue("@Address", driver.Address);
                command.Parameters.AddWithValue("@Status", driver.Status);

                connection.Open();

                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteDriver(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);

                string query = "DELETE FROM Drivers WHERE DriverId = @DriverId";

                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@DriverId", id);

                connection.Open();

                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}