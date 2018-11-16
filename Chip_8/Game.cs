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
        KeyboardState keyState



        public Game(GameWindow window)
        {
            this.window = window;

            window.Load += window_Load;
            window.UpdateFrame += window_UpdateFrame;
            window.RenderFrame += window_RenderFrame;

            window.VSync = OpenTK.VSyncMode.On;
            

        }

       

        void window_Load(object sender, EventArgs e)
        {
            chip8.init();
            chip8.load("pong.rom");

            width = window.Width;
            height = window.Height;

            tileHeight = height / 32;
            tileWidth = width / 64;

            keyState = Keyboard.GetState();


        }

        void window_UpdateFrame(object sender, FrameEventArgs e)
        {
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }

            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }

            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }

            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }
            if (keyState.IsKeyDown(Key.Number1)) { }


            chip8.eCycle();
            chip8.setKeys();



        }

        void window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            
            GL.MatrixMode(MatrixMode.Projection);
            
            
            //chip8.drawFlag()
            if (true)
            {

                
                for (int x = 0; x < chip8.Gfx().GetLength(0); x++)
                {
                    for (int y = 0; y < chip8.Gfx().GetLength(1); y++)
                    {
                        float px = (2.0f / (64.0f)) * (float)x;
                        float py = (2.0f / (32.0f)) * (float)y;
                        RectangleF rect = new RectangleF(px - 1.0f, py - 1.0f, 2.0f / 64, 2.0f / 32);

                        GL.Begin(PrimitiveType.Quads);
                    
                        if (chip8.Gfx()[x, y] != 0)
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
