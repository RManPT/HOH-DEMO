using System;
using System.Collections.Generic;

using System.Text;


namespace HOH_Library
{
    public class ProtocolExercise
    {
        /// <summary>
        /// References the exercise
        /// </summary>
        public Exercise Exercise { get; set; }
        /// <summary>
        /// Number of repetitions
        /// </summary>
        public int Repetitions { get; set; }

        public ProtocolExercise()
        {
            
        }

        public ProtocolExercise(Exercise exercise, int repetitions)
        {
            Exercise = exercise;
            Repetitions = repetitions;
        }


        public void Execute()
        {

        }

    }
}
