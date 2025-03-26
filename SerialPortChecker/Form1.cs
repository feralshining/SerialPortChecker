using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace CheckQRPort
{
    public partial class Form1 : Form
    {
        private static SerialPort sp = new SerialPort();
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; //대충 때우기
        }

        private void Form1_Load(object sender, EventArgs e) => comboBox1.DataSource = SerialPort.GetPortNames();

        private void button1_Click(object sender, EventArgs e)
        {
            string portName = comboBox1.Text;
            sp = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);

            if (!sp.IsOpen)
            {
                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                try
                {
                    sp.Open();
                    textBox1.Text = "연결됨. 데이터 수신 시 이곳에 표시됩니다.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "알림", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        private void sp_DataReceived(object sender, EventArgs e)
        {
            try
            {
                int size = sp.BytesToRead;
                byte[] buffer = new byte[size];
                sp.Read(buffer, 0, size);

                string qrdata = Encoding.UTF8.GetString(buffer);
                textBox1.Text = qrdata;
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sp.IsOpen)
            {
                sp.Close();
                sp.Dispose();
                textBox1.Text = "연결 해제됨";
            }
        }
    }
}
