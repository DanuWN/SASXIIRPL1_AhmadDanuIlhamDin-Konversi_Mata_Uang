using System;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SASXIIRPL1_AhmadDanuIlhamDin
{
    public partial class Form1 : Form
    {
        // Ganti password dan db sesuai environment-mu
        private string connString = "server=localhost;user=root;password='';database=dbkonversidanu;";

        public Form1()
        {
            InitializeComponent();
        }

        // Saat form load: isi comboBox dari database (jika tabel ada),
        // jika gagal, fallback ke daftar statis.
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxMataUang.Items.Clear();

            try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT singkatan FROM Tmatauang", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxMataUang.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Jika gagal connect atau tabel tidak ada, tambahkan opsi default
                comboBoxMataUang.Items.AddRange(new object[] { "USD", "JYN" });
            }

            if (comboBoxMataUang.Items.Count > 0)
                comboBoxMataUang.SelectedIndex = 0;
        }

        // Event handler tombol konversi
        private void buttonKonversi_Click(object sender, EventArgs e)
        {
            if (comboBoxMataUang.SelectedItem == null || string.IsNullOrWhiteSpace(textBoxNominal.Text))
            {
                MessageBox.Show("Pilih mata uang dan masukkan nominal.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string singkatan = comboBoxMataUang.SelectedItem.ToString();
            if (!double.TryParse(textBoxNominal.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out double nominal))
            {
                MessageBox.Show("Nominal harus berupa angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT id, nama, kurs FROM Tmatauang WHERE singkatan = @singkatan LIMIT 1";
                        cmd.Parameters.AddWithValue("@singkatan", singkatan);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Mata uang tidak ditemukan di database.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            int id = reader.GetInt32("id");
                            // nama mungkin tidak diperlukan di UI sekarang, tapi kita ambil.
                            string nama = reader.IsDBNull(reader.GetOrdinal("nama")) ? string.Empty : reader.GetString("nama");
                            double kurs = reader.GetDouble("kurs");

                            double hasil = nominal * kurs;
                            // format ribuan tanpa desimal
                            textBoxRupiah.Text = hasil.ToString("N0", CultureInfo.CurrentCulture);

                            reader.Close();

                            // Simpan konversi ke tabel Tkonversi
                            using (var cmd2 = conn.CreateCommand())
                            {
                                cmd2.CommandText = "INSERT INTO Tkonversi (id_matauang, nominal, hasil, tanggal) VALUES (@id, @nominal, @hasil, @tanggal)";
                                cmd2.Parameters.AddWithValue("@id", id);
                                cmd2.Parameters.AddWithValue("@nominal", nominal);
                                cmd2.Parameters.AddWithValue("@hasil", hasil);
                                cmd2.Parameters.AddWithValue("@tanggal", DateTime.Now);
                                cmd2.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi error saat koneksi/kueri: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}