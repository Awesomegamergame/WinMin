using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinMin.Functions
{
    public class WMManifest
    {
        public string manifest { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string version { get; set; }
        public string cover { get; set; }
        public string description { get; set; }
        public IList<string> patchFiles { get; set; }
        public List<string> supportedVersions = new List<string>
        {
            "1.0.0",
            "1.0.1"
        };
    }
}
