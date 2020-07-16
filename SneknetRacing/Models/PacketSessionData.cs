using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace SneknetRacing.Models
{
    public class PacketSessionData : BaseModel
    {
        #region Fields

        private PacketHeader _header;                    // Header

        private byte _weather;                   // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                            // 3 = light rain, 4 = heavy rain, 5 = storm
        private sbyte _trackTemperature;          // Track temp. in degrees celsius
        private sbyte _airTemperature;            // Air temp. in degrees celsius
        private byte _totalLaps;                 // Total number of laps in this race
        private UInt16 _trackLength;               // Track length in metres
        private byte _sessionType;               // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P
                                            // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
                                            // 10 = R, 11 = R2, 12 = Time Trial
        private sbyte _trackId;                   // -1 for unknown, 0-21 for tracks, see appendix
        private byte _formula;                   // Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2,
                                            // 3 = F1 Generic
        private UInt16 _sessionTimeLeft;           // Time left in session in seconds
        private UInt16 _sessionDuration;           // Session duration in seconds
        private byte _pitSpeedLimit;             // Pit speed limit in kilometres per hour
        private byte _gamePaused;                // Whether the game is paused
        private byte _isSpectating;              // Whether the player is spectating
        private byte _spectatorCarIndex;         // Index of the car being spectated
        private byte _sliProNativeSupport;     // SLI Pro support, 0 = inactive, 1 = active
        private byte _numMarshalZones;           // Number of marshal zones to follow
        private MarshalZone[] _marshalZones;          // List of marshal zones – max 21
        private byte _safetyCarStatus;           // 0 = no safety car, 1 = full safety car
                                                 // 2 = virtual safety car
        private byte _networkGame;               // 0 = offline, 1 = online
        private byte _numWeatherForecastSamples; // Number of weather samples to follow
        private WeatherForecastSample[] _weatherForecastSamples;   // Array of weather forecast samples

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

        public byte Weather
        {
            get
            {
                return _weather;
            }
            set
            {
                _weather = value;
                OnPropertyChanged("Weather");
            }
        }

        public sbyte TrackTemperature
        {
            get
            {
                return _trackTemperature;
            }
            set
            {
                _trackTemperature = value;
                OnPropertyChanged("TrackTemperature");
            }
        }
        public sbyte AirTemperature
        {
            get
            {
                return _airTemperature;
            }
            set
            {
                _airTemperature = value;
                OnPropertyChanged("AirTemperature");
            }
        }
        public byte TotalLaps
        {
            get
            {
                return _totalLaps;
            }
            set
            {
                _totalLaps = value;
                OnPropertyChanged("TotalLaps");
            }
        }
        public UInt16 TrackLength
        {
            get
            {
                return _trackLength;
            }
            set
            {
                _trackLength = value;
                OnPropertyChanged("TrackLength");
            }
        }
        public byte SessionType
        {
            get
            {
                return _sessionType;
            }
            set
            {
                _sessionType = value;
                OnPropertyChanged("SessionType");
            }
        }
        public sbyte TrackID
        {
            get
            {
                return _trackId;
            }
            set
            {
                _trackId = value;
                OnPropertyChanged("TrackID");
            }
        }
        public byte Formula
        {
            get
            {
                return _formula;
            }
            set
            {
                _formula = value;
                OnPropertyChanged("Formula");
            }
        }
        public UInt16 SessionTimeLeft
        {
            get
            {
                return _sessionTimeLeft;
            }
            set
            {
                _sessionTimeLeft = value;
                OnPropertyChanged("SessionTimeLeft");
            }
        }

        public UInt16 SessionDuration
        {
            get
            {
                return _sessionDuration;
            }
            set
            {
                _sessionDuration = value;
                OnPropertyChanged("SessionDuration");
            }
        }
        public byte PitSpeedLimit
        {
            get
            {
                return _pitSpeedLimit;
            }
            set
            {
                _pitSpeedLimit = value;
                OnPropertyChanged("PitSpeedLimit");
            }
        }

        public byte GamePaused
        {
            get
            {
                return _gamePaused;
            }
            set
            {
                _gamePaused = value;
                OnPropertyChanged("GamePaused");
            }
        }

        public byte IsSpectating
        {
            get
            {
                return _isSpectating;
            }
            set
            {
                _isSpectating = value;
                OnPropertyChanged("IsSpectating");
            }
        }

        public byte SpectatorCarIndex
        {
            get
            {
                return _spectatorCarIndex;
            }
            set
            {
                _spectatorCarIndex = value;
                OnPropertyChanged("SpectatorCarIndex");
            }
        }
        public byte SliProNativeSupport
        {
            get
            {
                return _sliProNativeSupport;
            }
            set
            {
                _sliProNativeSupport = value;
                OnPropertyChanged("SliProNativeSupport");
            }
        }
        public byte NumMarshalZones
        {
            get
            {
                return _numMarshalZones;
            }
            set
            {
                _numMarshalZones = value;
                OnPropertyChanged("NumMarshalZones");
            }
        }
        public MarshalZone[] MarshalZones
        {
            get
            {
                return _marshalZones;
            }
            set
            {
                _marshalZones = value;
                OnPropertyChanged("MarshalZones");
            }
        }
        public byte SafetyCarStatus
        {
            get
            {
                return _safetyCarStatus;
            }
            set
            {
                _safetyCarStatus = value;
                OnPropertyChanged("SafetyCarStatus");
            }
        }
        public byte NetworkGame
        {
            get
            {
                return _networkGame;
            }
            set
            {
                _networkGame = value;
                OnPropertyChanged("NetworkGame");
            }
        }
        public byte NumWeatherForecastSamples
        {
            get
            {
                return _numWeatherForecastSamples;
            }
            set
            {
                _numWeatherForecastSamples = value;
                OnPropertyChanged("NumWeatherForecastSamples");
            }
        }

        public WeatherForecastSample[] WeatherForecastSamples
        {
            get
            {
                return _weatherForecastSamples;
            }
            set
            {
                _weatherForecastSamples = value;
                OnPropertyChanged("WeatherForecastSamples");
            }
        }

        #endregion

        public PacketSessionData()
        {
            Header = new PacketHeader();
            MarshalZones = new MarshalZone[21];
            WeatherForecastSamples = new WeatherForecastSample[20];
        }

        #region Methods
        public override BaseModel Desserialize(byte[] data)
        {
            PacketSessionData temp = new PacketSessionData();
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

                    temp.Weather = reader.ReadByte();
                    temp.TrackTemperature = reader.ReadSByte();
                    temp.AirTemperature = reader.ReadSByte();
                    temp.TotalLaps = reader.ReadByte();
                    temp.TrackLength = reader.ReadUInt16();
                    temp.SessionType = reader.ReadByte();
                    temp.TrackID = reader.ReadSByte();
                    temp.Formula = reader.ReadByte();
                    temp.SessionTimeLeft = reader.ReadUInt16();
                    temp.SessionDuration = reader.ReadUInt16();
                    temp.PitSpeedLimit = reader.ReadByte();
                    temp.GamePaused = reader.ReadByte();
                    temp.IsSpectating = reader.ReadByte();
                    temp.SpectatorCarIndex = reader.ReadByte();
                    temp.SliProNativeSupport = reader.ReadByte();
                    temp.NumMarshalZones = reader.ReadByte();

                    for (int i = 0; i < 21; i++)
                    {
                        temp.MarshalZones[i] = new MarshalZone
                        {
                            ZoneStart = reader.ReadSingle(),
                            ZoneFlag = reader.ReadSByte()
                        };
                    }

                    temp.SafetyCarStatus = reader.ReadByte();
                    temp.NetworkGame = reader.ReadByte();
                    temp.NumWeatherForecastSamples = reader.ReadByte();

                    for(int i = 0; i < 20; i++)
                    {
                        temp.WeatherForecastSamples[i] = new WeatherForecastSample
                        {
                            SessionType = reader.ReadByte(),
                            TimeOffset = reader.ReadByte(),
                            Weather = reader.ReadByte(),
                            TrackTemperature = reader.ReadSByte(),
                            AirTemperature = reader.ReadSByte()
                        };
                    }
                }
            }
            return temp;
        }
        #endregion
    }
}
