using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic; //for the input box in Remove function

namespace cS_Assignment2_Organizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[,] schedule = new string[10, 8]{
            {"", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"},
            {"06-08am", "-", "-", "-", "-", "-", "-", "-"}, 
            {"08-10am", "-", "-", "-", "-", "-", "-", "-"}, 
            {"10-12am", "-", "-", "-", "-", "-", "-", "-"}, 
            {"12-02pm", "-", "-", "-", "-", "-", "-", "-"}, 
            {"02-04pm", "-", "-", "-", "-", "-", "-", "-"}, 
            {"04-06pm", "-", "-", "-", "-", "-", "-", "-"}, 
            {"06-08pm", "-", "-", "-", "-", "-", "-", "-"}, 
            {"08-10pm", "-", "-", "-", "-", "-", "-", "-"},
            {"10-12pm", "-", "-", "-", "-", "-", "-", "-"}}; //7 days (+1 header), 8 hours (+1 header)

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.GridLines = true;
            listView1.View = View.Details;
            display();
        }
        public void display()
        {
            listView1.Items.Clear();
            ListViewItem row;
            for (int i = 1; i < 10; i++)
            {
                row = new ListViewItem(schedule[i, 0]); //every column has to be as subitem
                row.SubItems.Add(schedule[i, 1]);
                row.SubItems.Add(schedule[i, 2]);
                row.SubItems.Add(schedule[i, 3]);
                row.SubItems.Add(schedule[i, 4]);
                row.SubItems.Add(schedule[i, 5]);
                row.SubItems.Add(schedule[i, 6]);
                row.SubItems.Add(schedule[i, 7]);
                listView1.Items.Add(row);
            }
            //http://stackoverflow.com/questions/4111308/2-dimensional-integer-array-to-datagridview
            //foreach (var item in schedule)
            //{
            //    ListViewItem itm = new ListViewItem(item);
            //    listView1.Items.Add(itm);
            //}
        }
        //public void check(out string d, out string t)
        //{
        //    d = cbxDay.GetItemText(cbxDay.SelectedItem); //get selected day name
        //    t = cbxTime.GetItemText(cbxTime.SelectedItem); //get selected time
        //    if (d == "") MessageBox.Show("Choose the day");
        //    else if (t == "") MessageBox.Show("Choose the time");
        //    int a = cbxDay.SelectedIndex;
        //    tbxDescription.Text = a.ToString();
        //}
        public void check2(out int d, out int t)
        {
            d = cbxDay.SelectedIndex; //get selected day index 
            t = cbxTime.SelectedIndex; //get selected time index
            if (d == -1)
            {
                MessageBox.Show("Choose the day");
                cbxDay.Focus();
            }
            else if (t == -1)
            {
                MessageBox.Show("Choose the time");
                cbxTime.Focus();
            }
            else if (tbxDescription.Text == "")
            {
                MessageBox.Show("Write some description");
                tbxDescription.Focus();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //string dd, tt;
            //check(out dd, out tt); //checks that day & time was choosen
            int dd, tt;
            check2(out dd, out tt);
            if (dd != -1 && tt != -1 && tbxDescription.Text != "")
            {
                if (schedule[tt+1, dd + 1] != "-")
                {
                    var result = MessageBox.Show("Do you want to replace (YES) or add new task (NO)?", "Warrning", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes) //replace
                    {
                        schedule[tt + 1, dd + 1] = tbxDescription.Text; //0 column is for times, 1st column is Monday, 2nd - Tuesday
                    }
                    else if (result == DialogResult.No)//add
                    {
                        schedule[tt + 1, dd + 1] += ", " + tbxDescription.Text;
                    }
                }
                else
                {
                    schedule[tt + 1, dd + 1] = tbxDescription.Text; 
                }
            }
            display();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            int dd, tt;
            check2(out dd, out tt);
            if (schedule[tt + 1, dd + 1] != "-")
                {
                    var result = MessageBox.Show("Do you want to replace (YES) or add new task (NO)?", "Warrning", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes) //replace
                    {
                        schedule[tt + 1, dd + 1] = tbxDescription.Text; //0 column is for times, 1st column is Monday, 2nd - Tuesday
                    }
                    else if (result == DialogResult.No) //add
                    {
                        schedule[tt + 1, dd + 1] += ", " + tbxDescription.Text;
                    }
                }
            display();
        }
        string[] tasks;
        string sss;

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int dd, tt;
            check2(out dd, out tt);
            if (schedule[tt + 1, dd + 1] != "-")
            {
                string task = schedule[tt+1, dd + 1].ToString();
                int first = task.IndexOf(",");
                if (first != -1) //if there is several task for that day&hour
                {
                    tasks = task.Split(','); //splits the tasks string into array with every task
                    sss = "";
                    for (int i = 0; i < tasks.Length; i++)
                    {
                        sss += i + 1 + ") " + tasks[i].Trim() + Environment.NewLine; //creates text to display in form2
                    }
                    int choice = 0;
                    ShowMyDialogBox(sss, ref choice); //gets the value from form2 which task to remove (if there is several ones)
                    if ((choice > 0 && choice <= tasks.Length))
                    {
                        tasks[choice - 1] = "-";
                        string result = string.Join(", ", tasks);
                        schedule[tt + 1, dd + 1] = result;
                    }
                    else if (choice < 0 || choice > tasks.Length)
                        MessageBox.Show("Wrong value!");
                }
                else //if there is just one task
                    schedule[tt + 1, dd + 1] = "-"; 
            }
            display();

        }
        public void ShowMyDialogBox(string sss, ref int choice)
        {
            Form2 testDialog = new Form2();
            testDialog.lblSSS.Text = sss; //imputs the text sss in the label on form2

            if (testDialog.ShowDialog(this) == DialogResult.OK) // Show testDialog as a modal dialog and determine if DialogResult = OK.
            {
                int num = -1;
                if (!int.TryParse(testDialog.TextBox1.Text, out num))
                    MessageBox.Show("Wrong value, I need number :p");
                else
                    choice = Convert.ToInt32(testDialog.TextBox1.Text); // Read the contents of testDialog's TextBox.
            }
            testDialog.Dispose();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            int dd, tt;
            check2(out dd, out tt);
            if (schedule[tt + 1, dd + 1] != "-")
            {
                string show = "Day: " + schedule[0, dd+1].ToString() + Environment.NewLine + "Time: " + schedule[tt+1, 0].ToString() + Environment.NewLine 
                    + Environment.NewLine + "Task(s): " + schedule[tt+1, dd+1].ToString();
                MessageBox.Show(show);
            }
            else
                MessageBox.Show("There is no task for that time");
        }

    }
}
