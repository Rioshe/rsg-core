namespace RSG
{
    public interface IBootSystem
    {
        int BootPriority { get; }
        void Initialize();
    }
}