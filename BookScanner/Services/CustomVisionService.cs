using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace BookScanner.Services
{
    public static class ApiKeys
    {
        public static string CustomVisionEndPoint => "https://ksbigfive-prediction.cognitiveservices.azure.com";
        public static string PredictionKey => "213c8ed37f13435b892215a386f8ecf7";
        public static string ProjectId => "6b10490e-87bf-413b-a87d-ada8cf4abcb2";
        public static string PublishedName => "BigFiveModel";
    }

    public class CustomVisionService
    {
        public static async Task<PredictionModel?> ClassifyImageAsync(Stream photoStream)
        {
            try
            {
                var endpoint = new CustomVisionPredictionClient(new ApiKeyServiceClientCredentials(ApiKeys.PredictionKey))
                {
                    Endpoint = ApiKeys.CustomVisionEndPoint
                };

                // Send image to the Custom Vision API
                var results = await endpoint.ClassifyImageAsync(Guid.Parse(ApiKeys.ProjectId), ApiKeys.PublishedName, photoStream);

                // Return the most likely prediction
                return results.Predictions?.OrderByDescending(x => x.Probability).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new PredictionModel();
            }
        }
    }
}
