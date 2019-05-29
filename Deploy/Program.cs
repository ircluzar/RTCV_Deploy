using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCV.Deploy
{
    class Program
    {
        static void Main(string[] args)
        {

            Builder.LoadPaths();
            Builder.CheckDirectories();
            Builder.LoadBuildParams();
            Builder.BuildAll();
            Builder.BuildToZip();

        }


    }


}
