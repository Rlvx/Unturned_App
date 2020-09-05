using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinaryShop
{
    public partial class Modifier : Form
    {
        public string Name { set { textBox1.Text = value; } }
        public string PrixDepart { get { return textBox2.Text; } set { textBox2.Text = value; } }
        public string Taux { get { return textBox3.Text; } set { textBox3.Text = value; } }
        public Modifier()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.Parse(PrixDepart)<0)
            {
                MessageBox.Show("Valeur Impossible");
            }
            else
            {
                DialogResult = DialogResult.Yes;
            }
        }
    }
}
