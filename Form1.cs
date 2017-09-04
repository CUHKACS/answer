using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace SimpleSerial
{
    public partial class Form1 : Form
    {

        public const int KEYEVENTF_EXTENDEDKEY = 1;
        public const int KEYEVENTF_KEYUP = 2;
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;
        public const int VK_MEDIA_PREV_TRACK = 0xB1;
        public const int VK_MEDIA_STOP = 0xB2;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;

        [DllImport("User32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        // Add this variable 
        string RxString;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 115200;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Parity = Parity.None;
            serialPort1.Handshake = Handshake.None;

            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                buttonStart.Enabled = false;
                buttonStop.Enabled = true;
                textBox1.ReadOnly = false;
                buttonReset.Enabled = true;
                send_reset();
                reset_ui();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                send_reset();
                serialPort1.Close();
                buttonStart.Enabled = true;
                buttonStop.Enabled = false;
                textBox1.ReadOnly = true;
                buttonReset.Enabled = false;
                reset_ui();
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            send_reset();

            //if(e.KeyChar == 'r')
            //    keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);

            e.Handled = true;
        }

        private void DisplayText(object sender, EventArgs e)
        {
            timer1.Start();
            //textBox1.AppendText(RxString);
            textBox1.Text = RxString;
            int RxInt=Convert.ToByte(RxString);
            switch (RxInt) {
                case 0:
                    label1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                case 1:
                case 11:
                    label2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                case 2:
                case 22:
                    label3.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                case 3:
                case 33:
                    label4.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                case 4:
                case 44:
                    label5.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                case 5:
                case 55:
                    label6.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                case 6:
                case 66:
                    label7.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                case 7:
                case 77:
                    label8.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);
                    break;
                default:
                    break;

            }

        }
        private void buttonReset_Click(object sender, EventArgs e)
        {
            send_reset();
            reset_ui();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            keybd_event(VK_MEDIA_STOP, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            RxString = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(DisplayText));
            Console.Beep(3500, 1000);
        }

        private void send_reset()
        {
            // If the port is closed, don't try to send a character.
            if (!serialPort1.IsOpen) return;

            // If the port is Open, declare a char[] array with one element.
            char[] buff = new char[1];

            // Load element 0 with the key character.
            buff[0] = 'r';

            // Send the one character buffer.
            serialPort1.Write(buff, 0, 1);
        }

        private void reset_ui()
        {
            textBox1.Text = "";
            label1.BackColor = SystemColors.Control;
            label2.BackColor = SystemColors.Control;
            label3.BackColor = SystemColors.Control;
            label4.BackColor = SystemColors.Control;
            label5.BackColor = SystemColors.Control;
            label6.BackColor = SystemColors.Control;
            label7.BackColor = SystemColors.Control;
            label8.BackColor = SystemColors.Control;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            send_reset();
            reset_ui();
            timer1.Stop();
        }


    }
}