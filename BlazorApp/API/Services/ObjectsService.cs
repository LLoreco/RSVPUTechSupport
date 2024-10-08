using API.Data;
using NLog;
using API.Models;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Services
{
    public class ObjectsService: IObjectService
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public ObjectsService(ApplicationDbContext _db)
        {
            _dbContext = _db;
        }
        public async Task<List<Objects>> GetObjects()
        {
            try
            {
                return await _dbContext.objects.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы OBJECTS в ObjectService");
                return new List<Objects>();
            }
        }
        public async Task<TaskResult<Objects>> GetObject(int id)
        {
            try
            {
                var objectDB = await _dbContext.objects.FindAsync(id);
                return new TaskResult<Objects>
                {
                    IsSuccess = true,
                    Result = objectDB
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Не удалось получить данные из таблицы OBJECTS в ObjectService");
                return new TaskResult<Objects>
                {
                    IsSuccess = false,
                    Result = null
                };
            }
        }
        public async Task<TaskResult<bool>> InsertRecord(Objects objects)
        {
            try
            {
                _dbContext.objects.Add(objects);
                _dbContext.SaveChanges();
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка добавления записи в ObjectService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
        public async Task<TaskResult<Objects>> EditRecord(int objectID)
        {
            try
            {
                var objectDB = await _dbContext.objects.FindAsync(objectID);
                return new TaskResult<Objects>
                {
                    IsSuccess = true,
                    Result = objectDB
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка редактирования записи в ObjectService");
                return new TaskResult<Objects>
                {
                    IsSuccess = false,
                    Result = null
                };
            }

        }
        public async Task<TaskResult<bool>> UpdateRecord(Objects objectsUpdate)
        {
            try
            {
                var objectDB = await _dbContext.objects.FindAsync(objectsUpdate.id);
                if (objectDB != null)
                {
                    objectDB.object_name = objectsUpdate.object_name;
                    objectDB.type = objectsUpdate.type;
                    objectDB.buy_date = objectsUpdate.buy_date;
                    objectDB.break_count = objectsUpdate.break_count;
                    objectDB.recovery_date = objectsUpdate.recovery_date;
                    objectDB.room_number = objectsUpdate.room_number;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new TaskResult<bool>
                    {
                        IsSuccess = false,
                        Result = false
                    };
                }
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка обновления записи в ObjectService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
        public async Task<TaskResult<bool>> DeleteRecord(Objects objectDelete)
        {
            try
            {
                var objectRecordDelete = await _dbContext.objects.FindAsync(objectDelete.id);
                if (objectRecordDelete != null)
                {
                    _dbContext.Remove(objectRecordDelete);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return new TaskResult<bool>
                    {
                        IsSuccess = false,
                        Result = false
                    };
                }
                return new TaskResult<bool>
                {
                    IsSuccess = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка удаления записи в ObjectService");
                return new TaskResult<bool>
                {
                    IsSuccess = false,
                    Result = false
                };
            }
        }
    }
}
