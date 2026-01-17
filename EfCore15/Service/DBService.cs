using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCORE15.Models;

namespace EFCORE15.Service
{
    public class DBService
    {
        private ElectronicsStoreContext context;
        public ElectronicsStoreContext Context => context;

        private static DBService? instance;

        public static DBService Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBService();
                return instance;
            }
        }
        private DBService()
        {
            context = new ElectronicsStoreContext();
        }
    }
}
