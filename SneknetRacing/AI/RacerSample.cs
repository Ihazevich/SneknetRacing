using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Data;

namespace SneknetRacing.AI
{
    public class RacerSample
    {
        public float Speed;
        public float Gear;
        public float EngineRPM;
        public float SurfaceTypeRL;
        public float SurfaceTypeRR;
        public float SurfaceTypeFL;
        public float SurfaceTypeFR;
        public float LapDistance;
        public float WorldPosX;
        public float WorldPosY;
        public float WorldForwardDirX;
        public float WorldForwardDirY;
        public float WorldRightDirX;
        public float WorldRightDirY;
        public float Yaw;
        public float Pitch;
        public float Roll;
    }

    public class RacerPrediction
    {
        public float Throttle;
        public float Steer;
        public float Brake;
        public float Gear;
    }
}
