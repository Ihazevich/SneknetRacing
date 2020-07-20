using System;

namespace SneknetRacing.AI
{
    [Serializable]
    public class RacerSample
    {
        public static int SampleSize = 9;

        public float Speed { get; set; }
        public float EngineRPM { get; set; }
        public float SurfaceTypeRL { get; set; }
        public float SurfaceTypeRR { get; set; }
        public float SurfaceTypeFL { get; set; }
        public float SurfaceTypeFR { get; set; }
        public float LapDistance { get; set; }
        public float WorldPosX { get; set; }
        public float WorldPosZ { get; set; }
        /*
        public double WorldForwardDirX { get; set; }
        public double WorldForwardDirZ { get; set; }
        public double WorldRightDirX { get; set; }
        public double WorldRightDirZ { get; set; }
        public double Yaw { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }
        */
        public float Throttle { get; set; }
        public float Steer { get; set; }
        public float CurrentGear { get; set; }

        public RacerSample()
        {
        }
    }
}
