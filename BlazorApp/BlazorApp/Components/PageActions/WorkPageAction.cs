using BlazorApp.Components.Data;
using BlazorApp.Components.Model;
using BlazorApp.Components.Services;
using NLog;

namespace BlazorApp.Components.PageActions
{
    public class WorkPageAction
    {
        private readonly WorkMapper _workMapper;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WorkPageAction(WorkMapper workMapper)
        {
            _workMapper = workMapper;
        }

        public List<WorkDataModel> OnInitializedWorkPageAction(WorkService service, List<WorkDataModel> workDataList)
        {
            try
            {
                var works = service.GetWorks();
                workDataList = _workMapper.MapWorksToWorkDataModels(works);
                return workDataList;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Не удалось инициализировать таблицу WORK в WorkPageAction");
                return new List<WorkDataModel>();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Ошибка аргумента при инициализации таблицы WORK в WorkPageAction");
                return new List<WorkDataModel>();
            }
        }
        public List<WorkDataModel> InsertRecordWorkPageAction(Work workObject, WorkService service, WorkDataModel workModel, List<WorkDataModel> workDataList, IList<Work> workList)
        {
            try
            {
                if (workObject.id > 0)
                {
                    var updateWorkDetails = false;
                    if (workModel.Id == 0)
                    {
                        workModel.Id = workObject.id;
                        workModel.workNumber = workObject.work_number;
                        workModel.employeeId = workObject.employee_id;
                        workModel.description = workObject.description;
                        workModel.sendTime = DateTime.UtcNow;
                        workModel.timeLimit = DateTime.SpecifyKind(workObject.time_limit, DateTimeKind.Utc);
                        workModel.totalTime = workObject.total_time;
                        workModel.status = workObject.status;
                        workModel.employeeId = workObject.employee_id;
                        workModel.employeeName = service.FindEmployee(workModel.Id);
                    }
                    updateWorkDetails = service.UpdateRecord(workModel);
                }
                else
                {
                    workObject.id = 0;
                    service.InsertRecord(workObject);
                }
                workList = service.GetWorks();
                workDataList = _workMapper.MapWorksToWorkDataModels(workList.ToList());
                return workDataList;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Не удалось записать данные в таблицу WORK в WorkPageAction");
                return new List<WorkDataModel>();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Ошибка аргумента при записи таблицы WORK в WorkPageAction");
                return new List<WorkDataModel>();
            }
        }
        public Work GetWorkDataModelPageAction(IList<Work> workList, WorkService service, WorkDataModel workEdit, Work workObject, List<WorkDataModel> workDataList)
        {
            try
            {
                workList = service.GetWorks();
                foreach (var item in workList)
                {
                    if (workEdit.Id == item.id)
                    {
                        workObject = item;
                        workList = service.GetWorks();
                        workDataList = _workMapper.MapWorksToWorkDataModels(workList.ToList());
                        break;
                    }
                }
                return workObject;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы WORK в WorkPageAction");
                return new Work();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex,"Ошибка аргумента при инициализации таблицы WORK в WorkPageAction");
                return new Work();
            }

        }
        public List<WorkDataModel> DeleteWorkPageAction(WorkService service, WorkDataModel workDelete, IList<Work> workList, List<WorkDataModel> workDataList)
        {
            try
            {
                var res = service.DeleteRecord(workDelete);
                workList = service.GetWorks();
                workList.Clear();
                workList = service.GetWorks();
                workDataList = _workMapper.MapWorksToWorkDataModels(workList.ToList());
                return workDataList;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex,"Не удалось удалить данные из WORK в WorkPageAction");
                return new List<WorkDataModel>();
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex,"Ошибка аргумента при инициализации таблицы WORK в WorkPageAction");
                return new List<WorkDataModel>();
            }

        }
    }
}
