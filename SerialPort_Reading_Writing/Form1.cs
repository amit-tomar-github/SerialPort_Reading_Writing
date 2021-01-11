using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialPort_Reading_Writing
{
    public partial class Form1 : Form
    {
        private SerialPort serialPortData;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                /*
                 * Serial Port Integration Classes and Property
                 * Serial Port will be opend on CheckBox click and even on Form Closing It will be closed
                 */
                serialPortData = new SerialPort();

                serialPortData.PortName = ConfigurationManager.AppSettings["ComPort"].ToString();
                serialPortData.BaudRate = int.Parse(ConfigurationManager.AppSettings["BaudRate"].ToString());
                serialPortData.Parity = (Parity)Enum.Parse(typeof(Parity), ConfigurationManager.AppSettings["Parity"].ToString());
                serialPortData.StopBits = (StopBits)Enum.Parse(typeof(StopBits), ConfigurationManager.AppSettings["StopBits"].ToString());
                serialPortData.DataBits = int.Parse(ConfigurationManager.AppSettings["DataBits"].ToString());
                serialPortData.Handshake = Handshake.None;
                serialPortData.ReadTimeout = 2000;
                serialPortData.DataReceived += SerialPortData_DataReceived;
                OpenSerialPort();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #region Serial Port Communication

        private bool OpenSerialPort()
        {
            try
            {
                if (serialPortData.IsOpen) return true;
                else
                {
                    serialPortData.Open(); return true;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        private void CloseSerialPort()
        {
            try
            {
                if (serialPortData != null && serialPortData.IsOpen)
                {
                    serialPortData.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        string readData = "";
        private void SerialPortData_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = sender as SerialPort;
                if (serialPortData.IsOpen)
                {
                    string receivedData = serialPort.ReadLine();
                    this.Invoke(new Action(() =>
                    {
                        try
                        {
                            rtReceiveData.Text = receivedData;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => MessageBox.Show(ex.Message)));
            }
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                /*
                * Weighing Machine Integration
                */
                if (serialPortData != null && serialPortData.IsOpen)
                {
                    serialPortData.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortData.IsOpen)
                {
                    serialPortData.WriteLine(rtSendData.Text);
                }
                else
                    MessageBox.Show("Serial port is not opened");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
