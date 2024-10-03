using API.Data;
namespace API.Interfaces
{
    public interface IWorkService
    {
        List<Work> GetWorks();
        public Work GetWork(int id);
        bool InsertRecord(Work work);
        Work EditRecord(int workID);
        bool UpdateRecord(Work workUpdate);
        bool DeleteRecord(Work workDelete);
        string FindEmployee(int id, string fromWhom = null);
        int GetNextAvailableWorkNumber();
    }
}
