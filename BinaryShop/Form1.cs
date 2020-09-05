using System;
using System.Data;
using System.Windows.Forms;
using System.Timers;
using MySql.Data.MySqlClient;

namespace BinaryShop
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer aTimer;
        bool connecté = false;
        MySqlConnection cn = new MySqlConnection("SERVER=192.168.1.115;DATABASE=binaryshop;UID=root;PASSWORD=;PORT=3306;charset=utf8");

        public Form1()
        {
            InitializeComponent();
            Shown += Form1_Shown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cn.Open();
            if (cn.State == ConnectionState.Open)
            {
                connecté = true;
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            SetTimer();
            if (connecté)
            {
                int count = 0;
                listView1.Invoke(new MethodInvoker(delegate { count = listView1.SelectedItems.Count; }));
                if (count == 0)
                {
                    listView1.Invoke(new MethodInvoker(delegate { listView1.Items.Clear(); }));
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM `item`", cn);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {
                        while (Lire.Read())
                        {
                            string ID = Lire["ID"].ToString();
                            string Prix = Lire["Prix"].ToString();
                            string Nom = Lire["Name"].ToString();
                            string Taux = Lire["Taux"].ToString();
                            string PrixDepart = Lire["PrixDepart"].ToString();
                            listView1.Invoke(new MethodInvoker(delegate
                            {
                                listView1.Items.Add(new ListViewItem(new[] { ID, Prix, Nom, Taux, PrixDepart })); ;

                            }));
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous n'etes pas connecté a la Base de Donnée");
            }
        }
        public void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (connecté)
            {
                int count = 0;
                listView1.Invoke(new MethodInvoker(delegate { count = listView1.SelectedItems.Count; }));
                if (count == 0)
                {
                    listView1.Invoke(new MethodInvoker(delegate { listView1.Items.Clear(); }));
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM `item`", cn);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {
                        while (Lire.Read())
                        {
                            string ID = Lire["ID"].ToString();
                            string Prix = Lire["Prix"].ToString();
                            string Nom = Lire["Name"].ToString();
                            string Taux = Lire["Taux"].ToString();
                            string PrixDepart = Lire["PrixDepart"].ToString();
                            listView1.Invoke(new MethodInvoker(delegate
                            {
                                listView1.Items.Add(new ListViewItem(new[] { ID, Prix, Nom, Taux, PrixDepart })); ;

                            }));
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous n'etes pas connecté a la Base de Donnée");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(textBox1.Text == "" || textBox2.Text == "" || textBox4.Text == ""))
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `item` (`ID`, `Prix`, `Name`, `PrixDepart`) VALUES (@ID,@PRIX,@NAME,@PRIX) ", cn);
                cmd.Parameters.AddWithValue("@ID", int.Parse(textBox1.Text));
                cmd.Parameters.AddWithValue("@PRIX", int.Parse(textBox2.Text));
                cmd.Parameters.AddWithValue("@NAME", textBox4.Text);
                cmd.ExecuteNonQuery();
                MySqlCommand cmd1 = new MySqlCommand("CREATE DEFINER=`root`@`localhost` EVENT `"+textBox4.Text+"` ON SCHEDULE EVERY 2 MINUTE STARTS '2020-06-10 16:00:00' ON COMPLETION NOT PRESERVE ENABLE DO INSERT INTO `bourse`( bourse.ID, bourse.Prix, bourse.MyTimeStamp ) VALUES( (SELECT item.ID FROM `item` WHERE item.ID=@ID), (SELECT item.Prix FROM `item` WHERE item.ID=@ID), (SELECT CURRENT_TIME()) )", cn);
                cmd1.Parameters.AddWithValue("@ID", int.Parse(textBox1.Text));
                cmd1.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Veuillez renseigner tout les champs");
            }
        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connecté)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem element = listView1.SelectedItems[0];
                    string Taux = element.SubItems[3].Text;
                    string PrixDepart = element.SubItems[4].Text;
                    string Name = element.SubItems[2].Text;
                    string ID = element.SubItems[0].Text;
                    using (Modifier m = new Modifier())
                    {
                        m.Taux = Taux;
                        m.PrixDepart = PrixDepart;
                        m.Name = Name;
                        if (m.ShowDialog() == DialogResult.Yes)
                        {
                            MySqlCommand cmd = new MySqlCommand("UPDATE item SET Taux=@Taux,PrixDepart=@PrixDepart WHERE ID=@ID", cn);
                            cmd.Parameters.AddWithValue("@Taux", float.Parse(m.Taux));
                            cmd.Parameters.AddWithValue("@PrixDepart", int.Parse(m.PrixDepart));
                            cmd.Parameters.AddWithValue("@ID", int.Parse(ID));
                            cmd.ExecuteNonQuery();
                            element.SubItems[3].Text = m.Taux;
                            element.SubItems[4].Text = m.PrixDepart;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vous n'etes pas connecté a la Base de Donnée");
            }
        }
    }
}
