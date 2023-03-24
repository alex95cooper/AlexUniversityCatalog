using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AlexUniversityCatalog
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private const string ErrorMessage = "You entered an invalid value(s). Maybe you not have filled in the required fields(*) or entered an already existing value.";

        private readonly string _tableName;
        private readonly SqlConnection _connection;
        private readonly InsertQueryGenerator _insertQureyGenerator;

        public AddWindow(string tableName, SqlConnection connection)
        {
            InitializeComponent();
            _tableName = tableName;
            _connection = connection;
            _insertQureyGenerator = new(tableName);
            SelectWindowView(tableName);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = SelectSqlQuery();
                SqlCommand sqlCommand = new(query, _connection);
                sqlCommand.ExecuteNonQuery();
                AddSubjectsIfNeeded();
                this.DialogResult = true;
            }
            catch
            {
                MessageBox.Show(ErrorMessage);
            }
        }

        private void SelectWindowView(string tableName)
        {
            if (tableName == "Faculties" || tableName == "Subjects")
            {
                FacultyOrSubjectsGrid.Visibility = Visibility.Visible;
            }
            else
            {
                TeachersOrStudentsGrid.Visibility = Visibility.Visible;
            }

            switch (tableName)
            {
                case "Subjects":
                    SubjectsGrid.Visibility = Visibility.Visible;
                    break;
                case "Teachers":
                    TeachersGrid.Visibility = Visibility.Visible;
                    break;
                case "Students":
                    StudentsGrid.Visibility = Visibility.Visible;
                    break;
            };
        }

        private string SelectSqlQuery()
        {
            return _tableName switch
            {
                "Faculties" => GetInsertFacultyQuery(),
                "Subjects" => GetInsertSubjectQuery(),
                "Teachers" => GetInsertTeacherQuery(),
                "Students" => GetInsertStudentsQuery(),
                _ => string.Empty
            };
        }

        private string GetInsertFacultyQuery()
        {
            return _insertQureyGenerator.GetInsertFacultyQuery(NameTextBox.Text, DescriptionTextBox.Text);
        }

        private string GetInsertSubjectQuery()
        {
            return _insertQureyGenerator.GetInsertSubjectQuery(NameTextBox.Text,
                FacultyNameTextBox.Text, DescriptionTextBox.Text);
        }

        private string GetInsertTeacherQuery()
        {
            return _insertQureyGenerator.GetInsertTeacherQuery(FirstNameTextBox.Text,
                LastNameTextBox.Text, AgeTextBox.Text, ExperienceTextBox.Text, SubjectTextBox.Text);
        }

        private string GetInsertStudentsQuery()
        {
            return _insertQureyGenerator.GetInsertStudentsQuery(FirstNameTextBox.Text,
                LastNameTextBox.Text, AgeTextBox.Text, YearTextBox.Text, FacultyTextBox.Text);
        }

        private void AddSubjectsIfNeeded()
        {
            if (_tableName == "Students")
            {
                try
                {
                    SqlCommand command = new("SELECT MAX(ID) FROM Students", _connection);
                    List<string> queries = InsertQueryGenerator.GetInsertStudentsSubjectsQueries((int)command.ExecuteScalar(), SubjectsTextBox.Text);
                    foreach (string query in queries)
                    {
                        command = new(query, _connection);
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    CancelAddingStudent();
                }
            }
        }

        private void CancelAddingStudent()
        {
            SqlCommand command = new("DELETE FROM Students WHERE ID = (SELECT MAX(ID) FROM Students)", _connection);
            command.ExecuteNonQuery();
            MessageBox.Show(ErrorMessage);
        }
    }
}
