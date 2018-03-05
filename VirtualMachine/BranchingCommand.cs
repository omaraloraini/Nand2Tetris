namespace VirtualMachine
{
    public class BranchingCommand
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
                "M=M-1",
                "A=M",
                "D=M",
                label.Address,
                "D;JNE"
            });
        }
    }
}