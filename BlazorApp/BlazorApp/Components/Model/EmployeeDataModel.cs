namespace BlazorApp.Components.Model
{
    public class EmployeeDataModel:DataModels
    {
        public string fullName { get; set; }
        public string division { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public long phone { get; set; }
        public int work_amount { get; set; }
        public int salary { get; set; }
        public bool status { get; set; }
        public string password { get; set; }
        public string login { get; set; }
    }
}
