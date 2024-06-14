using DocumentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Controller
{
    public class DbCommentController : DbBaseController<Comment>
    {
        public DbCommentController() {
            _tableName = "Comments";
        }
    }
}
