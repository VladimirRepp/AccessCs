using DocumentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Controller
{
    public class DbRoleController : DbBaseController<Role>
    {
        public DbRoleController() {
            _tableName = "Roles";
        }
    }
}
