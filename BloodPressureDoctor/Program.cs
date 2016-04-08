using System;
using System.Collections.Generic;
using System.Threading;
using BloodPressureDoctor.GetPatient;
using CommonDbManager.DbManager;
using CommonDbManager.Interface;
using Newtonsoft.Json;

namespace BloodPressureDoctor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Welcome to Patient Managment System." + Environment.NewLine);
            //Console.WriteLine("1-) Bring the information on the specific date range" + Environment.NewLine + "2-) Bring the information since year" + Environment.NewLine + "3-) Bring the all information" + Environment.NewLine);
            //var selection = Console.ReadLine();
            Random rnd = new Random();
            //int choice;

            IDbManager DbManager = new MongoManager();
            GetPatientInfo Info = new GetPatientInfo(DbManager);

            while (true)
            {
                List<int> allClientNos = Info.GetAllClientIds();
                int selectedClientId = allClientNos[rnd.Next(0, allClientNos.Count)];

                string PatientData = Info.GetInfoWithClientNo(selectedClientId);
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
                
                Console.WriteLine("Doctor read: {0} for patient: {1}", PatientData, selectedClientId);

                Thread.Sleep(2);
            }
        }
    }

}
