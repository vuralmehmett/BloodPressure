using System;
using System.Threading;
using CommonDbManager.DbManager;
using CommonDbManager.Interface;
using Newtonsoft.Json;
using Ninject;

namespace BloodPressureDoctor.GetPatient
{
    public class GetPatientInfo
    {
        private readonly IDbManager _dbManager;

        public GetPatientInfo(IDbManager dbManager)
        {
            StandardKernel kernel = new StandardKernel();
            kernel.Load(new DbModule());
            _dbManager = dbManager;
        }

        public string GetInfoWithDateBetween(string clientNo, string startDate, string finishDate)
        {
            var result = JsonConvert.SerializeObject(_dbManager.GetWithDateRange(clientNo, startDate, finishDate));
            return result;
        }

        public string GetInfoWithDate(string clientNo, string finishDate)
        {
            var result = JsonConvert.SerializeObject(_dbManager.GetWithDate(clientNo, finishDate));
            return result;
        }

        public string GetInfoWithClientNo(string clientNo)
        {
            string result = "";
            var patientNo = _dbManager.GetClientNo();
            for (int i = 0; i < patientNo.Count; i++)
            {
                result = JsonConvert.SerializeObject(_dbManager.GetWithClientNo(patientNo[i].ToString()));
                Console.WriteLine((i+1)+ " - " + result + Environment.NewLine);
                Thread.Sleep(10000);
            }

            return result;
        }
    }
}
