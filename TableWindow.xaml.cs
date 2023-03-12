using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace AlexUniversityCatalog
{
    public partial class TableWindow : Window
    {
        private const string RowsCountQuery = "SELECT COUNT(*) FROM ";
        private const string ServerName = @"ALEXCOOPER\MSQLSERVER";
        private const string CatalogName = "University";
        private const int FetchRowsCount = 5;

        private SqlConnectionStringBuilder _stringBuilder = null;
        private SqlConnection _conection = null;
        private SelectQueryGenerator _selectQueryGenerator = null;
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
            SqlCommand cmd = new(RowsCountQuery + _tableName, _conection);
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
                MessageBox.Show("You entered incorrect name(s) of column(s)");
            }
        }

        private void OpenConnection()
        {
            SetFieldsForStringBuilder();
            _conection = new(_stringBuilder.ConnectionString);
            _conection.Open();
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
            _selectQueryGenerator = new(tableName);
            _tableName = tableName;
            TableNameBlock.Text = tableName;
            _nameOrderBy = tableName + ".ID";
            ShowTable();
        }

        private void ShowTable()
        {
            ShowPagesCounter();
            string sortingOrder = ((bool)AscendingButton.IsChecked) ? "ASC " : "DESC ";
            string query = _selectQueryGenerator.GetSelectQuery(_offsetCounter, FetchRowsCount, _nameOrderBy, sortingOrder);
            SqlDataAdapter adapter = new(query, _conection);
            _dataSet = new();
            adapter.Fill(_dataSet);
            Table.ItemsSource = _dataSet.Tables[0].DefaultView;
        }

        private void ShowPagesCounter()
        {
            SqlCommand cmd = new(RowsCountQuery + _tableName, _conection);
            int rowCount = (int)cmd.ExecuteScalar();
            int pageCount = rowCount / FetchRowsCount;
            pageCount = (rowCount % FetchRowsCount > 0) ? pageCount + 1 : pageCount;
            PagesCounterBlock.Text = ((_offsetCounter / FetchRowsCount) + 1).ToString() + " / " + pageCount.ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new(_tableName, _conection);
            addWindow.Owner = this;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, System.EventArgs e)
        {

        }
    }
}
