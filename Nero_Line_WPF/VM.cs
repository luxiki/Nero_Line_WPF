using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;

namespace Nero_Line_WPF
{
    class VM:DependencyObject
    {
        private static Nero_BL Logic = new Nero_BL();
        public DelegateCommand ButtonOpenFileCommand { get; set; }
        public DelegateCommand ButtonSaveFileCommand { get; set; }
        public DelegateCommand ButtonOpenEditorCommand { get; set; }

        public ObservableCollection<string> Combo {get;} = new ObservableCollection<string>();

       public VM()
        {
             string[] temp = Logic.GetLine();
             for (int i = 0; i < temp.Length; i++)
             { 
                Combo.Add(temp[i]);
             }

            ButtonOpenFileCommand = new DelegateCommand(ButtonOpenFile);
            ButtonSaveFileCommand = new DelegateCommand(ButtonSaveFile);
            ButtonOpenEditorCommand = new DelegateCommand(ButtonOpenEditor);
            Logic.SendMessage += Logic_SendMessage;
        }

        private void Logic_SendMessage(object sender, EventMessage e)
        {
            StatusStrip = e.Message;
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(VM), new PropertyMetadata(false));

        public string SelectBox
        {
            get { return (string)GetValue(SelectBoxProperty); }
            set { SetValue(SelectBoxProperty, value); }
        }
        public static readonly DependencyProperty SelectBoxProperty =
            DependencyProperty.Register("SelectBox", typeof(string), typeof(VM), new PropertyMetadata("ViewTitle 1"));

        public string StatusStrip
        {
            get { return (string)GetValue(StatusStripProperty); }
            set { SetValue(StatusStripProperty, value); }
        }
        public static readonly DependencyProperty StatusStripProperty =
            DependencyProperty.Register("StatusStrip", typeof(string), typeof(VM), new PropertyMetadata(""));


        private void ButtonOpenFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Теккстовые фаилы|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                IsOpen = Logic.CheckFile(filePath);
            }
            
        }

        private void ButtonSaveFile(object obj)
        {
            Logic.SaveFile(SelectBox);
        }

        private void ButtonOpenEditor(object obj)
        {
            Editor.EditorWindow editor = new Editor.EditorWindow(SelectBox);
            editor.Show();
        }

      

    }
}
