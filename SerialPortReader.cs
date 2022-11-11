/************************************************************************************
 * Copyright (C) 2012-Present E.R.P. Consultores y Asociados, C.A.                  *
 * Contributor(s): Yamel Senih ysenih@erpya.com                                     *
 * This program is free software: you can redistribute it and/or modify             *
 * it under the terms of the GNU General Public License as published by             *
 * the Free Software Foundation, either version 2 of the License, or                *
 * (at your option) any later version.                                              *
 * This program is distributed in the hope that it will be useful,                  *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of                   *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the                     *
 * GNU General Public License for more details.                                     *
 * You should have received a copy of the GNU General Public License                *
 * along with this program.	If not, see <https://www.gnu.org/licenses/>.            *
 ************************************************************************************/

using System;
using System.IO.Ports;
using System.Threading;

// Default namespace
namespace SerialPortReader {
    
    // Class for read scale
    public class Reader {
        static bool _continue;
        static SerialPort _serialPort;


        public static void Main(string[] args) {
            if(args == null) {
                throw new Exception("Arguments Not Found");
            }
            //	
            if(args == null || args.Length == 0) {
                throw new Exception("Arguments Must Be: [property file name]");
            }
            Thread readThread = new Thread(Read);
            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();
            // Allow the user to set the appropriate properties.
            _serialPort.PortName = args[0];//"/dev/ttyUSB0";
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.RtsEnable = true;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            //  Open Port
            _serialPort.Open();
            // X
            // _serialPort.Write(new byte[] { 2, 27, 57, 28, 88, 28, 84, 3, 48, 49, 51, 68 }, 0, 12);
            // Status
            byte[] data = new byte[] { 2, 122, 56, 28, 78, 3, 48, 49, 50, 49 };
            for (int i = 0; i < 10; i++) {
                Console.WriteLine("Sending Data {0}", data);
                _serialPort.Write(data, 0, data.Length);
                Thread.Sleep(1000);
            }
            _continue = true;
            readThread.Start();
            readThread.Join();
            _serialPort.Close();
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e) {
            int milliseconds = 5;
            Thread.Sleep(milliseconds);
            String input = _serialPort.ReadExisting();
            Console.WriteLine("{0}", input);
            Console.WriteLine("---------------------------------------------------------");
            for (int i = 0; i < input.Length; i++) {
                Console.Write("{0}{1}:{2}", (i > 0 && i < input.Length? "|": ""), input[i], (int)input[i]);
            }
            Console.WriteLine("\n---------------------------------------------------------");
            Console.WriteLine("---------------------------------------------------------");
            for (int i = 0; i < input.Length; i++) {
                Console.Write("{0}{1}", (i > 0 && i < input.Length? "|": ""), (int)input[i]);
            }
            Console.WriteLine("\n---------------------------------------------------------");
        }

        //  Wait for
        public static void Read() {
            while (_continue) {
                //  Read by events
                Console.WriteLine("Waiting...");
                Thread.Sleep(10000);
            }
        }
    }
}