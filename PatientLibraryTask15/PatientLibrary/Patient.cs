namespace PatientLibrary
{
    public class Patient : IComparable<Patient>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public readonly int PolicyNumber;
        public DateTime Arrival;
        public DateTime Discharge;
        public ServiceType Type;
        public int Price;
        public Patient (string name, string surname, int policyNumber)
        {
            Name = name;
            Surname = surname;
            PolicyNumber = policyNumber;
        }
        public int CompareTo(Patient? other)
        {
            if (other is null) return 1;

            int surnameComparison = Surname.CompareTo(other.Surname);
            if (surnameComparison != 0)
                return surnameComparison;

            return Name.CompareTo(other.Name);
        }
        public void DatesCheck(DateTime arrival, DateTime discharge)
        {
            if (discharge <= arrival)
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

        public class InpatientPatient : Patient
        {
            public string Department { get; set; }
            public int RoomNumber { get; set; }

            public InpatientPatient(string name, string surname, int policyNumber, string department, int roomNumber) : base(name, surname, policyNumber)
            {
                Department = department;
                RoomNumber = roomNumber;
            }
            public override string[] GetInfo()
            {
                var patientInfo = base.GetInfo();
                return new string[]
                {
                    patientInfo[0],
                    $"{patientInfo[1]}, Название отделения: {Department}, Номер палаты: {RoomNumber}",
                    patientInfo[2]
                };
            }
        }
        public class DayPatient : Patient
        {
            public DateTime ArrivalDateTime { get; set; }
            public DateTime LeaveDateTime { get; set; }

            public DayPatient(string name, string surname, int policyNumber, DateTime arrivalDateTime, DateTime leaveDateTime)
                : base(name, surname, policyNumber)
            {
                ArrivalDateTime = arrivalDateTime;
                LeaveDateTime = leaveDateTime;
            }
            public override string[] GetInfo()
            {
                var baseInfo = base.GetInfo();

                string arrivalFormatted = ArrivalDateTime.ToString("HH:mm:ss");
                string leaveFormatted = LeaveDateTime.ToString("HH:mm:ss");

                return new string[]
                {
                    baseInfo[0],
                    $"{baseInfo[1]}, Точное время прихода: {arrivalFormatted}, Точное время ухода: {leaveFormatted}", 
                    baseInfo[2] 
                };
            }

        }
        public class AmbulatoryPatient : Patient
        {
            public string DoctorName { get; set; }

            public AmbulatoryPatient(string name, string surname, int policyNumber, string doctorName)
                : base(name, surname, policyNumber)
            {
                DoctorName = doctorName;
            }

            public override string[] GetInfo()
            {
                var baseInfo = base.GetInfo();
                return new string[]
                {
                        baseInfo[0],
                        $"{baseInfo[1]}, ФИО лечащего врача: {DoctorName}",
                        baseInfo[2]
                };
            }

        }
    }
}