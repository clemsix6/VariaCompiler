using VariaCompiler.Compiling.Instructions;


namespace VariaCompiler.Compiling;

public static class Words
{
    public enum RegisterType
    {
        A,
        C,
        D,
        B,
        SP,
        BP,
        SI,
        DI,
        R8,
        R9,
        R10,
        R11,
        R12,
        R13,
        R14,
        R15
    }


    public static Dictionary<RegisterType, Register> registers = new()
    {
        {RegisterType.A, new Register(RegisterType.A)},
        {RegisterType.C, new Register(RegisterType.C)},
        {RegisterType.D, new Register(RegisterType.D)},
        {RegisterType.B, new Register(RegisterType.B)},
        {RegisterType.SP, new Register(RegisterType.SP)},
        {RegisterType.BP, new Register(RegisterType.BP)},
        {RegisterType.SI, new Register(RegisterType.SI)},
        {RegisterType.DI, new Register(RegisterType.DI)},
        {RegisterType.R8, new Register(RegisterType.R8)},
        {RegisterType.R9, new Register(RegisterType.R9)},
        {RegisterType.R10, new Register(RegisterType.R10)},
        {RegisterType.R11, new Register(RegisterType.R11)},
        {RegisterType.R12, new Register(RegisterType.R12)},
        {RegisterType.R13, new Register(RegisterType.R13)},
        {RegisterType.R14, new Register(RegisterType.R14)},
        {RegisterType.R15, new Register(RegisterType.R15)}
    };


    public static string GetWordType(string type)
    {
        switch (type) {
            case "byte":  return "byte";
            case "short": return "word";
            case "int":   return "dword";
            case "long":  return "qword";
            default:      throw new Exception("Unknown type");
        }
    }


    public static string GetWordType(int size)
    {
        switch (size) {
            case 1:  return "byte";
            case 2:  return "word";
            case 4:  return "dword";
            case 8:  return "qword";
            default: throw new Exception("Unknown type");
        }
    }


    public static int GetWordSize(string word)
    {
        switch (word) {
            case "byte":  return 1;
            case "word":  return 2;
            case "dword": return 4;
            case "qword": return 8;
            default:      throw new Exception("Unknown word");
        }
    }


    public static string GetRegisterName(RegisterType register, int size)
    {
        var sizes = new List<int>
        {
            1,
            2,
            4,
            8
        };

        switch (register) {
            case RegisterType.A:
            {
                var registers = new[] {"al", "ax", "eax", "rax"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.C:
            {
                var registers = new[] {"cl", "cx", "ecx", "rcx"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.D:
            {
                var registers = new[] {"dl", "dx", "edx", "rdx"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.B:
            {
                var registers = new[] {"bl", "bx", "ebx", "rbx"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.SP:
            {
                var registers = new[] {"spl", "sp", "esp", "rsp"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.BP:
            {
                var registers = new[] {"bpl", "bp", "ebp", "rbp"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.SI:
            {
                var registers = new[] {"sil", "si", "esi", "rsi"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.DI:
            {
                var registers = new[] {"dil", "di", "edi", "rdi"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R8:
            {
                var registers = new[] {"r8l", "r8w", "r8d", "r8"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R9:
            {
                var registers = new[] {"r9l", "r9w", "r9d", "r9"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R10:
            {
                var registers = new[] {"r10l", "r10w", "r10d", "r10"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R11:
            {
                var registers = new[] {"r11l", "r11w", "r11d", "r11"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R12:
            {
                var registers = new[] {"r12l", "r12w", "r12d", "r12"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R13:
            {
                var registers = new[] {"r13l", "r13w", "r13d", "r13"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R14:
            {
                var registers = new[] {"r14l", "r14w", "r14d", "r14"};
                return registers[sizes.IndexOf(size)];
            }
            case RegisterType.R15:
            {
                var registers = new[] {"r15l", "r15w", "r15d", "r15"};
                return registers[sizes.IndexOf(size)];
            }
            default: throw new Exception("Unknown register");
        }
    }


    public static int GetRegisterSize(string register)
    {
        switch (GetRegisterPrefix(register)) {
            case "e": return 4;
            case "r": return 8;
            default:  throw new Exception("Unknown register prefix");
        }
    }


    public static int GetTypeSize(string type)
    {
        switch (type) {
            case "byte":  return 1;
            case "short": return 2;
            case "int":   return 4;
            case "long":  return 8;
            default:      throw new Exception("Unknown type");
        }
    }


    public static string GetRegisterPrefix(string register)
    {
        return register[..1];
    }


    public static string GetRegisterSuffix(string register)
    {
        return register[1..];
    }
}