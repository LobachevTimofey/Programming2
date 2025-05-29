namespace PatientLibrary
{
    public class Patient
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public readonly int PolicyNumber;
        public DateTime Arrival;
        public DateTime Discharge;
        public ServiceType Type;
        public int Price;

        public Patient(string name, string surname, int policyNumber)
        {
            Name = name;
            Surname = surname;
            PolicyNumber = policyNumber;
        }
        public void DatesCheck(DateTime arrival, DateTime discharge)
        {
            if ((discharge - arrival).TotalDays <= 0)
                throw new ArgumentException("Дата поступления должна быть раньше даты выписки.");

            Arrival = arrival;
            Discharge = discharge;
        }

        public virtual string[] GetInfo()
        {
            string serviceType;
            if (Type == ServiceType.Insurance)
                serviceType = "Страховое";
            else
                serviceType = "Платное";

            string arrival = Arrival.ToString("dd.MM.yyyy");
            string discharge = Discharge.ToString("dd.MM.yyyy");

            var info = new string[3];
            info[0] = $"Имя: {Name}, Фамилия: {Surname}, Номер полиса: {PolicyNumber}";
            info[1] = $"Тип обслуживания: {serviceType}, Дата поступления: {arrival}, Дата выписки: {discharge}";
            info[2] = $"Стоимость лечения: {Price} руб.";
            return info;
        }
    }
}
