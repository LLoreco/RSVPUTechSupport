using BlazorApp.Components.Data;
using BlazorApp.Components.Model;

namespace BlazorApp.Components.PageActions
{
    public class WorkMapper
    {
        public WorkDataModel MapWorkToWorkDataModel(Work work)
        {
            return new WorkDataModel
            {
                Id = work.id,
                fromWhomName = work.from_whom_id,
                workNumber = work.work_number,
                description = work.description,
                sendTime = work.send_time,
                timeLimit = work.time_limit,
                totalTime = work.total_time,
                status = work.status,
                employeeId = work.employee_id,
                employeeName = work.image
            };
        }

        public List<WorkDataModel> MapWorksToWorkDataModels(List<Work> works)
        {
            return works.Select(MapWorkToWorkDataModel).ToList();
        }
    }
}
