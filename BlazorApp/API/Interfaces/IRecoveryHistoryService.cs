using API.Data;
using API.Models;

namespace API.Interfaces
{
    public interface IRecoveryHistoryService
    {
        public Task<List<RecoveryHistory>> GetALlRecoveryHistory();
        public Task<TaskResult<RecoveryHistory>> GetRecoveryHistory(int id);
        public Task<TaskResult<bool>> InsertRecord(RecoveryHistory recoveryHistory, bool fromWork);
        public Task<TaskResult<RecoveryHistory>> EditRecord(int recoveryHistoryId);
        public Task<TaskResult<bool>> UpdateRecord(RecoveryHistory recoveryHistoryUpdate);
        public Task<TaskResult<bool>> DeleteRecord(RecoveryHistory recoveryHistoryDelete);
    }
}
