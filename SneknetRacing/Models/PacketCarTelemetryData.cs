using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
                        
                        UInt16[] brakesTemp = new UInt16[4];
                        brakesTemp[0] = reader.ReadUInt16();
                        brakesTemp[1] = reader.ReadUInt16();
                        brakesTemp[2] = reader.ReadUInt16();
                        brakesTemp[3] = reader.ReadUInt16();
                        temp.CarTelemetryData[i].BrakesTemperature = brakesTemp;
                        
                        byte[] temps = new byte[4];
                        temps[0] = reader.ReadByte();
                        temps[1] = reader.ReadByte();
                        temps[2] = reader.ReadByte();
                        temps[3] = reader.ReadByte();
                        temp.CarTelemetryData[i].TyresSurfaceTemperature = temps;
                        
                        temps[0] = reader.ReadByte();
                        temps[1] = reader.ReadByte();
                        temps[2] = reader.ReadByte();
                        temps[3] = reader.ReadByte();
                        temp.CarTelemetryData[i].TyresInnerTemperature = temps;

                        temp.CarTelemetryData[i].EngineTemperature = reader.ReadUInt16();
                        
                        float[] pressure = new float[4];
                        pressure[0] = reader.ReadSingle();
                        pressure[1] = reader.ReadSingle();
                        pressure[2] = reader.ReadSingle();
                        pressure[3] = reader.ReadSingle();
                        temp.CarTelemetryData[i].TyresPressure = pressure;

                        temps[0] = reader.ReadByte();
                        temps[1] = reader.ReadByte();
                        temps[2] = reader.ReadByte();
                        temps[3] = reader.ReadByte();
                        temp.CarTelemetryData[i].SurfaceType = temps;
                    }
                }
            }
            return temp;
        }
    }
}
