using System.Threading;
using System.IO.Ports;
using UnityEngine;

namespace FingerTrackerForVMC
{
    public class SerialHandler : MonoBehaviour
    {
        public string portName = "COM1";
        public int baudRate = 9600;
        private SerialPort _serialPort = null;

        private Thread _thread;
        private bool _isRunning = false;
        private string _message;
        private bool _isNewMessageReceived = false;

        public string[] RecevedMessage;
        public bool isOpen { 
            get {
                if (_serialPort == null) return false;
                return _serialPort.IsOpen;
            }
            set { }
        }
        private void Update()
        {
            if (_isNewMessageReceived)
            {
                OnDataReceived(_message);
            }
            _isNewMessageReceived = false;
        }

        public void Open()
        {
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.None);
            _serialPort.Open();

            _isRunning = true;
            _thread = new Thread(Read);
            _thread.Start();
        }

        private void Read()
        {
            while (_isRunning && _serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    _message = _serialPort.ReadLine();
                    _isNewMessageReceived = true;
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }
        public void Close()
        {
            _isRunning = false;

            if (_thread != null && _thread.IsAlive)
            {
                _thread.Join();
            }

            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
        }

        private void OnDataReceived(string message)
        {
            var data = message.Split(',');
            if (data.Length < 2) return;
            RecevedMessage = data;
        }
    }
}
