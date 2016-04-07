using System.Collections.Generic;
using CommonDbManager.Model;

namespace CommonDbManager.Interface
{
    public interface IDbManager
    {
        List<BloodPressureModel> GetData();
        void InsertMongo(string model);
        List<BloodPressureModel> GetWithDateRange(string clientNo, string startDate, string finishDate);
        List<BloodPressureModel> GetWithDate(string clientNo, string finishDate);
        List<BloodPressureModel> GetWithClientNo(string clientNo);
        bool InsertListOfMessage(List<string> model);
        List<int> GetClientNo();

    }
}
