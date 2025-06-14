namespace PatientLibrary.UnitTests
{
    [TestFixture]
    public class PatientUnitTests
    {
        [Test]
        public void ConstructorTest()
        {
            Patient john = CreateTestPatient();

            Assert.That(john.Name, Is.EqualTo("John"));
            Assert.That(john.Surname, Is.EqualTo("Smith"));
            Assert.That(john.Arrival.ToString("dd.MM.yyyy"), Is.EqualTo("15.07.2003"));
            Assert.That(john.Discharge.ToString("dd.MM.yyyy"), Is.EqualTo("18.07.2003"));
            Assert.That(john.Type, Is.EqualTo(ServiceType.Paid));
            Assert.That(john.PolicyNumber, Is.EqualTo(123546869));
            Assert.That(john.Price, Is.EqualTo(500));
        }


        [Test]
        public void DatesCheck_ValidDates()
        {
            var patient = new Patient("����", "������", 12345);
            DateTime arrival = new DateTime(2025, 4, 10);
            DateTime discharge = new DateTime(2025, 4, 15);

            Assert.DoesNotThrow(() => patient.DatesCheck(arrival, discharge));
            Assert.That(patient.Arrival, Is.EqualTo(arrival));
            Assert.That(patient.Discharge, Is.EqualTo(discharge));
        }
        [Test]
        public void DatesCheck_InvalidDates()
        {
            var patient = new Patient("����", "������", 12345);
            DateTime arrival = new DateTime(2025, 4, 15);
            DateTime discharge = new DateTime(2025, 4, 15);

            var exception = Assert.Throws<ArgumentException>(() => patient.DatesCheck(arrival, discharge));

            Assert.That(exception.Message, Is.EqualTo("���� ����������� ������ ���� ������ ���� �������."));
        }
        [Test]
        public void GetInfoTest()
        {
            Patient patient = CreateTestPatient();
            string[] actual = patient.GetInfo();

            Assert.That(actual.Length, Is.EqualTo(3));
            Assert.That(actual[0], Is.EqualTo("���: John, �������: Smith, ����� ������: 123546869"));
            Assert.That(actual[1], Is.EqualTo("��� ������������: �������, ���� �����������: 15.07.2003, ���� �������: 18.07.2003"));
            Assert.That(actual[2], Is.EqualTo("��������� �������: 500 ���."));
        }

        private Patient CreateTestPatient()
        {
            Patient patient = new Patient("John", "Smith", 123546869);
            patient.DatesCheck(new DateTime(2003, 7, 15), new DateTime(2003, 7, 18));
            patient.Type = ServiceType.Paid;
            patient.Price = 500;

            return patient;
        }

    }

    [TestFixture]
    public class PatientTypesUnitTests
    {
        [Test]
        public void InpatientPatient_Constructor_Check()
        {
            string expectedDepartment = "�����������";
            int expectedRoomNumber = 101;

            var inpatient = new Patient.InpatientPatient("����", "������", 12345, expectedDepartment, expectedRoomNumber);

            Assert.That(inpatient.Department, Is.EqualTo(expectedDepartment));
            Assert.That(inpatient.RoomNumber, Is.EqualTo(expectedRoomNumber));
        }

        [Test]
        public void DayPatient_Constructor_Check()
        {
            DateTime expectedArrivalDateTime = new DateTime(2025, 4, 13);
            DateTime expectedLeaveDateTime = new DateTime(2025, 4, 14);

            var dayPatient = new Patient.DayPatient("����", "��������", 54321, expectedArrivalDateTime, expectedLeaveDateTime);

            Assert.That(dayPatient.ArrivalDateTime, Is.EqualTo(expectedArrivalDateTime));
            Assert.That(dayPatient.LeaveDateTime, Is.EqualTo(expectedLeaveDateTime));
        }

        [Test]
        public void AmbulatoryPatient_Constructor_Check()
        {
            string expectedDoctorName = "������ ������";

            var ambulatoryPatient = new Patient.AmbulatoryPatient("����", "������", 67890, expectedDoctorName);

            Assert.That(ambulatoryPatient.DoctorName, Is.EqualTo(expectedDoctorName));
        }
    }

    [TestFixture]
    public class GetInfoOverridesUnitTests
    {
        [Test]
        public void InpatientPatient_GetInfo_Check()
        {
            var inpatient = new Patient.InpatientPatient("����", "������", 12345, "�����������", 101);
            inpatient.Price = 5000;
            inpatient.Type = ServiceType.Paid;
            inpatient.DatesCheck(new DateTime(2025, 4, 10), new DateTime(2025, 4, 15));

            string[] info = inpatient.GetInfo();

            Assert.That(info.Length, Is.EqualTo(3));
            Assert.That(info[0], Is.EqualTo($"���: ����, �������: ������, ����� ������: 12345"));
            Assert.That(info[1], Is.EqualTo($"��� ������������: �������, ���� �����������: 10.04.2025, ���� �������: 15.04.2025, �������� ���������: �����������, ����� ������: 101"));
            Assert.That(info[2], Is.EqualTo($"��������� �������: 5000 ���."));
        }

        [Test]
        public void DayPatient_GetInfo_Check()
        {
            var dayPatient = new Patient.DayPatient(
                "����", "��������", 54321,
                new DateTime(2025, 4, 13, 9, 0, 1),
                new DateTime(2025, 4, 13, 17, 0, 1)
            );
            dayPatient.Price = 3000;
            dayPatient.Type = ServiceType.Insurance;

            dayPatient.DatesCheck(new DateTime(2025, 4, 13, 8, 0, 1), new DateTime(2025, 4, 14, 18, 0, 1));

            string[] actualInfo = dayPatient.GetInfo();

            Assert.That(actualInfo.Length, Is.EqualTo(3));
            Assert.That(actualInfo[0], Is.EqualTo($"���: ����, �������: ��������, ����� ������: 54321"));
            Assert.That(actualInfo[1], Is.EqualTo($"��� ������������: ���������, ���� �����������: 13.04.2025, ���� �������: 14.04.2025, ������ ����� �������: 09:00:01, ������ ����� �����: 17:00:01"));
            Assert.That(actualInfo[2], Is.EqualTo($"��������� �������: 3000 ���."));
        }
        [Test]
        public void AmbulatoryPatient_GetInfo_Check()
        {
            var ambulatoryPatient = new Patient.AmbulatoryPatient(
                "����", "������", 67890, "������ �������"
            );
            ambulatoryPatient.Price = 2000;
            ambulatoryPatient.Type = ServiceType.Paid;
            ambulatoryPatient.DatesCheck(new DateTime(2025, 4, 12), new DateTime(2025, 4, 13));

            string[] info = ambulatoryPatient.GetInfo();

            Assert.That(info.Length, Is.EqualTo(3));
            Assert.That(info[0], Is.EqualTo($"���: ����, �������: ������, ����� ������: 67890"));
            Assert.That(info[1], Is.EqualTo($"��� ������������: �������, ���� �����������: 12.04.2025, ���� �������: 13.04.2025, ��� �������� �����: ������ �������"));
            Assert.That(info[2], Is.EqualTo($"��������� �������: 2000 ���."));
        }

    }
}