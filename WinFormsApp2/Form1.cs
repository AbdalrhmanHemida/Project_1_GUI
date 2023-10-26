using System.IO.Ports;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        SerialPort port = null;
        string data_rx = ""; // any receiving data is stored here 
        bool flag_start = false;  // data_rx only being loaded with data when this flag = 1
        bool flag_complete = true;


        public Form1()
        {
            InitializeComponent();
            refresh_com();
            label1.Text = "ddis connected";
            label1.ForeColor = Color.Red;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            refresh_com();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            disconnect();
        }

        private void refresh_com()
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames();

                if (portNames.Length > 0)
                {
                    comboBox1.DataSource = portNames;
                }
                else
                {
                    comboBox1.DataSource = null;
                    comboBox1.Items.Add("No ports available");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                MessageBox.Show("Error: refresh_com() method" + ex.Message);
            }
        }

        private void connect()
        {
            port = new SerialPort(comboBox1.SelectedItem.ToString());
            // To add interrupt handler for the recieve
            port.DataReceived += new SerialDataReceivedEventHandler(data_ex_handler);

            port.BaudRate = 2400;
            port.DataBits = 8;
            port.StopBits = StopBits.One;

            try
            {
                if (!port.IsOpen)
                {
                    port.Open();
                    label1.Text = "connected";
                    label1.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: connect() method" + ex.Message);
            }
        }

        private void disconnect()
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                    label1.Text = "ddis connected";
                    label1.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: disconnect() method" + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            disconnect();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            send();
        }

        private void send()
        {
            try
            {
                port.Write("$" + textBox1.Text + "!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: send() method" + ex.Message);

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = data_rx;
        }

        // any event is written as follow => the sender & the event args, but here the args for the handler
        // now any data recieved is stored on data_rx
        private void data_ex_handler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string tmp = sp.ReadExisting();
            if (tmp.Contains("|"))
            {
                data_rx = "";
                flag_start = true;
            }
            else if (tmp.Contains("$"))
            {
                flag_start = false;
                MessageBox.Show(data_rx);
                // build handler function => 
            }
            else if (tmp.Contains('%'))
            {
                //////
            }

            if (flag_start)
            {
                data_rx += tmp;
            }
            data_rx += tmp;
        }



        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                send();
            }
        }

        // vid3
        private void DataReceiveHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = "";

            try
            {
                indata = sp.ReadExisting();
            }
            catch (Exception ex)
            {

            }

            int idx_end = indata.IndexOf("|");
            if ((idx_end >= 0) && flag_start)
            {
                flag_start = false;
                data_rx += indata.Substring(0, idx_end);

            }

            if (flag_start)
            {
                data_rx += indata;
            }

            int idx_start = indata.IndexOf('$');
            if (idx_start >= 0)
            {
                flag_start = true;
                data_rx = "";

                if (indata.Length > (idx_start + 1))
                {
                    data_rx += indata.Substring((idx_start + 1), (indata.Length - 1));

                    // $2!
                    // if a full frame was already received, then handle it
                    int idx = data_rx.IndexOf('!');
                    if (idx >= 0)
                    {
                        data_rx = data_rx.Substring(0, idx);
                    }

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox2.Text = data_rx;
        }
    }
}