using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roullete
{
    public partial class Form1 : Form
    {
  
       public int valueAfterSpin { get; private set;}
       private List<int> valueOfRoulette;
       private Player currentPlayer;
       private int winNumder;
       private int countControls;
       private bool flagWasReBetClick = false;
       private double xRoulletCenter;
       private double yRoulletCenter;
       private double xBall;
       private double yBall;

        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("..\\roulet_img.jpg");
            countControls = this.Controls.Count;
            currentPlayer = new Player(Chip5.BackgroundImage);
            valueOfRoulette = new List<int>() { 
                17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31,
                9, 22, 18, 29, 7, 28, 12, 35, 3, 26, 0, 32, 15, 19, 4, 21, 2, 25 };
            System.Drawing.Drawing2D.GraphicsPath myPath =  new System.Drawing.Drawing2D.GraphicsPath();
            myPath.AddEllipse(0, 0, main_button.Width, main_button.Height);
            Region myRegion = new Region(myPath);
            this.main_button.Region = myRegion;
            foreach (Control item in Controls)
            {
                item.Hide();
            }
            
            System.Drawing.Drawing2D.GraphicsPath Img_rlt = new System.Drawing.Drawing2D.GraphicsPath();
            Img_rlt.AddEllipse(1, 1, Roleta.Width - 1, Roleta.Height - 1);
            System.Drawing.Region Img_rlt_Region = new System.Drawing.Region(Img_rlt);
            this.Roleta.Region = Img_rlt_Region;
            System.Drawing.Drawing2D.GraphicsPath Img_Ball = new System.Drawing.Drawing2D.GraphicsPath();
            Img_Ball.AddEllipse(1, 1, Ball.Width - 2, Ball.Height - 1);
            System.Drawing.Region Img_Ball_Region = new System.Drawing.Region(Img_Ball);
            this.Ball.Region = Img_Ball_Region;
            this.Ball.BringToFront();
            this.main_button.Show();
            xRoulletCenter = 150;
            yRoulletCenter = 155;      
        }

        private void main_button_Click(object sender, EventArgs e)
        {

            this.BackgroundImage = Image.FromFile("..\\roulletetable.jpg");
            foreach (Control item in Controls)
            {
                item.Show();
            }
            this.Figure.Visible = false;
            this.main_button.Hide();
            this.main_button.Dispose();
        }
         
        private void Spin_Click(object sender, EventArgs e)
        {
            this.tWinning.Text = "0";
            this.Figure.BringToFront();
            this.Figure.Visible = false;
            Thread t1 = new Thread(new ThreadStart(this.Spinb));
            t1.Start();
            t1.Join();
            calculateWin();
            clearAfterSpin();
            this.Figure.Visible = true;
       }


        private int paymants(string boxType)
        {
            if (boxType.Contains("Even") || boxType.Contains("Odd") || boxType.Contains("Red") || boxType.Contains("Black") ||boxType.Contains("1_18") || boxType.Contains("19_36"))       
            {
                return 2;
            }

            if (boxType.Contains("1_12") || boxType.Contains("13_24") || boxType.Contains("25_36") || boxType.Contains("1st") || boxType.Contains("2nd") || boxType.Contains("3rd"))
            {
                return 3;
            }
            if (boxType.Contains("DTRow"))
            {
                return 6;
            }

            if (boxType.Contains("QBox"))
            {
                return 6;
            }
            if (boxType.Contains("TBox"))
            {
                return 12;
            }

            if (boxType.Contains("DBox"))
            {
                return 18;
            }

            return 36;
        }


        private void calculateWin()
        {
            string winBox = "Box" + winNumder.ToString();
            int winning = 0;
            List<int> Reds = new List<int>() { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            foreach (var item in currentPlayer.currentBet)
            {
                
                if (item.currentBox.Name.Contains(winBox))
                {
                    winning += item.chip.valueDigit * paymants(winBox);
                }
                 if (item.currentBox.Name.Contains("1st") && (winNumder - 1) % 3 == 0)
                {
                    winning += item.chip.valueDigit * paymants("1st");
                 }

                 if (item.currentBox.Name.Contains("2nd") && (winNumder + 1) % 3 == 0)
                 {
                     winning += item.chip.valueDigit * paymants("2nd");
                 }

                 if (item.currentBox.Name.Contains("3rd") && (winNumder % 3) == 0)
                 {
                     winning += item.chip.valueDigit * paymants("3rd");
                 }
                if (item.currentBox.Name.Contains("Box_1_12") && winNumder <= 12)
                {
                    winning += item.chip.valueDigit * paymants("Box_1_12");
                }
                if (item.currentBox.Name.Contains("Box_13_24") && winNumder > 12 && winNumder <= 24)
                {
                    winning += item.chip.valueDigit * paymants("Box_13_24");
                }
                if (item.currentBox.Name.Contains("Box_25_36") && winNumder > 24)
                {
                    winning += item.chip.valueDigit * paymants("Box_25_36");
                }
                if (item.currentBox.Name.Contains("Box_1_18") && winNumder <= 18)
                {
                    winning += item.chip.valueDigit * paymants("Box_1_18");
                }
                if (item.currentBox.Name.Contains("Box_19_36") && winNumder > 18)
                {
                    winning += item.chip.valueDigit * paymants("Box_19_36");
                }
                if (item.currentBox.Name.Contains("Box_Even") && winNumder %2 == 0)
                {
                    winning += item.chip.valueDigit * paymants("Box_Even");
                }
                if (item.currentBox.Name.Contains("Box_Odd") && winNumder % 2 != 0)
                {
                    winning += item.chip.valueDigit * paymants("Box_Odd");
                }
                if (item.currentBox.Name.Contains("Box_Red") && Reds.Contains(winNumder)==true)
                {
                    winning += item.chip.valueDigit * paymants("Box_Red");
                }
                if (item.currentBox.Name.Contains("Box_Black") && Reds.Contains(winNumder)==false)
                {
                    winning += item.chip.valueDigit * paymants("Box_Black");
                }


            }
            this.tWinning.Text = winning.ToString();
            currentPlayer.balance += winning;
            this.tBalance.Text = currentPlayer.balance.ToString();
        }



       
    private void Spinb()
    {
        
          int rand = new Random().Next(1,36);
          int step = 10;
          int radius = 70;
          int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int angle = 0; angle < 360; angle += 10)
            {
               
                if (rand == 0)
                {
                    index = angle;
                    break;
                }
                    if (i + 1 == 5)
                {
                    rand--;
                }
                double deg2rad = ((angle * 3.14159) / 180);
                xBall = xRoulletCenter + radius * Math.Cos(deg2rad);
                yBall = yRoulletCenter + radius * Math.Sin(deg2rad);
                this.Ball.Location = new System.Drawing.Point((int)xBall, (int)yBall);
                this.Ball.Update();
                Thread.Sleep( 25 + step);
            }
           step += step;
           }
            double temp = index /10;
            winNumder = valueOfRoulette[index/10];
            tWinning.Text = winNumder.ToString();
            setFigureToWinBox();
            this.Spin.Enabled = true;
            this.GetChips.Enabled = true;
            this.Rebet.Enabled = true;
           
    }

    private void Box_MouseClick(object sender, MouseEventArgs e)
    {
        this.Figure.Visible = false;
        this.tWinning.Text = "0";
        if (currentPlayer.balance >= currentPlayer.currentChip.valueDigit)
        {
            PictureBox chipBet = new System.Windows.Forms.PictureBox();
            chipBet.BackgroundImage = currentPlayer.currentChip.chipBox.BackgroundImage;
            chipBet.Name = "chipBet" + currentPlayer.currentBet.Count.ToString();
            chipBet.Size = new System.Drawing.Size(25, 25);
            chipBet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            chipBet.TabIndex = 1;
            chipBet.BackgroundImageLayout = ImageLayout.Zoom;
            chipBet.BringToFront();
            chipBet.TabStop = false;
            chipBet.BackColor = Color.Transparent;
            chipBet.Location = new System.Drawing.Point((int)(sender as PictureBox).Left + (sender as PictureBox).Width / 2 - chipBet.Width / 2, (int)(sender as PictureBox).Top + (sender as PictureBox).Height / 2 - chipBet.Height / 2);
            this.Controls.Add(chipBet);
            System.Drawing.Drawing2D.GraphicsPath Img_Chip = new System.Drawing.Drawing2D.GraphicsPath();
            Img_Chip.AddEllipse(1, 1, chipBet.Width - 1, chipBet.Height - 1);
            System.Drawing.Region Img_Chip_Region = new System.Drawing.Region(Img_Chip);
            chipBet.Region = Img_Chip_Region;
            currentPlayer.currentBet.Add(new Bet(currentPlayer.currentChip.valueDigit, (sender as PictureBox), chipBet, currentPlayer.currentChip.chipBox.BackgroundImage));
            currentPlayer.balance -= currentPlayer.currentChip.valueDigit;
            this.tBalance.Text = currentPlayer.balance.ToString();
            currentPlayer.totalBet += currentPlayer.currentChip.valueDigit;
            this.tTotalbet.Text = currentPlayer.totalBet.ToString();
            foreach (var item in currentPlayer.currentBet)
            {
                item.chip.chipBox.BringToFront();
            }
        }
    }

    private void Undo_Click(object sender, EventArgs e)
    {
        if (flagWasReBetClick == true && currentPlayer.currentBet.Count==currentPlayer.savedBetToRebet.Count || this.Figure.Visible == true)
        {
            return;
        }
        int countOfBets = currentPlayer.currentBet.Count;
        if (countOfBets > 0)
        {
            PictureBox lastBetBox = currentPlayer.currentBet[countOfBets - 1].currentBox;
            Chip lastChip = currentPlayer.currentBet[countOfBets - 1].chip;
            currentPlayer.balance += lastChip.valueDigit;
            this.tBalance.Text = currentPlayer.balance.ToString();
            currentPlayer.totalBet -= lastChip.valueDigit;
            this.tTotalbet.Text = currentPlayer.totalBet.ToString();
            int controlsToDel = Controls.IndexOf(currentPlayer.currentBet[countOfBets - 1].chip.chipBox);
            if (controlsToDel >= 0)
            {
                Controls.RemoveAt(controlsToDel);
            }
            currentPlayer.currentBet.RemoveAt(countOfBets - 1);

        }
        this.Figure.Visible = false;
    }



    private void Clear_Click(object sender, EventArgs e)
    {

        int countOfBets = currentPlayer.currentBet.Count;
        while (countOfBets > 0)
        {
            PictureBox lastBetBox = currentPlayer.currentBet[countOfBets - 1].currentBox;
            Chip lastChip = currentPlayer.currentBet[countOfBets - 1].chip;
            currentPlayer.balance += lastChip.valueDigit;
            currentPlayer.totalBet -= lastChip.valueDigit;
            int controlsToDel = Controls.IndexOf(currentPlayer.currentBet[countOfBets - 1].chip.chipBox);
            if (controlsToDel>=0)
            {
                Controls.RemoveAt(controlsToDel);
            }
            this.tBalance.Text = currentPlayer.balance.ToString();
            this.tTotalbet.Text = "0";
            currentPlayer.currentBet.RemoveAt(countOfBets - 1);
            countOfBets--;
            this.tWinning.Text = "0";

        }
        this.Figure.Visible = false;
    }



    private void clearAfterSpin()
    {

        int countOfBets = currentPlayer.currentBet.Count;
        currentPlayer.savedBetToRebet = new List<Bet>(currentPlayer.currentBet);
        while (countOfBets > 0)
        {
            currentPlayer.currentChip = new Chip(5, new System.Windows.Forms.PictureBox(), Chip5.BackgroundImage);
            this.tTotalbet.Text = "0";
            int controlsToDel = Controls.IndexOf(currentPlayer.currentBet[countOfBets - 1].chip.chipBox);
            if (controlsToDel >= 0)
            {
                Controls.RemoveAt(controlsToDel);
            }
            currentPlayer.currentBet.RemoveAt(countOfBets - 1);
            countOfBets = currentPlayer.currentBet.Count;
        }
    }

    private void Rebet_Click(object sender, EventArgs e)
    {
        this.Figure.Visible = false;
        this.tWinning.Text = "0";
        currentPlayer.totalBet = 0;
        currentPlayer.currentBet = new List<Bet>(currentPlayer.savedBetToRebet);
        foreach (var item in currentPlayer.currentBet)
        {
            item.chip.chipBox.Location = new System.Drawing.Point((int)item.currentBox.Left + item.currentBox.Width / 2 - item.chip.chipBox.Width / 2, (int)item.currentBox.Top + item.currentBox.Height / 2 - item.chip.chipBox.Height / 2);
            this.Controls.Add(item.chip.chipBox);
            currentPlayer.totalBet += item.chip.valueDigit;
        }
        currentPlayer.balance-=currentPlayer.totalBet;
        this.tBalance.Text = currentPlayer.balance.ToString();
        this.tTotalbet.Text = currentPlayer.totalBet.ToString();
        flagWasReBetClick = true;
    }


   private void setFigureToWinBox()
    {       
        switch (winNumder)
        {
            case 0:
                this.Figure.Location = new System.Drawing.Point((int)this.Box0.Left + this.Box0.Width / 2, (int)this.Box0.Top + this.Box0.Height / 2 - this.Figure.Height);
                break;
            case 1:
                this.Figure.Location = new System.Drawing.Point((int)this.Box1.Left + this.Box1.Width / 2, (int)this.Box1.Top + this.Box1.Height / 2 - this.Figure.Height);
                break;
            case 2:
                this.Figure.Location = new System.Drawing.Point((int)this.Box2.Left + this.Box2.Width / 2, (int)this.Box2.Top + this.Box2.Height / 2 - this.Figure.Height);
                break;
            case 3:
                this.Figure.Location = new System.Drawing.Point((int)this.Box3.Left + this.Box3.Width / 2, (int)this.Box3.Top + this.Box3.Height / 2 - this.Figure.Height);
                break;
            case 4:
                this.Figure.Location = new System.Drawing.Point((int)this.Box4.Left + this.Box4.Width / 2, (int)this.Box4.Top + this.Box4.Height / 2 - this.Figure.Height);
                break;
            case 5:
                this.Figure.Location = new System.Drawing.Point((int)this.Box5.Left + this.Box5.Width / 2, (int)this.Box5.Top + this.Box5.Height / 2 - this.Figure.Height);
                break;
            case 6:
                this.Figure.Location = new System.Drawing.Point((int)this.Box6.Left + this.Box6.Width / 2, (int)this.Box6.Top + this.Box6.Height / 2 - this.Figure.Height);
                break;
            case 7:
                this.Figure.Location = new System.Drawing.Point((int)this.Box7.Left + this.Box7.Width / 2, (int)this.Box7.Top + this.Box7.Height / 2 - this.Figure.Height);
                break;
            case 8:
                this.Figure.Location = new System.Drawing.Point((int)this.Box8.Left + this.Box8.Width / 2, (int)this.Box8.Top + this.Box8.Height / 2 - this.Figure.Height);
                break;
            case 9:
                this.Figure.Location = new System.Drawing.Point((int)this.Box9.Left + this.Box9.Width / 2, (int)this.Box9.Top + this.Box9.Height / 2 - this.Figure.Height);
                break;
            case 10:
                this.Figure.Location = new System.Drawing.Point((int)this.Box10.Left + this.Box10.Width / 2, (int)this.Box10.Top + this.Box10.Height / 2 - this.Figure.Height);
                break;
            case 11:
                this.Figure.Location = new System.Drawing.Point((int)this.Box11.Left + this.Box11.Width / 2, (int)this.Box11.Top + this.Box11.Height / 2 - this.Figure.Height);
                break;
            case 12:
                this.Figure.Location = new System.Drawing.Point((int)this.Box12.Left + this.Box12.Width / 2, (int)this.Box12.Top + this.Box12.Height / 2 - this.Figure.Height);
                break;
            case 13:
                this.Figure.Location = new System.Drawing.Point((int)this.Box13.Left + this.Box13.Width / 2, (int)this.Box13.Top + this.Box13.Height / 2 - this.Figure.Height);
                break;
            case 14:
                this.Figure.Location = new System.Drawing.Point((int)this.Box14.Left + this.Box14.Width / 2, (int)this.Box14.Top + this.Box14.Height / 2 - this.Figure.Height);
                break;
            case 15:
                this.Figure.Location = new System.Drawing.Point((int)this.Box15.Left + this.Box15.Width / 2, (int)this.Box15.Top + this.Box15.Height / 2 - this.Figure.Height);
                break;
            case 16:
                this.Figure.Location = new System.Drawing.Point((int)this.Box16.Left + this.Box16.Width / 2, (int)this.Box16.Top + this.Box16.Height / 2 - this.Figure.Height);
                break;
            case 17:
                this.Figure.Location = new System.Drawing.Point((int)this.Box17.Left + this.Box17.Width / 2, (int)this.Box17.Top + this.Box17.Height / 2 - this.Figure.Height);
                break;
            case 18:
                this.Figure.Location = new System.Drawing.Point((int)this.Box18.Left + this.Box18.Width / 2, (int)this.Box18.Top + this.Box18.Height / 2 - this.Figure.Height);
                break;
            case 19:
                this.Figure.Location = new System.Drawing.Point((int)this.Box19.Left + this.Box19.Width / 2, (int)this.Box19.Top + this.Box19.Height / 2 - this.Figure.Height);
                break;
            case 20:
                this.Figure.Location = new System.Drawing.Point((int)this.Box20.Left + this.Box20.Width / 2, (int)this.Box20.Top + this.Box20.Height / 2 - this.Figure.Height);
                break;
            case 21:
                this.Figure.Location = new System.Drawing.Point((int)this.Box21.Left + this.Box21.Width / 2, (int)this.Box21.Top + this.Box21.Height / 2 - this.Figure.Height);
                break;
            case 22:
                this.Figure.Location = new System.Drawing.Point((int)this.Box22.Left + this.Box22.Width / 2, (int)this.Box22.Top + this.Box22.Height / 2 - this.Figure.Height);
                break;
            case 23:
                this.Figure.Location = new System.Drawing.Point((int)this.Box23.Left + this.Box23.Width / 2, (int)this.Box23.Top + this.Box23.Height / 2 - this.Figure.Height);
                break;
            case 24:
                this.Figure.Location = new System.Drawing.Point((int)this.Box24.Left + this.Box24.Width / 2, (int)this.Box24.Top + this.Box24.Height / 2 - this.Figure.Height);
                break;
            case 25:
                this.Figure.Location = new System.Drawing.Point((int)this.Box25.Left + this.Box25.Width / 2, (int)this.Box25.Top + this.Box25.Height / 2 - this.Figure.Height);
                break;
            case 26:
                this.Figure.Location = new System.Drawing.Point((int)this.Box26.Left + this.Box26.Width / 2, (int)this.Box26.Top + this.Box26.Height / 2 - this.Figure.Height);
                break;
            case 27:
                this.Figure.Location = new System.Drawing.Point((int)this.Box27.Left + this.Box27.Width / 2, (int)this.Box27.Top + this.Box27.Height / 2 - this.Figure.Height);
                break;
            case 28:
                this.Figure.Location = new System.Drawing.Point((int)this.Box28.Left + this.Box28.Width / 2, (int)this.Box28.Top + this.Box28.Height / 2 - this.Figure.Height);
                break;
            case 29:
                this.Figure.Location = new System.Drawing.Point((int)this.Box29.Left + this.Box29.Width / 2, (int)this.Box29.Top + this.Box29.Height / 2 - this.Figure.Height);
                break;
            case 30:
                this.Figure.Location = new System.Drawing.Point((int)this.Box30.Left + this.Box30.Width / 2, (int)this.Box30.Top + this.Box30.Height / 2 - this.Figure.Height);
                break;
            case 31:
                this.Figure.Location = new System.Drawing.Point((int)this.Box31.Left + this.Box31.Width / 2, (int)this.Box31.Top + this.Box31.Height / 2 - this.Figure.Height);
                break;
            case 32:
                this.Figure.Location = new System.Drawing.Point((int)this.Box32.Left + this.Box32.Width / 2, (int)this.Box32.Top + this.Box32.Height / 2 - this.Figure.Height);
                break;
            case 33:
                this.Figure.Location = new System.Drawing.Point((int)this.Box33.Left + this.Box33.Width / 2, (int)this.Box33.Top + this.Box33.Height / 2 - this.Figure.Height);
                break;
            case 34:
                this.Figure.Location = new System.Drawing.Point((int)this.Box34.Left + this.Box34.Width / 2, (int)this.Box34.Top + this.Box34.Height / 2 - this.Figure.Height);
                break;
            case 35:
                this.Figure.Location = new System.Drawing.Point((int)this.Box35.Left + this.Box35.Width / 2, (int)this.Box35.Top + this.Box35.Height / 2 - this.Figure.Height);
                break;
            case 36:
                this.Figure.Location = new System.Drawing.Point((int)this.Box36.Left + this.Box36.Width / 2, (int)this.Box36.Top + this.Box36.Height / 2 - this.Figure.Height);
                break;
        }

    }


     private void GetChips_Click_1(object sender, EventArgs e)
    {
        if (currentPlayer.balance < 1000 && currentPlayer.currentBet.Count==0)
        {
            currentPlayer.balance = 1000;
            this.tBalance.Text = currentPlayer.balance.ToString();
        }
    }

     private void Chip5_Click(object sender, EventArgs e)
     {
         currentPlayer.currentChip.valueDigit = 5;
         currentPlayer.currentChip.chipBox.BackgroundImage = Chip5.BackgroundImage;
     }

     private void Chip25_Click(object sender, EventArgs e)
     {
         currentPlayer.currentChip.valueDigit = 25;
         currentPlayer.currentChip.chipBox.BackgroundImage = Chip25.BackgroundImage;
     }

     private void Chip50_Click(object sender, EventArgs e)
     {
         currentPlayer.currentChip.valueDigit = 50;
         currentPlayer.currentChip.chipBox.BackgroundImage = Chip50.BackgroundImage;
     }

     private void Chip100_Click(object sender, EventArgs e)
     {
         currentPlayer.currentChip.valueDigit = 100;
         currentPlayer.currentChip.chipBox.BackgroundImage = Chip100.BackgroundImage;
     }

     private void Chip500_Click(object sender, EventArgs e)
     {
         currentPlayer.currentChip.valueDigit = 500;
         currentPlayer.currentChip.chipBox.BackgroundImage = Chip500.BackgroundImage;
     }

     private void Box_MouseMove(object sender, MouseEventArgs e)
     {
         (sender as PictureBox).BorderStyle = BorderStyle.FixedSingle;
         Thread.Sleep(5);
     }
   
        private void boxBorderStyleNone()
        {
            foreach (Control item in Controls)
            {
               if (item is PictureBox)
                {
                    (item as PictureBox).BorderStyle = BorderStyle.None;                
                }
            }

        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
         {
            int index = currentPlayer.currentBet.Count;
                 foreach (var item in currentPlayer.currentBet)
                {
                    item.chip.chipBox.BringToFront();
                }               
         }
            

        private void Box_MouseLeave(object sender, EventArgs e)
     {
         Thread.Sleep(5);
         boxBorderStyleNone();
         (sender as PictureBox).BorderStyle = BorderStyle.None;
       
     }


     private void Row1_2_3_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box1.BringToFront();
         this.Box2.BringToFront();
         this.Box3.BringToFront();
         this.Box1.BorderStyle = BorderStyle.FixedSingle;
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
         this.Box3.BorderStyle = BorderStyle.FixedSingle;
         this.Box1.SendToBack();
         this.Box2.SendToBack();
         this.Box3.SendToBack();
     }

     private void Row1_2_3_4_5_6_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box1.BringToFront();
         this.Box2.BringToFront();
         this.Box3.BringToFront();
         this.Box4.BringToFront();
         this.Box5.BringToFront();
         this.Box6.BringToFront();
         this.Box1.BorderStyle = BorderStyle.FixedSingle;
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
         this.Box3.BorderStyle = BorderStyle.FixedSingle;
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box6.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row4_5_6_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box4.BringToFront();
         this.Box5.BringToFront();
         this.Box6.BringToFront();
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box6.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row4_5_6_7_8_9_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box5.BringToFront();
         this.Box6.BringToFront();
         this.Box7.BringToFront();
         this.Box8.BringToFront();
         this.Box9.BringToFront();
         this.Box4.BringToFront();
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box6.BorderStyle = BorderStyle.FixedSingle;
         this.Box7.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box9.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row7_8_9_MouseMove(object sender, MouseEventArgs e)
     {

         this.Box7.BringToFront();
         this.Box8.BringToFront();
         this.Box9.BringToFront();
         this.Box7.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box9.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row7_8_9_10_11_12_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box7.BringToFront();
         this.Box8.BringToFront();
         this.Box9.BringToFront();
         this.Box10.BringToFront();
         this.Box11.BringToFront();
         this.Box12.BringToFront();
         this.Box7.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box9.BorderStyle = BorderStyle.FixedSingle;
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box12.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row10_11_12_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box10.BringToFront();
         this.Box11.BringToFront();
         this.Box12.BringToFront();
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box12.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row10_11_12_13_14_15_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box10.BringToFront();
         this.Box11.BringToFront();
         this.Box12.BringToFront();
         this.Box13.BringToFront();
         this.Box14.BringToFront();
         this.Box15.BringToFront();
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box12.BorderStyle = BorderStyle.FixedSingle;
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box15.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row13_14_15_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box13.BringToFront();
         this.Box14.BringToFront();
         this.Box15.BringToFront();
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box15.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row13_14_15_16_17_18_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box13.BringToFront();
         this.Box14.BringToFront();
         this.Box15.BringToFront();
         this.Box16.BringToFront();
         this.Box17.BringToFront();
         this.Box18.BringToFront();
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box15.BorderStyle = BorderStyle.FixedSingle;
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box18.BorderStyle = BorderStyle.FixedSingle;

     }

     private void Row16_17_18_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box16.BringToFront();
         this.Box17.BringToFront();
         this.Box18.BringToFront();
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box18.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row16_17_18_19_20_21_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box16.BringToFront();
         this.Box17.BringToFront();
         this.Box18.BringToFront();
         this.Box19.BringToFront();
         this.Box20.BringToFront();
         this.Box21.BringToFront();
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box18.BorderStyle = BorderStyle.FixedSingle;
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box21.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row19_20_21_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box19.BringToFront();
         this.Box20.BringToFront();
         this.Box21.BringToFront();
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box21.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row19_20_21_22_23_24_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box19.BringToFront();
         this.Box20.BringToFront();
         this.Box21.BringToFront();
         this.Box22.BringToFront();
         this.Box23.BringToFront();
         this.Box24.BringToFront();
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box21.BorderStyle = BorderStyle.FixedSingle;
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box24.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row22_23_24_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box22.BringToFront();
         this.Box23.BringToFront();
         this.Box24.BringToFront();
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box24.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row22_23_24_25_26_27_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box22.BringToFront();
         this.Box23.BringToFront();
         this.Box24.BringToFront();
         this.Box25.BringToFront();
         this.Box26.BringToFront();
         this.Box27.BringToFront();
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box24.BorderStyle = BorderStyle.FixedSingle;
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box27.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row25_26_27_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box25.BringToFront();
         this.Box26.BringToFront();
         this.Box27.BringToFront();
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box27.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row25_26_27_28_29_30_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box25.BringToFront();
         this.Box26.BringToFront();
         this.Box27.BringToFront();
         this.Box28.BringToFront();
         this.Box29.BringToFront();
         this.Box30.BringToFront();
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box27.BorderStyle = BorderStyle.FixedSingle;
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box30.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row28_29_30_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box28.BringToFront();
         this.Box29.BringToFront();
         this.Box30.BringToFront();
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box30.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row28_29_30_31_32_33_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box28.BringToFront();
         this.Box29.BringToFront();
         this.Box30.BringToFront();
         this.Box31.BringToFront();
         this.Box32.BringToFront();
         this.Box33.BringToFront();
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box30.BorderStyle = BorderStyle.FixedSingle;
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box33.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row31_32_33_Move(object sender, EventArgs e)
     {
         this.Box31.BringToFront();
         this.Box32.BringToFront();
         this.Box33.BringToFront();
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box33.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row31_32_33_34_35_36_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box31.BringToFront();
         this.Box32.BringToFront();
         this.Box33.BringToFront();
         this.Box34.BringToFront();
         this.Box35.BringToFront();
         this.Box36.BringToFront();
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box33.BorderStyle = BorderStyle.FixedSingle;
         this.Box34.BorderStyle = BorderStyle.FixedSingle;
         this.Box35.BorderStyle = BorderStyle.FixedSingle;
         this.Box36.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Row34_35_36_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box34.BringToFront();
         this.Box35.BringToFront();
         this.Box36.BringToFront();
         this.Box34.BorderStyle = BorderStyle.FixedSingle;
         this.Box35.BorderStyle = BorderStyle.FixedSingle;
         this.Box36.BorderStyle = BorderStyle.FixedSingle;
     }

   

     private void Box1x2_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box1.BringToFront();
         this.Box2.BringToFront();
        this.Box1.BorderStyle = BorderStyle.FixedSingle;
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box0x1_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box0.BringToFront();
         this.Box1.BringToFront();
         this.Box0.BorderStyle = BorderStyle.FixedSingle;
         this.Box1.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box1x4_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box1.BringToFront();
         this.Box4.BringToFront();
         this.Box1.BorderStyle = BorderStyle.FixedSingle;
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box4x7_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box4.BringToFront();
         this.Box7.BringToFront();
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
         this.Box7.BorderStyle = BorderStyle.FixedSingle;

     }

     private void Box7x10_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box7.BringToFront();
         this.Box10.BringToFront();
         this.Box7.BorderStyle = BorderStyle.FixedSingle;
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box10x13_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box10.BringToFront();
         this.Box13.BringToFront();
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box13x16_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box13.BringToFront();
         this.Box16.BringToFront();
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box16x19_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box16.BringToFront();
         this.Box19.BringToFront();
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box19x22_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box19.BringToFront();
         this.Box22.BringToFront();
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box22x25_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box22.BringToFront();
         this.Box25.BringToFront();
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box25x28_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box25.BringToFront();
         this.Box28.BringToFront();
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box28x31_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box28.BringToFront();
         this.Box31.BringToFront();
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box31x34_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box31.BringToFront();
         this.Box34.BringToFront();
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
         this.Box34.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box0x1x2_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box0.BringToFront();
         this.Box1.BringToFront();
         this.Box2.BringToFront();
         this.Box0.BorderStyle = BorderStyle.FixedSingle;
         this.Box1.BorderStyle = BorderStyle.FixedSingle;
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box1x2x4x5_MouseMove(object sender, MouseEventArgs e)
     {

         this.Box1.BringToFront();
         this.Box2.BringToFront();
         this.Box4.BringToFront();
         this.Box5.BringToFront();
         this.Box1.BorderStyle = BorderStyle.FixedSingle;
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box4x5_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box4.BringToFront();
         this.Box5.BringToFront();
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;

     }

     private void Box4x5x7x8_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box4.BringToFront();
         this.Box5.BringToFront();
         this.Box7.BringToFront();
         this.Box8.BringToFront();
         this.Box4.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box7.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box7x8_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box7.BringToFront();
         this.Box8.BringToFront();
         this.Box7.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box7x8x10x11_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box7.BringToFront();
         this.Box8.BringToFront();
         this.Box10.BringToFront();
         this.Box11.BringToFront();
         this.Box7.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box10x11_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box10.BringToFront();
         this.Box11.BringToFront();
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box10x11x13x14_MouseMove(object sender, MouseEventArgs e)
     {

         this.Box10.BringToFront();
         this.Box11.BringToFront();
         this.Box13.BringToFront();
         this.Box14.BringToFront();
         this.Box10.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box13x14_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box13.BringToFront();
         this.Box14.BringToFront();
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box13x14x16x17_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box13.BringToFront();
         this.Box14.BringToFront();
         this.Box16.BringToFront();
         this.Box17.BringToFront();
         this.Box13.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box16x17_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box16.BringToFront();
         this.Box17.BringToFront();
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box16x17x19x20_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box16.BringToFront();
         this.Box17.BringToFront();
         this.Box19.BringToFront();
         this.Box20.BringToFront();
         this.Box16.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box19x20_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box19.BringToFront();
         this.Box20.BringToFront();
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box19x20x22x23_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box19.BringToFront();
         this.Box20.BringToFront();
         this.Box22.BringToFront();
         this.Box23.BringToFront();
         this.Box19.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box22x23_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box22.BringToFront();
         this.Box23.BringToFront();
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box22x23x25x26_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box22.BringToFront();
         this.Box23.BringToFront();
         this.Box25.BringToFront();
         this.Box26.BringToFront();
         this.Box22.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box25x26_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box25.BringToFront();
         this.Box26.BringToFront();
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box25x26x28x29_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box25.BringToFront();
         this.Box26.BringToFront();
         this.Box28.BringToFront();
         this.Box29.BringToFront();
         this.Box25.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box28x29_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box28.BringToFront();
         this.Box29.BringToFront();
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box28x29x31x32_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box28.BringToFront();
         this.Box29.BringToFront();
         this.Box31.BringToFront();
         this.Box32.BringToFront();
         this.Box28.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box31x32_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box31.BringToFront();
         this.Box32.BringToFront();
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box31x32x34x35_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box31.BringToFront();
         this.Box32.BringToFront();
         this.Box34.BringToFront();
         this.Box35.BringToFront();
         this.Box31.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box34.BorderStyle = BorderStyle.FixedSingle;
         this.Box35.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box34x35_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box34.BringToFront();
         this.Box35.BringToFront();
         this.Box34.BorderStyle = BorderStyle.FixedSingle;
         this.Box35.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box2x5_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box2.BringToFront();
         this.Box5.BringToFront();
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box5x8_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box5.BringToFront();
         this.Box8.BringToFront();
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box8x11_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box8.BringToFront();
         this.Box11.BringToFront();
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box11x14_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box11.BringToFront();
         this.Box14.BringToFront();
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box14x17_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box14.BringToFront();
         this.Box17.BringToFront();
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box17x20_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box17.BringToFront();
         this.Box20.BringToFront();
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box20x23_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box20.BringToFront();
         this.Box23.BringToFront();
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box23x26_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box23.BringToFront();
         this.Box26.BringToFront();
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box26x29_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box26.BringToFront();
         this.Box29.BringToFront();
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box29x32_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box29.BringToFront();
         this.Box32.BringToFront();
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box32x35_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box32.BringToFront();
         this.Box35.BringToFront();
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box35.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box0x2x3_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box0.BringToFront();
         this.Box2.BringToFront();
         this.Box3.BringToFront();
         this.Box0.BorderStyle = BorderStyle.FixedSingle;
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
         this.Box3.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box2x3_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box2.BringToFront();
         this.Box3.BringToFront();
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
         this.Box3.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box2x3x5x6_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box2.BringToFront();
         this.Box3.BringToFront();
         this.Box5.BringToFront();
         this.Box6.BringToFront();
         this.Box2.BorderStyle = BorderStyle.FixedSingle;
         this.Box3.BorderStyle = BorderStyle.FixedSingle;
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box6.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box5x6_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box5.BringToFront();
         this.Box6.BringToFront();
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box6.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box5x6x8x9_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box5.BringToFront();
         this.Box6.BringToFront();
         this.Box8.BringToFront();
         this.Box9.BringToFront();
         this.Box5.BorderStyle = BorderStyle.FixedSingle;
         this.Box6.BorderStyle = BorderStyle.FixedSingle;
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box9.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box8x9_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box8.BringToFront();
         this.Box9.BringToFront();
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box9.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box8x9x11x12_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box8.BringToFront();
         this.Box9.BringToFront();
         this.Box11.BringToFront();
         this.Box12.BringToFront();
         this.Box8.BorderStyle = BorderStyle.FixedSingle;
         this.Box9.BorderStyle = BorderStyle.FixedSingle;
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box12.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box11x12_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box11.BringToFront();
         this.Box12.BringToFront();
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box12.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box11x12x14x15_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box11.BringToFront();
         this.Box12.BringToFront();
         this.Box14.BringToFront();
         this.Box15.BringToFront();
         this.Box11.BorderStyle = BorderStyle.FixedSingle;
         this.Box12.BorderStyle = BorderStyle.FixedSingle;
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box15.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box14x15_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box14.BringToFront();
         this.Box15.BringToFront();
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box15.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box14x15x17x18_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box14.BringToFront();
         this.Box15.BringToFront();
         this.Box17.BringToFront();
         this.Box18.BringToFront();
         this.Box14.BorderStyle = BorderStyle.FixedSingle;
         this.Box15.BorderStyle = BorderStyle.FixedSingle;
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box18.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box17x18_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box17.BringToFront();
         this.Box18.BringToFront();
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box18.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box17x18x20x21_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box17.BringToFront();
         this.Box18.BringToFront();
         this.Box20.BringToFront();
         this.Box21.BringToFront();
         this.Box17.BorderStyle = BorderStyle.FixedSingle;
         this.Box18.BorderStyle = BorderStyle.FixedSingle;
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box21.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box20x21_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box12.BringToFront();
         this.Box21.BringToFront();
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box21.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box20x21x23x24_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box20.BringToFront();
         this.Box21.BringToFront();
         this.Box23.BringToFront();
         this.Box24.BringToFront();
         this.Box20.BorderStyle = BorderStyle.FixedSingle;
         this.Box21.BorderStyle = BorderStyle.FixedSingle;
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box24.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box23x24_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box23.BringToFront();
         this.Box24.BringToFront();
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box24.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box23x24x26x27_MouseMove(object sender, MouseEventArgs e)
     {

         this.Box23.BringToFront();
         this.Box24.BringToFront();
         this.Box26.BringToFront();
         this.Box27.BringToFront();
         this.Box23.BorderStyle = BorderStyle.FixedSingle;
         this.Box24.BorderStyle = BorderStyle.FixedSingle;
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box27.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box26x27_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box26.BringToFront();
         this.Box27.BringToFront();
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box27.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box26x27x29x30_MouseMove(object sender, MouseEventArgs e)
     {

         this.Box26.BringToFront();
         this.Box27.BringToFront();
         this.Box29.BringToFront();
         this.Box30.BringToFront();
         this.Box26.BorderStyle = BorderStyle.FixedSingle;
         this.Box27.BorderStyle = BorderStyle.FixedSingle;
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box30.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box29x30_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box29.BringToFront();
         this.Box30.BringToFront();
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box30.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box29x30x32x33_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box29.BringToFront();
         this.Box30.BringToFront();
         this.Box32.BringToFront();
         this.Box33.BringToFront();
         this.Box29.BorderStyle = BorderStyle.FixedSingle;
         this.Box30.BorderStyle = BorderStyle.FixedSingle;
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box33.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box32x33_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box32.BringToFront();
         this.Box33.BringToFront();
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box33.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box32x33x35x36_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box32.BringToFront();
         this.Box33.BringToFront();
         this.Box35.BringToFront();
         this.Box36.BringToFront();
         this.Box32.BorderStyle = BorderStyle.FixedSingle;
         this.Box33.BorderStyle = BorderStyle.FixedSingle;
         this.Box35.BorderStyle = BorderStyle.FixedSingle;
         this.Box36.BorderStyle = BorderStyle.FixedSingle;
     }

     private void Box35x36_MouseMove(object sender, MouseEventArgs e)
     {
         this.Box35.BringToFront();
         this.Box36.BringToFront();
         this.Box35.BorderStyle = BorderStyle.FixedSingle;
         this.Box36.BorderStyle = BorderStyle.FixedSingle;
     }

     

    

  

    

    

    }
}


