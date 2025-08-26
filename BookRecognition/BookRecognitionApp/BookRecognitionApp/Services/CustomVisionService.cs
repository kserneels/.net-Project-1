using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using SkiaSharp;

public class CustomVisionService
{
    private readonly CustomVisionPredictionClient _customVisionClient;
    private readonly DocumentAnalysisClient _documentAnalysisClient;

    //Keys hier plakken

    public CustomVisionService()
    {
        _customVisionClient = new CustomVisionPredictionClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(PredictionKey))
        {
            Endpoint = CustomVisionEndPoint
        };
        _documentAnalysisClient = new DocumentAnalysisClient(new Uri(FormRecognizerEndpoint), new AzureKeyCredential(FormRecognizerKey));
    }

    public async Task<string> ExtractISBN(byte[] imageData)
    {
        byte[] resizedImage = ResizeImage(imageData, 1024, 1024);

        using (var memoryStream = new MemoryStream(resizedImage))
        {
            var result = await _customVisionClient.DetectImageAsync(Guid.Parse(ProjectId), PublishedName, memoryStream);
            var isbnPrediction = result.Predictions.FirstOrDefault(x => x.TagName.Equals("ISBN", StringComparison.OrdinalIgnoreCase) && x.Probability > 0.6);

            if (isbnPrediction != null)
            {
                return await ExtractISBNFromOCR(resizedImage);
            }
        }

        return "Het ISBN kon niet worden gelezen. Zorg ervoor dat de afbeelding scherp en goed belicht is, en probeer een andere foto te maken.";
    }

    public async Task<string> ExtractISBNFromOCR(byte[] imageData)
    {
        byte[] resizedImage = ResizeImage(imageData, 1024, 1024);

        using (var memoryStream = new MemoryStream(resizedImage))
        {
            var operation = await _documentAnalysisClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", memoryStream);
            var ocrResult = operation.Value;

            foreach (var page in ocrResult.Pages)
            {
                foreach (var line in page.Lines)
                {
                    if (line.Content.StartsWith("ISBN", StringComparison.OrdinalIgnoreCase))
                    {
                        return line.Content.Substring(5).Trim();
                    }
                }
            }
        }

        return "Het ISBN kon niet worden gelezen. Zorg ervoor dat de afbeelding scherp en goed belicht is, en probeer een andere foto te maken.";
    }

    private byte[] ResizeImage(byte[] imageData, int maxWidth, int maxHeight)
    {
        using (var memoryStream = new MemoryStream(imageData))
        {
            using (var skiaImage = SKBitmap.Decode(memoryStream))
            {
                int width = skiaImage.Width;
                int height = skiaImage.Height;

                if (width > maxWidth || height > maxHeight)
                {
                    float ratio = Math.Min((float)maxWidth / width, (float)maxHeight / height);
                    width = (int)(width * ratio);
                    height = (int)(height * ratio);
                }

                var resizedImage = skiaImage.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);

                using (var resizedMemoryStream = new MemoryStream())
                {
                    using (var image = SKImage.FromBitmap(resizedImage))
                    using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 90))
                    {
                        data.SaveTo(resizedMemoryStream);
                        return resizedMemoryStream.ToArray();
                    }
                }
            }
        }
    }
}
