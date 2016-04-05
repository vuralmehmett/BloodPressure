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
            var result =   JsonConvert.SerializeObject(_dbManager.GetWithDateRange(clientNo, startDate, finishDate));
            return result;
        }

        public string GetInfoWithDate(string clientNo, string finishDate)
        {
            var result = JsonConvert.SerializeObject(_dbManager.GetWithDate(clientNo, finishDate));
            return result;
        }

        public string GetInfoWithClientNo(string clientNo)
        {
            var result = JsonConvert.SerializeObject(_dbManager.GetWithClientNo(clientNo));
            return result;
        }
    }
}
