using API.Data;
using API.Models;

namespace API.Interfaces
{
    public interface IFAQService
    {
        public Task<List<FAQ>> GetAllFAQ();
        public Task<TaskResult<FAQ>> GetFAQ(int id);
        public Task<TaskResult<bool>> InsertRecord(FAQ faq);
        public Task<TaskResult<FAQ>> EditRecord(int faqid);
        public Task<TaskResult<bool>> UpdateRecord(FAQ faqUpdate);
        public Task<TaskResult<bool>> DeleteRecord(FAQ faqDelete);
    }
}
