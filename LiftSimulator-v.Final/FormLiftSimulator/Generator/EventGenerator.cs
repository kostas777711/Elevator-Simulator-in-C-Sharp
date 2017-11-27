using System;
using System.Collections.Generic;
using System.Text;

namespace Generator
{
    public class EventGenerator
    {
        static void Main(string[] args)
        {
        }

        public int buttonNumber;     //variable which carries the random integer
        Random rnd = new Random();   //rnd object

        public void produceRandomNumber()
        {
            while (true)
            {
                buttonNumber = rnd.Next(1, 11);
            }
        }

    }
}
