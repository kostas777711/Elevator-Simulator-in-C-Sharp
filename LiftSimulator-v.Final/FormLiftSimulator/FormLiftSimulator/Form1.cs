using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Media;
using System.Threading;
using Controller1;
using Generator;


namespace FormLiftSimulator
{
    public partial class Form1 : Form
    {
        //Second Basic Controller Thread
        Thread controllerThread;
        Thread generatorThread;

        //If i don't put System.Windows.Forms I ll have an error, because there are 2 Timer Classes (Threading & Forms)
        //Timer Objects
        System.Windows.Forms.Timer myTimer1;
        System.Windows.Forms.Timer myTimer2;
        System.Windows.Forms.Timer myTimer3;
        
        //Controller and EventGenerator Objects
        Controller myController;
        EventGenerator myEventGenerator;

        //Constants of the Location.Y's of the floors
        private const int FLOOR3 = 50;
        private const int FLOOR2 = 200;
        private const int FLOOR1 = 350;
        private const int GROUND = 500;

//--variables for the Event Generator--------------//

        //Generator State
        public string generatorState;  //{"PLAY","PAUSE"} 2-States

        //Generator Meant Event Time & 
        public int meantTime = 1000;   //initial mean time value
        public int  counter= 0;
        public int previousbuttonNumber = 10;  //here this initialization , and 10 because is a ground request, so i don't want a request like this because the lift starts from the ground floor 

//-------------End-of-Declarations----------------------//

//--------------------Start----------------------------------//


        public Form1()
        {
            InitializeComponent();  
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Initializing Pictures and Buttons

            //                                                              //
            //                                                              //
            //                                                              //
            // Change the links of the images and the wav to run the program//
            //                                                              //
            //                                                              //
            //                                                              //

            pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\blue.png");
            button10.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
            button9.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
            button7.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
            button5.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
            button6.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
            button8.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
            
            //play welcome wav "welcome to the simulator"
            playWelcome();

            //initializing the labels in the lift and on the floors
            label5.Text = "0";
            label13.Text = "0";
            label14.Text = "0";
            label15.Text = "0";
            label16.Text = "0";

            //Initializing Generator's State
            generatorState = "PAUSE";
            
            //The two other basic objects of the project(Controller and EventGenerator)
            myController = new Controller();
            myEventGenerator = new EventGenerator();

            //Setting up the timers
            myTimer1 = new System.Windows.Forms.Timer();
            myTimer1.Interval = 25;  //steady delay for timer 1
            myTimer1.Tick += timer1_Tick;
            myTimer2 = new System.Windows.Forms.Timer();
            myTimer2.Interval = 25;   //steady delay for timer 2
            myTimer2.Tick += timer2_Tick;
            myTimer3 = new System.Windows.Forms.Timer();
            myTimer3.Interval = meantTime;   //adjustable delay for timer 3
            myTimer3.Tick += timer3_Tick;
            

            //Type First given speed value 
            label7.Text = Convert.ToString(myController.speed);

            //Controller Thread is executing in parallel with the Form Code
            controllerThread = new Thread(new ThreadStart(myController.ControllerMethod));
            controllerThread.Start();

            //initialize the directionFlag
            myController.directionFlag = "STABLE";
        }

//-----------------------------Timers----------------Timers----------------------------------------//
    //-----------------------------Timers-------------------Timers-----------------------------------//
    

        //------------------------Timer1--------------------------//
        //------------------------Timer1--------------------------//

                            //----Going-Up----//

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (myController.permission == 3)
            {
                
                if (panel1.Top > FLOOR3)
                {
                        panel1.Top -= myController.speed;
                        resetLabels();
                        pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                        myController.directionFlag = "UP";
                        myController.flagy4 = 0;  //reset this flag so it can be inside this loop only once
                        myController.flagy1 = 0;
                }
                if(panel1.Top == FLOOR3)
                { 
                    myTimer1.Stop();
                    myController.userThirdFloorRequestFlag = false;
                    myController.floorThirdFloorRequestFlag = false;
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\blue.png");
                    button4.ForeColor = Color.Black;
                    
                    myController.directionFlag = "STABLE";
                    button10.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
                    SetLabelsAndPlayWav();
                    Application.DoEvents(); //better response of the form application for displaying everuthing and especially the labels that are displaying the current floor
                    Thread.Sleep(700); //delay in each station

                    //so when the lift stops in the 3rd check which requests are still on (all for down movement) and call timer2
                    //implementing memory with these last if statements
                    if (myController.floorSecondFloorRequestFlagDown || myController.floorSecondFloorRequestFlagUp || myController.userSecondFloorRequestFlag
                        || myController.userFirstFloorRequestFlag || myController.floorFirstFloorRequestFlagDown || myController.floorFirstFloorRequestFlagUp
                        || myController.userGroundRequestFlag || myController.floorGroundRequestFlag)
                    {
                      myTimer2.Start();
                    }
                    
                }
            }
            if (myController.permission == 1)
                 
