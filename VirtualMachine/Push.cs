namespace VirtualMachine
{
    public static class Push
    {
        public static Command Constant(int constant)
        {
            return new Command(new[]
            {
                '@' + constant.ToString(),
                "D=A",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D"
            });
        }

        public static Command Local(int index) => PushFromSegment("@LCL", index);
        public static Command Argument(int index) => PushFromSegment("@ARG", index);
        public static Command This(int index) => PushFromSegment("@THIS", index);
        public static Command That(int index) => PushFromSegment("@THAT", index);
        public static Command Temp(int index) => PushFromLabel('@' + (5 + index).ToString());
        public static Command Static(string fileName, int index) => 
            PushFromLabel($"{fileName}.{index}");
        
        public static Command Pointer(int index) =>
            index == 0
                ? PushFromLabel("@THIS")
                : PushFromLabel("@THAT");

        private static Command PushFromLabel(string label)
        {
            label = label.StartsWith('@') ? label : '@' + label;
            return new Command(new[]
            {
                label,
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D"
            });
        }

        private static Command PushFromSegment(string baseAddress, int offset)
        {
            baseAddress = baseAddress.StartsWith('@') ? baseAddress : '@' + baseAddress;
            var offsetAddress = '@' + offset.ToString();
            
            return new Command(new[]
            {
                baseAddress,
                "D=M",
                offsetAddress,
                "A=D+A",
                "D=M",
                "@SP",
                "M=M+1",
                "A=M-1",
                "M=D",
            });
        }
    }
}