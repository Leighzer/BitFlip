using System;
using System.Collections;

namespace BitFlip.VM
{
    // noting we currently don't have any array access/index checks for now
    public class BitFlipVirtualMachine
    {
        private byte[] _memory { get; set; } = new byte[0];
        private int _ip = 0;

        private BitArray _tape { get; set; } = new BitArray(new bool[0]);
        private int _headPosition { get; set; } = 0;
        private bool _bucket { get; set; } = false;
        private bool _flag { get; set; } = false;
        private bool _isDone { get; set; } = false;

        // toggle - flip the bit that the head is currently pointing at.
        private void Toggle()
        {
            // TODO perhaps throw a specific exception if out of BitArray bounds
            _tape[_headPosition] = !_tape[_headPosition];
        }

        // set [ < bool > ] - set the bit the head is currently pointing at to bool (inlineValue).

        private void Set(bool inlineValue)
        {
            _tape[_headPosition] = inlineValue;
        }

        // copy - copy the bit the head is pointing at to the bucket.
        private void Copy()
        {
            _bucket = _tape[_headPosition];
        }

        // write - write the bucket's binary value to the memory location the head is pointing at.
        private void Write()
        {
            _tape[_headPosition] = _bucket;
        }

        // test - set flag value to result of bucket && value @ head.
        private void Test()
        {
            _flag = _tape[_headPosition] && _bucket;
        }

        // right - move the head one index to the right
        private void Right()
        {
            _headPosition++;
        }

        // left - move the head one index to the left
        private void Left()
        {
            _headPosition--;
        }

        // jump < label > - jump flow of execution to label location in program
        private void Jump(int newMemoryPosition)
        {
            _ip = newMemoryPosition;
        }

        // cjump < label > - if flag is true, jump flow of execution to label loaction in program, else fall through to next instruction
        private void ConditionalJump(int newMemoryPosition)
        {
            if (_flag)
            {
                Jump(newMemoryPosition);
            }
        }

        // tape [ < arg > ] - initialize and use a new tape of size arg (tapeSize)
        private void Tape(int newTapeSize)
        {
            _tape = new BitArray(new bool[newTapeSize]);
        }

        // exit - close the program
        private void Exit()
        {
            _isDone = true;
        }

        public void Run()
        {
            while (!_isDone)
            {
                ProcessNextInstruction();
            }
        }

        private void ProcessNextInstruction()
        {
            // TODO perhaps throw a specific exception if out of memory bounds
            var currentByte = _memory[_ip];
            switch (currentByte)
            {
                case (int)BitFlipInstruction.Toggle: // 0x0
                    Toggle();
                    break;
                case (int)BitFlipInstruction.SetFalse: // 0x1
                    Set(false);
                    break;
                case (int)BitFlipInstruction.SetTrue: // 0x2
                    Set(true);
                    break;
                case (int)BitFlipInstruction.Copy: // 0x3
                    Copy();
                    break;
                case (int)BitFlipInstruction.Write: // 0x4
                    Write();
                    break;
                case (int)BitFlipInstruction.Test: // 0x5
                    Test();
                    break;
                case (int)BitFlipInstruction.Right: // 0x6
                    Right();
                    break;
                case (int)BitFlipInstruction.Left: // 0x7
                    Left();
                    break;
                case (int)BitFlipInstruction.Exit: // 0x8
                    Exit();
                    break;
                case (int)BitFlipInstruction.Jump: // 0x9
                    {
                        int newHeadPosition = BitConverter.ToInt32(_memory, _ip + 1);
                        Jump(newHeadPosition);
                        break;
                    }
                case (int)BitFlipInstruction.ConditionalJump: // 0xa
                    {
                        int newHeadPosition = BitConverter.ToInt32(_memory, _ip + 1);
                        ConditionalJump(newHeadPosition);
                        break;
                    }
                case (int)BitFlipInstruction.Tape: // 0xb
                    {
                        int newTapeSize = BitConverter.ToInt32(_memory, _ip + 1);
                        Tape(newTapeSize);
                        break;
                    }
                default:
                    throw new Exception("Unknow instruction");
            }
        }
    }

    public enum BitFlipInstruction
    {
        Toggle, SetFalse, SetTrue, Copy, Write, Test, Right, Left, Jump, ConditionalJump, Exit, Tape,
    }
}
