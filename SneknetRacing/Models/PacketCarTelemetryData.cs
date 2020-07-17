using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketCarTelemetryData : BaseModel
    {
        #region Fields
        private PacketHeader _header;         // Header
        private CarTelemetryData[] _carTelemetryData;

        private UInt32 _buttonStatus;        // Bit flags specifying which buttons are being pressed
                                      // currently - see appendices

        // Added in Beta 3:
        private byte _mfdPanelIndex;       // Index of MFD panel open - 255 = MFD closed
                                     // Single player, race – 0 = Car setup, 1 = Pits
                                     // 2 = Damage, 3 =  Engine, 4 = Temperatures
                                     // May vary depending on game mode
        private byte _mfdPanelIndexSecondaryPlayer;   // See above
        private byte _suggestedGear;       // Suggested gear for the player (1-8)
                                    // 0 if no gear suggested
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
        public CarTelemetryData[] CarTelemetryData
        {
            get
            {
                return _carTelemetryData;
            }
            set
            {
                _carTelemetryData = value;
                OnPropertyChanged("CarTelemetryData");
            }
        }
        public UInt32 ButtonStatus
        {
            get
            {
                return _buttonStatus;
            }
            set
            {
                _buttonStatus = value;
                OnPropertyChanged("ButtonStatus");
            }
        }
        public byte MfdPanelIndex
        {
            get
            {
                return _mfdPanelIndex;
            }
            set
            {
                _mfdPanelIndex = value;
                OnPropertyChanged("MfdPanelIndex");
            }
        }

        public byte MfdPanelIndexSecondaryPlayer
        {
            get
            {
                return _mfdPanelIndexSecondaryPlayer;
            }
            set
            {
                _mfdPanelIndexSecondaryPlayer = value;
                OnPropertyChanged("MfdPanelIndexSecondaryPlayer");
            }
        }

        public byte SuggestedGear
        {
            get
            {
                return _suggestedGear;
            }
            set
            {
                _suggestedGear = value;
                OnPropertyChanged("SuggestedGear");
            }
        }
        #endregion

        public PacketCarTelemetryData()
        {
            Header = new PacketHeader();
            CarTelemetryData = new CarTelemetryData[22];
        }

        public override BaseModel Desserialize(byte[] data)
        {
            PacketCarTelemetryData temp = new PacketCarTelemetryData();
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

                    for(int i = 0; i < 22; i++)
                    {
                        temp.CarTelemetryData[i] = new CarTelemetryData()
                        {
                            Speed = reader.ReadUInt16(),
                            Throttle = reader.ReadSingle(),
                            Steer = reader.ReadSingle(),
                            Brake = reader.ReadSingle(),
                            Clutch = reader.ReadByte(),
                            Gear = reader.ReadSByte(),
                            EngineRPM = reader.ReadUInt16(),
                            Drs = reader.ReadByte(),
                            RevLightsPercent = reader.ReadByte()
                        };

                        temp.CarTelemetryData[i].BrakesTemperature.Add(reader.ReadUInt16());
                        temp.CarTelemetryData[i].BrakesTemperature.Add(reader.ReadUInt16());
                        temp.CarTelemetryData[i].BrakesTemperature.Add(reader.ReadUInt16());
                        temp.CarTelemetryData[i].BrakesTemperature.Add(reader.ReadUInt16());

                        temp.CarTelemetryData[i].TyresSurfaceTemperature = reader.ReadBytes(4).OfType<byte>().ToList();
                        temp.CarTelemetryData[i].TyresInnerTemperature = reader.ReadBytes(4).OfType<byte>().ToList();

                        temp.CarTelemetryData[i].EngineTemperature = reader.ReadUInt16();

                        temp.CarTelemetryData[i].TyresPressure.Add(reader.ReadSingle());
                        temp.CarTelemetryData[i].TyresPressure.Add(reader.ReadSingle());
                        temp.CarTelemetryData[i].TyresPressure.Add(reader.ReadSingle());
                        temp.CarTelemetryData[i].TyresPressure.Add(reader.ReadSingle());

                        temp.CarTelemetryData[i].SurfaceType = reader.ReadBytes(4).OfType<byte>().ToList();
                    }
                }
            }
            return temp;
        }
    }
}
