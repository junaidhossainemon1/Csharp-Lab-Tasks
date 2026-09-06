using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LabtaskForm_final
{
    public partial class Form1 : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CustomerDB;Integrated Security=True;Encrypt=False";

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name.");
                return;
            }

            var gender = GetSelectedGender();
            var category = GetSelectedCategory();

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var checkCmd = new SqlCommand("SELECT COUNT(1) FROM Customers WHERE [Name] = @name", conn);
            checkCmd.Parameters.AddWithValue("@name", name);
            var exists = (int)checkCmd.ExecuteScalar() > 0;

            if (exists)
            {
                MessageBox.Show("Customer Already Existed");
            }
            else
            {
                using var insertCmd = new SqlCommand(
                    "INSERT INTO Customers([Name],[Password],[Gender],[Category]) VALUES(@name,@password,@gender,@category)",
                    conn);
                insertCmd.Parameters.AddWithValue("@name", name);
                insertCmd.Parameters.AddWithValue("@password", string.IsNullOrEmpty(txtPassword.Text) ? (object)DBNull.Value : txtPassword.Text);
                insertCmd.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(gender) ? (object)DBNull.Value : gender);
                insertCmd.Parameters.AddWithValue("@category", string.IsNullOrEmpty(category) ? (object)DBNull.Value : category);

                var rows = insertCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Added information");
                }
                else
                {
                    MessageBox.Show("Insert failed");
                }
            }

            ClearFields();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name to update.");
                return;
            }

            var gender = GetSelectedGender();
            var category = GetSelectedCategory();

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var checkCmd = new SqlCommand("SELECT COUNT(1) FROM Customers WHERE [Name] = @name", conn);
            checkCmd.Parameters.AddWithValue("@name", name);
            var exists = (int)checkCmd.ExecuteScalar() > 0;

            if (!exists)
            {
                MessageBox.Show("User not found");
            }
            else
            {
                using var updateCmd = new SqlCommand(
                    "UPDATE Customers SET [Password]=@password, [Gender]=@gender, [Category]=@category WHERE [Name]=@name",
                    conn);
                updateCmd.Parameters.AddWithValue("@name", name);
                updateCmd.Parameters.AddWithValue("@password", string.IsNullOrEmpty(txtPassword.Text) ? (object)DBNull.Value : txtPassword.Text);
                updateCmd.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(gender) ? (object)DBNull.Value : gender);
                updateCmd.Parameters.AddWithValue("@category", string.IsNullOrEmpty(category) ? (object)DBNull.Value : category);

                var rows = updateCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Updated information");
                }
                else
                {
                    MessageBox.Show("Update failed");
                }
            }

            ClearFields();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name to delete.");
                return;
            }

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var checkCmd = new SqlCommand("SELECT COUNT(1) FROM Customers WHERE [Name] = @name", conn);
            checkCmd.Parameters.AddWithValue("@name", name);
            var exists = (int)checkCmd.ExecuteScalar() > 0;

            if (!exists)
            {
                MessageBox.Show("User not found");
            }
            else
            {
                using var deleteCmd = new SqlCommand("DELETE FROM Customers WHERE [Name] = @name", conn);
                deleteCmd.Parameters.AddWithValue("@name", name);
                var rows = deleteCmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Deleted information");
                }
                else
                {
                    MessageBox.Show("Delete failed");
                }
            }

            ClearFields();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            var name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name to search.");
                return;
            }

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            using var searchCmd = new SqlCommand("SELECT [Name],[Password],[Gender],[Category] FROM Customers WHERE [Name] = @name", conn);
            searchCmd.Parameters.AddWithValue("@name", name);

            using var reader = searchCmd.ExecuteReader();
            if (reader.Read())
            {
                var foundName = reader["Name"]?.ToString();
                var pwd = reader["Password"]?.ToString();
                var gender = reader["Gender"]?.ToString();
                var category = reader["Category"]?.ToString();

                MessageBox.Show($"Name: {foundName}\nPassword: {pwd}\nGender: {gender}\nCategory: {category}");
            }
            else
            {
                MessageBox.Show("user not found");
            }

            ClearFields();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private string GetSelectedGender()
        {
            if (rdoMale.Checked) return "Male";
            if (rdoFemale.Checked) return "Female";
            return null;
        }

        private string GetSelectedCategory()
        {
            if (rdoRegular.Checked) return "Regular";
            if (rdoIrregular.Checked) return "Irregular";
            return null;
        }

        private void ClearFields()
        {
            txtName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            rdoMale.Checked = false;
            rdoFemale.Checked = false;
            rdoRegular.Checked = false;
            rdoIrregular.Checked = false;
        }
    }
}
