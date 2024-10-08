using API.Data;
using API.Models;

namespace API.Interfaces
{
    public interface IObjectService
    {
        public Task<List<Objects>> GetObjects();
        public Task<TaskResult<Objects>> GetObject(int id);
        public Task<TaskResult<bool>> InsertRecord(Objects objects);
        public Task<TaskResult<Objects>> EditRecord(int objectID);
        public Task<TaskResult<bool>> UpdateRecord(Objects objectsUpdate);
        public Task<TaskResult<bool>> DeleteRecord(Objects objectDelete);
    }
}
