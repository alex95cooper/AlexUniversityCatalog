using System.Collections.Generic;
using System.Windows;

namespace AlexUniversityCatalog
{
    public partial class AddWindow : Window
    {
        private const string ErrorMessage = "You entered an invalid value(s). Maybe you not have filled in the required fields(*) or entered an already existing value.";

        private readonly string _tableName;
        private readonly string _connectionString;

        public AddWindow(string tableName, string connectionString)
        {
            InitializeComponent();
            _tableName = tableName;
            _connectionString = connectionString;
            SelectWindowView(tableName);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectEntityToInsert();
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

        private void SelectEntityToInsert()
        {
            switch (_tableName)
            {
                case "Faculties":
                    InsertNewFaculty();
                    break;
                case "Subjects":
                    InsertNewSubject();
                    break;
                case "Teachers":
                    InsertNewTeacher();
                    break;
                case "Students":
                    InsertNewStudent();
                    break;
            };
        }

        private void InsertNewFaculty()
        {
            Faculty faculty = new();
            faculty.Name = NameTextBox.Text;
            faculty.Description = DescriptionTextBox.Text;
            FacultyRepository.Insert(faculty, _connectionString);
        }

        private void InsertNewSubject()
        {
            Subject subject = new();
            subject.Name = NameTextBox.Text;
            subject.Description = DescriptionTextBox.Text;
            subject.Faculty = new();
            subject.Faculty.Name = FacultyNameTextBox.Text;
            SubjectRepository.Insert(subject, _connectionString);
        }

        private void InsertNewTeacher()
        {
            Teacher teacher = new();
            teacher.FirstName = FirstNameTextBox.Text;
            teacher.LastName = LastNameTextBox.Text;
            teacher.Age = int.Parse(AgeTextBox.Text);
            teacher.Experience = int.Parse(ExperienceTextBox.Text);
            teacher.Subject = new();
            teacher.Subject.Name = SubjectTextBox.Text;
            TeacherRepository.Insert(teacher, _connectionString);
        }

        private void InsertNewStudent()
        {
            Student student = new();
            student.FirstName = FirstNameTextBox.Text;
            student.LastName = LastNameTextBox.Text;
            student.Age = int.Parse(AgeTextBox.Text);
            student.Year = int.Parse(YearTextBox.Text);
            student = FillStudentWithSubjects(student);
            student.Faculty = new();
            student.Faculty.Name= FacultyTextBox.Text;
            StudentRepository.Insert(student, _connectionString);
        }

        private Student FillStudentWithSubjects(Student student)
        {
            student.Subjects = new();
            List<string> subjectNamesCollection = new(SubjectsTextBox.Text.Split(", "));
            foreach (string subjectName in subjectNamesCollection)
            {
                Subject subject = new();
                subject.Name = subjectName;
                student.Subjects.Add(subject);
            }

            return student;
        }
    }
}
