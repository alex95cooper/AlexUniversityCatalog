using Dapper;
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
        const string ServerName = @"ALEXCOOPER\MSQLSERVER";
        const string CatalogName = "University";
        const int FetchRowsCount = 5;

        private readonly string _tableName;

        private SqlConnectionStringBuilder _stringBuilder = null;
        private string _nameOrderBy;
        private int _offsetCounter;

        public TableWindow(string tableName)
        {
            InitializeComponent();
            _tableName = tableName;
            _offsetCounter = 0;
            TableNameBlock.Text = tableName;
            _nameOrderBy = _tableName + ".ID";
            SetFieldsForStringBuilder();

            ShowTable();
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
            if (_offsetCounter + FetchRowsCount < GetEntityCount())
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
            AddWindow addWindow = new(_tableName, _stringBuilder.ConnectionString);
            addWindow.Owner = this;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addWindow.ShowDialog();
            ShowTable();
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
                using IDbConnection db = new SqlConnection(_stringBuilder.ConnectionString);
                db.Execute($"DELETE FROM {_tableName} WHERE ID = " + row.Row["ID"]);
                ShowTable();
            }
        }

        private void UpdteButton_Click(object sender, RoutedEventArgs e)
        {
            for (int item = 0; item < Table.Items.Count; item++)
            {
                try
                {
                    DataRowView rowData = Table.Items[item] as DataRowView;
                    SelectUpdateQuery(rowData);
                }
                catch
                {
                    MessageBox.Show(IncorrectDataErrorMessage);
                }
            }
        }

        private void SetFieldsForStringBuilder()
        {
            _stringBuilder = new();
            _stringBuilder.DataSource = ServerName;
            _stringBuilder.InitialCatalog = CatalogName;
            _stringBuilder.IntegratedSecurity = true;
        }

        private void ShowTable()
        {
            ShowPagesCounter();
            string sortingOrder = ((bool)AscendingButton.IsChecked) ? "ASC " : "DESC ";
            DataTable table = _tableName switch
            {
                "Faculties" => FacultyRepository.GetTable(_stringBuilder.ConnectionString, _nameOrderBy, sortingOrder, _offsetCounter, FetchRowsCount),
                "Subjects" => SubjectRepository.GetTable(_stringBuilder.ConnectionString, _nameOrderBy, sortingOrder, _offsetCounter, FetchRowsCount),
                "Teachers" => TeacherRepository.GetTable(_stringBuilder.ConnectionString, _nameOrderBy, sortingOrder, _offsetCounter, FetchRowsCount),
                "Students" => StudentRepository.GetTable(_stringBuilder.ConnectionString, _nameOrderBy, sortingOrder, _offsetCounter, FetchRowsCount),
                _ => new()
            };

            Table.ItemsSource = table.DefaultView;
        }

        private void ShowPagesCounter()
        {
            int rowCount = GetEntityCount();
            int pageCount = rowCount / FetchRowsCount;
            pageCount = (rowCount % FetchRowsCount > 0) ? pageCount + 1 : pageCount;
            PagesCounterBlock.Text = ((_offsetCounter / FetchRowsCount) + 1).ToString() + " / " + pageCount.ToString();
        }

        private int GetEntityCount()
        {
            using IDbConnection db = new SqlConnection(_stringBuilder.ConnectionString);      
            return db.ExecuteScalar<int>($"SELECT COUNT(*) FROM {_tableName}");
        }

        private void Table_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        public void SelectUpdateQuery(DataRowView rowData)
        {
            switch (_tableName)
            {
                case "Faculties":
                    FacultyRepository.Update(rowData, _stringBuilder.ConnectionString);
                    break;
                case "Subjects":
                    SubjectRepository.Update(rowData, _stringBuilder.ConnectionString);
                    break;
                case "Teachers":
                    TeacherRepository.Update(rowData, _stringBuilder.ConnectionString);
                    break;
                case "Students":
                    StudentRepository.Update(rowData, _stringBuilder.ConnectionString);
                    break;
            };
        }
    }
}
