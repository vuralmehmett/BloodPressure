using System.Web.Http;
using BloodPressureWebApi.Bussiness.Kafka;
using BloodPressureWebApi.Bussiness.Mongo;
using BloodPressureWebApi.Models;

namespace BloodPressureWebApi.Controllers
{
    public class BloodPressureController : ApiController
    {
        [HttpPost]
        public IHttpActionResult SendPatientData(BloodPressureModel model)
        {
            var sendMessage = new SendMessageToTopic();
            var time = sendMessage.SendMessage("atiba", model);

            var mongo = new InsertMongo();
            mongo.InsertMongoDb(model);
            return Ok(time);
        }

        [HttpGet]
        public IHttpActionResult ReadPatientData()
        {
            var kafka = new GetMessageFromKafka();
            var messageList = kafka.GetMessage("atiba");
            ReadMongo.GetData();
            return Ok("Success");
        }
    }
}
