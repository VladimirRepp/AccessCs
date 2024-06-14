using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Model
{
    public interface IBaseModel
    {
        int Id { get; set; }

        string ToInsert { get; }
        string ToUpdate { get; }
        string[] ToStrArray { get; }

        void SetData(OleDbDataReader reader);
    }
}
