using System.IO.Ports;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        SerialPort port = null;
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
            port.BaudRate = 9600;
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
                port.Write(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: send() method" + ex.Message);

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}