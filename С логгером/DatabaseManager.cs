using System;
using System.Data.OleDb;

namespace Confectionery
{
    enum RequestStatus
    {
        Empty = 0,
        Error,
        Done
    }

    class DatabaseManager
    {
        // Поля класса
        private string _connectionString = "provider=Microsoft.Jet.OLEDB.4.0;Data Source=Database.mdb"; // строка соеденения
        private OleDbConnection _dbConnection; 
        private OleDbDataReader _dbReader;

        // Свойства класса
        public OleDbConnection Connection => _dbConnection;
        public OleDbDataReader DataReader => _dbReader;

        // Методы класса
        public DatabaseManager()
        {
            _dbConnection = new OleDbConnection(_connectionString); // создаем соеденение
        }

        public void OpenConnection()
        {
            if (_dbConnection.State != System.Data.ConnectionState.Open)
                _dbConnection.Open();
        }

        public void CloseConnection()
        {
            if (_dbConnection.State != System.Data.ConnectionState.Closed)
                _dbConnection.Close();
        }

        public void CloseReader()
        {
            if (!_dbReader.IsClosed)
                _dbReader.Close();
        }

        public RequestStatus ExecuteCommand(string query)
        {
            RequestStatus status = RequestStatus.Empty;

            try
            {
                OpenConnection();
                OleDbCommand dbCommand = new OleDbCommand(query, _dbConnection); // команда

                // Выполнение команды
                if (dbCommand.ExecuteNonQuery() == 1)
                {
                    status = RequestStatus.Done;
                }
                else
                {
                    status = RequestStatus.Error;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }

            return status;
        }

        public OleDbDataReader ExecuteCommandReader(string query)
        {
            try
            {
                OpenConnection();
                OleDbCommand dbCommand = new OleDbCommand(query, _dbConnection); // команда
                _dbReader = dbCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //CloseConnection();
            }

            return _dbReader;
        }
    }
}
