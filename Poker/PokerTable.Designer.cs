namespace Poker
{
    partial class PokerTable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonFold = new System.Windows.Forms.Button();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.buttonCall = new System.Windows.Forms.Button();
            this.buttonRaise = new System.Windows.Forms.Button();
            this.progressBarTimer = new System.Windows.Forms.ProgressBar();
            this.textBoxPlayerChips = new System.Windows.Forms.TextBox();
            this.buttonAddChips = new System.Windows.Forms.Button();
            this.textBoxAddChips = new System.Windows.Forms.TextBox();
            this.textBoxChipsBot5 = new System.Windows.Forms.TextBox();
            this.textBoxChipsBot4 = new System.Windows.Forms.TextBox();
            this.textBoxChipsBot3 = new System.Windows.Forms.TextBox();
            this.textBoxChipsBot2 = new System.Windows.Forms.TextBox();
            this.textBoxChipsBot1 = new System.Windows.Forms.TextBox();
            this.textBoxPotAmount = new System.Windows.Forms.TextBox();
            this.buttonChangeBlindsValue = new System.Windows.Forms.Button();
            this.buttonBigBlind = new System.Windows.Forms.Button();
            this.textBoxSmallBlind = new System.Windows.Forms.TextBox();
            this.buttonSmallBlind = new System.Windows.Forms.Button();
            this.textBoxBigBlind = new System.Windows.Forms.TextBox();
            this.labelBot5Status = new System.Windows.Forms.Label();
            this.labelBot4Status = new System.Windows.Forms.Label();
            this.labelBot3Status = new System.Windows.Forms.Label();
            this.labelBot1Status = new System.Windows.Forms.Label();
            this.labelPlayerStatus = new System.Windows.Forms.Label();
            this.labelBot2Status = new System.Windows.Forms.Label();
            this.labelPot = new System.Windows.Forms.Label();
            this.textBoxRaiseAmount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonFold
            // 
            this.buttonFold.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonFold.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFold.Location = new System.Drawing.Point(335, 660);
            this.buttonFold.Name = "buttonFold";
            this.buttonFold.Size = new System.Drawing.Size(130, 62);
            this.buttonFold.TabIndex = 0;
            this.buttonFold.Text = "Fold";
            this.buttonFold.UseVisualStyleBackColor = true;
            this.buttonFold.Click += new System.EventHandler(this.bFold_Click);
            // 
            // buttonCheck
            // 
            this.buttonCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCheck.Location = new System.Drawing.Point(494, 660);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(134, 62);
            this.buttonCheck.TabIndex = 2;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // buttonCall
            // 
            this.buttonCall.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCall.Location = new System.Drawing.Point(667, 661);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Size = new System.Drawing.Size(126, 62);
            this.buttonCall.TabIndex = 3;
            this.buttonCall.Text = "Call";
            this.buttonCall.UseVisualStyleBackColor = true;
            this.buttonCall.Click += new System.EventHandler(this.bCall_Click);
            // 
            // buttonRaise
            // 
            this.buttonRaise.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonRaise.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRaise.Location = new System.Drawing.Point(835, 661);
            this.buttonRaise.Name = "buttonRaise";
            this.buttonRaise.Size = new System.Drawing.Size(124, 62);
            this.buttonRaise.TabIndex = 4;
            this.buttonRaise.Text = "Raise";
            this.buttonRaise.UseVisualStyleBackColor = true;
            this.buttonRaise.Click += new System.EventHandler(this.bRaise_Click);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressBarTimer.BackColor = System.Drawing.SystemColors.Control;
            this.progressBarTimer.Location = new System.Drawing.Point(335, 631);
            this.progressBarTimer.Maximum = 1000;
            this.progressBarTimer.Name = "progressBarTimer";
            this.progressBarTimer.Size = new System.Drawing.Size(667, 23);
            this.progressBarTimer.TabIndex = 5;
            this.progressBarTimer.Value = 1000;
            // 
            // textBoxPlayerChips
            // 
            this.textBoxPlayerChips.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBoxPlayerChips.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPlayerChips.Location = new System.Drawing.Point(755, 553);
            this.textBoxPlayerChips.Name = "textBoxPlayerChips";
            this.textBoxPlayerChips.Size = new System.Drawing.Size(163, 23);
            this.textBoxPlayerChips.TabIndex = 6;
            this.textBoxPlayerChips.Text = "Player Chips : 0";
            // 
            // buttonAddChips
            // 
            this.buttonAddChips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddChips.Location = new System.Drawing.Point(12, 697);
            this.buttonAddChips.Name = "buttonAddChips";
            this.buttonAddChips.Size = new System.Drawing.Size(75, 25);
            this.buttonAddChips.TabIndex = 7;
            this.buttonAddChips.Text = "AddChips";
            this.buttonAddChips.UseVisualStyleBackColor = true;
            this.buttonAddChips.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // textBoxAddChips
            // 
            this.textBoxAddChips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxAddChips.Location = new System.Drawing.Point(93, 700);
            this.textBoxAddChips.Name = "textBoxAddChips";
            this.textBoxAddChips.Size = new System.Drawing.Size(125, 20);
            this.textBoxAddChips.TabIndex = 8;
            // 
            // textBoxChipsBot5
            // 
            this.textBoxChipsBot5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChipsBot5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxChipsBot5.Location = new System.Drawing.Point(1012, 553);
            this.textBoxChipsBot5.Name = "textBoxChipsBot5";
            this.textBoxChipsBot5.Size = new System.Drawing.Size(152, 23);
            this.textBoxChipsBot5.TabIndex = 9;
            this.textBoxChipsBot5.Text = "Bot5 Chips : 0";
            // 
            // textBoxChipsBot4
            // 
            this.textBoxChipsBot4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChipsBot4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxChipsBot4.Location = new System.Drawing.Point(970, 81);
            this.textBoxChipsBot4.Name = "textBoxChipsBot4";
            this.textBoxChipsBot4.Size = new System.Drawing.Size(123, 23);
            this.textBoxChipsBot4.TabIndex = 10;
            this.textBoxChipsBot4.Text = "Bot4 Chips : 0";
            // 
            // textBoxChipsBot3
            // 
            this.textBoxChipsBot3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChipsBot3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxChipsBot3.Location = new System.Drawing.Point(755, 81);
            this.textBoxChipsBot3.Name = "textBoxChipsBot3";
            this.textBoxChipsBot3.Size = new System.Drawing.Size(125, 23);
            this.textBoxChipsBot3.TabIndex = 11;
            this.textBoxChipsBot3.Text = "Bot3 Chips : 0";
            // 
            // textBoxChipsBot2
            // 
            this.textBoxChipsBot2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxChipsBot2.Location = new System.Drawing.Point(276, 81);
            this.textBoxChipsBot2.Name = "textBoxChipsBot2";
            this.textBoxChipsBot2.Size = new System.Drawing.Size(133, 23);
            this.textBoxChipsBot2.TabIndex = 12;
            this.textBoxChipsBot2.Text = "Bot2 Chips : 0";
            // 
            // textBoxChipsBot1
            // 
            this.textBoxChipsBot1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxChipsBot1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxChipsBot1.Location = new System.Drawing.Point(181, 553);
            this.textBoxChipsBot1.Name = "textBoxChipsBot1";
            this.textBoxChipsBot1.Size = new System.Drawing.Size(142, 23);
            this.textBoxChipsBot1.TabIndex = 13;
            this.textBoxChipsBot1.Text = "Bot1 Chips : 0";
            // 
            // textBoxPotAmount
            // 
            this.textBoxPotAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBoxPotAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPotAmount.Location = new System.Drawing.Point(606, 212);
            this.textBoxPotAmount.Name = "textBoxPotAmount";
            this.textBoxPotAmount.Size = new System.Drawing.Size(125, 23);
            this.textBoxPotAmount.TabIndex = 14;
            this.textBoxPotAmount.Text = "0";
            // 
            // buttonChangeBlindsValue
            // 
            this.buttonChangeBlindsValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonChangeBlindsValue.Location = new System.Drawing.Point(12, 12);
            this.buttonChangeBlindsValue.Name = "buttonChangeBlindsValue";
            this.buttonChangeBlindsValue.Size = new System.Drawing.Size(75, 36);
            this.buttonChangeBlindsValue.TabIndex = 15;
            this.buttonChangeBlindsValue.Text = "BB/SB";
            this.buttonChangeBlindsValue.UseVisualStyleBackColor = true;
            this.buttonChangeBlindsValue.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // buttonBigBlind
            // 
            this.buttonBigBlind.Location = new System.Drawing.Point(12, 254);
            this.buttonBigBlind.Name = "buttonBigBlind";
            this.buttonBigBlind.Size = new System.Drawing.Size(75, 23);
            this.buttonBigBlind.TabIndex = 16;
            this.buttonBigBlind.Text = "Big Blind";
            this.buttonBigBlind.UseVisualStyleBackColor = true;
            this.buttonBigBlind.Click += new System.EventHandler(this.bBB_Click);
            // 
            // textBoxSmallBlind
            // 
            this.textBoxSmallBlind.Location = new System.Drawing.Point(12, 228);
            this.textBoxSmallBlind.Name = "textBoxSmallBlind";
            this.textBoxSmallBlind.Size = new System.Drawing.Size(75, 20);
            this.textBoxSmallBlind.TabIndex = 17;
            this.textBoxSmallBlind.Text = "250";
            // 
            // buttonSmallBlind
            // 
            this.buttonSmallBlind.Location = new System.Drawing.Point(12, 199);
            this.buttonSmallBlind.Name = "buttonSmallBlind";
            this.buttonSmallBlind.Size = new System.Drawing.Size(75, 23);
            this.buttonSmallBlind.TabIndex = 18;
            this.buttonSmallBlind.Text = "Small Blind";
            this.buttonSmallBlind.UseVisualStyleBackColor = true;
            this.buttonSmallBlind.Click += new System.EventHandler(this.bSB_Click);
            // 
            // textBoxBigBlind
            // 
            this.textBoxBigBlind.Location = new System.Drawing.Point(12, 283);
            this.textBoxBigBlind.Name = "textBoxBigBlind";
            this.textBoxBigBlind.Size = new System.Drawing.Size(75, 20);
            this.textBoxBigBlind.TabIndex = 19;
            this.textBoxBigBlind.Text = "500";
            // 
            // labelBot5Status
            // 
            this.labelBot5Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBot5Status.Location = new System.Drawing.Point(1012, 579);
            this.labelBot5Status.Name = "labelBot5Status";
            this.labelBot5Status.Size = new System.Drawing.Size(152, 32);
            this.labelBot5Status.TabIndex = 26;
            // 
            // labelBot4Status
            // 
            this.labelBot4Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBot4Status.Location = new System.Drawing.Point(970, 107);
            this.labelBot4Status.Name = "labelBot4Status";
            this.labelBot4Status.Size = new System.Drawing.Size(123, 32);
            this.labelBot4Status.TabIndex = 27;
            // 
            // labelBot3Status
            // 
            this.labelBot3Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBot3Status.Location = new System.Drawing.Point(755, 107);
            this.labelBot3Status.Name = "labelBot3Status";
            this.labelBot3Status.Size = new System.Drawing.Size(125, 32);
            this.labelBot3Status.TabIndex = 28;
            // 
            // labelBot1Status
            // 
            this.labelBot1Status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelBot1Status.Location = new System.Drawing.Point(181, 579);
            this.labelBot1Status.Name = "labelBot1Status";
            this.labelBot1Status.Size = new System.Drawing.Size(142, 32);
            this.labelBot1Status.TabIndex = 29;
            // 
            // labelPlayerStatus
            // 
            this.labelPlayerStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelPlayerStatus.Location = new System.Drawing.Point(755, 579);
            this.labelPlayerStatus.Name = "labelPlayerStatus";
            this.labelPlayerStatus.Size = new System.Drawing.Size(163, 32);
            this.labelPlayerStatus.TabIndex = 30;
            // 
            // labelBot2Status
            // 
            this.labelBot2Status.Location = new System.Drawing.Point(276, 107);
            this.labelBot2Status.Name = "labelBot2Status";
            this.labelBot2Status.Size = new System.Drawing.Size(133, 32);
            this.labelBot2Status.TabIndex = 31;
            // 
            // labelPot
            // 
            this.labelPot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelPot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPot.Location = new System.Drawing.Point(654, 188);
            this.labelPot.Name = "labelPot";
            this.labelPot.Size = new System.Drawing.Size(31, 21);
            this.labelPot.TabIndex = 0;
            this.labelPot.Text = "Pot";
            // 
            // textBoxRaiseAmount
            // 
            this.textBoxRaiseAmount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBoxRaiseAmount.Location = new System.Drawing.Point(965, 703);
            this.textBoxRaiseAmount.Name = "textBoxRaiseAmount";
            this.textBoxRaiseAmount.Size = new System.Drawing.Size(108, 20);
            this.textBoxRaiseAmount.TabIndex = 0;
            // 
            // PokerTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Poker.Properties.Resources.poker_table___Copy;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.textBoxRaiseAmount);
            this.Controls.Add(this.labelPot);
            this.Controls.Add(this.labelBot2Status);
            this.Controls.Add(this.labelPlayerStatus);
            this.Controls.Add(this.labelBot1Status);
            this.Controls.Add(this.labelBot3Status);
            this.Controls.Add(this.labelBot4Status);
            this.Controls.Add(this.labelBot5Status);
            this.Controls.Add(this.textBoxBigBlind);
            this.Controls.Add(this.buttonSmallBlind);
            this.Controls.Add(this.textBoxSmallBlind);
            this.Controls.Add(this.buttonBigBlind);
            this.Controls.Add(this.buttonChangeBlindsValue);
            this.Controls.Add(this.textBoxPotAmount);
            this.Controls.Add(this.textBoxChipsBot1);
            this.Controls.Add(this.textBoxChipsBot2);
            this.Controls.Add(this.textBoxChipsBot3);
            this.Controls.Add(this.textBoxChipsBot4);
            this.Controls.Add(this.textBoxChipsBot5);
            this.Controls.Add(this.textBoxAddChips);
            this.Controls.Add(this.buttonAddChips);
            this.Controls.Add(this.textBoxPlayerChips);
            this.Controls.Add(this.progressBarTimer);
            this.Controls.Add(this.buttonRaise);
            this.Controls.Add(this.buttonCall);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.buttonFold);
            this.DoubleBuffered = true;
            this.Name = "PokerTable";
            this.Text = "GLS Texas Poker";
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.Layout_Change);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFold;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.Button buttonCall;
        private System.Windows.Forms.Button buttonRaise;
        private System.Windows.Forms.ProgressBar progressBarTimer;
        private System.Windows.Forms.TextBox textBoxPlayerChips;
        private System.Windows.Forms.Button buttonAddChips;
        private System.Windows.Forms.TextBox textBoxAddChips;
        private System.Windows.Forms.TextBox textBoxChipsBot5;
        private System.Windows.Forms.TextBox textBoxChipsBot4;
        private System.Windows.Forms.TextBox textBoxChipsBot3;
        private System.Windows.Forms.TextBox textBoxChipsBot2;
        private System.Windows.Forms.TextBox textBoxChipsBot1;
        private System.Windows.Forms.TextBox textBoxPotAmount;
        private System.Windows.Forms.Button buttonChangeBlindsValue;
        private System.Windows.Forms.Button buttonBigBlind;
        private System.Windows.Forms.TextBox textBoxSmallBlind;
        private System.Windows.Forms.Button buttonSmallBlind;
        private System.Windows.Forms.TextBox textBoxBigBlind;
        private System.Windows.Forms.Label labelBot5Status;
        private System.Windows.Forms.Label labelBot4Status;
        private System.Windows.Forms.Label labelBot3Status;
        private System.Windows.Forms.Label labelBot1Status;
        private System.Windows.Forms.Label labelPlayerStatus;
        private System.Windows.Forms.Label labelBot2Status;
        private System.Windows.Forms.Label labelPot;
        private System.Windows.Forms.TextBox textBoxRaiseAmount;



    }
}

