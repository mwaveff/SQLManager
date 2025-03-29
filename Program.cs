using Google.Protobuf.WellKnownTypes;
using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Windows.Forms;
using try1;

public class SqlManeger : Form
{
    private IDatabaseManager _dbManager;
    private TextBox outputBox, textBoxLimit, textBoxTable, textBoxColumn, textBoxValue, textBoxId;
    private Button button1, button2, button3, button4, button5, button6, button7;

    public SqlManeger()
    {
        InitializeComponent();
        Load += MainForm_Load;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        string connectionString = ShowConnectionDialog();
        if (string.IsNullOrEmpty(connectionString))
        {
            MessageBox.Show("Failed to initialize connection string.");
            return;
        }

        _dbManager = new MySQLManager(connectionString);

        if (_dbManager == null)
        {
            MessageBox.Show("Database manager not initialized.");
            return;
        }
    }

    private string ShowConnectionDialog()
    {
        Form dialog = new Form() { Width = 350, Height = 250, Text = "Connection data" };
        Label labelServer = new Label() { Left = 10, Top = 20, Text = "Server:" };
        Label labelDatabase = new Label() { Left = 10, Top = 50, Text = "Database:" };
        Label labelUser = new Label() { Left = 10, Top = 80, Text = "User:" };
        Label labelPassword = new Label() { Left = 10, Top = 110, Text = "Password:" };

        TextBox textBoxServer = new TextBox() { Left = 120, Top = 20, Width = 200 };
        TextBox textBoxDatabase = new TextBox() { Left = 120, Top = 50, Width = 200 };
        TextBox textBoxUser = new TextBox() { Left = 120, Top = 80, Width = 200 };
        TextBox textBoxPassword = new TextBox() { Left = 120, Top = 110, Width = 200, PasswordChar = '*' };

        Button buttonOk = new Button() { Text = "OK", Left = 120, Width = 100, Top = 150, DialogResult = DialogResult.OK };
        buttonOk.Click += (sender, e) => { dialog.Close(); };

        dialog.Controls.Add(labelServer);
        dialog.Controls.Add(labelDatabase);
        dialog.Controls.Add(labelUser);
        dialog.Controls.Add(labelPassword);
        dialog.Controls.Add(textBoxServer);
        dialog.Controls.Add(textBoxDatabase);
        dialog.Controls.Add(textBoxUser);
        dialog.Controls.Add(textBoxPassword);
        dialog.Controls.Add(buttonOk);

        dialog.AcceptButton = buttonOk;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            return $"Server={textBoxServer.Text};Database={textBoxDatabase.Text};User Id={textBoxUser.Text};Password={textBoxPassword.Text};";
        }
        return null;
    }

    private void TextBoxTable_TextChanged(object sender, EventArgs e)
    {

    }
    private void TextBoxLimit_TextChanged(object sender, EventArgs e)
    {

    }
    private void TextBoxColumn_TextChanged(object sender, EventArgs e)
    {

    }
    private void TextBoxValue_TextChanged(object sender, EventArgs e)
    {

    }
    private void TextBoxId_TextChanged(object sender, EventArgs e)
    {

    }

    private void Button1_Click(object sender, EventArgs e)
    {
        string tableName = textBoxTable.Text.Trim();
        string rowLimit = textBoxLimit.Text.Trim();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            MessageBox.Show("Enter a table name!");
            return;
        }

        if (!int.TryParse(rowLimit, out int limit) || limit <= 0)
        {
            MessageBox.Show("Invalid limit. Enter a number greater than 0..");
            return;
        }

        _dbManager.SelectAllFromTable(tableName, rowLimit, outputBox);
    }

    private void Button2_Click(object sender, EventArgs e)
    {
        string tableName = textBoxTable.Text.Trim();
        string rowLimit = textBoxLimit.Text.Trim();
        string column = textBoxColumn.Text.Trim();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            MessageBox.Show("Enter the table name!");
            return;
        }

        if (string.IsNullOrWhiteSpace(column))
        {
            MessageBox.Show("Enter a column name!");
            return;
        }

        if (!int.TryParse(rowLimit, out int limit) || limit <= 0)
        {
            MessageBox.Show("Invalid limit. Enter a number greater than 0..");
            return;
        }

        _dbManager.SelectColumn(tableName, column, rowLimit, outputBox);
    }

    private void Button3_Click(object sender, EventArgs e)
    {
        string tableName = textBoxTable.Text.Trim();
        string column = textBoxColumn.Text.Trim();
        string value = textBoxValue.Text.Trim();
        string id = textBoxId.Text.Trim();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            MessageBox.Show("Enter the table name!");
            return;
        }

        if (string.IsNullOrWhiteSpace(column))
        {
            MessageBox.Show("Введіть назву стовпця!");
            return;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            MessageBox.Show("Enter a value!");
            return;
        }

        if (!int.TryParse(id, out int idValue) || idValue <= 0)
        {
            MessageBox.Show("Invalid ID. Please enter a number greater than 0.");
            return;
        }

        _dbManager.UpdateTable(tableName, column, value, id, outputBox);
    }

    private void Button4_Click(object sender, EventArgs e)
    {
        string tableName = textBoxTable.Text.Trim();
        string column = textBoxColumn.Text.Trim();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            MessageBox.Show("Enter the table name!");
            return;
        }

        if (string.IsNullOrWhiteSpace(column))
        {
            MessageBox.Show("Введіть назву стовпця!");
            return;
        }

        _dbManager.DeleteColumn(tableName, column, outputBox);
    }

    private void Button5_Click(object sender, EventArgs e)
    {
        string tableName = textBoxTable.Text.Trim();
        string column = textBoxColumn.Text.Trim();
        string type = textBoxValue.Text.Trim();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            MessageBox.Show("Enter the table name!");
            return;
        }

        if (string.IsNullOrWhiteSpace(column))
        {
            MessageBox.Show("Enter a column name!");
            return;
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            MessageBox.Show("Enter the data type for the column.!");
            return;
        }

        _dbManager.AddNewColumn(tableName, column, type, outputBox);
    }

    private void ButtonSelectByLetter_Click(object sender, EventArgs e)
    {
        string tableName = textBoxTable.Text.Trim();
        string column = textBoxColumn.Text.Trim();
        string letter = textBoxValue.Text.Trim();

        if (string.IsNullOrWhiteSpace(tableName))
        {
            MessageBox.Show("Enter the table name!");
            return;
        }

        if (string.IsNullOrWhiteSpace(column))
        {
            MessageBox.Show("Enter the column name!");
            return;
        }

        if (string.IsNullOrWhiteSpace(letter))
        {
            MessageBox.Show("Enter a letter to search!");
            return;
        }

        _dbManager.SelectByLetter(tableName, column, letter, outputBox);
    }

    private void Button7_Click(object sender, EventArgs e)
    {
        outputBox.Text = "";
    }

    private void InitializeComponent()
    {
            
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.textBoxLimit = new System.Windows.Forms.TextBox();
            this.textBoxTable = new System.Windows.Forms.TextBox();
            this.textBoxColumn = new System.Windows.Forms.TextBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Info;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.button1.Location = new System.Drawing.Point(12, 376);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(385, 139);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select All";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.Info;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Location = new System.Drawing.Point(12, 511);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(385, 139);
            this.button4.TabIndex = 3;
            this.button4.Text = "Delete Column";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Info;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Location = new System.Drawing.Point(773, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(385, 139);
            this.button2.TabIndex = 5;
            this.button2.Text = "Select column";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.Info;
            this.button3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button3.Location = new System.Drawing.Point(392, 511);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(385, 139);
            this.button3.TabIndex = 6;
            this.button3.Text = "Add new column";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.Button5_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Info;
            this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.Location = new System.Drawing.Point(392, 376);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(385, 139);
            this.button5.TabIndex = 3;
            this.button5.Text = "Update column";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.Button3_Click);
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(392, 48);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(385, 232);
            this.outputBox.TabIndex = 0;
            this.outputBox.Text = "Enter...";
            this.outputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.outputBox.UseWaitCursor = true;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.Info;
            this.button6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button6.Location = new System.Drawing.Point(773, 511);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(385, 139);
            this.button6.TabIndex = 9;
            this.button6.Text = "Select by letter";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.ButtonSelectByLetter_Click);
            // 
            // textBoxLimit
            // 
            this.textBoxLimit.Location = new System.Drawing.Point(92, 195);
            this.textBoxLimit.Name = "textBoxLimit";
            this.textBoxLimit.Size = new System.Drawing.Size(200, 34);
            this.textBoxLimit.TabIndex = 0;
            this.textBoxLimit.Text = "Limit";
            this.textBoxLimit.TextChanged += new System.EventHandler(this.TextBoxLimit_TextChanged);
            // 
            // textBoxTable
            // 
            this.textBoxTable.Location = new System.Drawing.Point(92, 97);
            this.textBoxTable.Name = "textBoxTable";
            this.textBoxTable.Size = new System.Drawing.Size(200, 34);
            this.textBoxTable.TabIndex = 1;
            this.textBoxTable.Text = "Table";
            this.textBoxTable.TextChanged += new System.EventHandler(this.TextBoxTable_TextChanged);
            // 
            // textBoxColumn
            // 
            this.textBoxColumn.Location = new System.Drawing.Point(92, 146);
            this.textBoxColumn.Name = "textBoxColumn";
            this.textBoxColumn.Size = new System.Drawing.Size(200, 34);
            this.textBoxColumn.TabIndex = 1;
            this.textBoxColumn.Text = "Column";
            this.textBoxColumn.TextChanged += new System.EventHandler(this.TextBoxColumn_TextChanged);
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(92, 246);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(200, 34);
            this.textBoxValue.TabIndex = 1;
            this.textBoxValue.Text = "Value";
            this.textBoxValue.TextChanged += new System.EventHandler(this.TextBoxId_TextChanged);
            // 
            // textBoxId
            // 
            this.textBoxId.Location = new System.Drawing.Point(92, 295);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(200, 34);
            this.textBoxId.TabIndex = 1;
            this.textBoxId.Text = "Id";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.Info;
            this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button7.Location = new System.Drawing.Point(392, 295);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(385, 52);
            this.button7.TabIndex = 10;
            this.button7.Text = "CLEAR";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // SqlManeger
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1173, 756);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBoxLimit);
            this.Controls.Add(this.textBoxTable);
            this.Controls.Add(this.textBoxColumn);
            this.Controls.Add(this.textBoxValue);
            this.Controls.Add(this.textBoxId);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "SqlManeger";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new SqlManeger());
    }
}
