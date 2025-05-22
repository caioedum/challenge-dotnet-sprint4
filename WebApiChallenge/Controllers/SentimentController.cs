using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using WebApiChallenge.Models;

namespace WebApiChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SentimentController : ControllerBase
    {
        private readonly PredictionEnginePool<SentimentModelInput, SentimentModelOutput> _predictionEnginePool;

        public SentimentController(PredictionEnginePool<SentimentModelInput, SentimentModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        [HttpPost]
        public ActionResult<SentimentModelOutput> Predict([FromBody] SentimentModelInput input)
        {
            var prediction = _predictionEnginePool.Predict(
                modelName: "SentimentAnalysisModel",
                example: input
            );
            return Ok(prediction);
        }
    }
}
