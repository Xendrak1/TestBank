using System.Drawing;
using System.Windows.Forms;

namespace FrontER
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtBaseUrl;
        private TextBox txtUser;
        private TextBox txtPass;
        private Button btnLogin;
        private Label lblUrl;
        private Label lblUser;
        private Label lblPass;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtBaseUrl = new TextBox();
            txtUser = new TextBox();
            txtPass = new TextBox();
            btnLogin = new Button();
            lblUrl = new Label();
            lblUser = new Label();
            lblPass = new Label();

            // LoginForm
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 200);
            Controls.Add(lblUrl);
            Controls.Add(txtBaseUrl);
            Controls.Add(lblUser);
            Controls.Add(txtUser);
            Controls.Add(lblPass);
            Controls.Add(txtPass);
            Controls.Add(btnLogin);
            Name = "LoginForm";
            Text = "Login API BCP";

            // lblUrl
            lblUrl.AutoSize = true;
            lblUrl.Location = new Point(20, 22);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(69, 20);
            lblUrl.Text = "Base URL:";

            // txtBaseUrl
            txtBaseUrl.Location = new Point(120, 18);
            txtBaseUrl.Name = "txtBaseUrl";
            txtBaseUrl.Size = new Size(270, 27);
            txtBaseUrl.Text = "https://localhost:7195";

            // lblUser
            lblUser.AutoSize = true;
            lblUser.Location = new Point(20, 62);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(60, 20);
            lblUser.Text = "Usuario:";

            // txtUser
            txtUser.Location = new Point(120, 58);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(270, 27);
            txtUser.Text = "admin";

            // lblPass
            lblPass.AutoSize = true;
            lblPass.Location = new Point(20, 102);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(82, 20);
            lblPass.Text = "Contraseña:";

            // txtPass
            txtPass.Location = new Point(120, 98);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(270, 27);
            txtPass.UseSystemPasswordChar = true;
            txtPass.Text = "admin123";

            // btnLogin
            btnLogin.Location = new Point(120, 140);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(120, 30);
            btnLogin.Text = "Ingresar";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
        }
    }
}
