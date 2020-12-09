namespace KieServerAdapter
{
    public interface ICommand
    {
        KieCommandTypeEnum CommandType { get; }
    }

    public enum KieCommandTypeEnum : byte
    {
        SetGlobal = 20,
        Insert = 10,
        FireAllRules = 4,
        Query = 3,
        GetObjects = 2,
        GetGlobal = 1,
        StartProcess = 0
    }
}
