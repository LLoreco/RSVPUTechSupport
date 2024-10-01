using BlazorApp.Components.Data;
using BlazorApp.Components.Model;

namespace BlazorApp.Components.Interfaces
{
    public interface IWorkService
    {
        List<Work> GetWorks();
        bool InsertRecord(Work work);
        Work EditRecord(int workID);
        bool UpdateRecord(WorkDataModel workUpdate);
        bool DeleteRecord(WorkDataModel workDelete);
        string FindEmployee(int id, string fromWhom = null);
        int GetNextAvailableWorkNumber();
    }
}
