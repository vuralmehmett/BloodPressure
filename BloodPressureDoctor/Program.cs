﻿using System;
using BloodPressureDoctor.Kafka;

namespace BloodPressureDoctor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a patient number : ");
            string clientNo = Console.ReadLine();

            GetPatientInfo info = new GetPatientInfo();
            var patient = info.GetInfo(clientNo);
            Console.WriteLine("Patient Number : " + patient.ClientNo);
            Console.WriteLine("Pressure : "  + patient.Pressure);
            Console.WriteLine("Date : " + patient.TimeStamp);
            Console.ReadLine();
        }
    }
}
