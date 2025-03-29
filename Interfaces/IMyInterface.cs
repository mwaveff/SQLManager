using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace try1
{
    interface IDatabaseManager
    {
        void SelectAllFromTable(string table, string rowLimit, TextBox outputBox);
        void SelectColumn(string table, string column, string rowLimit, TextBox outputBox);
        void UpdateTable(string table, string column, string value, string id, TextBox outputBox);
        void AddNewColumn(string table, string column, string type, TextBox outputBox);
        void DeleteColumn(string table, string column, TextBox outputBox);
        void SelectByLetter(string table, string column, string letter, TextBox outputBox);
    }
}

