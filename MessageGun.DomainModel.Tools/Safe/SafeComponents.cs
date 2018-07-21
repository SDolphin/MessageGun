using MessageGun.DomainModel.Tools.Log;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.Tools.Safe
{
    class SafeComponents
    {
        private ILogger log = LogManager.GetLogger(typeof(SafeComponents));

        public void SafeAction(Action act)
        {
            try
            {
                act();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
        }



    }
}
