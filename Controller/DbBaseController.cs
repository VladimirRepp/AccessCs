using DocumentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Controller
{
    public class DbBaseController<T> where T : IBaseModel, new()
    {
        protected List<T> _data;
        protected DbManager _dbManager;
        protected string _tableName;

        public List<T> Data => _data;
        public string TableName => _tableName;
        public int Count => _data.Count;
        public T this[int i]
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

        public DbBaseController()
        {
            _dbManager = new DbManager();
            _data = new List<T>();
            _tableName = "default";
        }

        public void CloseConnection()
        {
            _dbManager.CloseConnection();
            _dbManager.CloseReader();
        }

        public T GetById(int id)
        {
            return _data.FirstOrDefault(x => x.Id == id);
        }

        public bool TryGetById(out T found, int id)
        {
            found = _data.FirstOrDefault(x => x.Id == id);
            return found != null;
        }

        /// <summary>
        /// Выборка всех данных
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool RequestLoading()
        {
            try
            {
                _dbManager.OpenConnection();

                string query = $"SELECT * FROM {_tableName}";
                _dbManager.ExecuteCommandReader(query);

                while (_dbManager.DataReader.Read())
                {
                    T item = new T();
                    item.SetData(_dbManager.DataReader);
                    _data.Add(item);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"DbBaseController.RequestLoading: {ex.Message}");
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return _data.Count > 0;
        }

        /// <summary>
        /// Запрос вставки данных
        /// </summary>
        /// <param name="d"> - данные для вставки</param>
        /// <returns> - результат запроса</returns>
        /// <exception cref="Exception"></exception>
        public bool RequstInsert(T d)
        {
            bool done = true;

            if (d == null)
            {
                throw new Exception($"DbBaseController.RequstAdding: Объект пустой!");
            }

            d.Id = GetLastId() + 1;

            try
            {
                _dbManager.OpenConnection();

                string query = $"INSERT INTO {_tableName} VALUES ({d.ToInsert})"; // строка запроса
                if (_dbManager.ExecuteCommand(query) == RequestStatus.Error)
                {
                    throw new Exception("DbBaseController.RequstInsert: Ошибка выполнения запроса!");
                }

                _data.Add(d);
            }
            catch (Exception ex)
            {
                throw new Exception($"DbBaseController.RequstInsert: {ex.Message}");
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return done;
        }

        /// <summary>
        /// Запрос обновления данных по Id
        /// </summary>
        /// <param name="d"> - параметр обновления</param>
        /// <returns></returns>
        public bool RequestUpdate(T d)
        {
            bool done = true;

            if (d == null)
            {
                throw new Exception("DbBaseController.RequestUpdate: Объект пустой!");
            }

            try
            {
                _dbManager.OpenConnection();

                string query = $"UPDATE {_tableName} SET {d.ToUpdate} WHERE Id = " + d.Id; // строка запроса
                if (_dbManager.ExecuteCommand(query) == RequestStatus.Error)
                {
                    throw new Exception("DbBaseController.RequestUpdate: Ошибка выполнения запроса!");
                }

                done = EditById(d);
            }
            catch (Exception ex)
            {
                throw new Exception($"DbBaseController.RequestUpdate: {ex.Message}");
            }
            finally
            {
                _dbManager.CloseConnection();
            }

            return done;
        }

        /// <summary>
        /// Запрос удаления записи по ID
        /// </summary>
        /// <param name="id"> - код</param>
        /// <returns></returns>
        public bool RequestDelete(int id)
        {
            bool done = true;

            try
            {
                _dbManager.OpenConnection();

                string query = $"DELETE FROM {_tableName} WHERE Id = " + id; ; // строка запроса
                if (_dbManager.ExecuteCommand(query) == RequestStatus.Error)
                {
                    throw new Exception("DbBaseController.RequestDelete: Ошибка выполнения запроса!");
                }

                done = RemoveById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"DbBaseController.RequestDelete: {ex.Message}");
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
            foreach (T d in _data)
            {
                if (d.Id > id)
                {
                    id = d.Id;
                }
            }

            return id;
        }

        private bool EditById(T d)
        {
            for(int i = 0; i < _data.Count; i++)
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
    }
}
