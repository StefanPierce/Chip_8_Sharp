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

        Stack<ushort> stack;

        byte[] key = new byte[16];

        public void init()
        {
            //Init Memory and Registers
            pc = (0x200);
            opcode = 0;
            I = 0;
            V = new byte[16];
            stack = new Stack<ushort>();
            //clear display
            //clear stack
            //clear registers V0-VF
            //clear memory

            //load font
            





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

            for (int i = 0; i < chip8_fontset.Length; i++)
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
            switch (opcode & (0xF000))
            {
                case (0x0000):
                    switch (opcode & (0x00FF))
                    {
                        case (0x00E0):  //0x00E0: Clears the screen
                            Console.WriteLine("00E0");
                            for (int i = 0; i < gfx.GetLength(0); i++)
                            {
                                for (int i2 = 0; i2 < gfx.GetLength(1); i2++)
                                {
                                    gfx[i, i2] = 0;
                                }
                            }
                            pc += 2;
                            // drawFlag = true;
                            break;
                        case (0x00EE):  //0x00EE:  Returns from subroutine
                            Console.WriteLine("00EE");
                            if (stack.Count > 0)
                            {
                                pc = stack.Pop();
                            }
                            else

                            {
                                Console.WriteLine("Tried exiting non existant subroutine");
                            }
                            pc += 2;
                            break;
                        default:
                            Console.WriteLine("Unknown opcode: " + (opcode & (0xFFFF)).ToString("X4"));
                            pc += 2;
                            break;
                    }
                    break;
                case (0xE000):
                    ushort registerEx = (ushort)(opcode & (0x0F00) >> 8);
                    ushort registerEy = (ushort)(opcode & (0x00F0) >> 4);
                    switch (opcode & (0x000F))
                    {
                        case (0x000E):
                            pc += 2;
                            if (V[registerEx] > 0)
                            {
                                pc += 2;
                            }
                            Console.WriteLine("EX9E");

                            break;
                        case (0x0001):
                            pc += 2;
                            if (V[registerEx] == 0)
                            {
                                pc += 2;
                            }
                            Console.WriteLine("EXA1");
                            break;
                        default:
                            pc += 2;
                            Console.WriteLine("Unknown opcode: " + (opcode & (0xFFFF)).ToString("X4"));
                            break;
                    }
                    break;
                case (0xF000):
                    ushort registerFx = (ushort)(opcode & (0x0F00) >> 8);
                    ushort registerFy = (ushort)(opcode & (0x00F0) >> 4);
                    switch (opcode & (0x00FF))
                    {
                        case (0x0007):
                            pc += 2;
                            V[registerFx] = delay_timer;
                            Console.WriteLine("FX07");
                            break;
                        case (0x000A):
                            pc += 2;
                            Console.WriteLine("FX0A");
                            break;
                        case (0x0015):
                            pc += 2;
                            delay_timer = V[registerFx];
                            Console.WriteLine("FX15");
                            break;
                        case (0x0018):
                            pc += 2;
                            sound_timer = V[registerFx];
                            Console.WriteLine("FX18");
                            break;
                        case (0x001E):
                            pc += 2;
                            I += V[registerFx];
                            Console.WriteLine("FX1E");
                            break;
                        case (0x0029):
                            pc += 2;
                            I = V[registerFx];
                            Console.WriteLine("FX29");
                            break;
                        case (0x0033):
                            pc += 2;
                            V[I] = (byte)(V[registerFx] / 100);
                            V[I + 1] = (byte)((V[registerFx] / 10) % 10);
                            V[I + 2] = (byte)((V[registerFx] % 100) % 10);
                            Console.WriteLine("FX33");
                            break;
                        case (0x0055):
                            pc += 2;
                            Console.WriteLine("FX55");
                            break;
                        case (0x0065):
                            pc += 2;
                            Console.WriteLine("FX55");
                            break;
                        default:
                            Console.WriteLine("Unknown opcode: " + (opcode & (0xFFFF)).ToString("X4"));
                            pc += 2;
                            break;
                    }
                    break;
                case (0xA000): //ANNN: Set I to the address NNN
                    Console.WriteLine("ANNN");
                    I = (ushort)(opcode & 0x0FFF);
                    pc += 2;

                    break;
                case (0x1000):
                    Console.WriteLine("1NNN: " + opcode);
                    pc = (ushort)(opcode & (0x0FFF));
                    break;
                case (0x2000): //2NNN: Jump to subroutine at address NNN
                    Console.WriteLine("2NNN");
                    stack.Push(pc);
                    pc = (ushort)(opcode & (0x0FFF));
                    
                    break;
                case (0x3000):
                    Console.WriteLine("3XNN");
                    ushort register3 = (ushort)(opcode & (0x0F00) >> 8);
                    ushort value3 = (ushort)(opcode & (0x00FF));
                    Console.WriteLine(value3);
                    if (V[register3] == value3)
                    {
                        pc += 2;
                    }
                    pc += 2;

                    break;
                case (0x4000):
                    Console.WriteLine("4XNN");
                    ushort register4 = (ushort)(opcode & (0x0F00) >> 8);
                    ushort value4 = (ushort)(opcode & (0x00FF));

                    if (V[register4] != value4)
                    {
                        pc += 2;
                    }
                    pc += 2;
                    break;
                case (0x5000):
                    Console.WriteLine("5XY0");
                    ushort register5x = (ushort)(opcode & (0x0F00) >> 8);
                    ushort register5y = (ushort)(opcode & (0x00F0) >> 4);

                    if (V[register5x] == V[register5y])
                    {
                        pc += 2;
                    }
                    pc += 2;

                    break;
                case (0x6000):
                    Console.WriteLine("6XNN");
                    ushort register6x = (ushort)(opcode & (0x0F00) >> 8);
                    byte value6 = (byte)(opcode & (0x00FF));
                    V[register6x] = value6;
                    pc += 2;
                    break;
                case (0x7000):
                    Console.WriteLine("7XNN");
                    pc += 2;
                    ushort register7x = (ushort)(opcode & (0x0F00) >> 8);
                    byte value7 = (byte)(opcode & (0x00FF));

                    V[register7x] += value7;

                    break;
                case (0x8000):
                    ushort register8x = (ushort)(opcode & (0x0F00) >> 8);
                    ushort register8y = (ushort)(opcode & (0x00F0) >> 4);

                    switch (opcode & (0x000F))
                    {
                        case (0x0000):
                            pc += 2;
                            V[register8x] = V[register8y];
                            Console.WriteLine("8XY0");
                            break;
                        case (0x0001):
                            pc += 2;
                            V[register8x] = (byte)(V[register8x] | V[register8y]);
                            Console.WriteLine("8XY1");
                            break;
                        case (0x0002):
                            pc += 2;
                            V[register8x] = (byte)(V[register8x] & V[register8y]);
                            Console.WriteLine("8XY2");
                            break;
                        case (0x0003):
                            pc += 2;
                            V[register8x] = (byte)(V[register8x] ^ V[register8y]);
                            Console.WriteLine("8XY3");
                            break;
                        case (0x0004):
                            if (V[register8x] + V[register8y] > 255)
                            {
                                V[0xF] = 1;
                            }
                            else
                            {
                                V[0xF] = 0;
                            }
                            V[register8x] += V[register8y];
                            pc += 2;
                            Console.WriteLine("8XY4");
                            break;
                        case (0x0005):
                            pc += 2;
                            if (V[register8x] - V[register8y] < 0)
                            {
                                V[0xF] = 0;
                            }
                            else
                            {
                                V[0xF] = 1;
                            }
                            V[register8x] -= V[register8y];
                            Console.WriteLine("8XY5");
                            break;
                        case (0x0006):
                            V[0xF] = (byte)(V[register8x] & 0x01);
                            V[register8x] >>= 1;
                            pc += 2;
                            Console.WriteLine("8XY6");
                            break;
                        case (0x0007):
                            if (V[register8y] - V[register8x] < 0)
                            {
                                V[0xF] = 0;
                            }
                            else
                            {
                                V[0xF] = 1;
                            }
                            V[register8x] = (byte)(V[register8y] - V[register8x]);
                            pc += 2;
                            Console.WriteLine("8XY7");
                            break;
                        case (0x000E):
                            pc += 2;
                            V[0xF] = (byte)((V[register8x] & 0x80) >> 7);
                            V[register8x] <<= 1;
                            Console.WriteLine("8XYE");
                            break;
                        default:
                            Console.WriteLine("Unknown opcode: " + (opcode & (0xFFFF)).ToString("X4"));
                            break;
                    }
                    break;
                case (0x9000):
                    Console.WriteLine("9XY0");
                    pc += 2;
                    ushort register9x = (ushort)(opcode & (0x0F00) >> 8);
                    ushort register9y = (ushort)(opcode & (0x00F0) >> 4);
                    if (V[register9x] != V[register9y])
                    {
                        pc += 2;
                    }
                    break;
                case (0xB000):
                    Console.WriteLine("BNNN");
                    pc += 2;
                    pc = (ushort)(opcode & (0x0FFF));
                    break;
                case (0xC000):
                    Console.WriteLine("CXNN");
                    ushort val = (ushort)((opcode & 0x0FFF));
                    pc = (ushort)(val + V[0]);
                    break;
                case (0xD000):
                    Console.WriteLine("DXYN");
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
                                gfx[x+xline, y+yline] ^= 1;
                            }
                        }
                    }
                    drawFlag = true;
                    pc += 2;
                    break;

                default:
                    Console.WriteLine("Unknown opcode: " + (opcode&(0xFFFF)).ToString("X4"));
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
