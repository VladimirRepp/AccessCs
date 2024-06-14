using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Model
{
    public class User : IBaseModel
    {
        private int _id;
        public int Id { get => _id; set => _id = value; }
        public string Name;
        public string Password;
        public int IdRole;

        public string ToInsert => $"{Id}, '{Name}', '{Password}', {IdRole}";
        public string ToUpdate => $"UserName = '{Name}', UserPassword = '{Password}', IdRole = {IdRole}";
        public string[] ToStrArray
        {
            get
            {
                return new string[] { Name, Password, IdRole.ToString() };
            }
        }

        public User() { }
        public User(int Id, string Name, string Password, int IdRole)
        {
            this.Id = Id;
            this.Name = Name;
            this.Password = Password;
            this.IdRole = IdRole;
        }
        public User(OleDbDataReader reader)
        {
            SetData(reader);
        }

        public void SetData(OleDbDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.IdRole = Convert.ToInt32(reader["IdRole"]);
            this.Name = Convert.ToString(reader["UserName"]);
            this.Password = Convert.ToString(reader["UserPassword"]);
        }
    }
}
