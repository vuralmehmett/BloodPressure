using System.Web.Http;
using BloodPressureWebApi.Bussiness.Mongo;
using BloodPressureWebApi.Bussiness.QueueStructure;
using BloodPressureWebApi.Models;

namespace BloodPressureWebApi.Controllers
{
    public class BloodPressureController : ApiController
    {
        private readonly DataTransfer _dataTransfer = new DataTransfer();
        [HttpPost]
        public IHttpActionResult SendPatientData(BloodPressureModel model)
        {
            var time = _dataTransfer.SendMessage(model);

            var mongo = new InsertMongo();
            mongo.InsertMongoDb(model);
            return Ok("Sucess");
        }

        [HttpGet]
        public IHttpActionResult ReadPatientData()
        {
            var messageList =_dataTransfer.GetMessage();
            ReadMongo.GetData();
            return Ok("Success");
        }
    }
}
