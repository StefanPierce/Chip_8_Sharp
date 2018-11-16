using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip_8
{
    class myChip8
    {
        Random rand = new Random();

        ushort opcode;

        byte[] memory = new byte[4096];
        byte[] V = new byte[16];
        byte[] fontset = new byte[80];

        int cycle;

        ushort I;
        ushort pc;


        byte[,] gfx = new byte[64, 32];

        byte delay_timer;
        byte sound_timer;

        ushort[] stack = new ushort[16];
        ushort sp;

        byte[] key = new byte[16];

        public void init()
        {
            //Init Memory and Registers
            pc = (0x200);
            opcode = 0;
            I = 0;
            sp = 0;

            //clear display
            //clear stack
            //clear registers V0-VF
            //clear memory

            //load font
            for (int i = 0; i < 80; i++)
            {
                memory[i] = fontset[i];
            }





        }

        public void load(string filename)
        {

            byte[] chip8_fontset = {
                  0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                  0x20, 0x60, 0x20, 0x20, 0x70, // 1
                  0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                  0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                  0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                  0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                  0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                  0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                  0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                  0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                  0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                  0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                  0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                  0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                  0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                  0xF0, 0x80, 0xF0, 0x80, 0x80  // F
                };

            for(int i = 0; i < chip8_fontset.Length; i++)
            {
                memory[i] = chip8_fontset[i];
            }

            byte[] file = System.IO.File.ReadAllBytes(filename);

            for (int i = 0; i < file.Length; i++)
            {
                memory[i + 512] = file[i];
            }
        }

        public void eCycle()
        {
            //for (int x = 0; x < 64; x++)
            //{
            //    for (int y = 0; y < 32; y++)
            //    {
            //        gfx[x, y] = (byte)(rand.Next(2) - 1);
            //    }
            //}

            //Fetch Opcode
            opcode = BitConverter.ToUInt16(new byte[2] { memory[pc], memory[pc + 1] }, 0);
            //Decode Opcode
            switch (opcode & (0x0FFF))
            {
                case (0x0000):
                    switch (opcode & (0x000F))
                    {
                        case (0x0000):  //0x00E0: Clears the screen
                            break;
                        case (0x000E):  //0x00EE:  Returns from subroutine
                            break;
                    }
                    break;
                case (0xA000): //ANNN: Set I to the address NNN
                    I = (ushort)(opcode & 0x0FFF);
                    pc += 2;

                    break;
                case (0x2000): //
                    stack[sp] = pc;
                    sp++;
                    pc = (ushort)(opcode & 0x0FFF);

                    break;
                case (0x0004):
                    if (V[(opcode & (0x00F0)) >> 4] > (0xFF - V[(opcode & (0x0F00)) >> 8]))
                    {
                        V[0xF] = 1;
                    }
                    else
                    {
                        V[0xF] = 0;
                    }
                    pc += 2;
                    break;
                case (0xD000):
                    ushort x = V[(opcode & (0x0F00)) >> 8];
                    ushort y = V[(opcode & (0x00F0)) >> 4];
                    ushort height = (ushort)(opcode & (0x000F));
                    ushort pixel;

                    V[0xF] = 0;
                    for (int yline = 0; yline < height; yline++)
                    {
                        pixel = memory[I + yline];
                        for (int xline = 0; xline < 8; xline++)
                        {
                            if ((pixel & (0x80 >> xline)) != 0)
                            {
                                if (gfx[x, y] == 1)
                                {
                                    V[0xF] = 1;
                                }
                                gfx[x, y] ^= 1;
                            }
                        }
                    }
                    drawFlag = true;
                    pc += 2;
                    break;

                default:
                    //Console.WriteLine("Unknown opcode: " + Convert.ToString(opcode & (0x0F00), 2));
                    pc += 2;
                    break;


            }

            //Execute Opcode

            //Update Timers
            if (delay_timer > 0)
            {
                delay_timer--;
            }
            if (sound_timer > 0)
            {
                if (sound_timer == 1)
                {
                    Console.WriteLine("BEEP");
                    sound_timer--;
                }
            }

        }

        public bool drawFlag = false;

        public void setKeys()
        {

        }

        public byte[,] Gfx()
        {
            return gfx;
        }
    }
}
