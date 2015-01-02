using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Utils
{
    public class StackTraza
    {
        public string File {get;set;}
        public string Member { get; set; }
        public int Line { get; set; }
        public string Text { get; set; }
        public IList<StackTraza> ParentMembers { get; set; }
        public StackTraza()
        {
            ParentMembers = new List<StackTraza>();
        }
    }
}
