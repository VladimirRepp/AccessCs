using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Confectionery
{
    public class Client
    {
        // Поля класса
        public int Id;
        public string Name;
        public string Phone;
        public string Comment;

        // События класса
        public string ToStrINSERT => $"{Id}, '{Name}', '{Phone}', '{Comment}'";

        public string ToStrUPDATE => $"Name = '{Name}', Phone = '{Phone}', Comment = '{Comment}'";

        public string[] ToDataGridView
        {
            get
            {
                string[] strs = new string[3];
                strs[0] = Name;
                strs[1] = Phone;
                strs[2] = Comment;

                return strs;
            }

            private set { }
        }

        // Методы класса
        public Client(int Id = 0, string Name = "", string Phone = "", string Comment = "")
        {
            this.Id = Id;
            this.Name = Name;
            this.Phone = Phone;
            this.Comment = Comment;
        }

        public Client(OleDbDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.Name = reader["Name"].ToString();
            this.Phone = reader["Phone"].ToString();
            this.Comment = reader["Comment"].ToString();
        }
    }

    class DClients
    {
        // Поля класса
        private DatabaseManager _dbManager;
        private string _tableName;
        private List<Client> _data;

        // Свойства класса
        public List<Client> Data => _data;
        public int Count => _data.Count;
        public Client this[int i]
        {
            get
            {
                if (i < 0 || i >= _data.Count)
                    throw new Exception("Индекс вне диапазона!");

                return _data[i];
            }
            set
            {
                if (i < 0 || i >= _data.Count)
                    throw new Exception("Индекс вне диапазона!");

                if (value == null)
                    throw new Exception("Объект присвоения пуст!");

                _data[i] = value;
            }
        }
        public string TableName => _tableName;

        // Методы класса
        public DClients()
        {
            _dbManager = new DatabaseManager();
            _data = new List<Client>();
            _tableName = "clients";
        }

        public bool Query_Load()
        {
            try
            {
                _dbManager.OpenConnection();

                string query = $"SELECT * FROM {_tableName}"; // строка запроса
                _dbManager.ExecuteCommandReader(query);

                while (_dbManager.DataReader.Read())
                {
                    _data.Add(new Client(_dbManager.DataReader));
                }

                Logger.GetInstance().Notify($"Таблица \'{_tableName}\' загружена!");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Notify($"Ошибка выполнения запроса в таблице \'{_tableName}\'!");
                throw ex;
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return _data.Count > 0;
        }       

        public bool Query_Add(Client d)
        {
            bool done = true;

            if (d == null)
            {
                throw new Exception("Объект пустой!");
            }

            d.Id = GetLastId() + 1;

            try
            {
                _dbManager.OpenConnection();

                string query = $"INSERT INTO {_tableName} VALUES ({d.ToStrINSERT})"; // строка запроса
                if (_dbManager.ExecuteCommand(query) == RequestStatus.Error)
                {
                    throw new Exception("Ошибка выполнения запроса!");
                }

                _data.Add(d);
                Logger.GetInstance().Notify($"Данные добавлены в таблицу \'{_tableName}\'!");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Notify($"Ошибка выполнения запроса в таблице \'{_tableName}\'!");
                throw ex;
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return done;
        }

        public bool Query_Update(Client d)
        {
            bool done = true;

            if (d == null)
            {
                throw new Exception("Объект пустой!");
            }

            try
            {
                _dbManager.OpenConnection();

                string query = $"UPDATE {_tableName} SET {d.ToStrUPDATE} WHERE Id = " + d.Id; // строка запроса
                if (_dbManager.ExecuteCommand(query) == RequestStatus.Error)
                {
                    throw new Exception("Ошибка выполнения запроса!");
                }

                done = EditById(d);
                Logger.GetInstance().Notify($"Данные изменены в таблице \'{_tableName}\'!");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Notify($"Ошибка выполнения запроса в таблице \'{_tableName}\'!");
                throw ex;
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return done;
        }

        public bool Query_Delete(int id)
        {
            bool done = true;

            try
            {
                _dbManager.OpenConnection();

                string query = $"DELETE FROM {_tableName} WHERE Id = " + id; ; // строка запроса
                if (_dbManager.ExecuteCommand(query) == RequestStatus.Error)
                {
                    throw new Exception("Ошибка выполнения запроса!");
                }

                done = RemoveById(id);
                Logger.GetInstance().Notify($"Данные удалены в таблице \'{_tableName}\'!");
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Notify($"Ошибка выполнения запроса в таблице \'{_tableName}\'!");
                throw ex;
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return done;
        }

        public void CloseConnection()
        {
            _dbManager.CloseConnection();
            _dbManager.CloseReader();
        }

        private bool EditById(Client d)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].Id == d.Id)
                {
                    _data[i] = d;
                    return true;
                }
            }

            return false;
        }

        private bool RemoveById(int id)
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].Id == id)
                {
                    _data.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private int GetLastId()
        {
            int id = 0;
            foreach (Client d in _data)
            {
                if (d.Id > id)
                {
                    id = d.Id;
                }
            }

            return id;
        }
    }
}
