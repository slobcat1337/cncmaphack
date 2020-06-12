using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace WindowsFormsApp1
{
    


    public partial class Form1 : Form
    {


        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers,int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

       

        public Form1()
        {

            RegisterHotKey(this.Handle, 1, 2,(int)Keys.R);
            RegisterHotKey(this.Handle, 2, 2, (int)Keys.S);
            InitializeComponent();
        }

      

        private void button2_Click(object sender, EventArgs e)
        {


            revealMap();
        



        }


        protected override void WndProc(ref Message m)
        {

          
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == 1)
            {
                revealMap();
            }

            if (m.Msg == 0x0312 && m.WParam.ToInt32() == 2)
            {
                reshroudMap();
            }
            base.WndProc(ref m);
        }

        private void reshroudMap()
        {


             memory mem = new memory();

            byte[] isVisible = new byte[] { 0x85, 0x42, 0x20, 0x0F, 0x95, 0xC0 };
            byte[] isMapped = new byte[] { 0x85, 0x42, 0x1C, 0x0F, 0x95, 0xC0 };


            mem.writeMemory("InstanceServerG", (IntPtr)0x22C4B, isVisible, true);

            mem.writeMemory("InstanceServerG", (IntPtr)0x22C1B, isMapped, true);

        }

        private void revealMap()
        {

            memory mem = new memory();

            byte[] isVisible = new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0x90 };
            byte[] isMapped = new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0x90 };


            mem.writeMemory("InstanceServerG", (IntPtr)0x22C4B, isVisible, true);

            mem.writeMemory("InstanceServerG", (IntPtr)0x22C1B, isMapped, true);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            reshroudMap();

        }
    }

}
