using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SASXIIRPL1_AhmadDanuIlhamDin
{
    public partial class Form1 : Form
    {

        private string connString = "server=localhost;user=root;password='';database=dbkonversidanu;";


        private Dictionary<string, Currency> currencies = new Dictionary<string, Currency>(StringComparer.OrdinalIgnoreCase);

        public Form1()
        {
            InitializeComponent();
        }

        private class Currency
        {
            public int Id { get; set; }
            public string Singkatan { get; set; }
            public string Nama { get; set; }
            public double Kurs { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxMataUang.Items.Clear();
            currencies.Clear();

            try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT id, singkatan, nama, kurs FROM Tmatauang", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cur = new Currency
                            {
                                Id = reader.GetInt32("id"),
                                Singkatan = reader.GetString("singkatan"),
                                Nama = reader.IsDBNull(reader.GetOrdinal("nama")) ? string.Empty : reader.GetString("nama"),
                                Kurs = reader.IsDBNull(reader.GetOrdinal("kurs")) ? 0.0 : reader.GetDouble("kurs")
                            };

                            if (!currencies.ContainsKey(cur.Singkatan))
                            {
                                currencies[cur.Singkatan] = cur;
                                comboBoxMataUang.Items.Add(cur.Singkatan);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            if (comboBoxMataUang.Items.Count > 0)
                comboBoxMataUang.SelectedIndex = 0;
        }

        private void comboBoxMataUang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMataUang.SelectedItem == null) return;

            string singkatan = comboBoxMataUang.SelectedItem.ToString();

            if (currencies.TryGetValue(singkatan, out Currency cur) && cur.Kurs > 0.0)
            {
                label2.Text = cur.Kurs.ToString("N2", CultureInfo.CurrentCulture);
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT id, nama, kurs FROM Tmatauang WHERE singkatan = @singkatan LIMIT 1", conn))
                    {
                        cmd.Parameters.AddWithValue("@singkatan", singkatan);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                string nama = reader.IsDBNull(reader.GetOrdinal("nama")) ? string.Empty : reader.GetString("nama");
                                double kurs = reader.IsDBNull(reader.GetOrdinal("kurs")) ? 0.0 : reader.GetDouble("kurs");

                                cur = new Currency { Id = id, Singkatan = singkatan, Nama = nama, Kurs = kurs };
                                currencies[singkatan] = cur;

                                label2.Text = kurs.ToString("N2", CultureInfo.CurrentCulture);
                                return;
                            }
                        }
                    }
                }

                label2.Text = "N/A";
            }
            catch (Exception)
            {
      
                label2.Text = "N/A";
            }
        }

        private void buttonKonversi_Click(object sender, EventArgs e)
        {
            if (comboBoxMataUang.SelectedItem == null || string.IsNullOrWhiteSpace(textBoxNominal.Text))
            {
                MessageBox.Show("Pilih mata uang dan masukkan nominal.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string singkatan = comboBoxMataUang.SelectedItem.ToString();

            if (!double.TryParse(textBoxNominal.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out double nominal))
            {
   
                if (!double.TryParse(textBoxNominal.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out nominal))
                {
                    MessageBox.Show("Nominal harus berupa angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            Currency cur = null;
            currencies.TryGetValue(singkatan, out cur);

            if (cur == null || cur.Kurs <= 0.0)
            {
                try
                {
                    using (var conn = new MySqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = new MySqlCommand("SELECT id, nama, kurs FROM Tmatauang WHERE singkatan = @singkatan LIMIT 1", conn))
                        {
                            cmd.Parameters.AddWithValue("@singkatan", singkatan);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    cur = new Currency
                                    {
                                        Id = reader.GetInt32("id"),
                                        Singkatan = singkatan,
                                        Nama = reader.IsDBNull(reader.GetOrdinal("nama")) ? string.Empty : reader.GetString("nama"),
                                        Kurs = reader.IsDBNull(reader.GetOrdinal("kurs")) ? 0.0 : reader.GetDouble("kurs")
                                    };
                                    currencies[singkatan] = cur;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Tidak bisa mengambil kurs dari database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (cur == null || cur.Kurs <= 0.0)
            {
                MessageBox.Show("Kurs tidak tersedia untuk mata uang ini.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            double hasil = nominal * cur.Kurs;
            textBoxRupiah.Text = hasil.ToString("N0", CultureInfo.CurrentCulture);
            label2.Text = cur.Kurs.ToString("N2", CultureInfo.CurrentCulture);

            if (cur.Id > 0)
            {
                try
                {
                    using (var conn = new MySqlConnection(connString))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "INSERT INTO Tkonversi (id_matauang, nominal, hasil, tanggal) VALUES (@id, @nominal, @hasil, @tanggal)";
                            cmd.Parameters.AddWithValue("@id", cur.Id);
                            cmd.Parameters.AddWithValue("@nominal", nominal);
                            cmd.Parameters.AddWithValue("@hasil", hasil);
                            cmd.Parameters.AddWithValue("@tanggal", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}