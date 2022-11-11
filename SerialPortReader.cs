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
            // _serialPort.Parity = _scaleValues.GetParity();
            // _serialPort.DataBits = _scaleValues.GetDataBits();
            // _serialPort.StopBits = _scaleValues.GetStopBits();
            // _serialPort.Handshake = _scaleValues.GetFlowControl();
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            //  Open Port
            _serialPort.Open();
            _continue = true;

            readThread.Start();
            readThread.Join();
            _serialPort.Close();
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e) {
            int milliseconds = 5;
            Thread.Sleep(milliseconds);
            Console.WriteLine("{0}", _serialPort.ReadExisting());
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