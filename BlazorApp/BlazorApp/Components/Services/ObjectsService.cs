using BlazorApp.Components.Data;

namespace BlazorApp.Components.Services
{
    public class ObjectsService
    {
        protected readonly ApplicationDbContext _dbContext;
        public ObjectsService(ApplicationDbContext _db)
        {
            _dbContext = _db;
        }
        public List<Objects> GetObjects()
        {
            return _dbContext.objects.ToList().OrderBy(e => e.id).ToList();
        }
        public bool InsertRecord(Objects objects)
        {
            _dbContext.objects.Add(objects);
            _dbContext.SaveChanges();
            return true;
        }
        public Objects EditRecord(int objectID)
        {
            Objects obj = new Objects();
            return _dbContext.objects.FirstOrDefault(u => u.id == objectID);
        }
        public bool UpdateRecord(Objects objectsUpdate)
        {
            var objectRecordUpdate = _dbContext.objects.FirstOrDefault(u => u.id == objectsUpdate.id);
            if (objectRecordUpdate != null)
            {
                objectRecordUpdate.object_name = objectsUpdate.object_name;
                objectRecordUpdate.type = objectsUpdate.type;
                objectRecordUpdate.buy_date = objectsUpdate.buy_date;
                objectRecordUpdate.break_count = objectsUpdate.break_count;
                objectRecordUpdate.recovery_date = objectsUpdate.recovery_date;
                objectRecordUpdate.room_number = objectsUpdate.room_number;
                _dbContext.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }
        public bool DeleteRecord(Objects objectDelete)
        {
            var obejectRecordDelete = _dbContext.objects.FirstOrDefault(u => u.id == objectDelete.id);
            if (obejectRecordDelete != null)
            {
                _dbContext.Remove(obejectRecordDelete);
                _dbContext.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
