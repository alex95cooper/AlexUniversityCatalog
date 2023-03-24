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
        const string ServerName = @"ALEXCOOPER\MSQLSERVER";
        const string CatalogName = "University";
        const int FetchRowsCount = 5;

        private readonly string _tableName;

        private object _entities = null;
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
            SetEntities(sortingOrder);
            DataTable table = _tableName switch
            {
                "Faculties" => FacultyRepository.GetTable((List<Faculty>)_entities, _offsetCounter, FetchRowsCount),
                "Subjects" => SubjectRepository.GetTable((List<Subject>)_entities, _offsetCounter, FetchRowsCount),
                "Teachers" => TeacherRepository.GetTable((List<Teacher>)_entities, _offsetCounter, FetchRowsCount),
                "Students" => StudentRepository.GetTable((List<Student>)_entities, _offsetCounter, FetchRowsCount),
                _ => new()
            };

            Table.ItemsSource = table.DefaultView;

            //string selectQuery = _queryGenerator.GetSelectQuery(_offsetCounter, FetchRowsCount);
            //SqlDataAdapter adapter = new(selectQuery, _connection);
            //_dataSet = new();
            //adapter.Fill(_dataSet);
            //Table.ItemsSource = _dataSet.Tables[0].DefaultView;
        }

        private void ShowPagesCounter()
        {
            int rowCount = GetEntityCount();
            int pageCount = rowCount / FetchRowsCount;
            pageCount = (rowCount % FetchRowsCount > 0) ? pageCount + 1 : pageCount;
            PagesCounterBlock.Text = ((_offsetCounter / FetchRowsCount) + 1).ToString() + " / " + pageCount.ToString();
        }

        private void SetEntities(string sortingOrder)
        {
            _entities = _tableName switch
            {
                "Faculties" => FacultyRepository.GetFaculties(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, sortingOrder),
                "Subjects" => SubjectRepository.GetSubjects(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, sortingOrder),
                "Teachers" => TeacherRepository.GetTeachers(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, sortingOrder),
                "Students" => StudentRepository.GetStudents(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, sortingOrder),
                _ => default
            };
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

        private int GetEntityCount()
        {
            return _tableName switch
            {
                "Faculties" => FacultyRepository.GetFaculties(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, "ASC").Count,
                "Subjects" => SubjectRepository.GetSubjects(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, "ASC").Count,
                "Teachers" => TeacherRepository.GetTeachers(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, "ASC").Count,
                "Students" => StudentRepository.GetStudents(_stringBuilder.ConnectionString, _tableName, _nameOrderBy, "ASC").Count,
                _ => default
            };
        }

        private void Table_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
