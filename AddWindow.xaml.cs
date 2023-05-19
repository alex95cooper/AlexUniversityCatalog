using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace AlexUniversityCatalog
{
    public partial class AddWindow : Window
    {
        private const string ErrorMessage = "You entered an invalid value(s). Maybe you not have filled in the required fields(*) or entered an already existing value.";

        private readonly string _tableName;
        private readonly SqlConnection _connection;
        private readonly Inserter _inserter;

        public AddWindow(string tableName, SqlConnection connection)
        {
            InitializeComponent();
            _tableName = tableName;
            _connection = connection;
            _inserter = new(_connection);
            SelectWindowView(tableName);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddToSelectedTable();
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

        private void AddToSelectedTable()
        {
            switch (_tableName)
            {
                case "Faculties":
                    _inserter.InsertFaculty(NameTextBox.Text, DescriptionTextBox.Text);
                    break;
                case "Subjects":
                    _inserter.InsertSubject(NameTextBox.Text, FacultyNameTextBox.Text,
                        DescriptionTextBox.Text);
                    break;
                case "Teachers":
                    _inserter.InsertTeacher(FirstNameTextBox.Text, LastNameTextBox.Text,
                        AgeTextBox.Text, ExperienceTextBox.Text, SubjectTextBox.Text);
                    break;
                case "Students":
                    _inserter.InsertStudent(FirstNameTextBox.Text, 
                        LastNameTextBox.Text, AgeTextBox.Text, YearTextBox.Text, FacultyTextBox.Text);
                    break;
            }
        }

        private void AddSubjectsIfNeeded()
        {
            if (_tableName == "Students")
            {
                try
                {
                    SqlCommand command = new("SELECT MAX(ID) FROM Students", _connection);
                    _inserter.InsertStudentsWithSubjects((int)command.ExecuteScalar(), SubjectsTextBox.Text);
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
