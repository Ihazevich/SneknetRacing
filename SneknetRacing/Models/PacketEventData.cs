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
            Header = new PacketHeader();
            EventStringCode = new byte[4];
        }

        public override BaseModel Desserialize(byte[] data)
        {
            PacketEventData temp = new PacketEventData();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    temp.Header.PacketFormat = reader.ReadUInt16();
                    temp.Header.GameMajorVersion = reader.ReadByte();
                    temp.Header.GameMinorVersion = reader.ReadByte();
                    temp.Header.PacketVersion = reader.ReadByte();
                    temp.Header.PacketID = reader.ReadByte();
                    temp.Header.SessionUID = reader.ReadUInt64();
                    temp.Header.SessionTime = reader.ReadSingle();
                    temp.Header.FrameIdentifier = reader.ReadUInt32();
                    temp.Header.PlayerCarIndex = reader.ReadByte();
                    temp.Header.SecondaryPlayerCarIndex = reader.ReadByte();

                    EventStringCode[0] = reader.ReadByte();
                    EventStringCode[1] = reader.ReadByte();
                    EventStringCode[2] = reader.ReadByte();
                    EventStringCode[3] = reader.ReadByte();

                    string eventCode = "" + EventStringCode[0] + EventStringCode[1] + EventStringCode[2] + EventStringCode[3];

                    switch(eventCode)
                    {
                        case "FTLP":
                            temp.EventDetails.FastestLapVehicleIdx = reader.ReadByte();
                            temp.EventDetails.FastestLapLapTime = reader.ReadSingle();
                            break;
                        case "RTMT":
                            temp.EventDetails.RetirementVehicleIdx = reader.ReadByte();
                            break;
                        case "TMPT":
                            temp.EventDetails.TeammateInPitsVehicleIdx = reader.ReadByte();
                            break;
                        case "RCWN":
                            temp.EventDetails.RaceWinnerVehicleIdx = reader.ReadByte();
                            break;
                        case "PENA":
                            temp.EventDetails.PenaltyType = reader.ReadByte();
                            temp.EventDetails.PenaltyInfringementType = reader.ReadByte();
                            temp.EventDetails.PenaltyVehicleIdx = reader.ReadByte();
                            temp.EventDetails.PenaltyOtherVehicleIdx = reader.ReadByte();
                            temp.EventDetails.PenaltyTime = reader.ReadByte();
                            temp.EventDetails.PenaltyLapNum = reader.ReadByte();
                            temp.EventDetails.PenaltyPlacesGained = reader.ReadByte();
                            break;
                        case "SPTP":
                            temp.EventDetails.SpeedTrapVehicleIdx = reader.ReadByte();
                            temp.EventDetails.SpeedTrapSpeed = reader.ReadSingle();
                            break;
                    }
                }
            }
            return temp;
        }
    }
}
