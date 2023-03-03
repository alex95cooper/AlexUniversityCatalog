using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;


namespace AlexUniversityCatalog
{
    public partial class TableWindow : Window
    {
        const string ServerName = @"ALEXCOOPER\MSQLSERVER";
        const string CatalogName = "University";
        const int FetchRowsCount = 5;

        readonly SqlConnection _conection = null;
        private readonly QueryGenerator _queryGenerator = null;

        private SqlConnectionStringBuilder _stringBuilder = null;
        private DataSet _dataSet = null;
        private string _tableName;
        private int _offsetCounter;

        public TableWindow(string tableName)
        {
            InitializeComponent();
            _queryGenerator = new(tableName);
            _tableName = tableName;
            _offsetCounter = 0;
            TableNameBlock.Text = tableName;
            SetFieldsForStringBuilder();
            _conection = new(_stringBuilder.ConnectionString);
            _conection.Open();

            ShowTable();
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
            SqlCommand cmd = new("SELECT COUNT(*) FROM " + _tableName, _conection);
            int rowCount = (int)cmd.ExecuteScalar();
            int pageCount = rowCount / FetchRowsCount;
            pageCount = (rowCount % FetchRowsCount > 0) ? pageCount + 1 : pageCount;
            PagesQuantityBlock.Text = ((_offsetCounter / FetchRowsCount) + 1).ToString() + " / " + pageCount.ToString();


            string selectQuery = _queryGenerator.GetSelectQuery(_offsetCounter, FetchRowsCount);
            SqlDataAdapter adapter = new(selectQuery, _conection);
            _dataSet = new();
            adapter.Fill(_dataSet);
            Table.ItemsSource = _dataSet.Tables[0].DefaultView;
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
            SqlCommand cmd = new("SELECT COUNT(*) FROM " + _tableName, _conection);
            if (_offsetCounter + FetchRowsCount < (int)cmd.ExecuteScalar())
            {
                _offsetCounter += FetchRowsCount;
                ShowTable();
            }
        }
    }
}
