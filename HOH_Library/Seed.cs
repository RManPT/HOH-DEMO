﻿using System;
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
                CallbackMsg = "Exit hand closing"
            };

            State condNone = new State
            {
                Name = "None",
                HOHCode = "00",
                UserMsg = "Resting",
                CallbackMsg = ""
            };

            clinic.Conditions.Add(condNone);
            clinic.Conditions.Add(condFullyOpen);
            clinic.Conditions.Add(condFullyClose);
            

            Exercise exerFullyClose = new Exercise
            {
                Name = "Fully Close",
                PreState = condFullyOpen,
                ExerciseTime = 20,
                SFCode = "22",
                UserMsg = "Close your hand!",
                TargetState = condFullyClose,
                PostState = condNone
            };

            Exercise exerFullyOpen = new Exercise()
            {
                Name = "Fully Open",
                PreState = condFullyClose,
                ExerciseTime = 20,
                SFCode = "21",
                UserMsg = "Open your hand!",
                TargetState = condFullyOpen,
                PostState = condNone
            };

            Exercise exerOpenRest = new Exercise
            {
                Name = "Open Rest",
                PreState = condFullyOpen,
                ExerciseTime = 20,
                SFCode = "20",
                UserMsg = "Relax with your hand closed",
                TargetState = condNone,
                PostState = condNone
            };

            Exercise exerCloseRest = new Exercise
            {
                Name = "Close Rest",
                PreState = condFullyClose,
                ExerciseTime = 20,
                SFCode = "20",
                UserMsg = "Relax with your hand closed",
                TargetState = condNone,
                PostState = condNone
            };

            Exercise exerJustRest = new Exercise()
            {
                Name = "Rest",
                PreState = condNone,
                ExerciseTime = 20,
                SFCode = "20",
                UserMsg = "Just relax!",
                TargetState = condNone,
                PostState = condNone
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

            Protocol protoc2 = new Protocol()
            {
                Name = "Protocol 2",
                Exercises = new List<ProtocolExercise>
                {
                    new ProtocolExercise(exerFullyClose, 3)
                }
            };

            clinic.Protocols.Add(protoc1);
            clinic.Protocols.Add(protoc2);

        }
    }


}
