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

        static GameWindow window;
        static Game game;
        

        static void Main(string[] args)
        {

            
            setupGraphics();
            setupInput();

            

        }


        static void setupGraphics()
        {
            window = new GameWindow(640, 320);
            game = new Game(window);
            window.Run();
            
            
          
        }

        static void draw() {
            
            
        }

        static void setupInput() { }

    }
}
