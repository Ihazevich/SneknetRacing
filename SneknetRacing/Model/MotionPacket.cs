using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.Model
{
    public class MotionPacket : BasePacket
    {
        struct CarMotionData
        {
            private float m_worldPositionX;           // World space X position
            private float m_worldPositionY;           // World space Y position
            private float m_worldPositionZ;           // World space Z position
            private float m_worldVelocityX;           // Velocity in world space X
            private float m_worldVelocityY;           // Velocity in world space Y
            private float m_worldVelocityZ;           // Velocity in world space Z
            private Int16 m_worldForwardDirX;         // World space forward X direction (normalised)
            private Int16 m_worldForwardDirY;         // World space forward Y direction (normalised)
            private Int16 m_worldForwardDirZ;         // World space forward Z direction (normalised)
            private Int16 m_worldRightDirX;           // World space right X direction (normalised)
            private Int16 m_worldRightDirY;           // World space right Y direction (normalised)
            private Int16 m_worldRightDirZ;           // World space right Z direction (normalised)
            private float m_gForceLateral;            // Lateral G-Force component
            private float m_gForceLongitudinal;       // Longitudinal G-Force component
            private float m_gForceVertical;           // Vertical G-Force component
            private float m_yaw;                      // Yaw angle in radians
            private float m_pitch;                    // Pitch angle in radians
            private float m_roll;                     // Roll angle in radians
        };


        struct PacketMotionData
        {
            PacketHeader m_header;                  // Header

            CarMotionData m_carMotionData[];      // Data for all cars on track

            // Extra player car ONLY data
            float m_suspensionPosition[];       // Note: All wheel arrays have the following order:
            float m_suspensionVelocity[];       // RL, RR, FL, FR
            float m_suspensionAcceleration[4];  // RL, RR, FL, FR
            float m_wheelSpeed[4];              // Speed of each wheel
            float m_wheelSlip[4];                // Slip ratio for each wheel
            float m_localVelocityX;             // Velocity in local space
            float m_localVelocityY;             // Velocity in local space
            float m_localVelocityZ;             // Velocity in local space
            float m_angularVelocityX;       // Angular velocity x-component
            float m_angularVelocityY;            // Angular velocity y-component
            float m_angularVelocityZ;            // Angular velocity z-component
            float m_angularAccelerationX;        // Angular velocity x-component
            float m_angularAccelerationY;   // Angular velocity y-component
            float m_angularAccelerationZ;        // Angular velocity z-component
            float m_frontWheelsAngle;            // Current front wheels angle in radians
        };
    }
}
