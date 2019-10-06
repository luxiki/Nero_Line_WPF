using System.Windows;

namespace Nero_Line_WPF.Editor
{
    /// <summary>
    /// Логика взаимодействия для EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        public EditorWindow(string line)
        {
            InitializeComponent();
            DataContext = new EditorVM(line);
        }
    }
}
