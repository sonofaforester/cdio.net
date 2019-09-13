using System;
using System.Runtime.InteropServices;
using System.Text;

namespace cdio.net
{
    public delegate void TriggerCallback(short bitNo, CdioTriggerTypeEnum action);

    public class CdioDevice : IDisposable
    {
        private readonly string _deviceName;
        private short _id = 0;



        /// <summary>
        /// Create new instance of Cdio device using default name "DIO000"
        /// </summary>
        /// <returns></returns>
        public CdioDevice() : this("DIO000") {}

        /// <summary>
        /// Create new instance of Cdio device using supplied device name
        /// </summary>
        /// <param name="deviceName"></param>
        public CdioDevice(string deviceName)
        {
            _deviceName = deviceName;
        }

        /// <summary>
        /// Initialize Cdio device
        /// </summary>
        /// <returns></returns>
        public void Init()
        {
            var pnt = Marshal.AllocHGlobal(Marshal.SizeOf(sizeof(short)));

            CheckErrorCode(CdioSdk.DioInit("DIO000", pnt));

            _id = Marshal.ReadInt16(pnt);            
        }

        public void GetMaxPorts(out short inputPorts, out short outputPorts) {
            var inPnt = Marshal.AllocHGlobal(Marshal.SizeOf(sizeof(short)));
            var outPnt = Marshal.AllocHGlobal(Marshal.SizeOf(sizeof(short)));
            CheckErrorCode(CdioSdk.DioGetMaxPorts(_id, inPnt, outPnt));

            inputPorts = Marshal.ReadInt16(inPnt);
            outputPorts = Marshal.ReadInt16(outPnt);
        }

        public void SetTriggerCallback(TriggerCallback callback) {
            CheckErrorCode(CdioSdk.DioSetTrgCallBackProc(_id, new CdioSdk.TrgCallBack((short id, short message, long wParam, long lParam, IntPtr userData1) => {
                    callback((short)wParam, (CdioTriggerTypeEnum)lParam);                 
            }), IntPtr.Zero));
        }

        public void SetTrigger(short bitNo, CdioTriggerTypeEnum triggerType, int pollingInterval)
        {
            CheckErrorCode(CdioSdk.DioSetTrgEvent(_id, bitNo, (short)triggerType, pollingInterval));
        }

        private void CheckErrorCode(long errorCode) {
            if(errorCode != 0)
            {
                var errorString = new StringBuilder(256);                

                CdioSdk.DioGetErrorString(errorCode, errorString);

                throw new CdioDeviceException(errorCode, errorString.ToString());               
            }
        }

        public void Dispose()
        {
            if(_id != 0)
                CheckErrorCode(CdioSdk.DioExit(_id));
        }
    }
}
