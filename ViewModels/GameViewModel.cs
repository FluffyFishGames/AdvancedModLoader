using AdvancedModLoader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedModLoader.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private Game _Game;
        public Game Game { get { return _Game; } set { this.SetProperty(ref _Game, value, "Game"); } }
    }
}
