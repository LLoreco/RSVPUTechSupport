using BlazorApp.Components.Data;
using BlazorApp.Components.Services;
using BlazorApp.Components.Model;

namespace BlazorApp.Components.Interfaces
{
    public interface IWorkPageAction
    {
        List<WorkDataModel> OnInitializedWorkPageAction(WorkService service, List<WorkDataModel> workDataList);
        List<WorkDataModel> InsertRecordWorkPageAction(Work workObject, WorkService service, WorkDataModel workModel, List<WorkDataModel> workDataList, IList<Work> workList);
        Work GetWorkDataModelPageAction(IList<Work> workList, WorkService service, WorkDataModel workEdit, Work workObject, List<WorkDataModel> workDataList);
        List<WorkDataModel> DeleteWorkPageAction(WorkService service, WorkDataModel workDelete, IList<Work> workList, List<WorkDataModel> workDataList);

    }
}
