using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.GameObjects
{
    class HumanPlayer : Player
    {
        private bool shouldRestart;

        public bool ShouldRestart
        {
            get
            {
                return shouldRestart;
            }

            set
            {
                shouldRestart = value;
            }
        }
    }
}
