using DocumentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Controller
{
    public class DbUserController : DbBaseController<User>
    {
        private static User SAVED_USER;

        public User SavedUser
        {
            get => SAVED_USER;
            set => SAVED_USER = value;  
        }

        public DbUserController() {
            _tableName = "users";
        }
    }
}
