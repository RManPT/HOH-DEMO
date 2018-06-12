using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOH_Library
{
    public class Seed
    {
        public static void SetupData(Clinic clinic)
        {
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
                CallbackMsg = "Exit Restoring"
            };

            clinic.Conditions.Add(condFullyOpen);
            clinic.Conditions.Add(condFullyClose);

            Exercise exerFullyClose = new Exercise
            {
                Name = "Fully Close",
                PreCondition = condFullyOpen,
                ExerciseTime = 20,
                SFCode = "22",
                UserMsg = "Close your hand!",
                Movement = condFullyClose,
                PostCondition = null
            };

            Exercise exerFullyOpen = new Exercise()
            {
                Name = "Fully Open",
                PreCondition = condFullyClose,
                ExerciseTime = 20,
                SFCode = "21",
                UserMsg = "Open your hand!",
                Movement = condFullyOpen,
                PostCondition = null
            };

            Exercise exerOpenRest = new Exercise
            {
                Name = "Open Rest",
                PreCondition = condFullyOpen,
                ExerciseTime = 20,
                SFCode = "20",
                UserMsg = "Relax with your hand closed",
                Movement = null,
                PostCondition = null
            };

            Exercise exerCloseRest = new Exercise
            {
                Name = "Close Rest",
                PreCondition = condFullyClose,
                ExerciseTime = 20,
                SFCode = "20",
                UserMsg = "Relax with your hand closed",
                Movement = null,
                PostCondition = null
            };

            Exercise exerJustRest = new Exercise()
            {
                Name = "Rest",
                PreCondition = null,
                ExerciseTime = 20,
                SFCode = "20",
                UserMsg = "Just relax!",
                Movement = null,
                PostCondition = null
            };

            clinic.Exercises.Add(exerFullyClose);
            clinic.Exercises.Add(exerFullyOpen);
            clinic.Exercises.Add(exerOpenRest);
            clinic.Exercises.Add(exerCloseRest);
            clinic.Exercises.Add(exerJustRest);

            ProtocolExercise ex1 = new ProtocolExercise()
            {
                Exercise = exerFullyOpen,
                Repetitions = 1
            };

            ProtocolExercise ex2 = new ProtocolExercise()
            {
                Exercise = exerJustRest,
                Repetitions = 1
            };

            ProtocolExercise ex3 = new ProtocolExercise()
            {
                Exercise = exerFullyClose,
                Repetitions = 1
            };

            clinic.ProtocolExercises.Add(ex1);
            clinic.ProtocolExercises.Add(ex2);
            clinic.ProtocolExercises.Add(ex3);

            Protocol protoc1 = new Protocol()
            {
                Name = "Protocol 1",
                Exercises = new List<ProtocolExercise>
                {
                    ex1,
                    ex2,
                    ex3
                }
            };

            clinic.Protocols.Add(protoc1);
           
        }
    }


}
