using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero_Line_WPF.Editor
{
    public  class EditorBL
    {
        public string SaveFolder { get; private set; }
        private string Line { get; set; }
        private readonly string directory = "Line\\";
        private readonly string[] newLine = new string[2]{ "C:\\", "TEST:90" };
        private string[] splitLine = new string[2];

        public SortedSet<Content> GetContent (string line)
        {

            if (!File.Exists(directory + line + ".ini")) { return null;}
            SortedSet<Content> contents = new SortedSet<Content>(new Content.ContentCompare());
            Line = line;
            string[] allLine = File.ReadAllLines(directory + line + ".ini");
            if (allLine.Length == 0)
            {
                File.WriteAllLines(directory + line + ".ini", newLine);
                contents.Add(new Content("TEST", 90));
                SaveFolder = newLine[0];
                return contents;
            }

            SaveFolder = allLine[0];

            for (int i = 1; i < allLine.Length; i++)
            {
                splitLine = allLine[i].Split(':');
                if(splitLine[1]!="")
                contents.Add(new Content(splitLine[0], Convert.ToInt32(splitLine[1])));
            }

            
            return contents;
        }

        public bool SetContent(string saveFolder, SortedSet<Content> content)
        {
            if (Line == null)
            {
                return false;
            }

            try
            {
                if(File.Exists(directory + Line + ".bak")){ File.Delete(directory + Line + ".bak");}

                File.Copy(directory + Line + ".ini", directory + Line + ".bak");
                string[] allLine = new string[content.Count + 1];
                allLine[0] = saveFolder;
                int i = 1;
                foreach (var cont in content)
                {
                    allLine[i] = cont.Name + ':' + cont.Rotation;
                    i++;
                }
                File.WriteAllLines(directory + Line + ".ini", allLine);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }

}
