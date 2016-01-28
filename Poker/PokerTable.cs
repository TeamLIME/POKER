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
using Poker.GameObjects;

namespace Poker
{
    /// <summary>
    /// Holds the main logic behind the Poker Game (e.g. shuffling and dealing cards, computing hand strength/value, 
    /// determining winner as well as user interaction).
    /// </summary>
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

        private HumanPlayer player = new HumanPlayer();

        private List<BotPlayer> bots = new List<BotPlayer>()
        {
            new BotPlayer(),
            new BotPlayer(),
            new BotPlayer(),
            new BotPlayer(),
            new BotPlayer()
        };

        private int callValue;
        private int currentBotsPlayingCount;
        private int bigBlindValue;
        private int smallBlindValue;

        private int windowWidth;
        private int windowHeight;
        private int winners;

        private double type;
        private double rounds = 0;
        private double bot1Power;
        private double bot2Power;
        private double bot3Power;
        private double b4Power;
        private double bot5Power;
        private double bot1Type = -1;
        private double bot2Type = -1;
        private double bot3Type = -1;
        private double bot4Type = -1;
        private double bot5Type = -1;
        
        private double playerPower = 0;
        private double playerType = -1;
        
        private double raise = 0;
        private int flop = 1;
        private int turn = 2;
        private int river = 3;
        private int end = 4;
        private int raisedTurn = 1;
        
        private bool areChipsAdded;
        private bool changed;
        private bool raising = false;
        
        private int maxLeft = 6;
        private int last = 0;
        
        private List<string> checkWinners = new List<string>();
        private List<int> totalAllInChips = new List<int>();
        
        private Type sortedHand;
        private string[] ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
        private int[] cardsOnTable = new int[TableCardsCount];
        private int t = 60;
        private int i;
        private int maxChipsCount = 10000000;
        private int turnCount = 0;
        #endregion

        public PokerTable()
        {
            this.bankruptPlayers = new List<bool?>();
            this.winningHands = new List<Type>();
            this.deckCardsImages = new Image[NumberOfCardsInDeck];
            this.pictureBoxDeckCards = new PictureBox[NumberOfCardsInDeck];
            this.winners = 0;
            
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

            this.InitializeComponent(); //Definition in the designer partial class.
            this.windowWidth = this.Width;
            this.windowHeight = this.Height;
            
            this.InitializePlayer();
            this.IntializeBots();
            this.InitializeTextBoxes();

            this.DisableTextBoxesUserInteraction();
            this.SetBlindButtonsVisibilityToFalse();

            this.DealCardsAsync();
        }
        
