using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Chip_8
{
    class Program
    {
        static void Main(string[] args)
        {

            myChip8 chip8 = new myChip8();
            
            setupGraphics();
            setupInput();

            chip8.init();
            chip8.load("test.ch8");

            for(; ; )
            {
                chip8.eCycle();

                if (chip8.drawFlag())
                {
                    draw();
                }

                chip8.setKeys();
                
            }

        }


        static void setupGraphics()
        {
          
        }

        static void draw() {
            
            
        }

        static void setupInput() { }

    }
}
