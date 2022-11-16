using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinMin.Functions
{
    internal class ShortcutFix
    {
        public static string ResolveShortcut(string filename)
        {
            ShellLink link = new ShellLink();
            ((IPersistFile)link).Load(filename, STGM_READ);
            StringBuilder sb = new StringBuilder(MAX_PATH);
            WIN32_FIND_DATAW data = new WIN32_FIND_DATAW();
            ((IShellLinkW)link).GetPath(sb, sb.Capacity, out data, 0);
            string final_string = sb.ToString();
            if (final_string.Length == 0)
                final_string = filename;
            string new_path = Validate_Shortcut_Path(final_string, "SysWOW64", "System32");
            if (File.Exists(new_path) == true && File.Exists(final_string) == false)
            {
                final_string = new_path;
            }
            new_path = Validate_Shortcut_Path(final_string, "Program Files (x86)", "Program Files");
            if (File.Exists(new_path) == true && File.Exists(final_string) == false)
            {
                final_string = new_path;
            }
            if (final_string.ToLower().IndexOf("windows\\installer") > -1)
                final_string = filename;
            if (File.Exists(final_string))
                return final_string;
            else
                return filename;
        }

        public static string Validate_Shortcut_Path(string final_string, string find_what, string replace_with)
        {
            string final_string_lower = final_string.ToLower();
            string find_what_lower = find_what.ToLower();
            int find_value = final_string_lower.IndexOf(find_what_lower);
            if (find_value > -1)
            {
                string new_string = final_string.Substring(0, find_value) + replace_with + final_string.Substring(find_value + find_what.Length);
                if (File.Exists(new_string) == true && File.Exists(final_string) == false)
                {
                    final_string = new_string;
                }
            }
            return final_string;
        }

        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        internal static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        [Flags()]
        enum SLGP_FLAGS
        {
            SLGP_SHORTPATH = 0x1,
            SLGP_UNCPRIORITY = 0x2,
            SLGP_RAWPATH = 0x4
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct WIN32_FIND_DATAW
        {
            public uint dwFileAttributes;
            public long ftCreationTime;
            public long ftLastAccessTime;
            public long ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [Flags()]
        enum SLR_FLAGS
        {
            SLR_NO_UI = 0x1,
            SLR_ANY_MATCH = 0x2,
            SLR_NOUPDATE = 0x8,
            SLR_NOSEARCH = 0x10,
            SLR_NOTRACK = 0x20,
            SLR_NOLINKINFO = 0x40,
            SLR_INVOKE_MSI = 0x80
        }

        [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F9-0000-0000-C000-000000000046")]
        interface IShellLinkW
        {
            void GetPath([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATAW pfd, SLGP_FLAGS fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
                int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, SLR_FLAGS fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);

        }

        [ComImport, Guid("0000010c-0000-0000-c000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersist
        {
            [PreserveSig]
            void GetClassID(out Guid pClassID);
        }

        [ComImport, Guid("0000010b-0000-0000-C000-000000000046"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistFile : IPersist
        {
            new void GetClassID(out Guid pClassID);
            [PreserveSig]
            int IsDirty();

            [PreserveSig]
            void Load([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);

            [PreserveSig]
            void Save([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
                [In, MarshalAs(UnmanagedType.Bool)] bool fRemember);

            [PreserveSig]
            void SaveCompleted([In, MarshalAs(UnmanagedType.LPWStr)] string pszFileName);

            [PreserveSig]
            void GetCurFile([In, MarshalAs(UnmanagedType.LPWStr)] string ppszFileName);
        }
        const uint STGM_READ = 0;
        const int MAX_PATH = 260;
        [
            ComImport(),
            Guid("00021401-0000-0000-C000-000000000046")
        ]
        public class ShellLink
        {
        }
    }
}
