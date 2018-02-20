using System;
using System.Collections;
using System.Linq;

namespace Assembler
{
    public static class BinaryConverter
    {
        public static BitArray To16Bit(this int numeral)
        {   
            var bits = Convert
                .ToString(numeral, 2)
                .PadLeft(16, '0')
                .Select(c => c == '1')
                .ToArray();

            if (bits.Length > 16) throw new ArgumentException();
            
            return new BitArray(bits);
        }
    }
}