using System;
using System.Collections.Generic;
using System.Text;

namespace Controller1
{
    public class Controller
    {
        //General Variables
        public int speed = 5;
        //Speed Flags
        public bool increaseSpeedFlag = false;
        public bool decreaseSpeedFlag = false;
        
        //Request Flags ( FROM USERS AND FROM THE FLOORS )
        public bool userGroundRequestFlag = false;
        public bool userFirstFloorRequestFlag = false;
        public bool userSecondFloorRequestFlag = false;
        public bool userThirdFloorRequestFlag = false;
        public bool floorGroundRequestFlag = false;
        public bool floorFirstFloorRequestFlagDown = false;
        public bool floorFirstFloorRequestFlagUp = false;
        public bool floorSecondFloorRequestFlagDown = false;
        public bool floorSecondFloorRequestFlagUp = false;
        public bool floorThirdFloorRequestFlag = false;
        //Direction Flags
        public string directionFlag;  //{"DOWN","UP","STABLE"}
        //Floor Flags
        public string floorFlag;       //{"0","1","2","3"}
        
        //Permission
         public int permission = 5;     //permission to nowhere  {0,1,2,3,5=nowhere}

        //Secondary-Debugging Flags
         public int flag = 0;   //{0,1,2,3,4} flag values to hold some requests (circle requests...,circle bugs...)

         public int flagy4 = 0;  // flags not to change direction of the lift due to new requests from other direction
         public int flagy1 = 0;
         public int flagy3 = 0;
         public int flagy2 = 0;
//-------------------------Controller--Method----------------------------------------------------------//

        public void ControllerMethod()
        {
            while (true)
            {
                
                if (increaseSpeedFlag == true)
                {
                    if ((speed > 0) && (speed < 10))
                        speed++;

                    increaseSpeedFlag = false;  
                }

                if (decreaseSpeedFlag == true)
                {

                    if ((speed > 1) && (speed <= 10))
                        speed--;

                    decreaseSpeedFlag = false; 
                }


                if ((floorThirdFloorRequestFlag || userThirdFloorRequestFlag)
                    || (userSecondFloorRequestFlag || floorSecondFloorRequestFlagUp)
                    || (userFirstFloorRequestFlag || floorFirstFloorRequestFlagUp))
                {
                        while (floorThirdFloorRequestFlag || userThirdFloorRequestFlag)
                        {
                            permission = 3;
                            while ((userSecondFloorRequestFlag || floorSecondFloorRequestFlagUp) && (flagy4 == 4))  
                            {
                                permission = 2;
                                while ((userFirstFloorRequestFlag || floorFirstFloorRequestFlagUp) && (flagy1 == 1))
                                {
                                    permission = 1;
                                }
                            }
                            while ((userFirstFloorRequestFlag || floorFirstFloorRequestFlagUp) && (flagy1 == 1))
                            {
                                permission = 1;
                            }
                        
                    }
                }


                if ((userGroundRequestFlag || floorGroundRequestFlag)
                    || (userFirstFloorRequestFlag || floorFirstFloorRequestFlagDown)
                    || (userSecondFloorRequestFlag || floorSecondFloorRequestFlagDown))
                {
                    while (floorGroundRequestFlag || userGroundRequestFlag)
                    {
                        permission = 0;
                        while ((userFirstFloorRequestFlag || floorFirstFloorRequestFlagDown) && (flagy3==3))
                        {
                            permission = 1;
                            while ((userSecondFloorRequestFlag || floorSecondFloorRequestFlagDown) && (flagy2 == 2))
                            {
                                permission = 2;
                            }
                        }
                        while ((userSecondFloorRequestFlag || floorSecondFloorRequestFlagDown) && (flagy2 == 2))
                        {
                            permission = 2;
                        }
                    }
                }

                if ((userSecondFloorRequestFlag || floorSecondFloorRequestFlagUp)
                   || (userFirstFloorRequestFlag || floorFirstFloorRequestFlagUp))
                {
                    while (userSecondFloorRequestFlag || floorSecondFloorRequestFlagUp)
                    {
                        permission = 2;
                        while ((userFirstFloorRequestFlag || floorFirstFloorRequestFlagUp) && (flagy1 == 1))
                        {
                            permission = 1;
                        }
                    }
                }
                

                if ((userSecondFloorRequestFlag || floorSecondFloorRequestFlagDown)
                    || (userFirstFloorRequestFlag || floorFirstFloorRequestFlagDown))
                {
                    while (userFirstFloorRequestFlag || floorFirstFloorRequestFlagDown)
                    {
                        permission = 1;
                        while ((userSecondFloorRequestFlag || floorSecondFloorRequestFlagDown) && (flagy2 == 2))
                        {
                            permission = 2;
                        }
                    }
                }

                
                    while (floorFirstFloorRequestFlagUp)
                    {
                        permission = 1;
                    }
 
                    while (floorSecondFloorRequestFlagDown)
                    {
                        permission = 2;
                    }

            }
        }


    }
}
