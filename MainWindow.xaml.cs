using System.Windows;

namespace AlexUniversityCatalog
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FacultiesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenTableWindow("Faculties");
        }

        private void SubjectsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenTableWindow("Subjects");
        }

        private void TeachersButton_Click(object sender, RoutedEventArgs e)
        {
            OpenTableWindow("Teachers");
        }

        private void StudentsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenTableWindow("Students");
        }

        private void OpenTableWindow(string tableName)
        {

            TableWindow tableWindow = new(tableName);
            tableWindow.Owner = this;
            tableWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tableWindow.ShowDialog();
        }
    }
}
