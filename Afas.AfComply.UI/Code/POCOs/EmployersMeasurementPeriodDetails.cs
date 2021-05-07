using System;



    public class EmployersMeasurementPeriodDetails
    {
        public int EmployerId { get; set; }
        public string EmployerName { get; set; }
        public int PlanYearID { get; set; }
        public string PlanYear { get; set; }
        public int EmployeeTypeID { get; set; }
        public string EmployeeType { get; set; }
        public DateTime MeasurementStart { get; set; }
        public DateTime MeasurementEnd { get; set; }
        public DateTime AdminStart { get; set; }
        public DateTime AdminEnd { get; set; }
        public DateTime OpenStart { get; set; }
        public DateTime OpenEnd { get; set; }
        public DateTime StabilityStart { get; set; }
        public DateTime StabilityEnd { get; set; }
    }
