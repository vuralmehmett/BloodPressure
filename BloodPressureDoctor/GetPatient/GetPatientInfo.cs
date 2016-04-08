using System;
using System.Collections.Generic;
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

        public List<int> GetAllClientIds()
        {
            return _dbManager.GetClientNo();
        }

        public string GetInfoWithClientNo(int clientNo)
        {
            string clientNoStr = clientNo.ToString();

            return JsonConvert.SerializeObject(_dbManager.GetWithClientNo(clientNoStr), Formatting.Indented);
        }
    }
}
