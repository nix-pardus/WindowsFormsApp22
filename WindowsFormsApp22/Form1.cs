using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp22
{

    public partial class Form1 : Form, INotifyPropertyChanged
    {
        int count = 0;
        int number = 0;
        string threadName;
        public int Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged(); }
        }
        Semaphore semaphore = new Semaphore(3, 3);

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            if (PropertyChanged != null)
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)(() => { LabelUpdate(); }));
                }
            }
        }
        void LabelUpdate()
        {
            label1.Text = number.ToString();
            listBox2.Items.RemoveAt(0);
            listBox2.Height -= 13;
        }
        public Form1()
        {
            InitializeComponent();
            PropertyChanged += Form1_PropertyChanged;
            Thread.CurrentThread.Name = "Main";
        }

        private void Form1_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Increment);
            thread.Name = $"Thread {++count}";
            listBox1.Height = listBox1.Height + 13;
            listBox1.Items.Add(thread);
        }
        void Increment()
        {
            semaphore.WaitOne();
            //MessageBox.Show(Thread.CurrentThread.Name, "Increment");
            threadName = Thread.CurrentThread.Name;
            if (InvokeRequired)
                Invoke((MethodInvoker)(() => { ListBox3Increment(threadName); }));
            //listBox2.Items.Remove(listBox2.Items.Contains(Thread.CurrentThread.Name));
            Thread.Sleep(3000);
            Number++;
            if(InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { ListBox3Decrement(); }));
            }
            semaphore.Release();
        }
        void ListBox3Increment(string t)
        {
            //MessageBox.Show(t, "ListBox3Increment");
            listBox3.Items.Add(t);
            listBox3.Height += 13;
        }
        void ListBox3Decrement()
        {
            listBox3.Items.RemoveAt(0);
            listBox3.Height -= 13;
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            listBox2.Height += 13;
            listBox2.Items.Add(listBox1.SelectedItem);
            listBox1.Items.Remove(listBox1.SelectedItem);
            listBox1.Height = listBox1.Height - 13;
            List<Thread> thr = new List<Thread>();
            foreach (Thread t in listBox2.Items)
            {
                if (t.ThreadState == ThreadState.Unstarted)
                {
                    thr.Add(t);
                    t.Start();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.DisplayMember = "Name";
            listBox2.DisplayMember = "Name";
            //listBox3.DisplayMember = "Name";
        }
    }
}
