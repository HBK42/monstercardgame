using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08A3A4HttpServerDemo.MTCG.Daos
{
    public class Round
    {
        private List<string> messages = new List<string>();

        public List<string> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        public void AddMessage(string message)
        {
            messages.Add(message);
        }

        public override string ToString()
        {
            string s = "";
            foreach (var m in Messages)
            {
                s += m + "\n";
            }
            s += "------------------------------------------";
            return s;
        }

    }
}
