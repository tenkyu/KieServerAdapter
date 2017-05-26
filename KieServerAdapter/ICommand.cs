namespace KieServerAdapter
{
    public interface ICommand
    {
        KieCommandTypeEnum CommandType { get; }
    }

    public enum KieCommandTypeEnum : byte
    {
        Insert = 10,
        FireAllRules = 1,
        StartProcess = 0
    }
}