            {
                 if (panel1.Top > FLOOR1)
                {
                    
                    panel1.Top -= myController.speed;
                    resetLabels();
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                    myController.directionFlag = "UP";      
                }
                 if (panel1.Top == FLOOR1)
                {
                    myTimer1.Stop();
                    myController.userFirstFloorRequestFlag = false;
                    myController.floorFirstFloorRequestFlagDown = false;
                    myController.floorFirstFloorRequestFlagUp = false;
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\blue.png");
                    button3.ForeColor = Color.Black;
                    
                    myController.directionFlag = "STABLE";
                    button7.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
                    button6.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
                    if ((myController.userThirdFloorRequestFlag || myController.floorThirdFloorRequestFlag
                        || myController.userSecondFloorRequestFlag || myController.floorSecondFloorRequestFlagUp)
                        && (myController.flag == 3))
                    {
                        myController.floorFirstFloorRequestFlagDown = true;
                        button7.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                    }
                    myController.flag = 0;
                    SetLabelsAndPlayWav();
                    Application.DoEvents(); //better response of the form application for displaying everuthing and especially the labels that are displaying the current floor
                    Thread.Sleep(700);

                     //when the lift stops in first check if there are requests in the same direction to continue, else change direction
                    if (myController.userThirdFloorRequestFlag || myController.floorThirdFloorRequestFlag
                        || myController.userSecondFloorRequestFlag || myController.floorSecondFloorRequestFlagUp || myController.floorSecondFloorRequestFlagDown)
                    {
                       myTimer1.Start();
                    }
                    else if (myController.userGroundRequestFlag || myController.floorGroundRequestFlag)
                    {
                        myTimer2.Start();
                    }
                    
                }
            }
            if (myController.permission==2)
            {
                if (panel1.Top > FLOOR2)
                {
                    panel1.Top -= myController.speed;
                    resetLabels();
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                    myController.directionFlag = "UP";
                    myController.flagy1 = 0;  //reset this flag so it can be inside this loop only once
                }
                if (panel1.Top == FLOOR2)
                {
                    myTimer1.Stop();
                    myController.userSecondFloorRequestFlag = false;
                    myController.floorSecondFloorRequestFlagDown = false;
                    myController.floorSecondFloorRequestFlagUp = false;
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\blue.png");
                    button2.ForeColor = Color.Black;
                    myController.directionFlag = "STABLE";
                    button9.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
                    button8.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
                    if ((myController.floorThirdFloorRequestFlag || myController.userThirdFloorRequestFlag)
                        && (myController.flag == 2))    
                    {
                        myController.floorSecondFloorRequestFlagDown = true;
                        button9.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                    }
                    myController.flag = 0;
                    SetLabelsAndPlayWav();
                    Application.DoEvents(); //better response of the form application for displaying everything and especially the labels that are displaying the current floor
                    Thread.Sleep(700); //pause to the Current floor


                    //when the lift stops in 2nd check if there are requests in the same direction to continue, else change direction
                    if (myController.userThirdFloorRequestFlag || myController.floorThirdFloorRequestFlag)    
                    {
                       myTimer1.Start();
                    }
                    else if (myController.userFirstFloorRequestFlag || myController.floorFirstFloorRequestFlagDown || myController.floorFirstFloorRequestFlagUp
                        || myController.userGroundRequestFlag || myController.floorGroundRequestFlag)
                    {
                        myTimer2.Start();
                    }
                }
            }

        }

        //------------------------Timer2--------------------------//
        //------------------------Timer2--------------------------//

