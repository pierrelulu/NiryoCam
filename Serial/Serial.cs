using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.Xml.Linq;
using TcpIp;

enum messageTypes
{
    RECEP,
    SENT,
    OTHER
}

public enum Infos
{
    NO_INFO = 0x00,
    READY_START = 0x01,
    READY_CAPTURE = 0x02,
    GO_START = 0x03,
    GO_CAPTURE = 0x04,

    NB_INFOS
}


namespace Serial
{
    
    public class SerialCOM
    {
        List<string> types = new List<string> { "Reception", "Sent", "Info" };
        List<string> messages = new List<string> { "NO_INFO", "READY_START", "READY_CAPTURE", "GO_START", "GO_CAPTURE" };


        SerialPort serial;
        TextBox logger = null;
        TCPstatus status = TCPstatus.CLOSED; 
        string port;

        public event Action<Infos> OnInfoReceived;

        public SerialCOM(string port, TextBox logger = null)
        {
            this.port = port;
            this.logger = logger;
        }

        public void open(string _port = null)
        {
            if (_port != null)
                port = _port;
            try
            {
                serial = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                serial.Open();
                serial.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                LogMessage("Port opened");
                status = TCPstatus.CLIENT_CONNECTED;
            }
            catch (Exception e)
            {
                LogMessage(e.Message);
            }
        }

        public void close()
        {
            if(status != TCPstatus.CLOSED)
            {
                serial?.Close();
                LogMessage("Port closed");
            }
            status = TCPstatus.CLOSED;
        }

        public void changePort(string _port)
        {
            close();
            port = _port;
        }

        private void LogMessage(Infos index, messageTypes type)
        {
            LogMessage(messages[(int)index], type);
        }

        private void LogMessage(string message, messageTypes type = messageTypes.OTHER)
        {
            if(logger == null)
            {
                return;
            }


            if (logger.InvokeRequired)
            {
                logger.Invoke(new Action(() => LogMessage(message, type)));
            }
            else
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                string tag = types[(int)type];

                string receivedData = $"[{timestamp}] {tag}: {message}\r\n";
                logger.AppendText(receivedData);
            }
        }


        public TCPstatus Status()
        {
            return status;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            foreach(byte data in Encoding.ASCII.GetBytes(indata))
            {
                Infos info = (Infos)data;

                LogMessage(info, messageTypes.RECEP);
                if(info != Infos.NO_INFO)
                    OnInfoReceived?.Invoke(info);
            }
            
        }

        public void SendData(Infos data)
        {
            string s = "" + (char)data;
            serial.WriteLine(s);
            LogMessage(data, messageTypes.SENT);
        }
    }
}
