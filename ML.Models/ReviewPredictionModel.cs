namespace ML.Model
{
    using Microsoft.ML;

    public class ReviewPredictionModel
    {
        private static Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictionEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(CreatePredictionEngine);

        public static ModelOutput Predict(ModelInput input)
        {
            ModelOutput result = PredictionEngine.Value.Predict(input);
            return result;
        }

        public static PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            MLContext mlContext = new MLContext();

            //string modelPath = @"C:\Users\radit\AppData\Local\Temp\MLVSTools\AutoMlTestML\AutoMlTestML.Model\MLModel.zip";

            //For unit testing
            string modelPath = Directory.GetCurrentDirectory() + "\\MLModel.zip";
            if (!modelPath.Contains("Test"))
            {
                modelPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\ML.Models\\MLModel.zip");
            }
            ITransformer mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            return predEngine;
        }
    }
}
