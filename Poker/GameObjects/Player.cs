namespace Poker.GameObjects
{
    public abstract class Player : IPlayer
    {
        private int chipsCount;
        private int call;
        private int raise;
        private bool hasBankrupted;
        private bool isOnTurn;
        private bool hasFolded;

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
