using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Loaf
{
    public static class ScreenHelper
    {
        private const uint SDC_APPLY = 0x00000080;

        private const uint SDC_TOPOLOGY_INTERNAL = 0x00000001;

        private const uint SDC_TOPOLOGY_CLONE = 0x00000002;

        /// <summary>
        /// 扩展模式
        /// </summary>
        private const uint SDC_TOPOLOGY_EXTEND = 0x00000004;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern long SetDisplayConfig(uint numPathArrayElements, IntPtr pathArray, uint numModeArrayElements,
                                                    IntPtr modeArray, uint flags);
        /// <summary>
        /// 设置屏幕的显示模式
        /// </summary>
        /// <param name="displayModel"></param>
        /// <returns></returns>
        private static bool SetScreen(uint displayModel)
        {
            return SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, SDC_APPLY | displayModel) == 0;
        }

        /// <summary>
        /// 设置屏幕的显示模式
        /// </summary>
        /// <param name="type">模式(0 - 主屏  1 - 双屏复制  2 - 双屏扩展</param>
        /// <returns></returns>
        public static bool SetScreenMode(int type)
        {
            uint smode;

            switch (type)
            {
                case 0:
                    smode = SDC_APPLY | SDC_TOPOLOGY_INTERNAL;
                    break;
                case 1:
                    smode = SDC_APPLY | SDC_TOPOLOGY_CLONE;
                    break;
                case 2:
                    smode = SDC_APPLY | SDC_TOPOLOGY_EXTEND;
                    break;
                default:
                    smode = SDC_APPLY | SDC_TOPOLOGY_INTERNAL;
                    break;
            }

            return SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, smode) == 0;
        }
    }
}
