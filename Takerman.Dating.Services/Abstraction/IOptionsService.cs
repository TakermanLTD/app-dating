namespace Takerman.Dating.Services.Abstraction
{
    public interface IOptionsService
    {
        IEnumerable<KeyValuePair<int, string>> GetEthnicities();
        
        IEnumerable<KeyValuePair<int, string>> GetDateTypes();
        
        IEnumerable<KeyValuePair<int, string>> GetDateStatuses();
    }
}