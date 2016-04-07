using System;
using BloodPressureDoctor.GetPatient;
using CommonDbManager.DbManager;
using CommonDbManager.Interface;

namespace BloodPressureDoctor
{
    class Program
    {
        private static readonly IDbManager DbManager = new MongoManager();
        public static readonly GetPatientInfo Info = new GetPatientInfo(DbManager);
        public static string PatientData;
        static void Main(string[] args)
        {
            //Console.WriteLine("Welcome to Patient Managment System." + Environment.NewLine);
            //Console.WriteLine("1-) Bring the information on the specific date range" + Environment.NewLine + "2-) Bring the information since year" + Environment.NewLine + "3-) Bring the all information" + Environment.NewLine);
            //var selection = Console.ReadLine();
            Random rnd = new Random();
            //int choice;
            var clientNo = "";
            PatientData = Info.GetInfoWithClientNo(rnd.Next(0, 10000).ToString());
            //if (int.TryParse(selection, out choice))
            //{
            //    string finishDate;
            //    switch (choice)
            //    {
            //        case 1:
            //            Console.WriteLine("Enter a patient number : ");
            //            clientNo = Console.ReadLine();
            //            Console.WriteLine("Enter a start date : ");
            //            var startDate = Console.ReadLine();
            //            Console.WriteLine("Enter a finish date : ");
            //            finishDate = Console.ReadLine();
            //            PatientData = Info.GetInfoWithDateBetween(clientNo, startDate, finishDate);

            //            break;
            //        case 2:
            //            Console.WriteLine("Enter a patient number : ");
            //            clientNo = Console.ReadLine();
            //            Console.WriteLine("Enter a finish date : ");
            //            finishDate = Console.ReadLine();
            //            PatientData = Info.GetInfoWithDate(clientNo, finishDate);

            //            break;
            //        default:
            //            Console.WriteLine("Enter a patient number : ");
            //            clientNo = Console.ReadLine();
            //            PatientData = Info.GetInfoWithClientNo(rnd.Next(0,10000).ToString());
            //            break;
            //    }

            //    if (PatientData == "[]")
            //    {
            //        Console.WriteLine("Does not information about patient.");
            //        Console.WriteLine("Enter exit." + Environment.NewLine);
            //        Console.ReadLine();
            //    }
            Console.WriteLine(PatientData + Environment.NewLine);
                Console.WriteLine("Enter exit." + Environment.NewLine);
                Console.ReadLine();
            }
        }
    
}
