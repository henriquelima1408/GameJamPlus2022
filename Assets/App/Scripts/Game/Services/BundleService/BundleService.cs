using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Game.Services
{
    public class BundleService : IBundleService
    {
        public bool IsInitialized => true;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
