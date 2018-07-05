using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HOH_Library
{
    public class Clinic
    {
        public string Name { get; set; }

        public List<Protocol> Protocols { get; set; }

        public List<State> States { get; set; }
        public List<Exercise> Exercises { get; set; }
        public static List<string> Rewards { get; set; }
  


        public Clinic()
        {
            Protocols = new List<Protocol>();
            States = new List<State>();
            Exercises = new List<Exercise>();
            Rewards = new List<string>();
        }

        public Clinic(Clinic c)
        {
            Protocols = new List<Protocol>(c.Protocols);
            States = new List<State>(c.States);
            Exercises = new List<Exercise>(c.Exercises);
            Rewards = new List<string>(Clinic.Rewards);
        }

        public Clinic ShallowCopy()
        {
            return (Clinic)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return ToJSON();
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public void FromJSON(string json)
        {
            Clinic c = (Clinic)JsonConvert.DeserializeObject<Clinic>(json);

            //js
            //dynamic receivedObject = JObject.Parse(json);

            this.Name = c.Name;
            this.Exercises = c.Exercises;
            this.Protocols = c.Protocols;
            this.States = c.States;
            Rewards = Clinic.Rewards;
        }
    }
}
