using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocumentManagement.Model
{
    public class Doc : IBaseModel
    {
        private int _id;
        public int Id { get => _id; set => _id = value; }
        public string Type;
        public int NumberDoc;
        public string CreateDate;
        public string Status;
        public string Content;
        public string Path;

        public string ToInsert => $"{Id}, '{Type}', {NumberDoc}, '{CreateDate}', '{Status}', '{Content}', '{Path}'";
        public string ToUpdate => $"Type = '{Type}', NumberDoc = {NumberDoc}, CreateDate = '{CreateDate}, Content = '{Content}, Path = '{Path}";
        public string[] ToStrArray
        {
            get
            {
                return new string[] { Type, NumberDoc.ToString(), CreateDate, Status, Content, Path };
            }
        }

        public Doc() { }
        public Doc(int Id, string Type, int NumberDoc, string CreateDate, string Status, string Content, string Path)
        {
            this.Id = Id;
            this.Type = Type;
            this.NumberDoc = NumberDoc;
            this.CreateDate = CreateDate;
            this.Status = Status;
            this.Content = Content;
            this.Path = Path;
        }
        public Doc(OleDbDataReader reader)
        {
            SetData(reader);
        }

        public void SetData(OleDbDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.Type = Convert.ToString(reader["Type"]);
            this.NumberDoc = Convert.ToInt32(reader["NumberDoc"]);
            this.CreateDate = Convert.ToString(reader["CreateDate"]);
            this.Status = Convert.ToString(reader["Status"]);
            this.Content = Convert.ToString(reader["Content"]);
            this.Path = Convert.ToString(reader["Path"]);
        }
    }
}
