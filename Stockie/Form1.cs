using System;
using System.Windows.Forms;
using Stockie.Models;
using Stockie.Services;

namespace Stockie
{
    public partial class LoginForm : Form
    {
        private const string VERSION = "V0.01";

        public LoginForm()
        {
            InitializeComponent();
            this.Text = $"Stockie - Login {VERSION}";
            lblVersion.Text = VERSION;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize database when application starts
                Data.DatabaseHelper.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var user = UserService.AuthenticateUser(username, password);
                if (user != null)
                {
                    MessageBox.Show($"Welcome {user.Username}! You are logged in as {user.Role}.", "Login Successful",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open main form and hide login form, pass the authenticated user
                    this.Hide();
                    using (var main = new MainForm(user))
                    {
                        main.ShowDialog();
                    }

                    // After main form is closed, close the login form to end the application
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
