namespace VirtualMachine
{
    public static partial class Commands
    {
        public static Command Label(Label label) =>
            new Command(new[] {label.Declaration});
        
        public static Command Goto(Label label)
        {
            return new Command(new[]
            {
                label.Address,
                "0;JMP"
            });
        }
        
        public static Command IfGoto(Label label)
        {
            return new Command(new[]
            {
                "@SP",
                "AM=M-1",
                "D=M",
                label.Address,
                "D;JNE"
            });
        }
    }
}