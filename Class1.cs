using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoD2Saver
{
    internal class ClassData
    {
        public static readonly string version = "1.1";
        public static readonly string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\StateOfDecay2";
        public static string extended_mode = string.Empty;
        public static string user_path = string.Empty;
    }
}
