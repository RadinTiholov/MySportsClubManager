namespace ML.Model
{
    using Microsoft.ML.Data;

    public class ModelInput
    {
        [ColumnName("review"), LoadColumn(0)]
        public string Review { get; set; }


        [ColumnName("sentiment"), LoadColumn(1)]
        public string Sentiment { get; set; }
    }
}