                             //--Going-Down--//

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            if (myController.permission==0)
            {
                if (panel1.Top < GROUND)
                {
                    panel1.Top += myController.speed;
                    resetLabels();
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                    myController.directionFlag = "DOWN";
                    myController.flagy3 = 0;
                    myController.flagy2 = 0;
                }
                if (panel1.Top == GROUND)
                {
                    myTimer2.Stop();
                    myController.userGroundRequestFlag = false;
                    myController.floorGroundRequestFlag = false;
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\blue.png");
                    button1.ForeColor = Color.Black;
                    myController.directionFlag = "STABLE";
                    button5.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
                    SetLabelsAndPlayWav();
                    Application.DoEvents(); //better response of the form application for displaying everuthing and especially the labels that are displaying the current floor
                    Thread.Sleep(700);//delay

                    //ground floor now, so check if other requests are active to move upwards
                    if(myController.userThirdFloorRequestFlag || myController.floorThirdFloorRequestFlag
                        || myController.userSecondFloorRequestFlag || myController.floorSecondFloorRequestFlagUp || myController.floorSecondFloorRequestFlagDown
                        || myController.userFirstFloorRequestFlag || myController.floorFirstFloorRequestFlagUp || myController.floorFirstFloorRequestFlagDown)
                    {
                        myTimer1.Start();
                    }

                }
            }


