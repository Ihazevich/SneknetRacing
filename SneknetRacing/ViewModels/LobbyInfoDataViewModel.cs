using SneknetRacing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SneknetRacing.ViewModels
{
    public class LobbyInfoDataViewModel : BaseViewModel
    {
        public LobbyInfoDataViewModel() : base(new PacketLobbyInfoData())
        {

        }
    }
}
