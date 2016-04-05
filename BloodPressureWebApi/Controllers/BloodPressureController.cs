using System.Web.Http;
using BloodPressureWebApi.Models;
using CommonDbManager.Interface;
using CommonQueueManager.Interface;
using Newtonsoft.Json;

namespace BloodPressureWebApi.Controllers
{
    public class BloodPressureController : ApiController
    {
        private readonly IDbManager _manager;
        private readonly IQueueManager _queueManager;

        public BloodPressureController(IDbManager manager, IQueueManager queueManager)
        {
            _manager = manager;
            _queueManager = queueManager;
        }

        /// <summary>
        /// Gelen data önce kuyruga yollanıyor.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendPatientData(BloodPressureModel model)
        {
            var result = _queueManager.PutData(JsonConvert.SerializeObject(model));
            if (result)
                return Ok();
            return BadRequest();
        }

        /// <summary>
        /// Mongo 'dan kayıtlar okunuyor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ReadPatientData()
        {
            var model = _manager.GetData();
            return Ok(model);
        }
    }
}
