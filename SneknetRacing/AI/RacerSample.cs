using System;

namespace SneknetRacing.AI
{
    [Serializable]
    public class RacerSample
    {
        public static int SampleSize = 9;

        public double Speed { get; set; }
        public double EngineRPM { get; set; }
        public double SurfaceTypeRL { get; set; }
        public double SurfaceTypeRR { get; set; }
        public double SurfaceTypeFL { get; set; }
        public double SurfaceTypeFR { get; set; }
        public double LapDistance { get; set; }
        public double WorldPosX { get; set; }
        public double WorldPosZ { get; set; }
        /*
        public float WorldForwardDirX { get; set; }
        public float WorldForwardDirZ { get; set; }
        public float WorldRightDirX { get; set; }
        public float WorldRightDirZ { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        */
        public double Throttle { get; set; }
        public double Steer { get; set; }
        public double CurrentGear { get; set; }

        public RacerSample()
        {
        }
    }
}
