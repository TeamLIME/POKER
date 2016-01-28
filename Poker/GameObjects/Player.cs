namespace Poker.GameObjects
{
    /// <summary>
    /// Defines a poker game player.
    /// </summary>
    public abstract class Player : IPlayer
    {
        private int chipsCount;
        private int call;
        private int raise;
        private bool hasBankrupted;
        private bool isOnTurn;
        private bool hasFolded;

        /// <summary>
        /// Player's number of playing chips.
        /// </summary>
        public int ChipsCount
        {
            get
            {
                return chipsCount;
            }

            set
            {
                chipsCount = value;
            }
        }

        /// <summary>
        /// Players's call amount in terms of chips.
        /// </summary>
        public int Call
        {
            get
            {
                return call;
            }

            set
            {
                call = value;
            }
        }

        /// <summary>
        /// Players's raise amount in terms of chips.
        /// </summary>
        public int Raise
        {
            get
            {
                return raise;
            }

            set
            {
                raise = value;
            }
        }

        /// <summary>
        /// True if the player has ran out of chips.
        /// </summary>
        public bool HasBankrupted
        {
            get
            {
                return hasBankrupted;
            }

            set
            {
                hasBankrupted = value;
            }
        }

        public bool IsOnTurn
        {
            get
            {
                return isOnTurn;
            }

            set
            {
                isOnTurn = value;
            }
        }

        public bool HasFolded
        {
            get
            {
                return hasFolded;
            }

            set
            {
                hasFolded = value;
            }
        }
    }
}
