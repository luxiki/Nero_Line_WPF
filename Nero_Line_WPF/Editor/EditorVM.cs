using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace Nero_Line_WPF.Editor
{
    public class EditorVM:DependencyObject
    {
        private SortedSet<Content> contents;

        public DelegateCommand ChangeFolderSaveCommand { get; set; }
        public DelegateCommand AddContentCommand { get; set; }
        public DelegateCommand DeleteContentCommand { get; set; }
        public DelegateCommand SaveContentCommand { get; set; }

        private string Line { get; }
        private EditorBL logic = new EditorBL();

        public EditorVM(string line)
        {
            contents = logic.GetContent(line);
            if (contents != null)
            {
                Content = CollectionViewSource.GetDefaultView(contents);
                Content.Filter = FilterContentVisibl;
                Line = line;
                SaveFolder = logic.SaveFolder;
                ViewTitle = "Edit " + Line;
            }
            else
            {
                MessageBox.Show("File not found");
                Application.Current.Shutdown();
            }

            ChangeFolderSaveCommand = new DelegateCommand(ChangeSaveFolder);
            AddContentCommand = new DelegateCommand(AddContent);
            DeleteContentCommand = new DelegateCommand(DeleteContent);
            SaveContentCommand = new DelegateCommand(SaveContents);
        }
   
        private bool FilterContentVisibl(object obj)
        {

            var current = obj as Content;
            if (string.IsNullOrWhiteSpace(FilterContent))
            {
                return true;
            }

            if (current.Name != "" && current != null && current.Name.Contains(FilterContent))
            {
                return true;
            }

            return false;
        }

        public string FilterContent
        {
            get { return (string)GetValue(FilterContentProperty); }
            set { SetValue(FilterContentProperty, value); }
        }
        public static readonly DependencyProperty FilterContentProperty =
            DependencyProperty.Register("FilterContent", typeof(string), typeof(EditorVM), new PropertyMetadata("", FilterConentChange ));

        private static void FilterConentChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as EditorVM;
            if (current != null)
            {
                current.Content.Filter = null;
                current.Content.Filter = current.FilterContentVisibl;
            }
        }

        public ICollectionView Content
        {
            get { return (ICollectionView)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Content", typeof(ICollectionView), typeof(EditorVM), new PropertyMetadata(null));

        public Content SelectedContent
        {
            get { return (Content)GetValue(SelectedContentProperty); }
            set { SetValue(SelectedContentProperty, value); }
        }
        public static readonly DependencyProperty SelectedContentProperty =
            DependencyProperty.Register("SelectedContent", typeof(Content), typeof(EditorVM), new PropertyMetadata(null));
        
        public string ViewTitle
        {
            get { return (string)GetValue(ViewTitleProperty); }
            set { SetValue(ViewTitleProperty, value); }
        }
        public static readonly DependencyProperty ViewTitleProperty =
            DependencyProperty.Register("ViewTitle", typeof(string), typeof(EditorVM), new PropertyMetadata(""));

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(EditorVM), new PropertyMetadata(""));

        public string AddName
        {
            get { return (string)GetValue(AddNameProperty); }
            set { SetValue(AddNameProperty, value); }
        }
        public static readonly DependencyProperty AddNameProperty =
            DependencyProperty.Register("AddName", typeof(string), typeof(EditorVM), new PropertyMetadata(""));

        public string AddRotation
        {
            get { return (string)GetValue(AddRotationProperty); }
            set { SetValue(AddRotationProperty, value); }
        }
        public static readonly DependencyProperty AddRotationProperty =
            DependencyProperty.Register("AddRotation", typeof(string), typeof(EditorVM), new PropertyMetadata(""));

        public string SaveFolder
        {
            get { return (string)GetValue(SaveFolderProperty); }
            set { SetValue(SaveFolderProperty, value); }
        }
        public static readonly DependencyProperty SaveFolderProperty =
            DependencyProperty.Register("SaveFolder", typeof(string), typeof(EditorVM), new PropertyMetadata(""));


        private void AddContent(object obj)
        {
            try
            {
                foreach (var cont in contents)
                {
                    if (cont.Name.Equals(AddName))
                    {
                        Status = "Name already exists";
                        return;
                    }
                }

                if (AddName != "")
                {
                    contents.Add(new Content(AddName, Convert.ToInt32(AddRotation)));
                    Content = null;
                    Content = CollectionViewSource.GetDefaultView(contents);
                    Status = " Add " + AddName + AddRotation;
                }
                else
                {
                    Status = "Error Name";
                }

            }
            catch(Exception e)
            {
                Status = "Error " + e.Message;
            }
        }

        private void DeleteContent(object obj)
        {
            try
            {
                Status = "Delete " + SelectedContent.Name;
                contents.RemoveWhere(X => X.Name.Equals(SelectedContent.Name));
                Content = null;
                Content = CollectionViewSource.GetDefaultView(contents);
            }
            catch
            {
                Status = "Not delete";
            }
        }

        private void SaveContents(object obj)
        {
            string tmp = logic.SetContent(SaveFolder, contents) ? "Save OK" : "Error";
            Status = tmp;
            MessageBox.Show(tmp);
        }

        private void ChangeSaveFolder(object obj)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if(folderDialog.ShowDialog() == DialogResult.OK)
            SaveFolder = folderDialog.SelectedPath;
        }

    }
}
