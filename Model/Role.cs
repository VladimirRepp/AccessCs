using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Model
{
    public class Role : IBaseModel
    {
        private int _id;
        public int Id { get => _id; set => _id = value; }
        public string Name;
        public string AccessRights;

        public string ToInsert => $"{Id}, '{Name}', '{AccessRights}'";
        public string ToUpdate => $"RoleName = '{Name}', AccessRights = '{AccessRights}'";
        public string[] ToStrArray 
        {
            get
            {
                return new string[] { Name, AccessRights};
            }
        }

        public Role() { }
        public Role(int Id, string Name, string AccessRights) {
            this.Id = Id;
            this.Name = Name;
            this.AccessRights = AccessRights;
        }
        public Role(OleDbDataReader reader)
        {
           SetData(reader);
        }

        public void SetData(OleDbDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.Name = Convert.ToString(reader["RoleName"]);
            this.AccessRights = Convert.ToString(reader["AccessRights"]);
        }
    }
}
