using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace MessageGun.DomainModel.DB.Common
{
    public class DBProperties
    {

        private string _dataSource;
        private string _intitalCatalog;
        public DBProperties(string dataSource, string initialCatalog)
        {

            _dataSource = dataSource;
            _intitalCatalog = initialCatalog;
           
        }
        
        public string DataSource
        {
            get => _dataSource;
            set => _dataSource = value;
        }

        public string InitialCatalog
        {
            get => _intitalCatalog;
            set => _intitalCatalog = value;
        }

    }
}
