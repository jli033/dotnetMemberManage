using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SharedUtilitys.Environments
{
    public static class StandbyDetector
    {
        [DllImport("kernel32.dll")]
        extern static ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [Flags]
        public enum ExecutionState : uint
        {
            // 関数が失敗した時の戻り値
            Null = 0,
            // スタンバイを抑止
            SystemRequired = 1,
            // 画面OFFを抑止
            DisplayRequired = 2,
            // 効果を永続させる。ほかオプションと併用する。
            Continuous = 0x80000000,
        }

        public static void SetThreadExecutionState()
        {
            SetThreadExecutionState(ExecutionState.SystemRequired | ExecutionState.Continuous);

            SystemEvents.PowerModeChanged += SystemEventsOnPowerModeChanged;
        }

        private static void SystemEventsOnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            Console.WriteLine(e.Mode);
        }
    }
}
