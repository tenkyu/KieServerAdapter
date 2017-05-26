namespace KieServerAdapter
{
    public interface ICommand
    {
        KieCommandTypeEnum CommandType { get; }
    }

    public enum KieCommandTypeEnum : byte
    {
        Insert = 10,
        StartProcess = 0
    }
}
