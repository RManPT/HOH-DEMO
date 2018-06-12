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

        public List<State> Conditions { get; set; }
        public List<Exercise> Exercises { get; set; }
        public List<ProtocolExercise> ProtocolExercises { get; set; }



        public Clinic()
        {
            Protocols = new List<Protocol>();
            Conditions = new List<State>();
            Exercises = new List<Exercise>();
            ProtocolExercises = new List<ProtocolExercise>();
         }

        public Clinic ShallowCopy()
        {
            return (Clinic)this.MemberwiseClone();
        }
    }
}
