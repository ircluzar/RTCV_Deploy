using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deploy
{
    class BuildParams
    {
        public string productName = null;
        public string initials = null;
        public string version = null;
        public string pathFromProjectsToBuild = null;
        public string[] BuildDirDeleteDirectories = new string[0];
        public bool dupeDllCleanup = true;
    }
}
