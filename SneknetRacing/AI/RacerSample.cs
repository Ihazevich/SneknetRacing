using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace SneknetRacing.AI
{
    public class RacerSample
    {
        public static int SampleSize = 17;

        public double Speed;
        public double CurrentGear;
        public double EngineRPM;
        public double SurfaceTypeRL;
        public double SurfaceTypeRR;
        public double SurfaceTypeFL;
        public double SurfaceTypeFR;
        public double LapDistance;
        public double WorldPosX;
        public double WorldPosZ;
        public double WorldForwardDirX;
        public double WorldForwardDirZ;
        public double WorldRightDirX;
        public double WorldRightDirZ;
        public double Yaw;
        public double Pitch;
        public double Roll;

        public double Throttle;
        public double Steer;
        public double Brake;

        public RacerSample()
        {
            Console.WriteLine("Creating new RacerSample");
        }
    }
}
