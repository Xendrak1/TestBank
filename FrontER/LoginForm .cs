using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace FrontER
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var baseUrl = txtBaseUrl.Text.Trim();
            var user = txtUser.Text.Trim();
            var pass = txtPass.Text.Trim();

            var http = new HttpClient { BaseAddress = new Uri(baseUrl) };
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}"));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

            try
            {
                var res = await http.PostAsync("/api/auth/login", null);
                if (res.IsSuccessStatusCode)
                {
                    Hide();
                    using var main = new MainForm(baseUrl, user, pass);
                    main.ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("Usuario/clave inválidos o API no disponible.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la API: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
