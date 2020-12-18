namespace Solution
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Day14 : Solution
    {
        private static Regex memAssignementRegex = new Regex(@"mem\[(\d+)\] = (\d+)");

        public override void Solve(string dataPath)
        {
            var data = File.ReadAllLines(dataPath);
            var memorySumV1 = DecodeProgrammV1(data);
            Console.WriteLine($"(1) Sum of all written memory after the programm (V1) completes: {memorySumV1}");

            var memorySumV2 = DecodeProgrammV2(data);
            Console.WriteLine($"(2) Sum of all written memory after the programm (V2) completes: {memorySumV2}");
        }

        private static long DecodeProgrammV2(string[] data)
        {
            var mask = string.Empty;
            var memory = new Dictionary<long, long>();
            for (var line = 0; line < data.Length; line++)
            {
                var match = memAssignementRegex.Match(data[line]);
                if (match.Success && int.TryParse(match.Groups[1].Value, out var pos) && int.TryParse(match.Groups[2].Value, out var value))
                {
                    var memoryAddress = ApplyMemoryMask(ref mask, pos);
                    WriteToMemoryAddressV2(memory, ref memoryAddress, ref value);
                }
                else
                {
                    mask = data[line].Substring(7);
                }
            }

            return memory.Values.Sum();
        }

        private static char[] ApplyMemoryMask(ref string mask, long pos)
        {
            var binaryPos = Convert.ToString(pos, 2);
            var memoryAddress = mask.ToCharArray();
            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] == '0')
                {
                    // binaryPos may not be 36 bit resulting in less than 36 characters
                    // therefore start counting so that binaryPos fills up the least significant bits
                    var valueIndex = i - (mask.Length - binaryPos.Length);
                    memoryAddress[i] = valueIndex >= 0 ? binaryPos[valueIndex] : '0';
                }
            }

            return memoryAddress;
        }

        private static void WriteToMemoryAddressV2(Dictionary<long, long> memory, ref char[] memoryAddress, ref int value)
        {
            var floatingBit = Array.IndexOf(memoryAddress, 'X');
            if (floatingBit != -1)
            {
                memoryAddress[floatingBit] = '0';
                WriteToMemoryAddressV2(memory, ref memoryAddress, ref value);
                memoryAddress[floatingBit] = '1';
                WriteToMemoryAddressV2(memory, ref memoryAddress, ref value);
                memoryAddress[floatingBit] = 'X';
            }
            else
            {
                memory[Convert.ToInt64(new string(memoryAddress), 2)] = value;
            }
        }

        private static long DecodeProgrammV1(string[] data)
        {
            var mask = string.Empty;
            var memory = new Dictionary<int, long>();
            for (var line = 0; line < data.Length; line++)
            {
                var match = memAssignementRegex.Match(data[line]);
                if (match.Success && int.TryParse(match.Groups[1].Value, out var pos) && int.TryParse(match.Groups[2].Value, out var value))
                {
                    memory[pos] = ApplyValueMask(ref mask, value);
                }
                else
                {
                    mask = data[line].Substring(7);
                }
            }

            return memory.Values.Sum();
        }

        private static long ApplyValueMask(ref string mask, long value)
        {
            var binaryValue = Convert.ToString(value, 2);
            var newValue = mask.ToCharArray();
            for (var i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                {
                    // binaryValue may not be 36 bit resulting in less than 36 characters
                    // therefore start counting so that binaryValue fills up the least significant bits
                    var valueIndex = i - (mask.Length - binaryValue.Length);
                    newValue[i] = valueIndex >= 0 ? binaryValue[valueIndex] : '0';
                }
            }

            return Convert.ToInt64(new string(newValue), 2);
        }
    }
}
