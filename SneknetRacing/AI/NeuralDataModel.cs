using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.AI
{
    public class NeuralDataModel
    {
        public CarTelemetryData TelemetryData { get; set; }
        public CarStatusData StatusData { get; set; }
        public CarSetupData SetupData { get; set; }
        public LapData LapData { get; set; }
        public CarMotionData MotionData { get; set; }
    }
}
