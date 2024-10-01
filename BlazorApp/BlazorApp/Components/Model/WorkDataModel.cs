namespace BlazorApp.Components.Model
{
    public class WorkDataModel:DataModels
    {
        public int workNumber { get; set; }
        public int fromWhomName { get; set; }
        public string description { get; set; }
        public DateTime sendTime { get; set; }
        public DateTime timeLimit { get; set; }
        public DateTime totalTime { get; set; }
        public string status { get; set; }
        public int employeeId { get; set; }
        public string employeeName { get; set; }
    }
}
