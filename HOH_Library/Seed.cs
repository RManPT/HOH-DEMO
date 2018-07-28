using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOH_Library
{
    public class Seed
    {
        public static Clinic SetupData(Clinic clinic)
        { 


            clinic.Name = "HOH Clinic";
            clinic.Rewards.AddRange(new List<string>{ "Well done!", "Nice effort!", "Good try!" });



              State condFullyOpen = new State
                 {
                     Name = "FullyOpen",
                     HOHCode = "06",
                     UserMsg = "Opening Hand",
                     CallbackMsg = "Exit restoring"
                 };

                 State condFullyClose = new State
                 {
                     Name = "FullyClose",
                     HOHCode = "36",
                     UserMsg = "Closing Hand",
                     CallbackMsg = "Exit hand closing"
                 };

                 State condNone = new State
                 {
                     Name = "None",
                     HOHCode = "00",
                     UserMsg = "Resting",
                     CallbackMsg = ""
                 };

                 clinic.States.Add(condNone);
                 clinic.States.Add(condFullyOpen);
                 clinic.States.Add(condFullyClose);


              /*  Exercise exerFullyClose = new Exercise
                 {
                     Name = "Fully Close",
                     PreState = condFullyOpen,
                     ExerciseTime = 10,
                     SFCode = "22",
                     UserMsg = "Close your hand!",
                     TargetState = condFullyClose,
                     Repetitions = 1,
                     PostState = condNone
                 };

                 Exercise exerFullyOpen = new Exercise()
                 {
                     Name = "Fully Open",
                     PreState = condFullyClose,
                     ExerciseTime = 10,
                     SFCode = "21",
                     UserMsg = "Open your hand!",
                     TargetState = condFullyOpen,
                     Repetitions = 1,
                     PostState = condNone
                 };

                 Exercise exerOpenRest = new Exercise
                 {
                     Name = "Open Rest",
                     PreState = condFullyOpen,
                     ExerciseTime = 10,
                     SFCode = "20",
                     UserMsg = "Relax with your hand closed",
                     TargetState = condNone,
                     Repetitions = 1,
                     PostState = condNone
                 };

                 Exercise exerCloseRest = new Exercise
                 {
                     Name = "Close Rest",
                     PreState = condFullyClose,
                     ExerciseTime = 10,
                     SFCode = "20",
                     UserMsg = "Relax with your hand closed",
                     TargetState = condNone,
                     Repetitions = 1,
                     PostState = condNone
                 };

                 Exercise exerJustRest = new Exercise()
                 {
                     Name = "Rest",
                     PreState = condNone,
                     ExerciseTime = 10,
                     SFCode = "20",
                     UserMsg = "Just relax!",
                     TargetState = condNone,
                     Repetitions = 1,
                     PostState = condNone
                 };

                 clinic.Exercises.Add(exerFullyClose);
                 clinic.Exercises.Add(exerFullyOpen);
                 clinic.Exercises.Add(exerOpenRest);
                 clinic.Exercises.Add(exerCloseRest);
                 clinic.Exercises.Add(exerJustRest);

                 Protocol protoc1 = new Protocol()
                 {
                     Name = "Protocol 1",
                     Exercises = new List<Exercise>
                     {
                         exerFullyOpen,
                         exerJustRest,
                         exerFullyClose
                     }
                 };

                 Protocol protoc2 = new Protocol()
                 {
                     Name = "Protocol 2",
                     Exercises = new List<Exercise>
                     {
                         new Exercise("New exercise 1")
                     }
                 };

                 clinic.Protocols.Add(protoc1);
                 clinic.Protocols.Add(protoc2);*/

            return clinic;
        }
    }


}
