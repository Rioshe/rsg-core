using System.Threading.Tasks;

namespace RSG
{
    public interface IService
    {
        Task InitializeAsync();
        Task ShutdownAsync();
    }
}