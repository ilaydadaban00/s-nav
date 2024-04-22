using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp6
{
    public partial class FormTumSarkilar : Form
    {
        string baglanti = "Server=localhost;Database=muzik;Uid=root;Pwd=;";
        public FormTumSarkilar()
        {
            InitializeComponent();
        }

        private void FormTumSarkilar_Load(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                 baglan.Open();
                string sorgu = "SELECT * FROM sarkilar;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();


                da.Fill(dt);
                dgwSarkilar.DataSource = dt;
            }
            Doldur();

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr=dgwSarkilar.SelectedRows[0];
            int satirid = Convert.ToInt32(dr.Cells[0].Value);
            // MessageBox.Show(satirId.ToString());

            DialogResult cevap = MessageBox.Show("şarkıyı silmek istiyor musun?","???",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            //string, string, yesno, icon
            
            if(cevap == DialogResult.Yes)
            {
                string sorgu = "DELETE  FROM sarkilar WHERE id = @satirid";
                using (MySqlConnection baglan = new MySqlConnection(baglanti)) 
                {
                    baglan.Open();
                    MySqlCommand cmd=new MySqlCommand(sorgu, baglan);
                    cmd.Parameters.AddWithValue("@satirid",satirid);

                    cmd.ExecuteNonQuery();                    
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "UPDATE sarkilar\r\nSET ad=@sarkiad,sanatci=@sanatciad,yil=@yil,tur=@tur, sure=@sure,eklenme_tarihi=@tarih,favori=@favori\r\nWHERE id=@satirid;";
                DataGridViewRow dr = dgwSarkilar.SelectedRows[0];
                int satirId = Convert.ToInt32(dr.Cells[0].Value);
                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@sarkiAd", txtSarki.Text);
                cmd.Parameters.AddWithValue("@sanatciAd", txtSanatci.Text);
                cmd.Parameters.AddWithValue("@yil", txtYil.Text);
                cmd.Parameters.AddWithValue("@tur", cmbTur.SelectedValue);
                cmd.Parameters.AddWithValue("@sure", txtSure.Text);
                cmd.Parameters.AddWithValue("@tarih", dtTarih.Value);
                cmd.Parameters.AddWithValue("@favori", cbFavori.Checked);

                int id = Convert.ToInt32(dgwSarkilar.SelectedRows[0].Cells["id"].Value);
                cmd.Parameters.AddWithValue("@satirid",id);
                cmd.ExecuteNonQuery();



                Doldur();

            }
        }
        void Doldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT DISTINCT tur FROM sarkilar";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();


                da.Fill(dt);
                cmbTur.DataSource = dt;
                cmbTur.DisplayMember = "tur";
                cmbTur.ValueMember = "tur";

            }

        }

        private void dgwSarkilar_SelectionChanged(object sender, EventArgs e)
        {
            if(dgwSarkilar.SelectedRows.Count> 0)
            {
                txtSarki.Text = dgwSarkilar.SelectedRows[0].Cells["ad"].Value.ToString();
                txtSanatci.Text = dgwSarkilar.SelectedRows[0].Cells["sanatci"].Value.ToString();
                txtSure.Text = dgwSarkilar.SelectedRows[0].Cells["sure"].Value.ToString();
                cbFavori.Checked = Convert.ToBoolean(dgwSarkilar.SelectedRows[0].Cells["favori"].Value);
                dtTarih.Value= Convert.ToDateTime(dgwSarkilar.SelectedRows[0].Cells["eklenme_tarihi"].Value);
                txtYil.Text = dgwSarkilar.SelectedRows[0].Cells["yil"].Value.ToString();



                cmbTur.SelectedValue = dgwSarkilar.SelectedRows[0].Cells["tur"].Value.ToString();
            }
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            string sorgu = "";
            if (rbsanatci.Checked)
            {
                sorgu = "SELECT * FROM sarkilar WHERE sanatci LIKE %aranan% ";
            }
            else
            {
                sorgu = "SELECT * FROM sarkilar WHERE ad LIKE 'aranan';" ;
            }
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
               

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@aranan", txtAra.Text);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgwSarkilar.DataSource = dt;
            }
        }
    }
}
