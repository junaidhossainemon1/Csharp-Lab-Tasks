using System;
using System.Drawing;
using System.Windows.Forms;

namespace FinalLab_FormSubmission
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Wire up required events globally to avoid mismatches
            this.Load += Form1_Load;
            this.Username.TextChanged += Username_TextChanged;
            this.button1.Click += button1_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateSubmitButton();
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {
            UpdateSubmitButton();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateSubmitButton();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false; // Uncheck Female if Male is checked
            }
            UpdateSubmitButton();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false; // Uncheck Male if Female is checked
            }
            UpdateSubmitButton();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSubmitButton();
        }

        private void B_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSubmitButton();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                MessageBox.Show("Please complete all required fields properly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string gender = checkBox1.Checked ? "Male" : "Female";
            string group = radioButton1.Checked ? "A" : "B";

            string passwordMask = new string('*', Password.Text.Length);
            string message = $"Username: {Username.Text}\n" +
                             $"Password: {passwordMask}\n" +
                             $"Gender: {gender}\n" +
                             $"Group: {group}\n\n" +
                             "Do you want to submit?";

            DialogResult dr = MessageBox.Show(message, "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Record submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter a username with at least 3 characters.", "Hint", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter a password with at least 6 characters.", "Hint", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please select your gender.", "Hint", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please select your group (A or B).", "Hint", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- Empty event handlers that the Designer relies on ---
        private void checkBox3_CheckedChanged(object sender, EventArgs e) { }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { }

        // --- Validation Logic below ---

        private bool ValidateUsername()
        {
            bool isValid = !string.IsNullOrWhiteSpace(Username.Text) && Username.Text.Length >= 3;
            Username.BackColor = isValid ? SystemColors.Window : Color.LightPink;
            return isValid;
        }

        private bool ValidatePassword()
        {
            bool isValid = !string.IsNullOrWhiteSpace(Password.Text) && Password.Text.Length >= 6;
            Password.BackColor = isValid ? SystemColors.Window : Color.LightPink;
            return isValid;
        }

        private bool IsFormValid()
        {
            return ValidateUsername()
                   && ValidatePassword()
                   && (checkBox1.Checked || checkBox2.Checked)
                   && (radioButton1.Checked || B.Checked);
        }

        private void UpdateSubmitButton()
        {
            button1.Enabled = IsFormValid();
        }
    }
}