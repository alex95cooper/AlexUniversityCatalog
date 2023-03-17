using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace AlexUniversityCatalog
{
    public partial class TableWindow : Window
    {
        private const string OrderErrorMessage = "You entered incorrect name(s) of column(s)";
        private const string IncorrectDataErrorMessage = "You entered incorrect data";
        private const string DeleteErrorMessage = "Please, select row to delete!";
        private const string RowsCountQuery = "SELECT COUNT(*) FROM ";
        private const string ServerName = @"ALEXCOOPER\MSQLSERVER";
        private const string CatalogName = "University";
        private const int FetchRowsCount = 5;

        private SqlConnectionStringBuilder _stringBuilder = null;
        private SqlConnection _connection = null;
        private SelectQueryGenerator _selectQueryGenerator = null;
        private UpdateQueryGenerator _updateQueryGenerator = null;
        private SqlDataAdapter _adapter;
        private DataSet _dataSet = null;
        private string _nameOrderBy;
        private string _tableName;
        private int _offsetCounter;

        public TableWindow(string tableName)
        {
            InitializeComponent();
            OpenConnection();
            InitializeTable(tableName);
        }

        private void Table_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1 + _offsetCounter).ToString();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (_offsetCounter != 0)
            {
                _offsetCounter -= FetchRowsCount;
                ShowTable();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd = new(RowsCountQuery + _tableName, _connection);
            if (_offsetCounter + FetchRowsCount < (int)cmd.ExecuteScalar())
            {
                _offsetCounter += FetchRowsCount;
                ShowTable();
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            string oldNameOrderBy = _nameOrderBy;
            try
            {
                _nameOrderBy = OrderNameBox.Text;
                ShowTable();
            }
            catch
            {
                _nameOrderBy = oldNameOrderBy;
                MessageBox.Show(OrderErrorMessage);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new(_tableName, _connection);
            addWindow.Owner = this;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)Table.SelectedItem;
            if (row == null)
            {
                MessageBox.Show(DeleteErrorMessage);
            }
            else if (MessageBox.Show("Are you sure, you want to delete this row?", "Deleting", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SqlCommand command = new($"DELETE FROM {_tableName} WHERE ID = " + row.Row["ID"], _connection);
                command.ExecuteNonQuery();
                _dataSet.Tables[0].Rows.Remove(row.Row);
            }
        }

        private void UpdteButton_Click(object sender, RoutedEventArgs e)
        {
            for (int item = 0; item < Table.Items.Count - 1; item++)
            {
                try
                {
                    DataRowView rowData = Table.Items[item] as DataRowView;
                    SqlCommand command = new(_updateQueryGenerator.SelectUpdateQuery(rowData), _connection);
                    command.ExecuteNonQuery();
                    UpdateSubjectsIfNeeded(rowData);
                }
                catch
                {
                    MessageBox.Show(IncorrectDataErrorMessage);
                }
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _connection.Close();
        }

        private void Table_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void OpenConnection()
        {
            SetFieldsForStringBuilder();
            _connection = new(_stringBuilder.ConnectionString);
            _connection.Open();
        }

        private void SetFieldsForStringBuilder()
        {
            _stringBuilder = new();
            _stringBuilder.DataSource = ServerName;
            _stringBuilder.InitialCatalog = CatalogName;
            _stringBuilder.IntegratedSecurity = true;
        }

        private void InitializeTable(string tableName)
        {
            _tableName = tableName;
            _selectQueryGenerator = new(_tableName);
            _updateQueryGenerator = new(_tableName);
            TableNameBlock.Text = _tableName;
            _nameOrderBy = _tableName + ".ID";
            ShowTable();
        }

        private void ShowTable()
        {
            ShowPagesCounter();
            string sortingOrder = ((bool)AscendingButton.IsChecked) ? "ASC " : "DESC ";
            string query = _selectQueryGenerator.GetSelectQuery(_offsetCounter, FetchRowsCount, _nameOrderBy, sortingOrder);
            _adapter = new(query, _connection);
            _dataSet = new();
            _adapter.Fill(_dataSet);
            Table.ItemsSource = _dataSet.Tables[0].DefaultView;
        }

        private void ShowPagesCounter()
        {
            SqlCommand cmd = new(RowsCountQuery + _tableName, _connection);
            int rowCount = (int)cmd.ExecuteScalar();
            int pageCount = rowCount / FetchRowsCount;
            pageCount = (rowCount % FetchRowsCount > 0) ? pageCount + 1 : pageCount;
            PagesCounterBlock.Text = ((_offsetCounter / FetchRowsCount) + 1).ToString() + " / " + pageCount.ToString();
        }

        private void UpdateSubjectsIfNeeded(DataRowView rowData)
        {
            if (_tableName == "Students")
            {
                List<string> subjectNamesUiCollection = new(rowData["Subjects"].ToString().Split(", "));
                if (CheckIfSubjectsExists(subjectNamesUiCollection))
                {
                    SqlCommand command = new($"DELETE FROM Students_Subjects WHERE StudID = {rowData["ID"]}", _connection);
                    command.ExecuteNonQuery();
                    UpdateSubjects(rowData, subjectNamesUiCollection);
                }
                else
                {
                    MessageBox.Show(IncorrectDataErrorMessage);
                }
            }
        }

        private bool CheckIfSubjectsExists(List<string> subjectNamesUiCollection)
        {
            foreach (string subjectName in subjectNamesUiCollection)
            {
                SqlCommand command = new($"SELECT ID FROM Subjects WHERE Name = '{subjectName}'", _connection);
                if (command.ExecuteScalar() == null)
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateSubjects(DataRowView rowData, List<string> subjectNamesUiCollection)
        {
            foreach (string subjectName in subjectNamesUiCollection)
            {
                SqlCommand command = new(@$"INSERT INTO Students_Subjects (StudID, SubjID) 
                VALUES ({rowData["ID"]}, (SELECT ID FROM Subjects WHERE Name = '{subjectName}'))", _connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
