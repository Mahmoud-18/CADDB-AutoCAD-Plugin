using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD_DB_Project
{
    public abstract class ICadCommand
    {
        /// <summary>
        /// Execute a function in cad application
        /// </summary>
        public abstract void Execute();
    }
}
