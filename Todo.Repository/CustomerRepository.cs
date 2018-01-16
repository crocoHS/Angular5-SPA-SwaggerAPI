using System;
using System.Data;
using System.Data.SqlClient;
using Todo.Model;

namespace Todo.Repository
{
    public static class CustomerRepository
    {
        public static DataTable GetAll()
        {
            SqlDataReader reader = null;
            DataTable dt = new DataTable();

            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_GetCustomers", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader != null) dt.Load(reader);
                }
                catch (Exception)
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return dt;
        }

        public static DataTable Get(int customerId)
        {
            SqlDataReader reader = null;
            DataTable dt = new DataTable();

            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_GetCustomer", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@CustomerId", customerId);

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader != null) dt.Load(reader);
                }
                catch (Exception)
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return dt;
        }

        public static int Add(CustomerDT customer)
        {
            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_AddCustomer", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@OwnerId", customer.OwnerId);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@Gender", customer.Gender);
                command.Parameters.AddWithValue("@RegistrationDate", customer.RegistrationDate);
                command.Parameters.AddWithValue("@TechnologyList", customer.TechnologyList.ToJson());

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public static int Update(CustomerDT customer)
        {
            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_UpdateCustomer", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@CustomerID", customer.ID);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@Gender", customer.Gender);
                command.Parameters.AddWithValue("@RegistrationDate", customer.RegistrationDate);
                command.Parameters.AddWithValue("@TechnologyList", customer.TechnologyList.ToJson());

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public static int Delete(int customerId)
        {
            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_DeleteCustomer", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@CustomerID", customerId);

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public static DataTable GetTechnologyList()
        {
            SqlDataReader reader = null;
            DataTable dt = new DataTable();

            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_GetTechnologyList", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader != null) dt.Load(reader);
                }
                catch (Exception)
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return dt;
        }

        public static DataTable GetAccountSummaryAll()
        {
            SqlDataReader reader = null;
            DataTable dt = new DataTable();

            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_GetAccounts", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader != null) dt.Load(reader);
                }
                catch (Exception)
                {
                    if (reader != null)
                        reader.Close();
                }
            }

            return dt;
        }

        public static int AddAccount(int customerId, int accountType, string accountNumber, string accountName, double balance)
        {
            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_AddAccount", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@CustomerId", customerId);
                command.Parameters.AddWithValue("@AccountType", accountType);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                command.Parameters.AddWithValue("@AccountName", accountName);
                command.Parameters.AddWithValue("@Balance", balance);

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public static int DeleteAccount(int accountId)
        {
            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_DeleteAccount", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@AccountId", accountId);

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public static int AddTechnology(TechnologyDT technology)
        {
            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_AddTechnology", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@TechnologyName", technology.TechnologyName);

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public static int DeleteTechnology(string name)
        {
            using (SqlConnection connection = DBConnection.NewConnection())
            {
                SqlCommand command = new SqlCommand("sp_DeleteTechnology", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Name", name);

                try
                {
                    connection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
    }
}