        private async Task DealCardsAsync()
        {
            this.InitializeBankruptPlayersList();

            this.DisableButtonsDuringShuffle();
            
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            bool check = false;
            Bitmap backImage = new Bitmap("Assets\\Back\\Back.png");
            int horizontal = 580;
            int vertical = 480;

            this.ShuffleCards();

            for (i = 0; i < TableCardsCount; i++)
            {

                this.InitializeCardsOnTable();

                this.InitializePictureBoxes();
                
                await Task.Delay(200);

                ThrowCards(ref check, backImage, ref horizontal, ref vertical);

                this.HasBankrupted(this.bots[0], 2, 3);
                this.HasBankrupted(this.bots[1], 4, 5);
                this.HasBankrupted(this.bots[2], 6, 7);
                this.HasBankrupted(this.bots[3], 8, 9);
                this.HasBankrupted(this.bots[4], 10, 11);

                if (i == 16) //Last table card is dealt out.
                {
                    if (!player.ShouldRestart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }

                    timer.Start();
                }
            }

            if (currentBotsPlayingCount == 5)
            {
                DialogResult dialogResult = MessageBox.Show("Would You Like To Play Again ?", 
                    "You Won , Congratulations ! ", MessageBoxButtons.YesNo);
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

            if (i == TableCardsCount) //All cards are dealt out.
            {
                this.EnableButtons();
            }
        }

        private void HasBankrupted(BotPlayer bot, int cardOneIndex, int cadrTwoIndex)
        {
            if (bot.ChipsCount <= 0)
            {
                bot.HasBankrupted = true;
                this.pictureBoxDeckCards[cardOneIndex].Visible = false;
                this.pictureBoxDeckCards[cadrTwoIndex].Visible = false;
            }
            else
            {
                bot.HasBankrupted = false;
                if (i == cadrTwoIndex)
                {
                    if (this.pictureBoxDeckCards[cadrTwoIndex] != null)
                    {
                        this.pictureBoxDeckCards[cardOneIndex].Visible = true;
                        this.pictureBoxDeckCards[cadrTwoIndex].Visible = true;
                    }
                }
            }
        }

        private void ThrowCards(ref bool check, Bitmap backImage, ref int horizontal, ref int vertical)
        {
            if (i < 2) //Player's cards.
            {
                if (this.pictureBoxDeckCards[0].Tag != null)
                {
                    this.pictureBoxDeckCards[1].Tag = this.cardsOnTable[1];
                }

                this.pictureBoxDeckCards[0].Tag = this.cardsOnTable[0];
                this.pictureBoxDeckCards[i].Image = this.deckCardsImages[i];
                this.pictureBoxDeckCards[i].Anchor = (AnchorStyles.Bottom);
                this.pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                horizontal += pictureBoxDeckCards[i].Width;
                this.Controls.Add(playerPanel);
                this.playerPanel.Location = new Point(pictureBoxDeckCards[0].Left - 10, pictureBoxDeckCards[0].Top - 10);
                this.playerPanel.BackColor = Color.DarkBlue;
                this.playerPanel.Height = 150;
                this.playerPanel.Width = 180;
                this.playerPanel.Visible = false;
            }

            this.BotsThrowingCards(this.bots[0], ref check, backImage, ref horizontal, ref vertical, this.bot1Panel, 15, 420, 2, 3);

            this.BotsThrowingCards(this.bots[1], ref check, backImage, ref horizontal, ref vertical, this.bot2Panel, 75, 65, 4, 5);

            this.BotsThrowingCards(this.bots[2], ref check, backImage, ref horizontal, ref vertical, this.bot3Panel, 590, 25, 6, 7);

            this.BotsThrowingCards(this.bots[3], ref check, backImage, ref horizontal, ref vertical, this.bot4Panel, 1115, 65, 8, 9);

            this.BotsThrowingCards(this.bots[4], ref check, backImage, ref horizontal, ref vertical, this.bot5Panel, 1160, 420, 10, 11);
            
            if (i >= 12) //The five common cards on the table.
            {
                this.pictureBoxDeckCards[12].Tag = this.cardsOnTable[12];
                if (i > 12)
                {
                    this.pictureBoxDeckCards[13].Tag = this.cardsOnTable[13];
                }

                if (i > 13)
                {
                    this.pictureBoxDeckCards[14].Tag = this.cardsOnTable[14];
                }

                if (i > 14)
                {
                    this.pictureBoxDeckCards[15].Tag = this.cardsOnTable[15];
                }

                if (i > 15)
                {
                    this.pictureBoxDeckCards[16].Tag = this.cardsOnTable[16];

                }

                if (!check)
                {
                    horizontal = 410;
                    vertical = 265;
                }

                check = true;
                if (this.pictureBoxDeckCards[i] != null)
                {
                    this.pictureBoxDeckCards[i].Anchor = AnchorStyles.None;
                    this.pictureBoxDeckCards[i].Image = backImage;
                    this.pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                    horizontal += 110;
                }
            }
        }

        private void BotsThrowingCards
            (BotPlayer bot,
            ref bool check,
            Bitmap backImage,
            ref int horizontal,
            ref int vertical,
            Panel panel,
            int horizontalValue,
            int verticalValue,
            int cardOneIndex,
            int cardTwoIndex)
        {
            if (bot.ChipsCount > 0)
            {
                this.currentBotsPlayingCount--;
                if (i >= cardOneIndex && i <= cardTwoIndex)
                {
                    if (this.pictureBoxDeckCards[cardOneIndex].Tag != null)
                    {
                        this.pictureBoxDeckCards[cardTwoIndex].Tag = this.cardsOnTable[cardTwoIndex];
                    }


                    this.pictureBoxDeckCards[cardOneIndex].Tag = this.cardsOnTable[cardOneIndex];
                    if (!check)
                    {
                        horizontal = horizontalValue;
                        vertical = verticalValue;
                    }

                    check = true;
                    this.pictureBoxDeckCards[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                    this.pictureBoxDeckCards[i].Image = backImage;
                    this.pictureBoxDeckCards[i].Location = new Point(horizontal, vertical);
                    horizontal += pictureBoxDeckCards[i].Width;
                    this.pictureBoxDeckCards[i].Visible = true;
                    this.Controls.Add(panel);
                    
                    panel.Location = new Point(pictureBoxDeckCards[cardOneIndex].Left - 10, pictureBoxDeckCards[cardOneIndex].Top - 10);
                    panel.BackColor = Color.DarkBlue;
                    panel.Height = 150;
                    panel.Width = 180;
                    panel.Visible = false;
                    
                    if (i == cardTwoIndex)
                    {
                        check = false;
                    }
                }
            }
        }

        private async Task TurnsAsync()
        {
            #region Rotating
            if (!this.player.HasBankrupted)
            {
                if (player.IsOnTurn)
                {
                    FixCall(labelPlayerStatus, player, 1);
                    //MessageBox.Show("Player's Turn");
                    progressBarTimer.Visible = true;
                    progressBarTimer.Value = 1000;
                    t = 60;
                    maxChipsCount = 10000000;
                    timer.Start();
                    buttonRaise.Enabled = true;
                    buttonCall.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonRaise.Enabled = true;
                    buttonFold.Enabled = true;
                    turnCount++;
                    FixCall(labelPlayerStatus, player, 2);
                }
            }
            if (player.HasBankrupted || !player.IsOnTurn)
            {
                await AllIn();
                if (player.HasBankrupted && !player.HasFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bankruptPlayers.RemoveAt(0);
                        bankruptPlayers.Insert(0, null);
                        maxLeft--;
                        player.HasFolded = true;
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
                bots[0].IsOnTurn = true;
                if (!bots[0].HasBankrupted)
                {
                    if (bots[0].IsOnTurn)
                    {
                        FixCall(labelBot1Status, bots[0], 1);
                        FixCall(labelBot1Status, bots[0], 2);
                        Rules(2, 3, "Bot 1", ref bot1Type, ref bot1Power, bots[0].HasBankrupted);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, bots[0], labelBot1Status, 0, bot1Power, bot1Type);
                        turnCount++;
                        last = 1;
                        bots[0].IsOnTurn = false;
                        bots[1].IsOnTurn = true;
                    }
                }
                if (bots[0].HasBankrupted && !bots[0].HasFolded)
                {
                    bankruptPlayers.RemoveAt(1);
                    bankruptPlayers.Insert(1, null);
                    maxLeft--;
                    bots[0].HasFolded = true;
                }
                if (bots[0].HasBankrupted || !bots[0].IsOnTurn)
                {
                    await CheckRaise(1, 1);
                    bots[1].IsOnTurn = true;
                }
                if (!bots[1].HasBankrupted)
                {
                    if (bots[1].IsOnTurn)
                    {
                        FixCall(labelBot2Status, bots[1], 1);
                        FixCall(labelBot2Status, bots[1], 2);
                        Rules(4, 5, "Bot 2", ref bot2Type, ref bot2Power, bots[1].HasBankrupted);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, bots[1], labelBot2Status, 1, bot2Power, bot2Type);
                        turnCount++;
                        last = 2;
                        bots[1].IsOnTurn = false;
                        bots[2].IsOnTurn = true;
                    }
                }
                if (bots[1].HasBankrupted && !bots[1].HasFolded)
                {
                    bankruptPlayers.RemoveAt(2);
                    bankruptPlayers.Insert(2, null);
                    maxLeft--;
                    bots[1].HasFolded = true;
                }
                if (bots[1].HasBankrupted || !bots[1].IsOnTurn)
                {
                    await CheckRaise(2, 2);
                    bots[2].IsOnTurn = true;
                }
                if (!bots[2].HasBankrupted)
                {
                    if (bots[2].IsOnTurn)
                    {
                        FixCall(labelBot3Status, bots[2], 1);
                        FixCall(labelBot3Status, bots[2], 2);
                        Rules(6, 7, "Bot 3", ref bot3Type, ref bot3Power, bots[2].HasBankrupted);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, bots[2], labelBot3Status, 2, bot3Power, bot3Type);
                        turnCount++;
                        last = 3;
                        bots[2].IsOnTurn = false;
                        bots[3].IsOnTurn = true;
                    }
                }
                if (bots[2].HasBankrupted && !bots[2].HasFolded)
                {
                    bankruptPlayers.RemoveAt(3);
                    bankruptPlayers.Insert(3, null);
                    maxLeft--;
                    bots[2].HasFolded = true;
                }
                if (bots[2].HasBankrupted || !bots[2].IsOnTurn)
                {
                    await CheckRaise(3, 3);
                    bots[3].IsOnTurn = true;
                }
                if (!bots[3].HasBankrupted)
                {
                    if (bots[3].IsOnTurn)
                    {
                        FixCall(labelBot4Status, bots[3], 1);
                        FixCall(labelBot4Status, bots[3], 2);
                        Rules(8, 9, "Bot 4", ref bot4Type, ref b4Power, bots[3].HasBankrupted);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, bots[3], labelBot4Status, 3, b4Power, bot4Type);
                        turnCount++;
                        last = 4;
                        bots[3].IsOnTurn = false;
                        bots[4].IsOnTurn = true;
                    }
                }
                if (bots[3].HasBankrupted && !bots[3].HasFolded)
                {
                    bankruptPlayers.RemoveAt(4);
                    bankruptPlayers.Insert(4, null);
                    maxLeft--;
                    bots[3].HasFolded = true;
                }
                if (bots[3].HasBankrupted || !bots[3].IsOnTurn)
                {
                    await CheckRaise(4, 4);
                    bots[4].IsOnTurn = true;
                }
                if (!bots[4].HasBankrupted)
                {
                    if (bots[4].IsOnTurn)
                    {
                        FixCall(labelBot5Status, bots[4], 1);
                        FixCall(labelBot5Status, bots[4], 2);
                        Rules(10, 11, "Bot 5", ref bot5Type, ref bot5Power, bots[4].HasBankrupted);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, bots[4], labelBot5Status, 4, bot5Power, bot5Type);
                        turnCount++;
                        last = 5;
                        bots[4].IsOnTurn = false;
                    }
                }
                if (bots[4].HasBankrupted && !bots[4].HasFolded)
                {
                    bankruptPlayers.RemoveAt(5);
                    bankruptPlayers.Insert(5, null);
                    maxLeft--;
                    bots[4].HasFolded = true;
                }
                if (bots[4].HasBankrupted || !bots[4].IsOnTurn)
                {
                    await CheckRaise(5, 5);
                    player.IsOnTurn = true;
                }
                if (player.HasBankrupted && !player.HasFolded)
                {
                    if (buttonCall.Text.Contains("All in") == false || buttonRaise.Text.Contains("All in") == false)
                    {
                        bankruptPlayers.RemoveAt(0);
                        bankruptPlayers.Insert(0, null);
                        maxLeft--;
                        player.HasFolded = true;
                    }
                }
                #endregion
                await AllIn();
                if (!player.ShouldRestart)
                {
                    await TurnsAsync();
                }
                player.ShouldRestart = false;
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
                Straight[0] = cardsOnTable[c1];
                Straight[1] = cardsOnTable[c2];
                Straight1[0] = Straight[2] = cardsOnTable[12];
                Straight1[1] = Straight[3] = cardsOnTable[13];
                Straight1[2] = Straight[4] = cardsOnTable[14];
                Straight1[3] = Straight[5] = cardsOnTable[15];
                Straight1[4] = Straight[6] = cardsOnTable[16];
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
                    if (cardsOnTable[i] == int.Parse(pictureBoxDeckCards[c1].Tag.ToString()) && cardsOnTable[i + 1] == int.Parse(pictureBoxDeckCards[c2].Tag.ToString()))
                    {
                        //Pair from Hand current = 1

                        WinningHands.rPairFromHand(ref current, ref Power, winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rPairFromHand(ref current, ref Power);

                        #region Pair or Two Pair from Table current = 2 || 0
                        WinningHands.rPairTwoPair(ref current, ref Power, winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rPairTwoPair(ref current, ref Power);
                        #endregion

                        #region Two Pair current = 2
                        WinningHands.rTwoPair(ref current, ref Power, winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rTwoPair(ref current, ref Power);
                        #endregion

                        #region Three of a kind current = 3
                        WinningHands.rThreeOfAKind(ref current, ref Power, Straight, winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rThreeOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight current = 4
                        WinningHands.rStraight(ref current, ref Power, Straight, winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rStraight(ref current, ref Power, Straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        WinningHands.rFlush(ref current, ref Power, ref vf, Straight1,
                            winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rFlush(ref current, ref Power, ref vf, Straight1);
                        #endregion

                        #region Full House current = 6
                        WinningHands.rFullHouse(ref current, ref Power, ref done, Straight,
                            winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rFullHouse(ref current, ref Power, ref done, Straight);
                        #endregion

                        #region Four of a Kind current = 7
                        WinningHands.rFourOfAKind(ref current, ref Power, Straight,
                            winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rFourOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        WinningHands.rStraightFlush(ref current, ref Power, st1, st2, st3, st4,
                            winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rStraightFlush(ref current, ref Power, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        WinningHands.rHighCard(ref current, ref Power,
                            winningHands, sortedHand, ref type, ref i, cardsOnTable);
                        //rHighCard(ref current, ref Power);
                        #endregion
                    }
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
            if (current == sortedHand.Current)
            {
                if (Power == sortedHand.Power)
                {
                    winners++;
                    checkWinners.Add(currentText);
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
                    if (checkWinners.Contains("Player"))
                    {
                        player.ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxPlayerChips.Text = player.ChipsCount.ToString();
                        //playerPanel.Visible = true;

                    }
                    if (checkWinners.Contains("Bot 1"))
                    {
                        bots[0].ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot1.Text = bots[0].ChipsCount.ToString();
                        //bot1Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 2"))
                    {
                        bots[1].ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot2.Text = bots[1].ChipsCount.ToString();
                        //bot2Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 3"))
                    {
                        bots[2].ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot3.Text = bots[2].ChipsCount.ToString();
                        //bot3Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 4"))
                    {
                        bots[3].ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot4.Text = bots[3].ChipsCount.ToString();
                        //bot4Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 5"))
                    {
                        bots[4].ChipsCount += int.Parse(textBoxPotAmount.Text) / winners;
                        textBoxChipsBot5.Text = bots[4].ChipsCount.ToString();
                        //bot5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (checkWinners.Contains("Player"))
                    {
                        player.ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 1"))
                    {
                        bots[0].ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot1Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 2"))
                    {
                        bots[1].ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot2Panel.Visible = true;

                    }
                    if (checkWinners.Contains("Bot 3"))
                    {
                        bots[2].ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot3Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 4"))
                    {
                        bots[3].ChipsCount += int.Parse(textBoxPotAmount.Text);
                        //await Finish(1);
                        //bot4Panel.Visible = true;
                    }
                    if (checkWinners.Contains("Bot 5"))
                    {
                        bots[4].ChipsCount += int.Parse(textBoxPotAmount.Text);
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
                        this.raise = 0;
                        callValue = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!player.HasBankrupted)
                            labelPlayerStatus.Text = "";
                        if (!bots[0].HasBankrupted)
                            labelBot1Status.Text = "";
                        if (!bots[1].HasBankrupted)
                            labelBot2Status.Text = "";
                        if (!bots[2].HasBankrupted)
                            labelBot3Status.Text = "";
                        if (!bots[3].HasBankrupted)
                            labelBot4Status.Text = "";
                        if (!bots[4].HasBankrupted)
                            labelBot5Status.Text = "";
                    }
                }
            }
            if (rounds == flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (pictureBoxDeckCards[j].Image != deckCardsImages[j])
                    {
                        pictureBoxDeckCards[j].Image = deckCardsImages[j];
                        player.Call = 0; player.Raise = 0;
                        bots[0].Call = 0; bots[0].Raise = 0;
                        bots[1].Call = 0; bots[1].Raise = 0;
                        bots[2].Call = 0; bots[2].Raise = 0;
                        bots[3].Call = 0; bots[3].Raise = 0;
                        bots[4].Call = 0; bots[4].Raise = 0;
                    }
                }
            }
            if (rounds == turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (pictureBoxDeckCards[j].Image != deckCardsImages[j])
                    {
                        pictureBoxDeckCards[j].Image = deckCardsImages[j];
                        player.Call = 0; player.Raise = 0;
                        bots[0].Call = 0; bots[0].Raise = 0;
                        bots[1].Call = 0; bots[1].Raise = 0;
                        bots[2].Call = 0; bots[2].Raise = 0;
                        bots[3].Call = 0; bots[3].Raise = 0;
                        bots[4].Call = 0; bots[4].Raise = 0;
                    }
                }
            }
            if (rounds == river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (pictureBoxDeckCards[j].Image != deckCardsImages[j])
                    {
                        pictureBoxDeckCards[j].Image = deckCardsImages[j];
                        player.Call = 0; player.Raise = 0;
                        bots[0].Call = 0; bots[0].Raise = 0;
                        bots[1].Call = 0; bots[1].Raise = 0;
                        bots[2].Call = 0; bots[2].Raise = 0;
                        bots[3].Call = 0; bots[3].Raise = 0;
                        bots[4].Call = 0; bots[4].Raise = 0;
                    }
                }
            }
            if (rounds == end && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!labelPlayerStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref playerType, ref playerPower, player.HasBankrupted);
                }
                if (!labelBot1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref bot1Type, ref bot1Power, bots[0].HasBankrupted);
                }
                if (!labelBot2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref bot2Type, ref bot2Power, bots[1].HasBankrupted);
                }
                if (!labelBot3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref bot3Type, ref bot3Power, bots[2].HasBankrupted);
                }
                if (!labelBot4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref bot4Type, ref b4Power, bots[3].HasBankrupted);
                }
                if (!labelBot5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref bot5Type, ref bot5Power, bots[4].HasBankrupted);
                }
                Winner(playerType, playerPower, "Player", player.ChipsCount, fixedLast);
                Winner(bot1Type, bot1Power, "Bot 1", bots[0].ChipsCount, fixedLast);
                Winner(bot2Type, bot2Power, "Bot 2", bots[1].ChipsCount, fixedLast);
                Winner(bot3Type, bot3Power, "Bot 3", bots[2].ChipsCount, fixedLast);
                Winner(bot4Type, b4Power, "Bot 4", bots[3].ChipsCount, fixedLast);
                Winner(bot5Type, bot5Power, "Bot 5", bots[4].ChipsCount, fixedLast);
                player.ShouldRestart = true;
                player.IsOnTurn = true;
                player.HasBankrupted = false;
                bots[0].HasBankrupted = false;
                bots[1].HasBankrupted = false;
                bots[2].HasBankrupted = false;
                bots[3].HasBankrupted = false;
                bots[4].HasBankrupted = false;
                if (player.ChipsCount <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        player.ChipsCount = f2.a;
                        bots[0].ChipsCount += f2.a;
                        bots[1].ChipsCount += f2.a;
                        bots[2].ChipsCount += f2.a;
                        bots[3].ChipsCount += f2.a;
                        bots[4].ChipsCount += f2.a;
                        player.HasBankrupted = false;
                        player.IsOnTurn = true;
                        buttonRaise.Enabled = true;
                        buttonFold.Enabled = true;
                        buttonCheck.Enabled = true;
                        buttonRaise.Text = "Raise";
                    }
                }
                playerPanel.Visible = false; bot1Panel.Visible = false; bot2Panel.Visible = false; bot3Panel.Visible = false; bot4Panel.Visible = false; bot5Panel.Visible = false;
                player.Call = 0; player.Raise = 0;
                bots[0].Call = 0; bots[0].Raise = 0;
                bots[1].Call = 0; bots[1].Raise = 0;
                bots[2].Call = 0; bots[2].Raise = 0;
                bots[3].Call = 0; bots[3].Raise = 0;
                bots[4].Call = 0; bots[4].Raise = 0;
                last = 0;
                callValue = bigBlindValue;
                raise = 0;
                ImgLocation = Directory.GetFiles("Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                bankruptPlayers.Clear();
                rounds = 0;
                playerPower = 0; playerType = -1;
                type = 0; bot1Power = 0; bot2Power = 0; bot3Power = 0; b4Power = 0; bot5Power = 0;
                bot1Type = -1; bot2Type = -1; bot3Type = -1; bot4Type = -1; bot5Type = -1;
                totalAllInChips.Clear();
                checkWinners.Clear();
                winners = 0;
                winningHands.Clear();
                sortedHand.Current = 0;
                sortedHand.Power = 0;
                for (int os = 0; os < TableCardsCount; os++)
                {
                    pictureBoxDeckCards[os].Image = null;
                    pictureBoxDeckCards[os].Invalidate();
                    pictureBoxDeckCards[os].Visible = false;
                }
                textBoxPotAmount.Text = "0";
                labelPlayerStatus.Text = "";
                await DealCardsAsync();
                await TurnsAsync();
            }
        }
        //FixCall(labelPlayerStatus, ref player.Call, ref player.Raise, 1);
        void FixCall(Label status, Player player, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        player.Raise = int.Parse(changeRaise);
                    }
                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        player.Call = int.Parse(changeCall);
                    }
                    if (status.Text.Contains("Check"))
                    {
                        player.Raise = 0;
                        player.Call = 0;
                    }
                }
                if (options == 2)
                {
                    if (player.Raise != raise && player.Raise <= raise)
                    {
                        callValue = Convert.ToInt32(raise) - player.Raise;
                    }
                    if (player.Call != callValue || player.Call <= callValue)
                    {
                        callValue = callValue - player.Call;
                    }
                    if (player.Raise == raise && raise > 0)
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
            if (player.ChipsCount <= 0 && !areChipsAdded)
            {
                if (labelPlayerStatus.Text.Contains("Raise"))
                {
                    totalAllInChips.Add(player.ChipsCount);
                    areChipsAdded = true;
                }
                if (labelPlayerStatus.Text.Contains("Call"))
                {
                    totalAllInChips.Add(player.ChipsCount);
                    areChipsAdded = true;
                }
            }
            areChipsAdded = false;
            if (bots[0].ChipsCount <= 0 && !bots[0].HasBankrupted)
            {
                if (!areChipsAdded)
                {
                    totalAllInChips.Add(bots[0].ChipsCount);
                    areChipsAdded = true;
                }
                areChipsAdded = false;
            }
            if (bots[1].ChipsCount <= 0 && !bots[1].HasBankrupted)
            {
                if (!areChipsAdded)
                {
                    totalAllInChips.Add(bots[1].ChipsCount);
                    areChipsAdded = true;
                }
                areChipsAdded = false;
            }
            if (bots[2].ChipsCount <= 0 && !bots[2].HasBankrupted)
            {
                if (!areChipsAdded)
                {
                    totalAllInChips.Add(bots[2].ChipsCount);
                    areChipsAdded = true;
                }
                areChipsAdded = false;
            }
            if (bots[3].ChipsCount <= 0 && !bots[3].HasBankrupted)
            {
                if (!areChipsAdded)
                {
                    totalAllInChips.Add(bots[3].ChipsCount);
                    areChipsAdded = true;
                }
                areChipsAdded = false;
            }
            if (bots[4].ChipsCount <= 0 && !bots[4].HasBankrupted)
            {
                if (!areChipsAdded)
                {
                    totalAllInChips.Add(bots[4].ChipsCount);
                    areChipsAdded = true;
                }
            }
            if (totalAllInChips.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                totalAllInChips.Clear();
            }
            #endregion

            var abc = bankruptPlayers.Count(x => x == false);

            #region LastManStanding
            if (abc == 1)
            {
                int index = bankruptPlayers.IndexOf(false);
                if (index == 0)
                {
                    player.ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = player.ChipsCount.ToString();
                    playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    bots[0].ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bots[0].ChipsCount.ToString();
                    bot1Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    bots[1].ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bots[1].ChipsCount.ToString();
                    bot2Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    bots[2].ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bots[2].ChipsCount.ToString();
                    bot3Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    bots[3].ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bots[3].ChipsCount.ToString();
                    bot4Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    bots[4].ChipsCount += int.Parse(textBoxPotAmount.Text);
                    textBoxPlayerChips.Text = bots[4].ChipsCount.ToString();
                    bot5Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    pictureBoxDeckCards[j].Visible = false;
                }
                await Finish(1);
            }
            areChipsAdded = false;
            #endregion

            #region FiveOrLessLeft
            if (abc < 6 && abc > 1 && rounds >= end)
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
            callValue = bigBlindValue; raise = 0;
            currentBotsPlayingCount = 5;
            type = 0; rounds = 0; bot1Power = 0; bot2Power = 0; bot3Power = 0; b4Power = 0; bot5Power = 0; playerPower = 0; playerType = -1; raise = 0;
            bot1Type = -1; bot2Type = -1; bot3Type = -1; bot4Type = -1; bot5Type = -1;
            bots[0].IsOnTurn = false; bots[1].IsOnTurn = false; bots[2].IsOnTurn = false; bots[3].IsOnTurn = false; bots[4].IsOnTurn = false;
            bots[0].HasBankrupted = false; bots[1].HasBankrupted = false; bots[2].HasBankrupted = false; bots[3].HasBankrupted = false; bots[4].HasBankrupted = false;
            player.HasFolded = false; bots[0].HasFolded = false; bots[1].HasFolded = false; bots[2].HasFolded = false; bots[3].HasFolded = false; bots[4].HasFolded = false;
            player.HasBankrupted = false; player.IsOnTurn = true; player.ShouldRestart = false; raising = false;
            player.Call = 0; bots[0].Call = 0; bots[1].Call = 0; bots[2].Call = 0; bots[3].Call = 0; bots[4].Call = 0; player.Raise = 0; bots[0].Raise = 0; bots[1].Raise = 0; bots[2].Raise = 0; bots[3].Raise = 0; bots[4].Raise = 0;
            //windowHeight = 0; windowWidth = 0; 
            winners = 0; flop = 1; turn = 2; river = 3; end = 4; maxLeft = 6;
            last = 123; raisedTurn = 1;
            bankruptPlayers.Clear();
            checkWinners.Clear();
            totalAllInChips.Clear();
            winningHands.Clear();
            sortedHand.Current = 0;
            sortedHand.Power = 0;
            textBoxPotAmount.Text = "0";
            t = 60; maxChipsCount = 10000000; turnCount = 0;
            labelPlayerStatus.Text = "";
            labelBot1Status.Text = "";
            labelBot2Status.Text = "";
            labelBot3Status.Text = "";
            labelBot4Status.Text = "";
            labelBot5Status.Text = "";
            if (player.ChipsCount <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    player.ChipsCount = f2.a;
                    bots[0].ChipsCount += f2.a;
                    bots[1].ChipsCount += f2.a;
                    bots[2].ChipsCount += f2.a;
                    bots[3].ChipsCount += f2.a;
                    bots[4].ChipsCount += f2.a;
                    player.HasBankrupted = false;
                    player.IsOnTurn = true;
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
            await DealCardsAsync();
            //await Turns();
        }
        void FixWinners()
        {
            winningHands.Clear();
            sortedHand.Current = 0;
            sortedHand.Power = 0;
            string fixedLast = "qwerty";
            if (!labelPlayerStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref playerType, ref playerPower, player.HasBankrupted);
            }
            if (!labelBot1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref bot1Type, ref bot1Power, bots[0].HasBankrupted);
            }
            if (!labelBot2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref bot2Type, ref bot2Power, bots[1].HasBankrupted);
            }
            if (!labelBot3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref bot3Type, ref bot3Power, bots[2].HasBankrupted);
            }
            if (!labelBot4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref bot4Type, ref b4Power, bots[3].HasBankrupted);
            }
            if (!labelBot5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref bot5Type, ref bot5Power, bots[4].HasBankrupted);
            }
            Winner(playerType, playerPower, "Player", player.ChipsCount, fixedLast);
            Winner(bot1Type, bot1Power, "Bot 1", bots[0].ChipsCount, fixedLast);
            Winner(bot2Type, bot2Power, "Bot 2", bots[1].ChipsCount, fixedLast);
            Winner(bot3Type, bot3Power, "Bot 3", bots[2].ChipsCount, fixedLast);
            Winner(bot4Type, b4Power, "Bot 4", bots[3].ChipsCount, fixedLast);
            Winner(bot5Type, bot5Power, "Bot 5", bots[4].ChipsCount, fixedLast);
        }
        //AI(2, 3, ref bots[0].ChipsCount, ref bots[0].IsOnTurn, ref bots[0].HasBankrupted, labelBot1Status, 0, b1Power, b1Type);
        //ArtificialIntellect
        void AI(int c1, int c2, BotPlayer bot, Label botStatus, int name,
            double botPower, double botCurrent)
        {
            if (!bot.HasBankrupted)
            {
                if (botCurrent == -1)
                {
                    HighCard(bot, botStatus, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(bot, botStatus, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(bot, botStatus, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(bot, botStatus, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(bot, botStatus, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(bot, botStatus, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(bot, botStatus, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(bot, botStatus, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(bot, botStatus, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(bot, botStatus, name, botPower);
                }
            }
            if (bot.HasBankrupted)
            {
                pictureBoxDeckCards[c1].Visible = false;
                pictureBoxDeckCards[c2].Visible = false;
            }
        }
        //HighCard(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, botPower);
        private void HighCard(BotPlayer bot, Label botStatus, double botPower)
        {
            HP(bot, botStatus, botPower, 20, 25);
        }
        //PairTable(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, botPower);
        private void PairTable(BotPlayer bot, Label botStatus, double botPower)
        {
            HP(bot, botStatus, botPower, 16, 25);
        }
        //PairHand(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, botPower);
        private void PairHand(BotPlayer bot, Label botStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(bot, botStatus, rCall, 6, rRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                PH(bot, botStatus, rCall, 7, rRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                PH(bot, botStatus, rCall, 9, rRaise);
            }
        }
        //TwoPair(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, botPower);
        private void TwoPair(BotPlayer bot, Label botStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(bot, botStatus, rCall, 3, rRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                PH(bot, botStatus, rCall, 4, rRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                PH(bot, botStatus, rCall, 4, rRaise);
            }
        }
        //ThreeOfAKind(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, name, botPower);
        private void ThreeOfAKind(BotPlayer bot, Label botStatus, int name, double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);
            if (botPower <= 390 && botPower >= 330)
            {
                Smooth(bot, botStatus, name, tCall, tRaise);
            }
            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(bot, botStatus, name, tCall, tRaise);
            }
            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(bot, botStatus, name, tCall, tRaise);
            }
        }
        //Straight(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, name, botPower);
        private void Straight(BotPlayer bot, Label botStatus, int name, double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);
            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(bot, botStatus, name, sCall, sRaise);
            }
            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(bot, botStatus, name, sCall, sRaise);
            }
            if (botPower < 407 && botPower >= 404)
            {
                Smooth(bot, botStatus, name, sCall, sRaise);
            }
        }
        //Flush(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, name, botPower);
        private void Flush(BotPlayer bot, Label botStatus, int name, double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(bot, botStatus, name, fCall, fRaise);
        }
        //FullHouse(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, name, botPower);
        private void FullHouse(BotPlayer bot, Label botStatus, int name, double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(bot, botStatus, name, fhCall, fhRaise);
            }
            if (botPower < 620 && botPower >= 602)
            {
                Smooth(bot, botStatus, name, fhCall, fhRaise);
            }
        }
        //FourOfAKind(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, name, botPower);
        private void FourOfAKind(BotPlayer bot, Label botStatus, int name, double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(bot, botStatus, name, fkCall, fkRaise);
            }
        }
        //StraightFlush(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, name, botPower);
        private void StraightFlush(BotPlayer bot, Label botStatus, int name, double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(bot, botStatus, name, sfCall, sfRaise);
            }
        }
        //Fold(ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus);
        private void Fold(BotPlayer bot, Label botStatus)
        {
            raising = false;
            botStatus.Text = "Fold";
            bot.IsOnTurn = false;
            bot.HasBankrupted = true;
        }
        //Check(ref bot.IsOnTurn, botStatus)
        private void Check(BotPlayer bot, Label botStatus)
        {
            botStatus.Text = "Check";
            bot.IsOnTurn = false;
            raising = false;
        }
        //Call(ref bot.ChipsCount, ref bot.IsOnTurn, botStatus);
        private void Call(BotPlayer bot, Label botStatus)
        {
            raising = false;
            bot.IsOnTurn = false;
            bot.ChipsCount -= callValue;
            botStatus.Text = "Call " + callValue;
            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + callValue).ToString();
        }
        //Raised(ref bot.ChipsCount, ref bot.IsOnTurn, botStatus);
        private void Raised(BotPlayer bot, Label botStatus)
        {
            bot.ChipsCount -= Convert.ToInt32(raise);
            botStatus.Text = "Raise " + raise;
            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + Convert.ToInt32(raise)).ToString();
            callValue = Convert.ToInt32(raise);
            raising = true;
            bot.IsOnTurn = false;
        }
        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }
        //HP(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, botPower, 20, 25);
        private void HP(BotPlayer bot, Label botStatus, double botPower, int n, int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (callValue <= 0)
            {
                Check(bot, botStatus);
            }
            if (callValue > 0)
            {
                if (rnd == 1)
                {
                    if (callValue <= RoundN(bot.ChipsCount, n))
                    {
                        Call(bot, botStatus);
                    }
                    else
                    {
                        Fold(bot, botStatus);
                    }
                }
                if (rnd == 2)
                {
                    if (callValue <= RoundN(bot.ChipsCount, n1))
                    {
                        Call(bot, botStatus);
                    }
                    else
                    {
                        Fold(bot, botStatus);
                    }
                }
            }
            if (rnd == 3)
            {
                if (raise == 0)
                {
                    raise = callValue * 2;
                    Raised(bot, botStatus);
                }
                else
                {
                    if (raise <= RoundN(bot.ChipsCount, n))
                    {
                        raise = callValue * 2;
                        Raised(bot, botStatus);
                    }
                    else
                    {
                        Fold(bot, botStatus);
                    }
                }
            }
            if (bot.ChipsCount <= 0)
            {
                bot.HasBankrupted = true;
            }
        }
        //PH(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, rCall, 6, rRaise);
        private void PH(BotPlayer bot, Label botStatus, int n, int n1, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (rounds < 2)
            {
                if (callValue <= 0)
                {
                    Check(bot, botStatus);
                }
                if (callValue > 0)
                {
                    if (callValue >= RoundN(bot.ChipsCount, n1))
                    {
                        Fold(bot, botStatus);
                    }
                    if (raise > RoundN(bot.ChipsCount, n))
                    {
                        Fold(bot, botStatus);
                    }
                    if (!bot.HasBankrupted)
                    {
                        if (callValue >= RoundN(bot.ChipsCount, n) && callValue <= RoundN(bot.ChipsCount, n1))
                        {
                            Call(bot, botStatus);
                        }
                        if (raise <= RoundN(bot.ChipsCount, n) && raise >= (RoundN(bot.ChipsCount, n)) / 2)
                        {
                            Call(bot, botStatus);
                        }
                        if (raise <= (RoundN(bot.ChipsCount, n)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = RoundN(bot.ChipsCount, n);
                                Raised(bot, botStatus);
                            }
                            else
                            {
                                raise = callValue * 2;
                                Raised(bot, botStatus);
                            }
                        }

                    }
                }
            }
            if (rounds >= 2)
            {
                if (callValue > 0)
                {
                    if (callValue >= RoundN(bot.ChipsCount, n1 - rnd))
                    {
                        Fold(bot, botStatus);
                    }
                    if (raise > RoundN(bot.ChipsCount, n - rnd))
                    {
                        Fold(bot, botStatus);
                    }
                    if (!bot.HasBankrupted)
                    {
                        if (callValue >= RoundN(bot.ChipsCount, n - rnd) && callValue <= RoundN(bot.ChipsCount, n1 - rnd))
                        {
                            Call(bot, botStatus);
                        }
                        if (raise <= RoundN(bot.ChipsCount, n - rnd) && raise >= (RoundN(bot.ChipsCount, n - rnd)) / 2)
                        {
                            Call(bot, botStatus);
                        }
                        if (raise <= (RoundN(bot.ChipsCount, n - rnd)) / 2)
                        {
                            if (raise > 0)
                            {
                                raise = RoundN(bot.ChipsCount, n - rnd);
                                Raised(bot, botStatus);
                            }
                            else
                            {
                                raise = callValue * 2;
                                Raised(bot, botStatus);
                            }
                        }
                    }
                }
                if (callValue <= 0)
                {
                    raise = RoundN(bot.ChipsCount, r - rnd);
                    Raised(bot, botStatus);
                }
            }
            if (bot.ChipsCount <= 0)
            {
                bot.HasBankrupted = true;
            }
        }
        //Smooth(ref bot.ChipsCount, ref bot.IsOnTurn, ref bot.HasBankrupted, botStatus, name, tCall, tRaise);
        void Smooth(BotPlayer bot, Label botStatus, int name, int n, int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (callValue <= 0)
            {
                Check(bot, botStatus);
            }
            else
            {
                if (callValue >= RoundN(bot.ChipsCount, n))
                {
                    if (bot.ChipsCount > callValue)
                    {
                        Call(bot, botStatus);
                    }
                    else if (bot.ChipsCount <= callValue)
                    {
                        raising = false;
                        bot.IsOnTurn = false;
                        bot.ChipsCount = 0;
                        botStatus.Text = "Call " + bot.ChipsCount;
                        textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + bot.ChipsCount).ToString();
                    }
                }
                else
                {
                    if (raise > 0)
                    {
                        if (bot.ChipsCount >= raise * 2)
                        {
                            raise *= 2;
                            Raised(bot, botStatus);
                        }
                        else
                        {
                            Call(bot, botStatus);
                        }
                    }
                    else
                    {
                        raise = callValue * 2;
                        Raised(bot, botStatus);
                    }
                }
            }
            if (bot.ChipsCount <= 0)
            {
                bot.HasBankrupted = true;
            }
        }

        #region UI
        private async void timer_Tick(object sender, object e)
        {
            if (progressBarTimer.Value <= 0)
            {
                player.HasBankrupted = true;
                await TurnsAsync();
            }
            if (t > 0)
            {
                t--;
                progressBarTimer.Value = (t / 6) * 100;
            }
        }
        private void Update_Tick(object sender, object e)
        {
            if (player.ChipsCount <= 0)
            {
                textBoxPlayerChips.Text = "Player Chips : 0";
            }
            if (bots[0].ChipsCount <= 0)
            {
                textBoxChipsBot1.Text = "Bot1 Chips : 0";
            }
            if (bots[1].ChipsCount <= 0)
            {
                textBoxChipsBot2.Text = "Bot2 Chips : 0";
            }
            if (bots[2].ChipsCount <= 0)
            {
                textBoxChipsBot3.Text = "Bot3 Chips : 0";
            }
            if (bots[3].ChipsCount <= 0)
            {
                textBoxChipsBot4.Text = "Bot4 Chips : 0";
            }
            if (bots[4].ChipsCount <= 0)
            {
                textBoxChipsBot5.Text = "Bot5 Chips : 0";
            }
            textBoxPlayerChips.Text = "Player Chips : " + player.ChipsCount.ToString();
            textBoxChipsBot1.Text = "Bot1 Chips : " + bots[0].ChipsCount.ToString();
            textBoxChipsBot2.Text = "Bot2 Chips : " + bots[1].ChipsCount.ToString();
            textBoxChipsBot3.Text = "Bot3 Chips : " + bots[2].ChipsCount.ToString();
            textBoxChipsBot4.Text = "Bot4 Chips : " + bots[3].ChipsCount.ToString();
            textBoxChipsBot5.Text = "Bot5 Chips : " + bots[4].ChipsCount.ToString();
            if (player.ChipsCount <= 0)
            {
                player.IsOnTurn = false;
                player.HasBankrupted = true;
                buttonCall.Enabled = false;
                buttonRaise.Enabled = false;
                buttonFold.Enabled = false;
                buttonCheck.Enabled = false;
            }
            if (maxChipsCount > 0)
            {
                maxChipsCount--;
            }
            if (player.ChipsCount >= callValue)
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
            if (player.ChipsCount <= 0)
            {
                buttonRaise.Enabled = false;
            }
            int parsedValue;

            if (textBoxRaiseAmount.Text != "" && int.TryParse(textBoxRaiseAmount.Text, out parsedValue))
            {
                if (player.ChipsCount <= int.Parse(textBoxRaiseAmount.Text))
                {
                    buttonRaise.Text = "All in";
                }
                else
                {
                    buttonRaise.Text = "Raise";
                }
            }
            if (player.ChipsCount < callValue)
            {
                buttonRaise.Enabled = false;
            }
        }
        private async void bFold_Click(object sender, EventArgs e)
        {
            labelPlayerStatus.Text = "Fold";
            player.IsOnTurn = false;
            player.HasBankrupted = true;
            await TurnsAsync();
        }
        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (callValue <= 0)
            {
                player.IsOnTurn = false;
                labelPlayerStatus.Text = "Check";
            }
            else
            {
                //pStatus.Text = "All in " + Chips;

                buttonCheck.Enabled = false;
            }
            await TurnsAsync();
        }
        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, player.HasBankrupted);
            if (player.ChipsCount >= callValue)
            {
                player.ChipsCount -= callValue;
                textBoxPlayerChips.Text = "Chips : " + player.ChipsCount.ToString();
                if (textBoxPotAmount.Text != "")
                {
                    textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + callValue).ToString();
                }
                else
                {
                    textBoxPotAmount.Text = callValue.ToString();
                }
                player.IsOnTurn = false;
                labelPlayerStatus.Text = "Call " + callValue;
                player.Call = callValue;
            }
            else if (player.ChipsCount <= callValue && callValue > 0)
            {
                textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + player.ChipsCount).ToString();
                labelPlayerStatus.Text = "All in " + player.ChipsCount;
                player.ChipsCount = 0;
                textBoxPlayerChips.Text = "Chips : " + player.ChipsCount.ToString();
                player.IsOnTurn = false;
                buttonFold.Enabled = false;
                player.Call = player.ChipsCount;
            }
            await TurnsAsync();
        }
        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref playerType, ref playerPower, player.HasBankrupted);
            int parsedValue;
            if (textBoxRaiseAmount.Text != "" && int.TryParse(textBoxRaiseAmount.Text, out parsedValue))
            {
                if (player.ChipsCount > callValue)
                {
                    if (raise * 2 > int.Parse(textBoxRaiseAmount.Text))
                    {
                        textBoxRaiseAmount.Text = (raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (player.ChipsCount >= int.Parse(textBoxRaiseAmount.Text))
                        {
                            callValue = int.Parse(textBoxRaiseAmount.Text);
                            raise = int.Parse(textBoxRaiseAmount.Text);
                            labelPlayerStatus.Text = "Raise " + callValue.ToString();
                            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + callValue).ToString();
                            buttonCall.Text = "Call";
                            player.ChipsCount -= int.Parse(textBoxRaiseAmount.Text);
                            raising = true;
                            last = 0;
                            player.Raise = Convert.ToInt32(raise);
                        }
                        else
                        {
                            callValue = player.ChipsCount;
                            raise = player.ChipsCount;
                            textBoxPotAmount.Text = (int.Parse(textBoxPotAmount.Text) + player.ChipsCount).ToString();
                            labelPlayerStatus.Text = "Raise " + callValue.ToString();
                            player.ChipsCount = 0;
                            raising = true;
                            last = 0;
                            player.Raise = Convert.ToInt32(raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }
            player.IsOnTurn = false;
            await TurnsAsync();
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAddChips.Text == "") { }
            else
            {
                player.ChipsCount += int.Parse(textBoxAddChips.Text);
                bots[0].ChipsCount += int.Parse(textBoxAddChips.Text);
                bots[1].ChipsCount += int.Parse(textBoxAddChips.Text);
                bots[2].ChipsCount += int.Parse(textBoxAddChips.Text);
                bots[3].ChipsCount += int.Parse(textBoxAddChips.Text);
                bots[4].ChipsCount += int.Parse(textBoxAddChips.Text);
            }
            textBoxPlayerChips.Text = "Chips : " + player.ChipsCount.ToString();
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
            this.player.ChipsCount = DefaultChipsCount;
            this.player.Call = 0;
            this.player.Raise = 0;
            this.player.HasBankrupted = false;
            this.player.IsOnTurn = true;
            this.player.ShouldRestart = false;
        }

        private void IntializeBots()
        {
            this.currentBotsPlayingCount = InitialNumberOfBots;
            this.bots[0].ChipsCount = DefaultChipsCount;
            this.bots[1].ChipsCount = DefaultChipsCount;
            this.bots[2].ChipsCount = DefaultChipsCount;
            this.bots[3].ChipsCount = DefaultChipsCount;
            this.bots[4].ChipsCount = DefaultChipsCount;
            this.bots[0].IsOnTurn = false;
            this.bots[1].IsOnTurn = false;
            this.bots[2].IsOnTurn = false;
            this.bots[3].IsOnTurn = false;
            this.bots[4].IsOnTurn = false;
            this.bots[0].HasBankrupted = false;
            this.bots[1].HasBankrupted = false;
            this.bots[2].HasBankrupted = false;
            this.bots[3].HasBankrupted = false;
            this.bots[4].HasBankrupted = false;
            this.bots[0].Call = 0;
            this.bots[1].Call = 0;
            this.bots[2].Call = 0;
            this.bots[3].Call = 0;
            this.bots[4].Call = 0;
            this.bots[0].Raise = 0;
            this.bots[1].Raise = 0;
            this.bots[2].Raise = 0;
            this.bots[3].Raise = 0;
            this.bots[4].Raise = 0;
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
            this.textBoxPlayerChips.Text = "Player Chips : " + player.ChipsCount.ToString();
            this.textBoxChipsBot1.Text = "Bot1 Chips : " + bots[0].ChipsCount.ToString();
            this.textBoxChipsBot2.Text = "Bot2 Chips : " + bots[1].ChipsCount.ToString();
            this.textBoxChipsBot3.Text = "Bot3 Chips : " + bots[2].ChipsCount.ToString();
            this.textBoxChipsBot4.Text = "Bot4 Chips : " + bots[3].ChipsCount.ToString();
            this.textBoxChipsBot5.Text = "Bot5 Chips : " + bots[4].ChipsCount.ToString();
            this.textBoxRaiseAmount.Text = (bigBlindValue * 2).ToString();
        }

        private void SetBlindButtonsVisibilityToFalse()
        {
            textBoxBigBlind.Visible = false;
            textBoxSmallBlind.Visible = false;
            buttonBigBlind.Visible = false;
            buttonSmallBlind.Visible = false;
        }

        private void InitializeBankruptPlayersList()
        {
            this.bankruptPlayers.Add(player.HasBankrupted);
            this.bankruptPlayers.Add(bots[0].HasBankrupted);
            this.bankruptPlayers.Add(bots[1].HasBankrupted);
            this.bankruptPlayers.Add(bots[2].HasBankrupted);
            this.bankruptPlayers.Add(bots[3].HasBankrupted);
            this.bankruptPlayers.Add(bots[4].HasBankrupted);
        }

        private void DisableButtonsDuringShuffle()
        {
            this.buttonCall.Enabled = false;
            this.buttonRaise.Enabled = false;
            this.buttonFold.Enabled = false;
            this.buttonCheck.Enabled = false;
        }

        //Shuffle the cards literally, i.e. the algorithm below shuffles the card image locations.
        private void ShuffleCards()
        {
            Random randomizer = new Random();

            for (i = this.ImgLocation.Length; i > 0; i--)
            {
                int j = randomizer.Next(i);
                var k = ImgLocation[j];
                this.ImgLocation[j] = ImgLocation[i - 1];
                this.ImgLocation[i - 1] = k;
            }
        }

        //The file name (numbers from 1 to 52) of the first 17 cards(the table cards) 
        //are assigned to the cardsOnTable array of ints.
        private void InitializeCardsOnTable()
        {
            this.deckCardsImages[i] = Image.FromFile(ImgLocation[i]);
            var charsToRemove = new string[] { "Assets\\Cards\\", ".png" };
            foreach (var c in charsToRemove)
            {
                ImgLocation[i] = ImgLocation[i].Replace(c, string.Empty);
            }

            this.cardsOnTable[i] = int.Parse(ImgLocation[i]) - 1;
        }

        private void InitializePictureBoxes()
        {
            this.pictureBoxDeckCards[i] = new PictureBox();
            this.pictureBoxDeckCards[i].SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxDeckCards[i].Height = 130;
            this.pictureBoxDeckCards[i].Width = 80;
            this.Controls.Add(pictureBoxDeckCards[i]);
            this.pictureBoxDeckCards[i].Name = "pb" + i.ToString();
        }

        private void EnableButtons()
        {
            this.buttonRaise.Enabled = true;
            this.buttonCall.Enabled = true;
            this.buttonRaise.Enabled = true;
            this.buttonRaise.Enabled = true;
            this.buttonFold.Enabled = true;
        }
    }
}