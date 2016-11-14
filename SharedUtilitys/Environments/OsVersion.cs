using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SharedUtilitys.Environments
{
    public static class OsVersion
    {
        private static readonly StringBuilder Log = new StringBuilder();

        [StructLayout(LayoutKind.Sequential)]
        private struct Osversioninfoex
        {
            public int dwOSVersionInfoSize;
            public readonly int dwMajorVersion;
            public readonly int dwMinorVersion;
            public readonly int dwBuildNumber;
            public readonly int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public readonly string szCSDVersion;
            public readonly short wServicePackMajor;
            public readonly short wServicePackMinor;
            public readonly short wSuiteMask;
            public readonly byte wProductType;
            public readonly byte wReserved;
        }

        // GetVersionEx
        private const Int32 VerNtWorkstation = 1;
        private const Int32 VerSuiteStorageServer = 8192; //0x00002000;
        private const Int32 VerSuiteWhServer = 32768; //0x00008000;

        // GetSystemMetrics
        private const Int32 SmServerr2 = 89;

        // エラー
        private const string Err = "取得失敗";

        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref Osversioninfoex osVersionInfo);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(Int32 nIndex);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        public static string GetOsVersion()
        {
            var result = Err;
            var osVersionInfo = new Osversioninfoex();
            var osInfo = Environment.OSVersion;

            osVersionInfo.dwOSVersionInfoSize =
                   Marshal.SizeOf(typeof(Osversioninfoex));
            if (!GetVersionEx(ref osVersionInfo))
            {
                return Err;
            }

            var is64 = Is64();

            Log.AppendLine("osInfo.Platform=" + osInfo.Platform);
            Log.AppendLine("osInfo.Version.Major=" + osInfo.Version.Major);
            Log.AppendLine("osInfo.Version.Minor=" + osInfo.Version.Minor);
            Log.AppendLine("osVersionInfo.wProductType=" + osVersionInfo.wProductType);
            Log.AppendLine("osVersionInfo.wSuiteMask=" + osVersionInfo.wSuiteMask);
            Log.AppendLine("IntPtr.Size=" + IntPtr.Size);
            Log.AppendLine("64 bit OS=" + Convert.ToString(is64));

            if (osInfo.Platform == PlatformID.Win32NT)
            {
                if (osInfo.Version.Major > 4)
                {
                    result = "Microsoft ";
                }

                if (osInfo.Version.Major == 6)
                {
                    if (osInfo.Version.Minor == 0)
                    {
                        if (osVersionInfo.wProductType == VerNtWorkstation)
                        {
                            result += "Windows Vista";
                        }
                        else
                        {
                            result += "Windows Server 2008";
                        }
                    }
                    else if (osInfo.Version.Minor == 1)
                    {
                        if (osVersionInfo.wProductType == VerNtWorkstation)
                        {
                            result += "Windows 7";
                        }
                        else
                        {
                            result += "Windows Server 2008 R2";
                        }
                    }
                    else if (osInfo.Version.Minor == 2)
                    {
                        if (osVersionInfo.wProductType == VerNtWorkstation)
                        {
                            result += "Windows 8";
                        }
                        else
                        {
                            result += "Windows Server 2012";
                        }
                    }
                }
                else if (osInfo.Version.Major == 5)
                {
                    if (osInfo.Version.Minor == 2)
                    {
                        if (GetSystemMetrics(SmServerr2) != 0)
                        {
                            result += "Windows Server 2003 R2";
                        }
                        else if ((osVersionInfo.wSuiteMask & VerSuiteStorageServer) > 0)
                        {
                            result += "Windows Storage Server 2003";
                        }
                        else if ((osVersionInfo.wSuiteMask & VerSuiteWhServer) > 0)
                        {
                            result += "Windows Home Server";
                        }
                        else if (osVersionInfo.wProductType == VerNtWorkstation && is64)
                        {
                            result += "Windows XP Professional x64 Edition";
                        }
                        else
                        {
                            result += "Windows Server 2003";
                        }

                        // エディション取得処理は省略
                    }
                    else if (osInfo.Version.Minor == 1)
                    {
                        result += "Windows XP";
                    }
                    else if (osInfo.Version.Minor == 0)
                    {
                        result += "Windows 2000";
                    }
                }

                // Include service pack (if any)
                if (osVersionInfo.szCSDVersion.Length > 0)
                {
                    result += " " + osVersionInfo.szCSDVersion;
                }

                // Architecture
                if (is64 == false)
                {
                    result += ", 32-bit";
                }
                else
                {
                    result += ", 64-bit";
                }
            }
            else if (osInfo.Platform == PlatformID.Win32Windows)
            {
                result = "This sample does not support this version of Windows.";
            }

            return result;
        }

        public static bool IsClientOs()
        {
            var version = GetOsVersion();

            if (version.IndexOf("Windows Server", StringComparison.CurrentCulture) > -1)
            {
                return false;
            }
            return true;
        }

        public static bool IsXp()
        {
            var version = GetOsVersion();

            if (version.IndexOf("Windows XP", StringComparison.CurrentCulture) > -1)
            {
                return true;
            }
            return false;
        }

        private static bool Is64()
        {
            bool is64;

            if (IntPtr.Size == 4)
            {
                bool wow64 = false;
                IntPtr address = GetProcAddress(GetModuleHandle("Kernel32.dll"), "IsWow64Process");
                if (address != IntPtr.Zero)
                {
                    if (IsWow64Process(Process.GetCurrentProcess().Handle, out wow64) == false)
                    {
                        wow64 = false;
                    }
                }

                if (wow64)
                {
                    is64 = true;
                }
                else
                {
                    is64 = false;
                }
            }
            else
            {
                is64 = true;
            }

            return is64;
        }

        public static string GetLog()
        {
            return Log.ToString();
        }
    }
}