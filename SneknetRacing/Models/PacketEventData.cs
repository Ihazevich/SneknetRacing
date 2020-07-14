using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketEventData : BaseModel
    {
        #region Fields
        private PacketHeader _header;             // Header

        private byte[] _eventStringCode; // Event string code, see below
        private EventDataDetails _eventDetails;       // Event details - should be interpreted differently
                                               // for each type
        #endregion

        #region Properties
        public PacketHeader Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                OnPropertyChanged("Header");
            }
        }
        public byte[] EventStringCode
        {
            get
            {
                return _eventStringCode;
            }
            set
            {
                _eventStringCode = value;
                OnPropertyChanged("EventStringCode");
            }
        }
        public EventDataDetails EventDetails
        {
            get
            {
                return _eventDetails;
            }
            set
            {
                _eventDetails = value;
                OnPropertyChanged("EventDetails");
            }
        }
        #endregion

        public PacketEventData()
        {
            EventStringCode = new byte[4];
        }

        public override void Desserialize(byte[] data)
        {
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    Header.PacketFormat = reader.ReadUInt16();
                    Header.GameMajorVersion = reader.ReadByte();
                    Header.GameMinorVersion = reader.ReadByte();
                    Header.PacketVersion = reader.ReadByte();
                    Header.PacketID = reader.ReadByte();
                    Header.SessionUID = reader.ReadUInt64();
                    Header.SessionTime = reader.ReadSingle();
                    Header.FrameIdentifier = reader.ReadUInt32();
                    Header.PlayerCarIndex = reader.ReadByte();
                    Header.SecondaryPlayerCarIndex = reader.ReadByte();

                    EventStringCode[0] = reader.ReadByte();
                    EventStringCode[1] = reader.ReadByte();
                    EventStringCode[2] = reader.ReadByte();
                    EventStringCode[3] = reader.ReadByte();

                    string eventCode = "" + EventStringCode[0] + EventStringCode[1] + EventStringCode[2] + EventStringCode[3];

                    switch(eventCode)
                    {
                        case "FTLP":
                            EventDetails.FastestLapVehicleIdx = reader.ReadByte();
                            EventDetails.FastestLapLapTime = reader.ReadSingle();
                            break;
                        case "RTMT":
                            EventDetails.RetirementVehicleIdx = reader.ReadByte();
                            break;
                        case "TMPT":
                            EventDetails.TeammateInPitsVehicleIdx = reader.ReadByte();
                            break;
                        case "RCWN":
                            EventDetails.RaceWinnerVehicleIdx = reader.ReadByte();
                            break;
                        case "PENA":
                            EventDetails.PenaltyType = reader.ReadByte();
                            EventDetails.PenaltyInfringementType = reader.ReadByte();
                            EventDetails.PenaltyVehicleIdx = reader.ReadByte();
                            EventDetails.PenaltyOtherVehicleIdx = reader.ReadByte();
                            EventDetails.PenaltyTime = reader.ReadByte();
                            EventDetails.PenaltyLapNum = reader.ReadByte();
                            EventDetails.PenaltyPlacesGained = reader.ReadByte();
                            break;
                        case "SPTP":
                            EventDetails.SpeedTrapVehicleIdx = reader.ReadByte();
                            EventDetails.SpeedTrapSpeed = reader.ReadSingle();
                            break;
                    }
                }
            }
        }
    }
}
