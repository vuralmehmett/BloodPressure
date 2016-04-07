using System;
using BloodPressureConsole.BaseHtttpRequest;
using BloodPressureConsole.Request;

namespace BloodPressureConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Get();
            Post();
        }

        private static void Post()
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
            var connection = new Connection();
            var read = connection.Post(bloodPressureRequest);
       
            Console.WriteLine("bitti");
            Console.ReadLine();
        }

        private static void Get()
        {
            var connection = new Connection();
            var read = connection.Get();
            Console.WriteLine(read);
        }
    }
}
