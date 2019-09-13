using System;
using cdio.net;

namespace trigger
{
    class Program
    {
        static int Main(string[] args)
        {
            try {
                string deviceName = "DIO000";
                var numberOfInputs = 0;

                if(args.Length > 0)
                    deviceName = args[0];

                if(args.Length > 1)
                    numberOfInputs = Convert.ToInt32(args[1]);

                var device = new CdioDevice(deviceName);

                // Initialization

                device.Init();

                // Set trigger callback method

                device.SetTriggerCallback((bitNo, action) => {
                    Console.WriteLine($"Trigger:bit = {bitNo}, logic = {action}: input 'q' to quit.");
                });
                
                // Get number of device bits

                if(numberOfInputs == 0) {
                    short inputPorts, outputPorts;

                    device.GetMaxPorts(out inputPorts, out outputPorts);
                
                    numberOfInputs = inputPorts * 8;
                }

                if (numberOfInputs == 0) {
                    Console.Error.WriteLine("Input bit number = 0\n");
                    return -1;
                } else {
                    Console.WriteLine($"Trigger bit = 0 to {(int)numberOfInputs - 1:d}.\n");
                }

                // Setup triggers for each bit

                for (short i = 0; i < numberOfInputs; i++)
                {
                    device.SetTrigger(i, CdioTriggerTypeEnum.Both, 10); 
                }

                Console.WriteLine("Waiting for trigger...");
                
                Console.WriteLine("input 'q' to quit.");
                
                while(true)
                {    
                    var key = Console.ReadKey();

                    if(key.Key == ConsoleKey.Q) {
                        Console.WriteLine();
                        break;
                    }
                }

                device.Dispose();

                return 0;
            } catch(CdioDeviceException ex) {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}
