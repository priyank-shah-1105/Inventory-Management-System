using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CommonLayer;
using DataLayer.DatabaseModel;

namespace DataLayer
{
    public class SelectionList
    {
        public static List<UsersMaster> UserList(object selectedValue = null)
        {
            return new GenericRepository<UsersMaster>().GetEntities().OrderBy(p => p.Name).ToList();
        }

        public static List<BranchMaster> BranchList(object selectedValue = null)
        {
            return new GenericRepository<BranchMaster>().GetEntities().OrderBy(p => p.Name).ToList();
        }
    }
}
