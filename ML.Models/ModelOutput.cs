namespace ML.Model
{
    using Microsoft.ML.Data;

    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string Prediction { get; set; } = null!;
        public float[] Score { get; set; } = null!;
    }
}
