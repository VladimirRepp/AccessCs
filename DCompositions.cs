using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Publishing_house
{
    public class Composition
    {
        // Поля класса
        public int Id;
        public string Title;
        public string Description;
        public string Autor;
        public string Ganre;

        // События класса
        public string ToINTO => $"{Id}, '{Title}', '{Description}', '{Autor}', ' {Ganre}'";

        public string ToUPDATE => $"Title = '{Title}', Description = '{Description}', Autor = '{Autor}', Ganre = '{Ganre}'";

        public string[] ToDataGridView
        {
            get
            {
                string[] strs = new string[4];
                strs[0] = Title;
                strs[1] = Description;
                strs[2] = Autor;
                strs[3] = Ganre;

                return strs;
            }

            private set { }
        }

        // Методы класса
        public Composition() { }
        public Composition(int Id, string Title, string Description, string Autor, string Ganre)
        {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.Autor = Autor;
            this.Ganre = Ganre;
        }
        public Composition(OleDbDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.Title = reader["Title"].ToString();
            this.Description = reader["Description"].ToString();
            this.Autor = reader["Autor"].ToString();
            this.Ganre = reader["Ganre"].ToString();
        }
    }

    public class DCompositions
    {
        // Поля класса
        private DatabaseManager _dbManager;
        private string _tableName;
        private List<Composition> _data;

        // Свойства класса
        public List<Composition> Data => _data;
        public int Count => _data.Count;
        public Composition this[int i]{
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
        public DCompositions()
        {
            _dbManager = new DatabaseManager();
            _data = new List<Composition>();
            _tableName = "Compositions";
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
                    _data.Add(new Composition(_dbManager.DataReader));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return _data.Count > 0;
        }

        public void CloseConnection()
        {
            _dbManager.CloseConnection();
            _dbManager.CloseReader();
        }

        public bool Query_Add(Composition d)
        {
            bool done = true;

            if(d == null)
            {
                throw new Exception("Объект пустой!");
            }

            d.Id = GetLastId() + 1;

            try
            {
                _dbManager.OpenConnection();

                string query = $"INSERT INTO {_tableName} VALUES ({d.ToINTO})"; // строка запроса
                if( _dbManager.ExecuteCommand(query) == RequestStatus.Error){
                    throw new Exception("Ошибка выполнения запроса!");
                }

                _data.Add(d);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return done;
        }

        public bool Query_Update(Composition d)
        {
            bool done = true;

            if (d == null)
            {
                throw new Exception("Объект пустой!");
            }

            try
            {
                _dbManager.OpenConnection();

                string query = $"UPDATE {_tableName} SET {d.ToUPDATE} WHERE Id = " + d.Id; // строка запроса
                if (_dbManager.ExecuteCommand(query) == RequestStatus.Error)
                {
                    throw new Exception("Ошибка выполнения запроса!");
                }

               done = _data.EditById(d);
            }
            catch (Exception ex)
            {
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

                done = _data.RemoveById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return done;
        }

        private int GetLastId()
        {
            int id = 0;
            foreach(Composition d in _data)
            {
                if(d.Id > id)
                {
                    id = d.Id;
                }
            }

            return id;
        }
    }
}
