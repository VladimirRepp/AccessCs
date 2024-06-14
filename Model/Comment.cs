using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocumentManagement.Model
{
    public class Comment : IBaseModel
    {
        private int _id;
        public int Id { get => _id; set => _id = value; }
        public int IdDoc;
        public int IdUser;
        public string CreateDate;
        public string Text;

        public string ToInsert => $"{Id}, {IdDoc}, {IdUser}, '{CreateDate}', '{Text}'";
        public string ToUpdate => $"IdDoc = {IdDoc}, IdUser = {IdUser}, CreateDate = '{CreateDate}', Comment = '{Text}'";
        public string[] ToStrArray
        {
            get
            {
                return new string[] { IdDoc.ToString(), IdUser.ToString(), CreateDate, Text };
            }
        }

        public Comment() { }
        public Comment(int Id, int IdDoc, int IdUser, string CreateDate, string MyComment)
        {
            this.Id = Id;
            this.IdDoc = IdDoc;
            this.IdUser = IdUser;
            this.CreateDate = CreateDate;
            this.Text = MyComment;
        }
        public Comment(OleDbDataReader reader)
        {
            SetData(reader);
        }

        public void SetData(OleDbDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["Id"]);
            this.IdDoc = Convert.ToInt32(reader["IdDoc"]);
            this.IdUser = Convert.ToInt32(reader["IdUser"]);
            this.CreateDate = Convert.ToString(reader["CreateDate"]);
            this.Text = Convert.ToString(reader["Comment"]);
        }
    }
}
