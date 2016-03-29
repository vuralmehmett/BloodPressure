using System.Web.Http;
using BloodPressureWebApi.Bussiness.Mongo;
using BloodPressureWebApi.Bussiness.QueueStructure;
using BloodPressureWebApi.Models;

namespace BloodPressureWebApi.Controllers
{
    public class BloodPressureController : ApiController
    {
        private readonly DataTransfer _dataTransfer = new DataTransfer();

        /// <summary>
        /// Gelen data önce kuyruga yollanıyor ardından mongoya insert ediliyor.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendPatientData(BloodPressureModel model)
        {
            _dataTransfer.SendMessage(model);
            var mongo = new InsertMongo();
            mongo.InsertMongoDb(model);
            return Ok("Sucess");
        }

        /// <summary>
        /// Mongo 'dan kayıtlar okunuyor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ReadPatientData()
        {
            // todo : gökhana soru : burda veri kuyruktan mı okunacak yoksa mongodan mı ?
            //var messageList =_dataTransfer.GetMessage();
            var model =  ReadMongo.GetData();
            return Ok(model);
        }
    }
}
