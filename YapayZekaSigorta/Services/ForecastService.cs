using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace YapayZekaSigorta.Services
{
    public class PolicySalesData
    {
        public DateTime Date { get; set; }
        public float SaleCount { get; set; }
    }
    public class PolicySalesForecast
    {
        public float[] ForecastedValues { get; set; }
        public float[] LowerBoundValues { get; set; }
        public float[] UpperBoundValues { get; set; }
    }
    public class ForecastService
    {
        private readonly MLContext _mlContext;

        public ForecastService()
        {
            _mlContext = new MLContext();
        }

        public PolicySalesForecast GetForecast(List<PolicySalesData> salesData, int horizon = 3)
        {
            int count = salesData.Count;

            var dataView = _mlContext.Data.LoadFromEnumerable(salesData);
            var forecastingPipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedValues",
                inputColumnName: "SaleCount",
                windowSize:2,
                seriesLength:count,
                trainSize: count ,
                horizon: horizon,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundValues",
                confidenceUpperBoundColumn: "UpperBoundValues"
                );

            var model = forecastingPipeline.Fit(dataView);
            var forecastEngine = model.CreateTimeSeriesEngine<PolicySalesData, PolicySalesForecast>(_mlContext);
            return forecastEngine.Predict();
        }
    }
}
