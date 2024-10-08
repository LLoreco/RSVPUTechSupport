using API.Data;
using API.Models;
namespace API.Interfaces
{
    public interface IWorkService
    {
        public Task<List<Work>> GetWorks();
        public Task<TaskResult<Work>> GetWork(int id);
        public Task<TaskResult<bool>> InsertRecord(Work work);
        public Task<TaskResult<Work>> EditRecord(int workID);
        public Task<TaskResult<bool>> UpdateRecord(Work workUpdate);
        public Task<TaskResult<bool>> DeleteRecord(Work workDelete);
        string FindEmployee(int id, string fromWhom = null);
        int GetNextAvailableWorkNumber();
    }
}
