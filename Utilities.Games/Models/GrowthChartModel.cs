using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utilities.Games.Models
{
    public partial class GrowthChartModel
    {
        /// <summary>
        /// The title text for the Growth Chart component
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Collection of datasets that can be compared together.
        /// </summary>
        public List<Dataset> Datasets { get; set; } = new List<Dataset>();

        /// <summary>
        /// A label for each instance that is calculated. In most cases, this would be considered the "Level".
        /// </summary>
        public string InstanceLabel { get; set; } = "Level";

        public double Width { get; set; }

        public double Height { get; set; }

        public GrowthChartModel()
        {

        }

        public GrowthChartModel(string title)
        {
            Title = title;
        }

        public void Resize(double width, double height) {
            Width = width;
            Height = height;
        }

        public void Build(int growthInstances){
            foreach (var dataset in Datasets)
            {
                dataset.Build(growthInstances);
            }
        }
    }
    public partial class GrowthChartModel
    {
        public enum GrowthStyle
        {
            /// <summary>
            /// The value will increase cumulatively by the GrowthRate, starting at the BaseValue.
            /// </summary>
            PointsPerInstance,
        }
        public class Dataset{
            /// <summary>
            /// Label for the Growth Chart dataset.
            /// </summary>
            public string Label { get; set; }

            /// <summary>
            /// The line color for the dataset.
            /// </summary>
            public string Color { get; set; }

            /// <summary>
            /// Sets the style for which each instance value is calculated.
            /// </summary>
            public GrowthStyle GrowthStyle { get; set; } = GrowthStyle.PointsPerInstance;

            /// <summary>
            /// The base value for the Growth Chart dataset.
            /// </summary>
            public double BaseValue { get; set; }

            /// <summary>
            /// The rate for which the <see cref="BaseValue"/> grows.
            /// </summary>
            public double GrowthRate { get; set; }

            /// <summary>
            /// Collection of values for each instance, or "Leve" in most cases.
            /// </summary>
            public Dictionary<int, double> Values { get; private set; } = new Dictionary<int, double>();

            public void Build(int growthInstances) {
                if (growthInstances <= 0) {
                    throw new IndexOutOfRangeException("Growth instances cannot be less than or equal to zero.");
                }
                // Re-initialize values with the first instance using the BaseValue
                Values = new Dictionary<int, double>(){
                    { 0, BaseValue }
                };

                double growthValue = BaseValue;
                for (int i = 1; i < growthInstances; i++)
                {
                    switch (GrowthStyle)
                    {
                        case GrowthStyle.PointsPerInstance:
                        default:
                            growthValue += GrowthRate;
                            break;
                    }
                    Values.Add(i, growthValue);
                }
            }
        }
    }
}
