using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Poker
{

    public partial class PokerTable : Form
    {
        #region Variables
        private const int InitialNumberOfBots = 5;
        private const int DefaultBigBlindValue = 500;
        private const int DefaultSmallBlindValue = 250;
        private const int DefaultChipsCount = 10000;
        private const int NumberOfCardsInDeck = 52;
        private const int TableCardsCount = 17;
        
        private Panel playerPanel = new Panel();
        private Panel bot1Panel = new Panel();
        private Panel bot2Panel = new Panel();
        private Panel bot3Panel = new Panel();
        private Panel bot4Panel = new Panel();
        private Panel bot5Panel = new Panel();

        private List<bool?> bankruptPlayers;
        private List<Type> winningHands;
        private Image[] deckCardsImages;
        private PictureBox[] pictureBoxDeckCards;

        private Timer timer;
        private Timer updates;

        private int callValue; 
        private int currentBotsPlayingCount;
        private int bigBlindValue;
        private int smallBlindValue;
        
        private int playerChipsCount;
        private int playerCall;
        private int playerRaise;
        private bool hasPlayerBankrupted;
        private bool isPlayerTurn;
        private bool shouldRestart;
        
        private int bot1ChipsCount;
        private int bot2ChipsCount; 
        private int bot3ChipsCount;
        private int bot4ChipsCount;
        private int bot5ChipsCount;
        private bool isBotOneTurn;
        private bool isBotTwoTurn;
        private bool isBotThreeTurn;
        private bool isBotFourTurn;
        private bool isBotFiveTurn;
        private bool hasBotOneBankrupted;
        private bool hasBotTwoBankrupted; 
        private bool hasBotThreeBankrupted;
        private bool hasBotFourBankrupted;
        private bool hasBotFiveBankrupted;
        private bool hasPlayerFolded;
        private bool hasBotOneFolded;
        private bool hasBotTwoFolded;
        private bool hasBotThreeFolded;
        private bool hasBotFourFolded;
        private bool hasBotFiveFolded;
        private int botOneCall; 
        private int botTwoCall;
        private int botThreeCall;
        private int botFourCall;
        private int botFiveCall;
        private int botOneRaise;
        private int botTwoRaise;
        private int botThreeRaise;
        private int botFourRaise;
        private int botFiveRaise;

        private int windowWidth;
        private int windowHeight;
        private int winners;

        double type, rounds = 0, b1Power, b2Power, b3Power, b4Power, b5Power, pPower = 0, pType = -1, Raise = 0,
        b1Type = -1, b2Type = -1, b3Type = -1, b4Type = -1, b5Type = -1;
        bool intsadded, changed;
        int Flop = 1, Turn = 2, River = 3, End = 4, maxLeft = 6;
        int last = 123, raisedTurn = 1;
        List<string> CheckWinners = new List<string>();
        List<int> ints = new List<int>();
        bool raising = false;
        Type sorted;
        string[] ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        int[] Reserve = new int[TableCardsCount];
        int t = 60, i, up = 10000000, turnCount = 0;
        #endregion
        
        public PokerTable()
        {
            this.bankruptPlayers = new List<bool?>();
            this.winningHands = new List<Type>();
            this.deckCardsImages = new Image[NumberOfCardsInDeck];
            this.pictureBoxDeckCards = new PictureBox[NumberOfCardsInDeck];
            this.timer = new Timer();
            this.timer.Interval = (1 * 1 * 1000);
            this.timer.Tick += timer_Tick;
            this.updates = new Timer();
            this.updates.Interval = (1 * 1 * 100);
            this.updates.Tick += Update_Tick;
            this.updates.Start();
            this.callValue = bigBlindValue;
            this.bigBlindValue = DefaultBigBlindValue;
            this.smallBlindValue = DefaultSmallBlindValue;
            this.InitializePlayer();
            this.winners = 0;
            this.IntializeBots();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.InitializeComponent(); //Definition in the designer partial class.
            this.windowWidth = this.Width;
            this.windowHeight = this.Height;
            this.DisableTextBoxesUserInteraction();
            this.InitializeTextBoxes();
            this.SetBlindButtonsVisibilityToFalse();
            this.Shuffle();
        }
        async Task Shuffle()
        {
            bankruptPlayers.Add(hasPlayerBankrupted); bankruptPlayers.Add(hasBotOneBankrupted); bankruptPlayers.Add(hasBotTwoBankrupted); bankruptPlayers.Add(hasBotThreeBankrupted); bankruptPlayers.Add(hasBotFourBankrupted); bankruptPlayers.Add(hasBotFiveBankrupted);
            buttonCall.Enabled = false;
            buttonRaise.Enabled = false;
            buttonFold.Enabled = false;
            buttonCheck.Enabled = false;
            MaximizeBox = false;
            MinimizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");
            int horizontal = 580, vertical = 480;
            Random r = new Random();
            for (i = ImgLocation.Length; i > 0; i--)
            {
                int j = r.Next(i);
                var k = ImgLocation[j];
                ImgLocation[j] = ImgLocation[i - 1];
                ImgLocation[i - 1] = k;
            }
            for (i = 0; i < TableCardsCount; i++)
            {

                deckCardsImages[i] = Image.FromFile(ImgLocation[i]);
                var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
                foreach (var c in charsToRemove)
                {
                    ImgLocation[i] = ImgLocation[i].Replace(c, string.Empty);
                }
                Reserve[i] = int.Parse(ImgLocation[i]) - 1;
                pictureBoxDeckCards[i] = new PictureBox();
                pictureBoxDeckCards[i].SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxDeckCards[i].Height = 130;
                pictureBoxDeckCards[i].Width = 80;
                this.Controls.Add(pictureBoxDeckCards[i]);
                pictureBoxDeckCards[i].Name = "pb" + i.ToString();
                await Task.Delay(200);
                #region Throwing Cards
                if (i < 2)
                {
                    if (pictureBoxDeckCards[0].Tag != null)
                    {
                        pictureBoxDeckCards[1].Tag = Reserve[1];
                    }
                    pictureBoxDeckCards[0].Tag = Reserve[0];
                    pictureBoxDeckCards[i].Image = deckCardsImages[i];
                    pictureBoxDeckCards[i].Anchor = (AnchorStyles.Bottom);
                    //Holder[i].Dock = DockStyle.Top;
                    pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                    horizontal += pictureBoxDeckCards[i].Width;
                    this.Controls.Add(playerPanel);
                    playerPanel.Location = new Point(pictureBoxDeckCards[0].Left - 10, pictureBoxDeckCards[0].Top - 10);
                    playerPanel.BackColor = Color.DarkBlue;
                    playerPanel.Height = 150;
                    playerPanel.Width = 180;
                    playerPanel.Visible = false;
                }
                if (bot1ChipsCount > 0)
                {
                    currentBotsPlayingCount--;
                    if (i >= 2 && i < 4)
                    {
                        if (pictureBoxDeckCards[2].Tag != null)
                        {
                            pictureBoxDeckCards[3].Tag = Reserve[3];
                        }
                        pictureBoxDeckCards[2].Tag = Reserve[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }
                        check = true;
                        pictureBoxDeckCards[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        pictureBoxDeckCards[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                        horizontal += pictureBoxDeckCards[i].Width;
                        pictureBoxDeckCards[i].Visible = true;
                        this.Controls.Add(bot1Panel);
                        bot1Panel.Location = new Point(pictureBoxDeckCards[2].Left - 10, pictureBoxDeckCards[2].Top - 10);
                        bot1Panel.BackColor = Color.DarkBlue;
                        bot1Panel.Height = 150;
                        bot1Panel.Width = 180;
                        bot1Panel.Visible = false;
                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }
                if (bot2ChipsCount > 0)
                {
                    currentBotsPlayingCount--;
                    if (i >= 4 && i < 6)
                    {
                        if (pictureBoxDeckCards[4].Tag != null)
                        {
                            pictureBoxDeckCards[5].Tag = Reserve[5];
                        }
                        pictureBoxDeckCards[4].Tag = Reserve[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }
                        check = true;
                        pictureBoxDeckCards[i].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        pictureBoxDeckCards[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                        horizontal += pictureBoxDeckCards[i].Width;
                        pictureBoxDeckCards[i].Visible = true;
                        this.Controls.Add(bot2Panel);
                        bot2Panel.Location = new Point(pictureBoxDeckCards[4].Left - 10, pictureBoxDeckCards[4].Top - 10);
                        bot2Panel.BackColor = Color.DarkBlue;
                        bot2Panel.Height = 150;
                        bot2Panel.Width = 180;
                        bot2Panel.Visible = false;
                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }
                if (bot3ChipsCount > 0)
                {
                    currentBotsPlayingCount--;
                    if (i >= 6 && i < 8)
                    {
                        if (pictureBoxDeckCards[6].Tag != null)
                        {
                            pictureBoxDeckCards[7].Tag = Reserve[7];
                        }
                        pictureBoxDeckCards[6].Tag = Reserve[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }
                        check = true;
                        pictureBoxDeckCards[i].Anchor = (AnchorStyles.Top);
                        pictureBoxDeckCards[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                        horizontal += pictureBoxDeckCards[i].Width;
                        pictureBoxDeckCards[i].Visible = true;
                        this.Controls.Add(bot3Panel);
                        bot3Panel.Location = new Point(pictureBoxDeckCards[6].Left - 10, pictureBoxDeckCards[6].Top - 10);
                        bot3Panel.BackColor = Color.DarkBlue;
                        bot3Panel.Height = 150;
                        bot3Panel.Width = 180;
                        bot3Panel.Visible = false;
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }
                if (bot4ChipsCount > 0)
                {
                    currentBotsPlayingCount--;
                    if (i >= 8 && i < 10)
                    {
                        if (pictureBoxDeckCards[8].Tag != null)
                        {
                            pictureBoxDeckCards[9].Tag = Reserve[9];
                        }
                        pictureBoxDeckCards[8].Tag = Reserve[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }
                        check = true;
                        pictureBoxDeckCards[i].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        pictureBoxDeckCards[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                        horizontal += pictureBoxDeckCards[i].Width;
                        pictureBoxDeckCards[i].Visible = true;
                        this.Controls.Add(bot4Panel);
                        bot4Panel.Location = new Point(pictureBoxDeckCards[8].Left - 10, pictureBoxDeckCards[8].Top - 10);
                        bot4Panel.BackColor = Color.DarkBlue;
                        bot4Panel.Height = 150;
                        bot4Panel.Width = 180;
                        bot4Panel.Visible = false;
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }
                if (bot5ChipsCount > 0)
                {
                    currentBotsPlayingCount--;
                    if (i >= 10 && i < 12)
                    {
                        if (pictureBoxDeckCards[10].Tag != null)
                        {
                            pictureBoxDeckCards[11].Tag = Reserve[11];
                        }
                        pictureBoxDeckCards[10].Tag = Reserve[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }
                        check = true;
                        pictureBoxDeckCards[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        pictureBoxDeckCards[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                        horizontal += pictureBoxDeckCards[i].Width;
                        pictureBoxDeckCards[i].Visible = true;
                        this.Controls.Add(bot5Panel);
                        bot5Panel.Location = new Point(pictureBoxDeckCards[10].Left - 10, pictureBoxDeckCards[10].Top - 10);
                        bot5Panel.BackColor = Color.DarkBlue;
                        bot5Panel.Height = 150;
                        bot5Panel.Width = 180;
                        bot5Panel.Visible = false;
                        if (i == 11)
                        {
                            check = false;
                        }
                    }
                }
                if (i >= 12)
                {
                    pictureBoxDeckCards[12].Tag = Reserve[12];
                    if (i > 12) pictureBoxDeckCards[13].Tag = Reserve[13];
                    if (i > 13) pictureBoxDeckCards[14].Tag = Reserve[14];
                    if (i > 14) pictureBoxDeckCards[15].Tag = Reserve[15];
                    if (i > 15)
                    {
                        pictureBoxDeckCards[16].Tag = Reserve[16];

                    }
                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }
                    check = true;
                    if (pictureBoxDeckCards[i] != null)
                    {
                        pictureBoxDeckCards[i].Anchor = AnchorStyles.None;
                        pictureBoxDeckCards[i].Image = backImage;
                        //Holder[i].Image = Deck[i];
                        pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }
                #endregion
                if (bot1ChipsCount <= 0)
                {
                    hasBotOneBankrupted = true;
                    pictureBoxDeckCards[2].Visible = false;
                    pictureBoxDeckCards[3].Visible = false;
                }
                else
                {
                    hasBotOneBankrupted = false;
                    if (i == 3)
                    {
                        if (pictureBoxDeckCards[3] != null)
                        {
                            pictureBoxDeckCards[2].Visible = true;
                            pictureBoxDeckCards[3].Visible = true;
                        }
                    }
                }
                if (bot2ChipsCount <= 0)
                {
                    hasBotTwoBankrupted = true;
                    pictureBoxDeckCards[4].Visible = false;
                    pictureBoxDeckCards[5].Visible = false;
                }
                else
                {
                    hasBotTwoBankrupted = false;
                    if (i == 5)
                    {
                        if (pictureBoxDeckCards[5] != null)
                        {
                            pictureBoxDeckCards[4].Visible = true;
                            pictureBoxDeckCards[5].Visible = true;
                        }
                    }
                }
                if (bot3ChipsCount <= 0)
                {
                    hasBotThreeBankrupted = true;
                    pictureBoxDeckCards[6].Visible = false;
                    pictureBoxDeckCards[7].Visible = false;
                }
                else
                {
                    hasBotThreeBankrupted = false;
                    if (i == 7)
                    {
                        if (pictureBoxDeckCards[7] != null)
                        {
                            pictureBoxDeckCards[6].Visible = true;
                            pictureBoxDeckCards[7].Visible = true;
                        }
                    }
                }
                if (bot4ChipsCount <= 0)
                {
                    hasBotFourBankrupted = true;
                    pictureBoxDeckCards[8].Visible = false;
                    pictureBoxDeckCards[9].Visible = false;
                }
                else
                {
                    hasBotFourBankrupted = false;
                    if (i == 9)
                    {
                        if (pictureBoxDeckCards[9] != null)
                        {
                            pictureBoxDeckCards[8].Visible = true;
                            pictureBoxDeckCards[9].Visible = true;
                        }
                    }
                }
                if (bot5ChipsCount <= 0)
                {
                    hasBotFiveBankrupted = true;
                    pictureBoxDeckCards[10].Visible = false;
                    pictureBoxDeckCards[11].Visible = false;
                }
                else
                {
                    hasBotFiveBankrupted = false;
                    if (i == 11)
                    {
                        if (pictureBoxDeckCards[11] != null)
                        {
                            pictureBoxDeckCards[10].Visible = true;
                            pictureBoxDeckCards[11].Visible = true;
                        }
                    }
                }
                if (i == 16)
                {
                    if (!shouldRestart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    timer.Start();
                }
            }
            if (currentBotsPlayingCount == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                currentBotsPlayingCount = 5;
            }
            if (i == TableCardsCount)
            {
                buttonRaise.Enabled = true;
                buttonCall.Enabled = true;
                buttonRaise.Enabled = true;
                buttonRaise.Enabled = true;
                buttonFold.Enabled = true;
            }
        }
        async Task Turns()
        {
            #region Rotating
            if (!hasPlayerBankrupted)
            {
                if (isPlayerTurn)
                {
                    FixCall(labelPlayerStatus, ref playerCall, ref playerRaise, 1);
                    //MessageBox.Show("Player's Turn");
                    progressBarTimer.Visible = true;
                    progressBarTimer.Value = 1000;
                    t = 60;
                    up = 10000000;
                    timer.Start();
                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    turnCount++;
                    FixCall(labelPlayerStatus, ref playerCall, ref playerRaise, 2);
                }
            }
            if (hasPlayerBankrupted || !isPlayerTurn)
            {
                await AllIn();
                if (hasPlayerBankrupted && !hasPlayerFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bankruptPlayers.RemoveAt(0);
                        bankruptPlayers.Insert(0, null);
                        maxLeft--;
                        hasPlayerFolded = true;
                    }
                }
                await CheckRaise(0, 0);
                progressBarTimer.Visible = false;
                buttonRaise.Enabled = false;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                timer.Stop();
                isBotOneTurn = true;
                if (!hasBotOneBankrupted)
                {
                    if (isBotOneTurn)
                    {
                        FixCall(labelBot1Status, ref botOneCall, ref botOneRaise, 1);
                        FixCall(labelBot1Status, ref botOneCall, ref botOneRaise, 2);
                        Rules(2, 3, "Bot 1", ref b1Type, ref b1Power, hasBotOneBankrupted);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, ref bot1ChipsCount, ref isBotOneTurn, ref  hasBotOneBankrupted, labelBot1Status, 0, b1Power, b1Type);
                        turnCount++;
                        last = 1;
                        isBotOneTurn = false;
                        isBotTwoTurn = true;
                    }
                }
                if (hasBotOneBankrupted && !hasBotOneFolded)
                {
                    bankruptPlayers.RemoveAt(1);
                    bankruptPlayers.Insert(1, null);
                    maxLeft--;
                    hasBotOneFolded = true;
                }
                if (hasBotOneBankrupted || !isBotOneTurn)
                {
                    await CheckRaise(1, 1);
                    isBotTwoTurn = true;
                }
                if (!hasBotTwoBankrupted)
                {
                    if (isBotTwoTurn)
                    {
                        FixCall(labelBot2Status, ref botTwoCall, ref botTwoRaise, 1);
                        FixCall(labelBot2Status, ref botTwoCall, ref botTwoRaise, 2);
                        Rules(4, 5, "Bot 2", ref b2Type, ref b2Power, hasBotTwoBankrupted);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, ref bot2ChipsCount, ref isBotTwoTurn, ref  hasBotTwoBankrupted, labelBot2Status, 1, b2Power, b2Type);
                        turnCount++;
                        last = 2;
                        isBotTwoTurn = false;
                        isBotThreeTurn = true;
                    }
                }
                if (hasBotTwoBankrupted && !hasBotTwoFolded)
                {
                    bankruptPlayers.RemoveAt(2);
                    bankruptPlayers.Insert(2, null);
                    maxLeft--;
                    hasBotTwoFolded = true;
                }
                if (hasBotTwoBankrupted || !isBotTwoTurn)
                {
                    await CheckRaise(2, 2);
                    isBotThreeTurn = true;
                }
                if (!hasBotThreeBankrupted)
                {
                    if (isBotThreeTurn)
                    {
                        FixCall(labelBot3Status, ref botThreeCall, ref botThreeRaise, 1);
                        FixCall(labelBot3Status, ref botThreeCall, ref botThreeRaise, 2);
                        Rules(6, 7, "Bot 3", ref b3Type, ref b3Power, hasBotThreeBankrupted);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, ref bot3ChipsCount, ref isBotThreeTurn, ref  hasBotThreeBankrupted, labelBot3Status, 2, b3Power, b3Type);
                        turnCount++;
                        last = 3;
                        isBotThreeTurn = false;
                        isBotFourTurn = true;
                    }
                }
                if (hasBotThreeBankrupted && !hasBotThreeFolded)
                {
                    bankruptPlayers.RemoveAt(3);
                    bankruptPlayers.Insert(3, null);
                    maxLeft--;
                    hasBotThreeFolded = true;
                }
                if (hasBotThreeBankrupted || !isBotThreeTurn)
                {
                    await CheckRaise(3, 3);
                    isBotFourTurn = true;
                }
                if (!hasBotFourBankrupted)
                {
                    if (isBotFourTurn)
                    {
                        FixCall(labelBot4Status, ref botFourCall, ref botFourRaise, 1);
                        FixCall(labelBot4Status, ref botFourCall, ref botFourRaise, 2);
                        Rules(8, 9, "Bot 4", ref b4Type, ref b4Power, hasBotFourBankrupted);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, ref bot4ChipsCount, ref isBotFourTurn, ref  hasBotFourBankrupted, labelBot4Status, 3, b4Power, b4Type);
                        turnCount++;
                        last = 4;
                        isBotFourTurn = false;
                        isBotFiveTurn = true;
                    }
                }
                if (hasBotFourBankrupted && !hasBotFourFolded)
                {
                    bankruptPlayers.RemoveAt(4);
                    bankruptPlayers.Insert(4, null);
                    maxLeft--;
                    hasBotFourFolded = true;
                }
                if (hasBotFourBankrupted || !isBotFourTurn)
                {
                    await CheckRaise(4, 4);
                    isBotFiveTurn = true;
                }
                if (!hasBotFiveBankrupted)
                {
                    if (isBotFiveTurn)
                    {
                        FixCall(labelBot5Status, ref botFiveCall, ref botFiveRaise, 1);
                        FixCall(labelBot5Status, ref botFiveCall, ref botFiveRaise, 2);
                        Rules(10, 11, "Bot 5", ref b5Type, ref b5Power, hasBotFiveBankrupted);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, ref bot5ChipsCount, ref isBotFiveTurn, ref  hasBotFiveBankrupted, labelBot5Status, 4, b5Power, b5Type);
                        turnCount++;
                        last = 5;
                        isBotFiveTurn = false;
                    }
                }
                if (hasBotFiveBankrupted && !hasBotFiveFolded)
                {
                    bankruptPlayers.RemoveAt(5);
                    bankruptPlayers.Insert(5, null);
                    maxLeft--;
                    hasBotFiveFolded = true;
                }
                if (hasBotFiveBankrupted || !isBotFiveTurn)
                {
                    await CheckRaise(5, 5);
                    isPlayerTurn = true;
                }
                if (hasPlayerBankrupted && !hasPlayerFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bankruptPlayers.RemoveAt(0);
                        bankruptPlayers.Insert(0, null);
                        maxLeft--;
                        hasPlayerFolded = true;
                    }
                }
            #endregion
                await AllIn();
                if (!shouldRestart)
                {
                    await Turns();
                }
                shouldRestart = false;
            }
        }

        void Rules(int c1, int c2, string currentText, ref double current, ref double Power, bool foldedTurn)
        {
            if (!foldedTurn || c1 == 0 && c2 == 1 && labelPlayerStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];
                int[] Straight = new int[7];
                Straight[0] = Reserve[c1];
                Straight[1] = Reserve[c2];
                Straight1[0] = Straight[2] = Reserve[12];
                Straight1[1] = Straight[3] = Reserve[13];
                Straight1[2] = Straight[4] = Reserve[14];
                Straight1[3] = Straight[5] = Reserve[15];
                Straight1[4] = Straight[6] = Reserve[16];
                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(Straight); Array.Sort(st1); Array.Sort(st2); Array.Sort(st3); Array.Sort(st4);
                #endregion

                for (i = 0; i < 16; i++)
                {
                    if (Reserve[i] == int.Parse(pictureBoxDeckCards[c1].Tag.ToString()) && Reserve[i + 1] == int.Parse(pictureBoxDeckCards[c2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1

                        rPairFromHand(ref current, ref Power);

                        #region Pair or Two Pair from Table current = 2 || 0
                        rPairTwoPair(ref current, ref Power);
                        #endregion

                        #region Two Pair current = 2
                        rTwoPair(ref current, ref Power);
                        #endregion

                        #region Three of a kind current = 3
                        rThreeOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight current = 4
                        rStraight(ref current, ref Power, Straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        rFlush(ref current, ref Power, ref vf, Straight1);
                        #endregion

                        #region Full House current = 6
                        rFullHouse(ref current, ref Power, ref done, Straight);
                        #endregion

                        #region Four of a Kind current = 7
                        rFourOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        rStraightFlush(ref current, ref Power, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        rHighCard(ref current, ref Power);
                        #endregion
                    }
                }
            }
        }
        private void rStraightFlush(ref double current, ref double Power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        Power = (st1.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        Power = (st1.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        Power = (st2.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        Power = (st2.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        Power = (st3.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st3[0] == 0 && st3[1] == 9 && st3[2] == 10 && st3[3] == 11 && st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        Power = (st3.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        Power = (st4.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 8 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (st4[0] == 0 && st4[1] == 9 && st4[2] == 10 && st4[3] == 11 && st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        Power = (st4.Max()) / 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 9 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rFourOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 && Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        current = 7;
                        Power = (Straight[j] / 4) * 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 7 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        Power = 13 * 4 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 7 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rFullHouse(ref double current, ref double Power, ref bool done, int[] Straight)
        {
            if (current >= -1)
            {
                type = Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                Power = 13 * 2 + current * 100;
                                winningHands.Add(new Type() { Power = Power, Current = 6 });
                                sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                Power = fh.Max() / 4 * 2 + current * 100;
                                winningHands.Add(new Type() { Power = Power, Current = 6 });
                                sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }
                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }
                if (current != 6)
                {
                    Power = type;
                }
            }
        }
        private void rFlush(ref double current, ref double Power, ref bool vf, int[] Straight1)
        {
            if (current >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f1.Max() / 4 && Reserve[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f1[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f1.Length == 5)
                {
                    if (Reserve[i] % 4 == f1[0] % 4 && Reserve[i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f1[0] % 4 && Reserve[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f1.Min() / 4 && Reserve[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        Power = f1.Max() + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f2.Max() / 4 && Reserve[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f2[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (Reserve[i] % 4 == f2[0] % 4 && Reserve[i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f2[0] % 4 && Reserve[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f2.Min() / 4 && Reserve[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        Power = f2.Max() + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f3.Max() / 4 && Reserve[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f3[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (Reserve[i] % 4 == f3[0] % 4 && Reserve[i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f3[0] % 4 && Reserve[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f3.Min() / 4 && Reserve[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        Power = f3.Max() + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (Reserve[i] % 4 == Reserve[i + 1] % 4 && Reserve[i] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (Reserve[i] / 4 < f4.Max() / 4 && Reserve[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (Reserve[i] % 4 != Reserve[i + 1] % 4 && Reserve[i] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (Reserve[i + 1] % 4 != Reserve[i] % 4 && Reserve[i + 1] % 4 == f4[0] % 4)
                    {
                        if (Reserve[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = Reserve[i + 1] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 5 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (Reserve[i] % 4 == f4[0] % 4 && Reserve[i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (Reserve[i + 1] % 4 == f4[0] % 4 && Reserve[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = Reserve[i + 1] + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (Reserve[i] / 4 < f4.Min() / 4 && Reserve[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        Power = f4.Max() + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (Reserve[i] / 4 == 0 && Reserve[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (Reserve[i + 1] / 4 == 0 && Reserve[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rStraight(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            Power = op.Max() + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 4 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            Power = op[j + 4] + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 4 });
                            sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }
                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        Power = 13 + current * 100;
                        winningHands.Add(new Type() { Power = Power, Current = 4 });
                        sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }
        private void rThreeOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            Power = 13 * 3 + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 3 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 3 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }
        private void rTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (Reserve[i] / 4 != Reserve[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (Reserve[i] / 4 == Reserve[tc] / 4 && Reserve[i + 1] / 4 == Reserve[tc - k] / 4 ||
                                    Reserve[i + 1] / 4 == Reserve[tc] / 4 && Reserve[i] / 4 == Reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (Reserve[i] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 != 0 && Reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void rPairTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }
                        if (tc - k >= 12)
                        {
                            if (Reserve[tc] / 4 == Reserve[tc - k] / 4)
                            {
                                if (Reserve[tc] / 4 != Reserve[i] / 4 && Reserve[tc] / 4 != Reserve[i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (Reserve[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i] / 4) * 2 + 13 * 4 + current * 100;
                                            winningHands.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            winningHands.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[tc] / 4) * 2 + (Reserve[i + 1] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (Reserve[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (Reserve[tc] / 4) * 2 + (Reserve[i] / 4) * 2 + current * 100;
                                            winningHands.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + Reserve[i] / 4 + current * 100;
                                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = Reserve[tc] / 4 + Reserve[i] / 4 + current * 100;
                                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (Reserve[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + Reserve[i + 1] + current * 100;
                                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = Reserve[tc] / 4 + Reserve[i + 1] / 4 + current * 100;
                                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }
                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void rPairFromHand(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (Reserve[i] / 4 == Reserve[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (Reserve[i] / 4 == 0)
                        {
                            current = 1;
                            Power = 13 * 4 + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 1 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            Power = (Reserve[i + 1] / 4) * 4 + current * 100;
                            winningHands.Add(new Type() { Power = Power, Current = 1 });
                            sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (Reserve[i + 1] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[i + 1] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + Reserve[i] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (Reserve[i + 1] / 4) * 4 + Reserve[i] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                    if (Reserve[i] / 4 == Reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (Reserve[i] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + Reserve[i + 1] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (Reserve[tc] / 4) * 4 + Reserve[i + 1] / 4 + current * 100;
                                winningHands.Add(new Type() { Power = Power, Current = 1 });
                                sorted = winningHands.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }
        private void rHighCard(ref double current, ref double Power)
        {
            if (current == -1)
            {
                if (Reserve[i] / 4 > Reserve[i + 1] / 4)
                {
                    current = -1;
                    Power = Reserve[i] / 4;
                    winningHands.Add(new Type() { Power = Power, Current = -1 });
                    sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    Power = Reserve[i + 1] / 4;
                    winningHands.Add(new Type() { Power = Power, Current = -1 });
                    sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (Reserve[i] / 4 == 0 || Reserve[i + 1] / 4 == 0)
                {
                    current = -1;
                    Power = 13;
                    winningHands.Add(new Type() { Power = Power, Current = -1 });
                    sorted = winningHands.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(double current, double Power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (pictureBoxDeckCards[j].Visible)
                    pictureBoxDeckCards[j].Image = deckCardsImages[j];
            }
            if (current == sorted.Current)
            {
                if (Power == sorted.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }
            if (currentText == lastly)//lastfixed
            {
                if (winners > 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        playerChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxPlayerChips.Text = playerChipsCount.ToString();
                        //playerPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot1.Text = bot1ChipsCount.ToString();
                        //bot1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot2.Text = bot2ChipsCount.ToString();
                        //bot2Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot3.Text = bot3ChipsCount.ToString();
                        //bot3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot4.Text = bot4ChipsCount.ToString();
                        //bot4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot5.Text = bot5ChipsCount.ToString();
                        //bot5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        playerChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot2Panel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot5Panel.Visible = true;
                    }
                }
            }
        }
        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (raising)
            {
                turnCount = 0;
                raising = false;
                raisedTurn = currentTurn;
                changed = true;
            }
            else
            {
                if (turnCount >= maxLeft - 1 || !changed && turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        Raise = 0;
                        callValue = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!hasPlayerBankrupted)
                            labelPlayerStatus.Text = "";
                        if (!hasBotOneBankrupted)
                            labelBot1Status.Text = "";
                        if (!hasBotTwoBankrupted)
                            labelBot2Status.Text = "";
                        if (!hasBotThreeBankrupted)
                            labelBot3Status.Text = "";
                        if (!hasBotFourBankrupted)
                            labelBot4Status.Text = "";
                        if (!hasBotFiveBankrupted)
                            labelBot5Status.Text = "";
                    }
                }
            }
            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (pictureBoxDeckCards[j].Image != deckCardsImages[j])
                    {
                        pictureBoxDeckCards[j].Image = deckCardsImages[j];
                        playerCall = 0; playerRaise = 0;
                        botOneCall = 0; botOneRaise = 0;
                        botTwoCall = 0; botTwoRaise = 0;
                        botThreeCall = 0; botThreeRaise = 0;
                        botFourCall = 0; botFourRaise = 0;
                        botFiveCall = 0; botFiveRaise = 0;
                    }
                }
            }
            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (pictureBoxDeckCards[j].Image != deckCardsImages[j])
                    {
                        pictureBoxDeckCards[j].Image = deckCardsImages[j];
                        playerCall = 0; playerRaise = 0;
                        botOneCall = 0; botOneRaise = 0;
                        botTwoCall = 0; botTwoRaise = 0;
                        botThreeCall = 0; botThreeRaise = 0;
                        botFourCall = 0; botFourRaise = 0;
                        botFiveCall = 0; botFiveRaise = 0;
                    }
                }
            }
            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (pictureBoxDeckCards[j].Image != deckCardsImages[j])
                    {
                        pictureBoxDeckCards[j].Image = deckCardsImages[j];
                        playerCall = 0; playerRaise = 0;
                        botOneCall = 0; botOneRaise = 0;
                        botTwoCall = 0; botTwoRaise = 0;
                        botThreeCall = 0; botThreeRaise = 0;
                        botFourCall = 0; botFourRaise = 0;
                        botFiveCall = 0; botFiveRaise = 0;
                    }
                }
            }
            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!labelPlayerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref pType, ref pPower, hasPlayerBankrupted);
                }
                if (!labelBot1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref b1Type, ref b1Power, hasBotOneBankrupted);
                }
                if (!labelBot2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref b2Type, ref b2Power, hasBotTwoBankrupted);
                }
                if (!labelBot3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref b3Type, ref b3Power, hasBotThreeBankrupted);
                }
                if (!labelBot4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref b4Type, ref b4Power, hasBotFourBankrupted);
                }
                if (!labelBot5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref b5Type, ref b5Power, hasBotFiveBankrupted);
                }
                Winner(pType, pPower, "Player", playerChipsCount, fixedLast);
                Winner(b1Type, b1Power, "Bot 1", bot1ChipsCount, fixedLast);
                Winner(b2Type, b2Power, "Bot 2", bot2ChipsCount, fixedLast);
                Winner(b3Type, b3Power, "Bot 3", bot3ChipsCount, fixedLast);
                Winner(b4Type, b4Power, "Bot 4", bot4ChipsCount, fixedLast);
                Winner(b5Type, b5Power, "Bot 5", bot5ChipsCount, fixedLast);
                shouldRestart = true;
                isPlayerTurn = true;
                hasPlayerBankrupted = false;
                hasBotOneBankrupted = false;
                hasBotTwoBankrupted = false;
                hasBotThreeBankrupted = false;
                hasBotFourBankrupted = false;
                hasBotFiveBankrupted = false;
                if (playerChipsCount <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        playerChipsCount = f2.a;
                        bot1ChipsCount += f2.a;
                        bot2ChipsCount += f2.a;
                        bot3ChipsCount += f2.a;
                        bot4ChipsCount += f2.a;
                        bot5ChipsCount += f2.a;
                        hasPlayerBankrupted = false;
                        isPlayerTurn = true;
                        buttonRaise.Enabled = true;
                        buttonFold.Enabled = true;
                        buttonCheck.Enabled = true;
                        buttonRaise.Text = "Raise";
                    }
                }
                playerPanel.Visible = false; bot1Panel.Visible = false; bot2Panel.Visible = false; bot3Panel.Visible = false; bot4Panel.Visible = false; bot5Panel.Visible = false;
                playerCall = 0; playerRaise = 0;
                botOneCall = 0; botOneRaise = 0;
                botTwoCall = 0; botTwoRaise = 0;
                botThreeCall = 0; botThreeRaise = 0;
                botFourCall = 0; botFourRaise = 0;
                botFiveCall = 0; botFiveRaise = 0;
                last = 0;
                callValue = bigBlindValue;
                Raise = 0;
                ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bankruptPlayers.Clear();
                rounds = 0;
                pPower = 0; pType = -1;
                type = 0; b1Power = 0; b2Power = 0; b3Power = 0; b4Power = 0; b5Power = 0;
                b1Type = -1; b2Type = -1; b3Type = -1; b4Type = -1; b5Type = -1;
                ints.Clear();
                CheckWinners.Clear();
                winners = 0;
                winningHands.Clear();
                sorted.Current = 0;
                sorted.Power = 0;
                for (int os = 0; os < TableCardsCount; os++)
                {
                    pictureBoxDeckCards[os].Image = null;
                    pictureBoxDeckCards[os].Invalidate();
                    pictureBoxDeckCards[os].Visible = false;
                }
                textBoxPotAmount.Text = "0";
                labelPlayerStatus.Text = "";
                await Shuffle();
                await Turns();
            }
        }
        void FixCall(Label status, ref int cCall, ref int cRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }
                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }
                    if (status.Text.Contains("Check"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cRaise != Raise && cRaise <= Raise)
                    {
                        callValue = Convert.ToInt32(Raise) - cRaise;
                    }
                    if (cCall != callValue || cCall <= callValue)
                    {
                        callValue = callValue - cCall;
                    }
                    if (cRaise == Raise && Raise > 0)
                    {
                        callValue = 0;
                        buttonCall.Enabled = false;
                        buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }
        async Task AllIn()
        {
            #region All in
            if (playerChipsCount <= 0 && !intsadded)
            {
                if (labelPlayerStatus.Text.Contains("Raise"))
                {
                    ints.Add(playerChipsCount);
                    intsadded = true;
                }
                if (labelPlayerStatus.Text.Contains("Call"))
                {
                    ints.Add(playerChipsCount);
                    intsadded = true;
                }
            }
            intsadded = false;
            if (bot1ChipsCount <= 0 && !hasBotOneBankrupted)
            {
                if (!intsadded)
                {
                    ints.Add(bot1ChipsCount);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot2ChipsCount <= 0 && !hasBotTwoBankrupted)
            {
                if (!intsadded)
                {
                    ints.Add(bot2ChipsCount);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot3ChipsCount <= 0 && !hasBotThreeBankrupted)
            {
                if (!intsadded)
                {
                    ints.Add(bot3ChipsCount);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot4ChipsCount <= 0 && !hasBotFourBankrupted)
            {
                if (!intsadded)
                {
                    ints.Add(bot4ChipsCount);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot5ChipsCount <= 0 && !hasBotFiveBankrupted)
            {
                if (!intsadded)
                {
                    ints.Add(bot5ChipsCount);
                    intsadded = true;
                }
            }
            if (ints.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                ints.Clear();
            }
            #endregion

            var abc = bankruptPlayers.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = bankruptPlayers.IndexOf(false);
                if (index == 0)
                {
                    playerChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = playerChipsCount.ToString();
                    playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    bot1ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bot1ChipsCount.ToString();
                    bot1Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    bot2ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bot2ChipsCount.ToString();
                    bot2Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    bot3ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bot3ChipsCount.ToString();
                    bot3Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    bot4ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bot4ChipsCount.ToString();
                    bot4Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    bot5ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bot5ChipsCount.ToString();
                    bot5Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    pictureBoxDeckCards[j].Visible = false;
                }
                await Finish(1);
            }
            intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= End)
            {
                await Finish(2);
            }
            #endregion


        }
        async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }
            playerPanel.Visible = false; bot1Panel.Visible = false; bot2Panel.Visible = false; bot3Panel.Visible = false; bot4Panel.Visible = false; bot5Panel.Visible = false;
            callValue = bigBlindValue; Raise = 0;
            currentBotsPlayingCount = 5;
            type = 0; rounds = 0; b1Power = 0; b2Power = 0; b3Power = 0; b4Power = 0; b5Power = 0; pPower = 0; pType = -1; Raise = 0;
            b1Type = -1; b2Type = -1; b3Type = -1; b4Type = -1; b5Type = -1;
            isBotOneTurn = false; isBotTwoTurn = false; isBotThreeTurn = false; isBotFourTurn = false; isBotFiveTurn = false;
            hasBotOneBankrupted = false; hasBotTwoBankrupted = false; hasBotThreeBankrupted = false; hasBotFourBankrupted = false; hasBotFiveBankrupted = false;
            hasPlayerFolded = false; hasBotOneFolded = false; hasBotTwoFolded = false; hasBotThreeFolded = false; hasBotFourFolded = false; hasBotFiveFolded = false;
            hasPlayerBankrupted = false; isPlayerTurn = true; shouldRestart = false; raising = false;
            playerCall = 0; botOneCall = 0; botTwoCall = 0; botThreeCall = 0; botFourCall = 0; botFiveCall = 0; playerRaise = 0; botOneRaise = 0; botTwoRaise = 0; botThreeRaise = 0; botFourRaise = 0; botFiveRaise = 0;
            //windowHeight = 0; windowWidth = 0; 
            winners = 0; Flop = 1; Turn = 2; River = 3; End = 4; maxLeft = 6;
            last = 123; raisedTurn = 1;
            bankruptPlayers.Clear();
            CheckWinners.Clear();
            ints.Clear();
            winningHands.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            textBoxPotAmount.Text = "0";
            t = 60; up = 10000000; turnCount = 0;
            labelPlayerStatus.Text = "";
            labelBot1Status.Text = "";
            labelBot2Status.Text = "";
            labelBot3Status.Text = "";
            labelBot4Status.Text = "";
            labelBot5Status.Text = "";
            if (playerChipsCount <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    playerChipsCount = f2.a;
                    bot1ChipsCount += f2.a;
                    bot2ChipsCount += f2.a;
                    bot3ChipsCount += f2.a;
                    bot4ChipsCount += f2.a;
                    bot5ChipsCount += f2.a;
                    hasPlayerBankrupted = false;
                    isPlayerTurn = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    buttonCheck.Enabled = true;
                    buttonRaise.Text = "Raise";
                }
            }
            ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < TableCardsCount; os++)
            {
                pictureBoxDeckCards[os].Image = null;
                pictureBoxDeckCards[os].Invalidate();
                pictureBoxDeckCards[os].Visible = false;
            }
            await Shuffle();
            //await Turns();
        }
        void FixWinners()
        {
            winningHands.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!labelPlayerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref pType, ref pPower, hasPlayerBankrupted);
            }
            if (!labelBot1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref b1Type, ref b1Power, hasBotOneBankrupted);
            }
            if (!labelBot2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref b2Type, ref b2Power, hasBotTwoBankrupted);
            }
            if (!labelBot3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref b3Type, ref b3Power, hasBotThreeBankrupted);
            }
            if (!labelBot4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref b4Type, ref b4Power, hasBotFourBankrupted);
            }
            if (!labelBot5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref b5Type, ref b5Power, hasBotFiveBankrupted);
            }
            Winner(pType, pPower, "Player", playerChipsCount, fixedLast);
            Winner(b1Type, b1Power, "Bot 1", bot1ChipsCount, fixedLast);
            Winner(b2Type, b2Power, "Bot 2", bot2ChipsCount, fixedLast);
            Winner(b3Type, b3Power, "Bot 3", bot3ChipsCount, fixedLast);
            Winner(b4Type, b4Power, "Bot 4", bot4ChipsCount, fixedLast);
            Winner(b5Type, b5Power, "Bot 5", bot5ChipsCount, fixedLast);
        }
        void AI(int c1, int c2, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }
            if (sFTurn)
            {
                pictureBoxDeckCards[c1].Visible = false;
                pictureBoxDeckCards[c2].Visible = false;
            }
        }
        private void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 20, 25);
        }
        private void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }
        private void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }
        private void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }
        private void ThreeOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
        }
        private void Straight(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
            if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
        }
        private void Flush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fCall, fRaise);
        }
        private void FullHouse(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
        }
        private void FourOfAKind(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fkCall, fkRaise);
            }
        }
        private void StraightFlush(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sfCall, sfRaise);
            }
        }

        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            raising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }
        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            raising = false;
        }
        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            raising = false;
            sTurn = false;
            sChips -= callValue;
            sStatus.Text = "Call " + callValue;
            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + callValue).ToString();
        }
        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(Raise);
            sStatus.Text = "Raise " + Raise;
            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + Convert.ToInt32(Raise)).ToString();
            callValue = Convert.ToInt32(Raise);
            raising = true;
            sTurn = false;
        }
        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        private void HP(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (callValue <= 0)
            {
                Check(ref sTurn, sStatus);
            }
            if (callValue > 0)
            {
                if (rnd == 1)
                {
                    if (callValue <= RoundN(sChips, n))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
                if (rnd == 2)
                {
                    if (callValue <= RoundN(sChips, n1))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (rnd == 3)
            {
                if (Raise == 0)
                {
                    Raise = callValue * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (Raise <= RoundN(sChips, n))
                    {
                        Raise = callValue * 2;
                        Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }
        private void PH(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (callValue <= 0)
                {
                    Check(ref sTurn, sStatus);
                }
                if (callValue > 0)
                {
                    if (callValue >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (Raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (callValue >= RoundN(sChips, n) && callValue <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n) && Raise >= (RoundN(sChips, n)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= (RoundN(sChips, n)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = callValue * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (callValue > 0)
                {
                    if (callValue >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (Raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                    if (!sFTurn)
                    {
                        if (callValue >= RoundN(sChips, n - rnd) && callValue <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= RoundN(sChips, n - rnd) && Raise >= (RoundN(sChips, n - rnd)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }
                        if (Raise <= (RoundN(sChips, n - rnd)) / 2)
                        {
                            if (Raise > 0)
                            {
                                Raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                Raise = callValue * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
                if (callValue <= 0)
                {
                    Raise = RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }
            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }
        void Smooth(ref int botChips, ref bool botTurn, ref bool botFTurn, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (callValue <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (callValue >= RoundN(botChips, n))
                {
                    if (botChips > callValue)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= callValue)
                    {
                        raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
                        textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (Raise > 0)
                    {
                        if (botChips >= Raise * 2)
                        {
                            Raise *= 2;
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        Raise = callValue * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }
            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }

        #region UI
        private async void timer_Tick(object sender, object e)
        {
            if (progressBarTimer.Value <= 0)
            {
                hasPlayerBankrupted = true;
                await Turns();
            }
            if (t > 0)
            {
                t--;
                progressBarTimer.Value = (t / 6) * 100;
            }
        }
        private void Update_Tick(object sender, object e)
        {
            if (playerChipsCount <= 0)
            {
                textBoxPlayerChips.Text = "Player Chips : 0";
            }
            if (bot1ChipsCount <= 0)
            {
                textBoxChipsBot1.Text = "Bot1 Chips : 0";
            }
            if (bot2ChipsCount <= 0)
            {
                textBoxChipsBot2.Text = "Bot2 Chips : 0";
            }
            if (bot3ChipsCount <= 0)
            {
                textBoxChipsBot3.Text = "Bot3 Chips : 0";
            }
            if (bot4ChipsCount <= 0)
            {
                textBoxChipsBot4.Text = "Bot4 Chips : 0";
            }
            if (bot5ChipsCount <= 0)
            {
                textBoxChipsBot5.Text = "Bot5 Chips : 0";
            }
            textBoxPlayerChips.Text = "Player Chips : " + playerChipsCount.ToString();
            textBoxChipsBot1.Text = "Bot1 Chips : " + bot1ChipsCount.ToString();
            textBoxChipsBot2.Text = "Bot2 Chips : " + bot2ChipsCount.ToString();
            textBoxChipsBot3.Text = "Bot3 Chips : " + bot3ChipsCount.ToString();
            textBoxChipsBot4.Text = "Bot4 Chips : " + bot4ChipsCount.ToString();
            textBoxChipsBot5.Text = "Bot5 Chips : " + bot5ChipsCount.ToString();
            if (playerChipsCount <= 0)
            {
                isPlayerTurn = false;
                hasPlayerBankrupted = true;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                buttonCheck.Enabled = false;
            }
            if (up > 0)
            {
                up--;
            }
            if (playerChipsCount >= callValue)
            {
                buttonCall.Text = "Call " + callValue.ToString();
            }
            else
            {
                buttonCall.Text = "All in";
                buttonRaise.Enabled = false;
            }
            if (callValue > 0)
            {
                buttonCheck.Enabled = false;
            }
            if (callValue <= 0)
            {
                buttonCheck.Enabled = true;
                buttonCall.Text = "Call";
                buttonCall.Enabled = false;
            }
            if (playerChipsCount <= 0)
            {
                buttonRaise.Enabled = false;
            }
            int parsedValue;

            if (textBoxRaiseAmount.Text != "" && int.TryParse(textBoxRaiseAmount.Text, out parsedValue))
            {
                if (playerChipsCount <= int.Parse(textBoxRaiseAmount.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "Raise";
                }
            }
            if (playerChipsCount < callValue)
            {
                buttonRaise.Enabled = false;
            }
        }
        private async void bFold_Click(object sender, EventArgs e)
        {
            labelPlayerStatus.Text = "Fold";
            isPlayerTurn = false;
            hasPlayerBankrupted = true;
            await Turns();
        }
        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (callValue <= 0)
            {
                isPlayerTurn = false;
                labelPlayerStatus.Text = "Check";
            }
            else
            {
                //pStatus.Text = "All in " + Chips;

                buttonCheck.Enabled = false;
            }
            await Turns();
        }
        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref pType, ref pPower, hasPlayerBankrupted);
            if (playerChipsCount >= callValue)
            {
                playerChipsCount -= callValue;
                textBoxPlayerChips.Text = "Chips : " + playerChipsCount.ToString();
                if (textBoxPotAmount.Text != "")
                {
                    textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + callValue).ToString();
                }
                else
                {
                    textBoxPotAmount.Text = callValue.ToString();
                }
                isPlayerTurn = false;
                labelPlayerStatus.Text = "Call " + callValue;
                playerCall = callValue;
            }
            else if (playerChipsCount <= callValue && callValue > 0)
            {
                textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + playerChipsCount).ToString();
                labelPlayerStatus.Text = "All in " + playerChipsCount;
                playerChipsCount = 0;
                textBoxPlayerChips.Text = "Chips : " + playerChipsCount.ToString();
                isPlayerTurn = false;
                buttonFold.Enabled = false;
                playerCall = playerChipsCount;
            }
            await Turns();
        }
        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref pType, ref pPower, hasPlayerBankrupted);
            int parsedValue;
            if (textBoxRaiseAmount.Text != "" && int.TryParse(textBoxRaiseAmount.Text, out parsedValue))
            {
                if (playerChipsCount > callValue)
                {
                    if (Raise * 2 > int.Parse(textBoxRaiseAmount.Text))
                    {
                        textBoxRaiseAmount.Text = (Raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (playerChipsCount >= int.Parse(textBoxRaiseAmount.Text))
                        {
                            callValue = int.Parse(textBoxRaiseAmount.Text);
                            Raise = int.Parse(textBoxRaiseAmount.Text);
                            labelPlayerStatus.Text = "Raise " + callValue.ToString();
                            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + callValue).ToString();
                            buttonCall.Text = "Call";
                            playerChipsCount -= int.Parse(textBoxRaiseAmount.Text);
                            raising = true;
                            last = 0;
                            playerRaise = Convert.ToInt32(Raise);
                        }
                        else
                        {
                            callValue = playerChipsCount;
                            Raise = playerChipsCount;
                            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + playerChipsCount).ToString();
                            labelPlayerStatus.Text = "Raise " + callValue.ToString();
                            playerChipsCount = 0;
                            raising = true;
                            last = 0;
                            playerRaise = Convert.ToInt32(Raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }
            isPlayerTurn = false;
            await Turns();
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAddChips.Text == "") { }
            else
            {
                playerChipsCount += int.Parse(textBoxAddChips.Text);
                bot1ChipsCount += int.Parse(textBoxAddChips.Text);
                bot2ChipsCount += int.Parse(textBoxAddChips.Text);
                bot3ChipsCount += int.Parse(textBoxAddChips.Text);
                bot4ChipsCount += int.Parse(textBoxAddChips.Text);
                bot5ChipsCount += int.Parse(textBoxAddChips.Text);
            }
            textBoxPlayerChips.Text = "Chips : " + playerChipsCount.ToString();
        }
        private void ButtonChangeBlindsValue_Click(object sender, EventArgs e)
        {
            textBoxBigBlind.Text = bigBlindValue.ToString();
            textBoxSmallBlind.Text = smallBlindValue.ToString();
            if (textBoxBigBlind.Visible == false)
            {
                textBoxBigBlind.Visible = true;
                textBoxSmallBlind.Visible = true;
                buttonBigBlind.Visible = true;
                buttonSmallBlind.Visible = true;
            }
            else
            {
                textBoxBigBlind.Visible = false;
                textBoxSmallBlind.Visible = false;
                buttonBigBlind.Visible = false;
                buttonSmallBlind.Visible = false;
            }
        }
        private void bSB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (textBoxSmallBlind.Text.Contains(",") || textBoxSmallBlind.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                textBoxSmallBlind.Text = smallBlindValue.ToString();
                return;
            }
            if (!int.TryParse(textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                textBoxSmallBlind.Text = smallBlindValue.ToString();
                return;
            }
            if (int.Parse(textBoxSmallBlind.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                textBoxSmallBlind.Text = smallBlindValue.ToString();
            }
            if (int.Parse(textBoxSmallBlind.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }
            if (int.Parse(textBoxSmallBlind.Text) >= 250 && int.Parse(textBoxSmallBlind.Text) <= 100000)
            {
                smallBlindValue = int.Parse(textBoxSmallBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void bBB_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (textBoxBigBlind.Text.Contains(",") || textBoxBigBlind.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                textBoxBigBlind.Text = bigBlindValue.ToString();
                return;
            }
            if (!int.TryParse(textBoxSmallBlind.Text, out parsedValue))
            {
                MessageBox.Show("This is a number only field");
                textBoxSmallBlind.Text = bigBlindValue.ToString();
                return;
            }
            if (int.Parse(textBoxBigBlind.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                textBoxBigBlind.Text = bigBlindValue.ToString();
            }
            if (int.Parse(textBoxBigBlind.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }
            if (int.Parse(textBoxBigBlind.Text) >= 500 && int.Parse(textBoxBigBlind.Text) <= 200000)
            {
                bigBlindValue = int.Parse(textBoxBigBlind.Text);
                MessageBox.Show("The changes have been saved ! They will become available the next hand you play. ");
            }
        }
        private void Layout_Change(object sender, LayoutEventArgs e)
        {
            windowWidth = this.Width;
            windowHeight = this.Height;
        }
        #endregion

        private void InitializePlayer()
        {
            this.playerChipsCount = DefaultChipsCount;
            this.playerCall = 0;
            this.playerRaise = 0;
            this.hasPlayerBankrupted = false;
            this.isPlayerTurn = true;
            this.shouldRestart = false;
        }
        
        private void IntializeBots()
        {
            this.currentBotsPlayingCount = InitialNumberOfBots;
            this.bot1ChipsCount = DefaultChipsCount;
            this.bot2ChipsCount = DefaultChipsCount;
            this.bot3ChipsCount = DefaultChipsCount;
            this.bot4ChipsCount = DefaultChipsCount;
            this.bot5ChipsCount = DefaultChipsCount;
            this.isBotOneTurn = false;
            this.isBotTwoTurn = false;
            this.isBotThreeTurn = false;
            this.isBotFourTurn = false;
            this.isBotFiveTurn = false;
            this.hasBotOneBankrupted = false;
            this.hasBotTwoBankrupted = false; 
            this.hasBotThreeBankrupted = false;
            this.hasBotFourBankrupted = false;
            this.hasBotFiveBankrupted = false;
            this.botOneCall = 0; 
            this.botTwoCall = 0;
            this.botThreeCall = 0;
            this.botFourCall = 0;
            this.botFiveCall = 0;
            this.botOneRaise = 0;
            this.botTwoRaise = 0;
            this.botThreeRaise = 0;
            this.botFourRaise = 0;
            this.botFiveRaise = 0;
        }

        private void DisableTextBoxesUserInteraction()
        {
            this.textBoxPotAmount.Enabled = false;
            this.textBoxPlayerChips.Enabled = false;
            this.textBoxChipsBot1.Enabled = false;
            this.textBoxChipsBot2.Enabled = false;
            this.textBoxChipsBot3.Enabled = false;
            this.textBoxChipsBot4.Enabled = false;
            this.textBoxChipsBot5.Enabled = false;
        }

        private void InitializeTextBoxes()
        {
            this.textBoxPlayerChips.Text = "Player Chips : " + playerChipsCount.ToString();
            this.textBoxChipsBot1.Text = "Bot1 Chips : " + bot1ChipsCount.ToString();
            this.textBoxChipsBot2.Text = "Bot2 Chips : " + bot2ChipsCount.ToString();
            this.textBoxChipsBot3.Text = "Bot3 Chips : " + bot3ChipsCount.ToString();
            this.textBoxChipsBot4.Text = "Bot4 Chips : " + bot4ChipsCount.ToString();
            this.textBoxChipsBot5.Text = "Bot5 Chips : " + bot5ChipsCount.ToString();
            this.textBoxRaiseAmount.Text = (bigBlindValue * 2).ToString();
        }

        private void SetBlindButtonsVisibilityToFalse()
        {
            textBoxBigBlind.Visible = false;
            textBoxSmallBlind.Visible = false;
            buttonBigBlind.Visible = false;
            buttonSmallBlind.Visible = false;
        }
    }
}