using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOH_Library
{
    public class Clinic
    {
        public string Name { get; set; }

        public List<Protocol> Protocols { get; set; }

        public List<State> State { get; set; }
        public List<Exercise> Exercises { get; set; }
        public static List<string> Rewards { get; set; }
  


        public Clinic()
        {
            Protocols = new List<Protocol>();
            State = new List<State>();
            Exercises = new List<Exercise>();
            Rewards = new List<string>();
        }

        public Clinic ShallowCopy()
        {
            return (Clinic)this.MemberwiseClone();
        }
    }
}
