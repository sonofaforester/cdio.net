using System;
using System.Runtime.InteropServices;
using System.Text;

namespace cdio.net
{
    public class CdioSdk
    {        
        private const string cdioLibrary = "cdio";
        
        [global::System.Runtime.InteropServices.DllImport(cdioLibrary)]
        internal static extern long DioInit(string deviceName, IntPtr deviceId);

        [global::System.Runtime.InteropServices.DllImport(cdioLibrary)]
        public static extern long DioGetErrorString(long errorCode, [MarshalAs(UnmanagedType.LPStr)] StringBuilder errorString);

        [global::System.Runtime.InteropServices.DllImport(cdioLibrary)]
        public static extern long DioGetMaxPorts(short id, IntPtr inPortNum, IntPtr outPortNum);

        [global::System.Runtime.InteropServices.DllImport(cdioLibrary)]
        public static extern long DioDmSetStandAlone(short id);

        [global::System.Runtime.InteropServices.DllImport(cdioLibrary)]
        public static extern long DioSetTrgEvent(short id, short bitNo, short logic, long time);

        [global::System.Runtime.InteropServices.DllImport(cdioLibrary)]
        public static extern long DioSetTrgCallBackProc(short id, TrgCallBack callback, IntPtr userData);

        [global::System.Runtime.InteropServices.DllImport(cdioLibrary)]
        internal static extern long DioExit(short id);

        public delegate void TrgCallBack(short id, short message, long wParam, long lParam, IntPtr userData);
    }
}
