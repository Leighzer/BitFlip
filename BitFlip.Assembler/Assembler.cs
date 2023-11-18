using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitFlip.Asm
{
    public class Assembler
    {
        private readonly IConfiguration _configuration;

        private const string CommentDelimiter = "//";
        private const string StatementDelimiter = ";";
        private const string LabelDelimiter = ":";

        public Assembler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Process(string[] args)
        {
            var compileFlag = _configuration.GetValue<bool>("compile", false);
            if (compileFlag)
            {
                var sourceFilePath = _configuration.GetValue<string>("sourcefilepath", string.Empty);
                if (string.IsNullOrWhiteSpace(sourceFilePath))
                {
                    Console.WriteLine("Source file path is required.");
                    return;
                }

                var outputPath = _configuration.GetValue<string>("outputfilepath", "a.bitflipbc")!;

                Assemble(sourceFilePath, outputPath);
            }
            else
            {
                Console.WriteLine($"No recognized command present.");
            }
        }

        public byte[] Assemble(string sourceFilePath, string outputFilePath)
        {
            string sourceFileText = File.ReadAllText(sourceFilePath);

            var machineCode = new List<byte>();
            var labelsToAddress = new Dictionary<string, int>();
            

            string[] lines = sourceFileText.Split('\n');

            // label pass
            int address = 0;
            foreach (string line in lines)
            {
                string lineStrippedComment = StripComment(line).Trim();
                if (!string.IsNullOrEmpty(lineStrippedComment))
                {
                    if (lineStrippedComment.EndsWith(LabelDelimiter))
                    {
                        string label = lineStrippedComment.Substring(0, lineStrippedComment.Length - 1);
                        labelsToAddress[label] = address;
                    }
                    else
                    {
                        address++;
                    }
                }
            }

            // instruction pass
            foreach (string line in lines)
            {
                string lineStrippedComment = StripComment(line).Trim();
                if (!string.IsNullOrEmpty(lineStrippedComment) && !lineStrippedComment.EndsWith(";"))
                {
                    if (lineStrippedComment.EndsWith(StatementDelimiter))
                    {
                        string[] parts = lineStrippedComment.Split(' ');
                        string instruction = parts[0].ToLowerInvariant();

                        List<byte> instructionBytes = new List<byte>();
                        switch (instruction)
                        {
                            case "toggle":
                                instructionBytes.Add(0x0);
                                break;
                            case "set[0]":
                                instructionBytes.Add(0x1);
                                break;
                            case "set[1]":
                                instructionBytes.Add(0x2);
                                break;
                            case "copy":
                                instructionBytes.Add(0x3);
                                break;
                            case "write":
                                instructionBytes.Add(0x4);
                                break;
                            case "test":
                                instructionBytes.Add(0x5);
                                break;
                            case "right":
                                instructionBytes.Add(0x6);
                                break;
                            case "left":
                                instructionBytes.Add(0x7);
                                break;
                            case "exit":
                                instructionBytes.Add(0x8);
                                break;
                            case "jump":
                                byte jumpInstructionByte = 0x9;
                                int jumpAddress = labelsToAddress[parts[1]];
                                byte[] fullJumpInstruction = new byte[5] { jumpInstructionByte, 0x0, 0x0, 0x0, 0x0 };
                                Array.Copy(BitConverter.GetBytes(jumpAddress), 0, fullJumpInstruction, 1, 4);
                                instructionBytes.AddRange(fullJumpInstruction);
                                break;
                            case "cjump":
                                byte cjumpInstructionByte = 0xa;
                                int cjumpAddress = labelsToAddress[parts[1]];
                                byte[] fullcJumpInstruction = new byte[5] { cjumpInstructionByte, 0x0, 0x0, 0x0, 0x0 };
                                Array.Copy(BitConverter.GetBytes(cjumpAddress), 0, fullcJumpInstruction, 1, 4);
                                instructionBytes.AddRange(fullcJumpInstruction);
                                break;
                            case "tape":
                                byte tapeInstructionByte = 0xb;
                                int tapeAddress = labelsToAddress[parts[1]];
                                byte[] fulltapeInstruction = new byte[5] { tapeInstructionByte, 0x0, 0x0, 0x0, 0x0 };
                                Array.Copy(BitConverter.GetBytes(tapeAddress), 0, fulltapeInstruction, 1, 4);
                                instructionBytes.AddRange(fulltapeInstruction);
                                break;
                        }

                        if (instructionBytes.Count > 0)
                        {
                            machineCode.AddRange(instructionBytes);
                        }


                    }
                }
            }

            return machineCode.ToArray();
        }

        static string StripComment(string input)
        {
            // Find the index of the first occurrence of "//"
            int indexOfComment = input.IndexOf(CommentDelimiter);

            // Check if the input starts with "//"
            if (indexOfComment == 0)
            {
                return string.Empty; // Input starts with "//", return an empty string
            }
            else if (indexOfComment != -1)
            {
                return input.Substring(0, indexOfComment); // Return everything before the first "//"
            }
            else
            {
                return input; // "//" not found, return the original input
            }
        }
    }
}
