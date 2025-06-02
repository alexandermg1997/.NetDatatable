namespace OptimitationDatatable.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string department { get; set; } = string.Empty;
        public decimal salary { get; set; }
        public string email { get; set; } = string.Empty;
        public DateTime hire_date { get; set; }
    }
}