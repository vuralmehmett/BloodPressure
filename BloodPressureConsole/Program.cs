using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BloodPressureConsole.BaseHtttpRequest;
using BloodPressureConsole.Request;

namespace BloodPressureConsole
{
    class PseudoDataGenerator
    {
        private static void Post(int taskId)
        {
            Random rnd = new Random();
            //Console.WriteLine("Please enter Patient No : ");
            //var patientNo = Console.ReadLine();
            //Console.WriteLine("Please enter Patient Pressure");
            //var patientPressure = Console.ReadLine();

            var bloodPressureRequest = new BloodPressureRequest
            {
                ClientNo = Convert.ToInt16(rnd.Next(1, 10000)),
                Pressure = "test",//patientPressure,
                TimeStamp = DateTime.Now
            };

            Console.WriteLine("Generating a sample for task id {0}", taskId);

            var connection = new Connection();
            var read = connection.Post(bloodPressureRequest);

            Console.WriteLine("Generated a sample for task id {0}", taskId);
        }

        private static void Get()
        {
            var connection = new Connection();
            var read = connection.Get();
            Console.WriteLine(read);
        }

        public static void Start(int taskId)
        {
            while (true)
            {
                Post(taskId);

                //Thread.Sleep(5);
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            const int NO_OF_TASKS = 20;

            List<Task> listOfTask = new List<Task>();

            for (int i = 0; i < NO_OF_TASKS; ++i)
            {
                int taskId = i;

                Task tsk = new Task(() =>
                {
                    PseudoDataGenerator.Start(taskId);

                }, TaskCreationOptions.LongRunning);

                listOfTask.Add(tsk);
            }

            listOfTask.ForEach(t => t.Start());

            Task.WaitAll(listOfTask.ToArray());
        }
    }
}
