using System.Collections.Generic;

namespace VirtualMachine
{
    public class BranchingCommand : Command
    {
        private BranchingCommand(IEnumerable<string> hackInstructions) : base(hackInstructions)
        {
        }

        public static BranchingCommand Goto(string label)
        {
            if (!label.StartsWith('@')) label = '@' + label;
            
            return new BranchingCommand(new[]
            {
                label,
                "0;JMP"
            });
        }
        
        public static BranchingCommand IfGoto(string label)
        {
            if (!label.StartsWith('@')) label = '@' + label;
            
            return new BranchingCommand(new[]
            {
                "@SP",
                "M=M-1",
                "A=M",
                "D=M",
                label,
                "D;JNE"
            });
        }
    }
}