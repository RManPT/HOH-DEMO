using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string GetExerciseName
        {
            get { return Exercise.Name; }
        }

        public ProtocolExercise()
        {
            
        }

        public ProtocolExercise(Exercise exercise, int repetitions)
        {
            Exercise = exercise;
            Repetitions = repetitions;
        }

      

        public void Execute(MRNetwork NW)
        {
            Debug.WriteLine("   PRTOEXERCISE: START!");
            for (int i = 0; i < this.Repetitions; i++)
            {
                Debug.WriteLine("   PRTOEXERCISE: " + i);
                this.Exercise.Execute(NW);
            }
                
            //forçar espera por fim de execução para passar à proxima repetição.
            Debug.WriteLine("   PRTOEXERCISE: DONE!");
        }

    }
}
