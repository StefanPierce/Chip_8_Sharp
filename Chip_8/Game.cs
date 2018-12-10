using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace Chip_8
{
    class Game
    {
        GameWindow window;
        myChip8 chip8 = new myChip8();
        int width, height;
        float tileWidth, tileHeight;
        KeyboardState keyState;



        public Game(GameWindow window)
        {
            this.window = window;

            window.Load += window_Load;
            window.UpdateFrame += window_UpdateFrame;
            window.RenderFrame += window_RenderFrame;

           // window.VSync = OpenTK.VSyncMode.On;
            

        }

       

        void window_Load(object sender, EventArgs e)
        {
            chip8.init();

            //chip8.load(Console.ReadLine());
            chip8.load("roms/games/Blinky [Hans Christian Egeberg, 1991].ch8");

            width = window.Width;
            height = window.Height;

            tileHeight = height / 32;
            tileWidth = width / 64;

            keyState = Keyboard.GetState();


        }

        void window_UpdateFrame(object sender, FrameEventArgs e)
        {

           

            keyState = Keyboard.GetState();
            chip8.reset_Key();

            chip8.send_Key(1, keyState.IsKeyDown(Key.Number1));
            chip8.send_Key(2, keyState.IsKeyDown(Key.Number2));
            chip8.send_Key(3, keyState.IsKeyDown(Key.Number3));
            chip8.send_Key(12, keyState.IsKeyDown(Key.Number4));

            chip8.send_Key(4, keyState.IsKeyDown(Key.Q));
            chip8.send_Key(5, keyState.IsKeyDown(Key.W));
            chip8.send_Key(6, keyState.IsKeyDown(Key.E));
            chip8.send_Key(13, keyState.IsKeyDown(Key.R));

            chip8.send_Key(7, keyState.IsKeyDown(Key.A));
            chip8.send_Key(8, keyState.IsKeyDown(Key.S));
            chip8.send_Key(9, keyState.IsKeyDown(Key.D));
            chip8.send_Key(14, keyState.IsKeyDown(Key.F));

            chip8.send_Key(10, keyState.IsKeyDown(Key.Z));
            chip8.send_Key(0, keyState.IsKeyDown(Key.X));
            chip8.send_Key(11, keyState.IsKeyDown(Key.C));
            chip8.send_Key(15, keyState.IsKeyDown(Key.V));
            ////
            //chip8.send_Key(0x1, keyState.IsKeyDown(Key.Number1));
            //chip8.send_Key(0x2, keyState.IsKeyDown(Key.Number2));
            //chip8.send_Key(0x3, keyState.IsKeyDown(Key.Number3));
            //chip8.send_Key(0xC, keyState.IsKeyDown(Key.Number4));

            //chip8.send_Key(0x4, keyState.IsKeyDown(Key.Q));
            //chip8.send_Key(0x5, keyState.IsKeyDown(Key.W));
            //chip8.send_Key(0x6, keyState.IsKeyDown(Key.E));
            //chip8.send_Key(0xD, keyState.IsKeyDown(Key.R));

            //chip8.send_Key(0x7, keyState.IsKeyDown(Key.A));
            //chip8.send_Key(0x8, keyState.IsKeyDown(Key.S));
            //chip8.send_Key(0x9, keyState.IsKeyDown(Key.D));
            //chip8.send_Key(0xE, keyState.IsKeyDown(Key.F));

            //chip8.send_Key(0xA, keyState.IsKeyDown(Key.Z));
            //chip8.send_Key(0x0, keyState.IsKeyDown(Key.X));
            //chip8.send_Key(0xB, keyState.IsKeyDown(Key.C));
            //chip8.send_Key(0xF, keyState.IsKeyDown(Key.V));

            chip8.eCycle();

        }

        void window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            
            GL.MatrixMode(MatrixMode.Projection);

            //chip8.drawFlag()
            if (chip8.drawFlag)
            {

                
                for (int x = 0; x < chip8.Gfx().GetLength(0); x++)
                {
                    for (int y = 0; y < chip8.Gfx().GetLength(1); y++)
                    {
                        float px = (2.0f / (64.0f)) * (float)x;
                        float py = (2.0f / (32.0f)) * (float)y;
                        RectangleF rect = new RectangleF(px - 1.0f, py - 1.0f, 2.0f / 64, 2.0f / 32);

                        GL.Begin(PrimitiveType.Quads);
                    
                        if (chip8.Gfx()[x, 31-y] != 0)
                        {
                            //color white
                            GL.Color3(Color.LimeGreen);
                        }
                        else
                        {
                            //color lack
                            GL.Color3(Color.Black);
                        }

                        //draw shape
                        
                        GL.Vertex3(rect.X, rect.Y + rect.Height, 0);
                        GL.Vertex3(rect.X + rect.Width, rect.Y + rect.Height, 0);
                        GL.Vertex3(rect.X + rect.Width, rect.Y, 0);
                        GL.Vertex3(rect.X, rect.Y, 0);
                        GL.End();

                    }


                }
                window.SwapBuffers();
                chip8.drawFlag = false;
            }
            
        }
    }
}
