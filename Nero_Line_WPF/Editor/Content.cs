using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero_Line_WPF.Editor
{
    public class Content
    {
        public int Rotation { get; set; }
        public string Name { get; set; }

        public Content(string name , int rotation)
        {
            this.Name = name;
            this.Rotation = rotation;
        }

        internal class ContentCompare:IComparer<Content>
        {
            public  int Compare(Content x, Content y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }
    }
}
