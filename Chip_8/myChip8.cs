using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip_8
{
    class myChip8
    {

        ushort opcode;

        byte[] memory = new byte[4096];
        byte[] V = new byte[16];
        byte[] fontset = new byte[80];

        

        ushort I;
        ushort pc;


        byte[] gfx = new byte[64 * 32];

        byte delay_timer;
        byte sound_timer;

        ushort[] stack = new ushort[16];
        ushort sp;

        byte[] key = new byte[16];

        public void init()
        {
            //Init Memory and Registers
            pc = 0x200;
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
            byte[] file = System.IO.File.ReadAllBytes(filename);

            for(int i = 0; i < file.Length; i++)
            {
                memory[i + 512] = file[i];
            }
        }

        public void eCycle()
        {
            
            //Fetch Opcode
            opcode = BitConverter.ToUInt16(new byte[2] { memory[pc], memory[pc+1] }, 0);
            //Decode Opcode
            switch (opcode & (0x0FFF))
            {
                case (0x0000):
                    switch (opcode & (0x000F))
                    {
                        case (0x0000):
                            break;
                        case (0x000E):
                            break;
                    }
                    break;
                case (0xA000):
                    I = (ushort)(opcode & 0x0FFF);
                    pc += 2;
                    break;
                case (0x2000):
                    stack[sp] = pc;
                    sp++;
                    pc = (ushort)(opcode & 0x0FFF);
                    break;
                
                


                default:
                    //Console.WriteLine("Unknown opcode: " + Convert.ToString(opcode, 2));
                    break;


            }
            
            //Execute Opcode

            //Update Timers
            if(delay_timer > 0)
            {
                delay_timer--;
            }
            if(sound_timer > 0)
            {
                if(sound_timer == 1)
                {
                    Console.WriteLine("BEEP");
                    sound_timer--;
                }
            }

        }

        public bool drawFlag()
        {
            return false;
        }

        public void setKeys()
        {

        }
    }
}
