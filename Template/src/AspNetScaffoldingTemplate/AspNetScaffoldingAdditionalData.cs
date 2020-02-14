using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AspNetScaffoldingTemplate
{
    public partial class AspNetScaffoldingAdditionalData : Form
    {
        public Dictionary<string, bool> HasError { get; set; } = new Dictionary<string, bool>();

        public static string Company { get; set; }

        public static string Team { get; set; }

        public static string Path { get; set; }

        public static string Version { get; set; }

        public static string AuthorName { get; set; }

        public static string AuthorEmail { get; set; }

        public AspNetScaffoldingAdditionalData()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            this.HasError = new Dictionary<string, bool>
            {
                { "authorName", false },
                { "authorEmail", false },
                { "company", false },
                { "team", false },
                { "path", false },
                { "version", false }
            };

            AddPlaceholder("Thiago Barradas", this.authorNameTxt);
            AddPlaceholder("th.barradas@gmail.com", this.authorEmailTxt);
            AddPlaceholder("MyCompanyName", this.companyTxt);
            AddPlaceholder("MyTeamName", this.teamTxt);
            AddPlaceholder("path-prefix", this.pathTxt);
            AddPlaceholder("1", this.versionTxt, true);
        }

        private void doneBtn_Click(object sender, System.EventArgs e)
        {
            Company = this.companyTxt.Text;
            Team = this.teamTxt.Text;
            Path = this.pathTxt.Text;
            Version = this.versionTxt.Text;
            AuthorName = this.authorNameTxt.Text;
            AuthorEmail = this.authorEmailTxt.Text;
            this.Close();
        }

        private void companyTxt_TextChanged(object sender, System.EventArgs e)
        {
            this.companyTxt.Text =
               Regex.Replace(this.companyTxt.Text, "[^\\w]+", "");

            this.HasError["company"] = (this.companyTxt.Text.Length == 0);

            this.GoToEnd(this.companyTxt);
            this.CheckError();
        }

        private void teamTxt_TextChanged(object sender, System.EventArgs e)
        {
            this.teamTxt.Text =
                Regex.Replace(this.teamTxt.Text, "[^\\w]+", "");
            this.HasError["team"] = (this.teamTxt.Text.Length == 0);

            this.GoToEnd(this.teamTxt);
            this.CheckError();
        }

        private void pathTxt_TextChanged(object sender, System.EventArgs e)
        {
            this.pathTxt.Text =
               Regex.Replace(this.pathTxt.Text, "[^\\w]+", "").ToLowerInvariant();
            this.HasError["path"] = (this.pathTxt.Text.Length == 0);

            this.GoToEnd(this.pathTxt);
            this.CheckError();
        }

        private void versionTxt_TextChanged(object sender, System.EventArgs e)
        {
            this.versionTxt.Text =
               Regex.Replace(this.versionTxt.Text, "[^\\d]+", "").ToLowerInvariant();
            this.HasError["version"] = (this.versionTxt.Text.Length == 0);

            this.GoToEnd(this.versionTxt);
            this.CheckError();
        }

        private void authorEmailTxt_TextChanged(object sender, System.EventArgs e)
        {
            this.authorEmailTxt.Text = this.authorEmailTxt.Text.Trim().ToLowerInvariant();
            this.HasError["authorEmail"] = !this.IsValidEmail(this.authorEmailTxt.Text);

            this.GoToEnd(this.authorEmailTxt);
            this.CheckError();
        }

        private void authorNameTxt_TextChanged(object sender, System.EventArgs e)
        {
            this.authorNameTxt.Text = this.authorNameTxt.Text.TrimStart();
            this.HasError["authorName"] = (this.authorNameTxt.Text.Length == 0);

            this.GoToEnd(this.authorNameTxt);
            this.CheckError();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void backBtn_Click(object sender, System.EventArgs e)
        {
            throw new WizardBackoutException();
        }

        private void GoToEnd(TextBox txtBox)
        {
            txtBox.SelectionStart = txtBox.Text.Length;
            txtBox.SelectionLength = 0;
        }

        private void CheckError()
        {
            if (HasError.ContainsValue(true))
            {
                this.doneBtn.BackColor = Color.FromArgb(30, 30, 30);
                this.doneBtn.ForeColor = Color.FromArgb(200, 200, 200);
                this.doneBtn.Enabled = false;
            }
            else
            {
                this.doneBtn.BackColor = Color.FromArgb(67, 67, 70);
                this.doneBtn.ForeColor = Color.White;
                this.doneBtn.Enabled = true;
            }
        }

        private void AddPlaceholder(string placeholder, TextBox txtBox, bool ignoreReturn = false)
        {
            txtBox.GotFocus += (s, e) => { this.PlaceholderGotFocus(placeholder, txtBox, ignoreReturn); };
            txtBox.LostFocus += (s, e) => { this.PlaceholderLostFocus(placeholder, txtBox, ignoreReturn); };
            txtBox.TextChanged += (s, e) => { this.PlaceholderTextChanged(placeholder, txtBox, ignoreReturn); };
            txtBox.Text = placeholder;
            txtBox.ForeColor = Color.FromArgb(200, 200, 200);
        }

        private void PlaceholderGotFocus(string placeholder, TextBox txtBox, bool ignoreReturn)
        {
            if (ignoreReturn)
            {
                txtBox.ForeColor = Color.White;
                return;
            }

            if (txtBox.Text.Trim() == placeholder)
            {
                txtBox.Text = "";
                txtBox.ForeColor = Color.White;
            }
        }

        private void PlaceholderLostFocus(string placeholder, TextBox txtBox, bool ignoreReturn)
        {
            if (ignoreReturn)
            {
                txtBox.ForeColor = Color.White;
                return;
            }

            if (txtBox.Text.Trim() == "")
            {
                txtBox.Text = placeholder;
                txtBox.ForeColor = Color.FromArgb(200, 200, 200);
            }
        }

        private void PlaceholderTextChanged(string placeholder, TextBox txtBox, bool ignoreReturn)
        {
            if (ignoreReturn)
            {
                txtBox.ForeColor = Color.White;
                return;
            }

            if (txtBox.Text.Trim() == placeholder)
            {
                txtBox.ForeColor = Color.FromArgb(30, 30, 30);
            }
            else
            {
                txtBox.ForeColor = Color.White;
            }
        }
    }
}
