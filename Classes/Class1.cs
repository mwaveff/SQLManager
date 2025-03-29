using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;



namespace try1
{
    class MySQLManager : IDatabaseManager
    {
        private readonly string _connectionString;

        public MySQLManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        private bool IsValidIdentifier(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$");
        }

        public void SelectAllFromTable(string table, string rowLimit, TextBox outputBox)
        {
            if (!IsValidIdentifier(table))
            {
                outputBox.Text = "Invalid table name!";
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = $"SELECT * FROM `{table}` LIMIT {rowLimit}";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        outputBox.Clear();
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                outputBox.AppendText(reader[i] + "\t");
                            }
                            outputBox.AppendText("\n");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                outputBox.Text = "SQL Error: " + ex.Message;
            }
        }

        public void SelectColumn(string table, string column, string rowLimit, TextBox outputBox)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = $"SELECT {column} FROM {table} LIMIT @limit;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@limit", int.TryParse(rowLimit, out int lim) ? lim : 10);

                        using (var reader = cmd.ExecuteReader())
                        {
                            outputBox.Clear();
                            outputBox.AppendText($"{column}\n");
                            outputBox.AppendText(new string('-', 30) + "\n");

                            while (reader.Read())
                            {
                                outputBox.AppendText(reader[column].ToString() + "\n");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                outputBox.Text = "SQL Error: " + ex.Message;
            }
        }

        public void UpdateTable(string table, string column, string value, string id, TextBox outputBox)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = $"UPDATE {table} SET {column} = @value WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@id", int.TryParse(id, out int parsedId) ? parsedId : 0);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        outputBox.Clear();
                        outputBox.AppendText(rowsAffected > 0 ? "Update successful!" : "No rows updated.");
                    }
                }
            }
            catch (MySqlException ex)
            {
                outputBox.Text = "SQL Error: " + ex.Message;
            }
        }

        public void AddNewColumn(string table, string column, string type, TextBox outputBox)
        {
            if (!IsValidIdentifier(table) || !IsValidIdentifier(column))
            {
                outputBox.Text = "Invalid table or column name!";
                return;
            }

            if (!Regex.IsMatch(type, @"^(INT|VARCHAR\(\d+\)|TEXT|DATE|DECIMAL\(\d+,\d+\))$", RegexOptions.IgnoreCase))
            {
                outputBox.Text = "Invalid column type!";
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = $"ALTER TABLE `{table}` ADD `{column}` {type}";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        outputBox.Text = $"Column '{column}' ({type}) added to '{table}' successfully!";
                    }
                }
            }
            catch (MySqlException ex)
            {
                outputBox.Text = "SQL Error: " + ex.Message;
            }
        }

        public void DeleteColumn(string table, string column, TextBox outputBox)
        {
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = $"ALTER TABLE {table} DROP COLUMN {column}";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        outputBox.Text = $"Column '{column}' deleted from '{table}' successfully!";
                    }
                }
            }
            catch (Exception ex)
            {
                outputBox.Text = "SQL Error: " + ex.Message;
            }
        }
        public void SelectByLetter(string table, string column, string letter, TextBox outputBox)
        {
            if (string.IsNullOrWhiteSpace(letter) || letter.Length != 1)
            {
                MessageBox.Show("Please enter a single letter.");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = $"SELECT {column} FROM {table} WHERE {column} LIKE @letter;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@letter", letter + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            outputBox.Clear();
                            outputBox.AppendText($"{column}\n");
                            outputBox.AppendText(new string('-', 30) + "\n");

                            while (reader.Read())
                            {
                                outputBox.AppendText(reader[column].ToString() + "\n");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                outputBox.Text = "SQL Error: " + ex.Message;
            }
        }
    }
}


public class PlaceholderTextBox : TextBox
{
    private string _placeholderText;

    public string PlaceholderText
    {

        get { return _placeholderText; }
        set { _placeholderText = value; }

    }

    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        if (Text == _placeholderText)
        {
            Text = "";
            ForeColor = Color.Black;
        }
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        if (Text == "")
        {
            Text = _placeholderText;
            ForeColor = Color.Gray;
        }
    }
}