            if (myController.permission==1)
            {
                if (panel1.Top < FLOOR1)
                {
                    panel1.Top += myController.speed;
                    resetLabels();
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                    myController.directionFlag = "DOWN";
                    myController.flagy2 = 0;
                }
                if (panel1.Top == FLOOR1)
                {
                    myTimer2.Stop();
                    myController.userFirstFloorRequestFlag = false;
                    myController.floorFirstFloorRequestFlagDown = false;
                    myController.floorFirstFloorRequestFlagUp = false;
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\blue.png");
                    button3.ForeColor = Color.Black;
                    myController.directionFlag = "STABLE";
                    button7.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
                    button6.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
                    if ((myController.floorGroundRequestFlag || myController.userGroundRequestFlag)
                       && (myController.flag == 1))
                    {
                        myController.floorFirstFloorRequestFlagUp = true;
                        button6.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                    }
                    myController.flag = 0;
                    SetLabelsAndPlayWav();
                    Application.DoEvents();//better response of the form application for displaying everuthing and especially the labels that are displaying the current floor
                    Thread.Sleep(700);  //pause in each floor for 0.5 sec
                   
                    //lift in the 1st, check if there are requests from below to continue in the same direction(down), else change direction
                    if (myController.userGroundRequestFlag || myController.floorGroundRequestFlag)
                    {
                        myTimer2.Start();
                    }
                    else if (myController.userSecondFloorRequestFlag || myController.floorSecondFloorRequestFlagDown || myController.floorSecondFloorRequestFlagUp
                        || myController.userThirdFloorRequestFlag || myController.floorThirdFloorRequestFlag)
                    {
                        myTimer1.Start();
                    }
                }
            }
            if (myController.permission==2)
            {
                if (panel1.Top < FLOOR2)
                {
                    panel1.Top += myController.speed;
                    resetLabels();
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                    myController.directionFlag = "DOWN";     
                }
                if (panel1.Top == FLOOR2)
                {
                    myTimer2.Stop();
                    myController.userSecondFloorRequestFlag = false;
                    myController.floorSecondFloorRequestFlagDown = false;
                    myController.floorSecondFloorRequestFlagUp = false;
                    pictureBox1.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\blue.png");
                    button2.ForeColor = Color.Black;
                    myController.directionFlag = "STABLE";
                    button9.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downLightBlue.png");
                    button8.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upLightBlue.png");
                    if ((myController.userGroundRequestFlag || myController.floorGroundRequestFlag
                        || myController.userFirstFloorRequestFlag || myController.floorFirstFloorRequestFlagDown)
                        && (myController.flag == 4))
                    {
                        myController.floorSecondFloorRequestFlagUp = true;
                        button8.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                    }
                    myController.flag = 0;
                    SetLabelsAndPlayWav();
                    Application.DoEvents();
                    Thread.Sleep(700);//delay

                    //lift in the 2nd, check if there are requests from below to continue in the same direction(down), else change direction
                    if (myController.userFirstFloorRequestFlag || myController.floorFirstFloorRequestFlagDown || myController.floorFirstFloorRequestFlagUp
                        || myController.userGroundRequestFlag || myController.floorGroundRequestFlag)
                    {
                        myTimer2.Start();
                    }
                    else if(myController.userThirdFloorRequestFlag || myController.floorThirdFloorRequestFlag)
                    {
                        myTimer1.Start();
                    }
                    
                }
            }
        }

        //---------------timer3---------------------//
        //---------------timer3---------------------//

               //----For-the-Generator-------//

        //I came up with this solution (i mean using Timer) for the Event Generator because
        //I couldn't use the while loop (while(generatorState=="PLAY"){//do these;}
        //I couldn't follow the way that i program the event generator in the DllLiftSimulator

        private void timer3_Tick(object sender, EventArgs e)
        {

            
                myTimer3.Interval = meantTime;  //updating interval when the timer is executing
                //I want to program a filter for the random event generator (to be not so random)

                if (myEventGenerator.buttonNumber != previousbuttonNumber)     //filter 2 same choices in a row
                {

                    switch (myEventGenerator.buttonNumber)
                    {
                        case 1:
                            if (previousbuttonNumber != 6 && previousbuttonNumber != 7)
                            {
                                button2_Click(this, EventArgs.Empty);       //2nd User Request
                                counter++;
                            }
                            break;
                        case 2:
                            if (previousbuttonNumber != 8 && previousbuttonNumber != 9)
                            {
                                button3_Click(this, EventArgs.Empty);       //1st User Request
                                counter++;
                            }
                            break;
                        case 3:
                            if (previousbuttonNumber != 5)
                            {
                                button4_Click(this, EventArgs.Empty);       //3rd User Request
                                counter++;
                            }
                            break;
                        case 4:
                            if (previousbuttonNumber != 10)
                            {
                                button1_Click(this, EventArgs.Empty);      //Ground User Request
                                counter++;
                            }
                            break;
                        case 5:
                            if (previousbuttonNumber != 3)
                            {
                                button10_Click(this, EventArgs.Empty);       //3rd Floor Request
                                counter++;
                            }
                            break;
                        case 6:
                            if (previousbuttonNumber != 7 && previousbuttonNumber != 1)
                            {
                                button8_Click(this, EventArgs.Empty);      //2nd Floor Request Up
                                counter++;
                            }
                            break;
                        case 7:
                            if (previousbuttonNumber != 7 && previousbuttonNumber != 1)
                            {
                                button9_Click(this, EventArgs.Empty);       //2nd Floor Request Down
                                counter++;
                            }
                            break;
                        case 8:
                            if (previousbuttonNumber != 9 && previousbuttonNumber != 2)
                            {
                                button7_Click(this, EventArgs.Empty);     //1st Floor Request Down
                                counter++;
                            }
                            break;
                        case 9:
                            if (previousbuttonNumber != 8 && previousbuttonNumber != 2)
                            {
                                button6_Click(this, EventArgs.Empty);   //1st Floor Request Up
                                counter++;
                            }
                            break;
                        case 10:
                            if (previousbuttonNumber != 4)
                            {
                                button5_Click(this, EventArgs.Empty);   //Ground Floor Request
                                counter++;
                            }
                            break;

                    }

                }

                //Update Labels in the Event Generator Panel
                label12.Text = Convert.ToString(counter);
                label10.Text = Convert.ToString(meantTime);

                //Store random choice to previousbuttonNumber to do basic filtering of the random number generation
                previousbuttonNumber = myEventGenerator.buttonNumber;

            
        }

    //-------------------------User-Requests----------------------------------//
        //-----------------------User-Requests----------------------//

        private void button1_Click(object sender, EventArgs e)
        {
            //User Ground Floor Request
            button1.ForeColor = Color.OrangeRed;
            myController.userGroundRequestFlag = true;
            if(generatorState=="PAUSE")
                playGoingDown();
            myTimer2.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //User Floor 3 Request
            button4.ForeColor = Color.OrangeRed;
            myController.userThirdFloorRequestFlag = true;
            if (generatorState == "PAUSE")
                playGoingUp();
            myTimer1.Start();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //User Floor 1 Request
            button3.ForeColor = Color.OrangeRed;
            if (panel1.Top > FLOOR1)
            {
                myController.userFirstFloorRequestFlag = true;
                myController.flagy1 = 1;
                if (generatorState == "PAUSE")
                    playGoingUp();
                myTimer1.Start();
            }
            else if (panel1.Top < FLOOR1)
            {
                myController.userFirstFloorRequestFlag = true;
                myController.flagy3 = 3;
                if (generatorState == "PAUSE")
                    playGoingDown();
                myTimer2.Start();
            }   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //User Floor 2 Request
            button2.ForeColor = Color.OrangeRed;
            if (panel1.Top > FLOOR2)
            {
                myController.userSecondFloorRequestFlag = true;
                myController.flagy4 = 4;
                if (generatorState == "PAUSE")
                    playGoingUp();
                myTimer1.Start();
            }
            else if (panel1.Top < FLOOR2)
            {
                myController.userSecondFloorRequestFlag = true;
                myController.flagy2 = 2;
                if (generatorState == "PAUSE")
                     playGoingDown();
                myTimer2.Start();
            }
        }


        //----------------Floor-Requests------------------------------//
                //-----------Floor-Requests------------//

        private void button5_Click(object sender, EventArgs e)
        {
            //Floor Ground Request
            myController.floorGroundRequestFlag = true;
            button5.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
            if (generatorState == "PAUSE")
                playGoingDown();
            myTimer2.Start();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Floor Floor 3 Request
            myController.floorThirdFloorRequestFlag = true;
            button10.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
            if (generatorState == "PAUSE")
                 playGoingUp();
            myTimer1.Start(); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Floor Floor 1 Request Down
            if (panel1.Top > FLOOR1)
            {
                myController.floorFirstFloorRequestFlagDown = true;
                myController.flag = 3;
                button7.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                if (generatorState == "PAUSE")
                    playGoingUp();
                myTimer1.Start();
            }
            else if (panel1.Top < FLOOR1)
            {
                myController.floorFirstFloorRequestFlagDown = true;
                button7.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                myController.flagy3 = 3;
                if (generatorState == "PAUSE")
                      playGoingDown();
                myTimer2.Start();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Floor Floor 1 Request Up
            if (panel1.Top > FLOOR1)
            {
                myController.floorFirstFloorRequestFlagUp = true;
                button6.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                myController.flagy1 = 1;
                if (generatorState == "PAUSE")
                    playGoingUp();
                myTimer1.Start();
            }
            else if (panel1.Top < FLOOR1)
            {
                myController.floorFirstFloorRequestFlagUp = true;
                button6.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                myController.flag = 1;
                if (generatorState == "PAUSE")
                    playGoingDown();
                myTimer2.Start();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Floor Floor 2 Request Down
            if (panel1.Top > FLOOR2)
            {
                myController.floorSecondFloorRequestFlagDown = true;
                myController.flag = 2;
                button9.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                if (generatorState == "PAUSE")
                    playGoingUp();
                myTimer1.Start();
            }
            else if (panel1.Top < FLOOR2)
            {
                myController.floorSecondFloorRequestFlagDown = true;
                button9.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\downYellow.png");
                myController.flagy2 = 2;
                if (generatorState == "PAUSE")
                     playGoingDown();
                myTimer2.Start();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Floor Floor 2 Request Up
            if (panel1.Top > FLOOR2)
            {
                myController.floorSecondFloorRequestFlagUp = true;
                button8.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                myController.flagy4 = 4;
                if (generatorState == "PAUSE")
                    playGoingUp();
                myTimer1.Start();
            }
            else if (panel1.Top < FLOOR2)
            {
                myController.floorSecondFloorRequestFlagUp = true;
                button8.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\upYellow.png");
                myController.flag = 4;
                if (generatorState == "PAUSE")
                    playGoingDown();
                myTimer2.Start();
            }
        }

//----------------------------Play---Wav----Files---Methods--------------------------------------//
        
        private void play3Floor()
        {
            using (SoundPlayer mySoundPlayer = new SoundPlayer(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer\FormLiftSimulator\Audio\third floor.wav"))
            {
                mySoundPlayer.Play();   //Quicker than PlaySync()
            }
        }

        private void play2Floor()
        {
            using (SoundPlayer mySoundPlayer = new SoundPlayer(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer\FormLiftSimulator\Audio\second floor.wav"))
            {
                mySoundPlayer.Play();       //Quicker than PlaySync()
                                            //Play() uses a new Thread
            }
        }

        private void play1Floor()
        {
            using (SoundPlayer mySoundPlayer = new SoundPlayer(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer\FormLiftSimulator\Audio\first floor.wav"))
            {
                mySoundPlayer.Play();       //Quicker than PlaySync()
                                            //Play() uses a new Thread
            }
        }


        private void play0Floor()
        {
            using (SoundPlayer mySoundPlayer = new SoundPlayer(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer\FormLiftSimulator\Audio\ground floor.wav"))
            {
                mySoundPlayer.Play();       //Quicker than PlaySync()
                                            //Play() uses a new Thread
            }
        }

        private void playGoingDown()
        {
            using (SoundPlayer mySoundPlayer = new SoundPlayer(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer\FormLiftSimulator\Audio\going down.wav"))
            {
                mySoundPlayer.Play();       //Quicker than PlaySync()
            }
        }

        private void playGoingUp()
        {
            using (SoundPlayer mySoundPlayer = new SoundPlayer(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer\FormLiftSimulator\Audio\going up.wav"))
            {
                mySoundPlayer.Play();       //Quicker than PlaySync()
            }
        }
        private void playWelcome()
        {
            using (SoundPlayer mySoundPlayer = new SoundPlayer(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer\FormLiftSimulator\Audio\welcome.wav"))
            {
                mySoundPlayer.Play();       //Quicker than PlaySync()
            }
        }


        //----------Display--Lift's--Position-------------//
            //------Display--Lift's--Position--------//

        public void SetLabelsAndPlayWav()
        {
            if ((GROUND - 5 < panel1.Top) && (panel1.Top < GROUND + 5))
            {
                myController.floorFlag = "0";
                if (generatorState == "PAUSE")
                    play0Floor();
            }
            else if ((FLOOR1 - 5 < panel1.Top) && (panel1.Top < FLOOR1 + 5))
            {
                myController.floorFlag = "1";
                if (generatorState == "PAUSE")
                    play1Floor();
            }
            else if ((FLOOR2 - 5 < panel1.Top) && (panel1.Top < FLOOR2 + 5))
            {
                myController.floorFlag = "2";
                if (generatorState == "PAUSE")
                    play2Floor();
            }
            else if ((FLOOR3 - 5 < panel1.Top) && (panel1.Top < FLOOR3 + 5))
            {
                myController.floorFlag = "3";
                if (generatorState == "PAUSE")
                    play3Floor();
            }
            else
            {
                label5.Text = "";
                label13.Text = "";
                label14.Text = "";
                label15.Text = "";
                label16.Text = "";
            }

            label5.Text = myController.floorFlag;
            label13.Text = myController.floorFlag;
            label14.Text = myController.floorFlag;
            label15.Text = myController.floorFlag; 
            label16.Text = myController.floorFlag;
        }

  //--------Reaset-Labels-Method------//

        public void resetLabels()
        {
            label5.Text = "";
            label13.Text = "";
            label14.Text = "";
            label15.Text = "";
            label16.Text = "";
        }

//-----------------------Lift--Speed--Button-Requests-------------//

        private void button11_Click(object sender, EventArgs e)
        {
            //Increase Speed Lift
            myController.increaseSpeedFlag = true;
            label7.Text = Convert.ToString(myController.speed);

        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Decrease Speed Lift
            myController.decreaseSpeedFlag = true;
            label7.Text = Convert.ToString(myController.speed);
        }


        //-----------------Event-Generator-Buttons-----------------//
            //-------------Event-Generator-Buttons--------------//

        private void button13_Click(object sender, EventArgs e)
        {
            //Play/Pause Generator Button

            if (generatorState == "PAUSE")
                generatorState = "PLAY";
            else
                generatorState = "PAUSE";

            
            if (generatorState == "PLAY")
            {
                button13.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\pause.png");
                myTimer3.Start();
                generatorThread = new Thread(new ThreadStart(myEventGenerator.produceRandomNumber));
                generatorThread.Start();
                
            }

            if (generatorState == "PAUSE")
                 {
                     button13.Image = Image.FromFile(@"H:\Visual Basic 2008 - PROJECTS C#\LiftSimulator-v.Timer-v.1\FormLiftSimulator\Images\play.png");
                     myTimer3.Stop();
                     generatorThread.Suspend();
                     counter = 0;  //the  is zeroed here and in the initialization field, when i declared this variable
                }

            }
        

        private void button14_Click(object sender, EventArgs e)
        {
            //increase mean time
            meantTime += 100;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //decrease mean time
            if (meantTime > 100 )
                meantTime -= 100;
            
        }

        

    }
}
