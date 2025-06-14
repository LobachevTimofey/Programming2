using System.Reflection;
using static PatientLibrary.Patient;

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
        public void CompareToTest()
        {
            var peter = new Patient("Peter", "Gabriel", 123);
            var kate = new Patient("Kate", "Bush", 456);
            var phil = new Patient("Phil", "Collins", 789);
            var john = new Patient("John", "Fogerty", 145);
            var tom = new Patient("Tom", "Fogerty", 43673);

            Assert.That(phil.CompareTo(peter), Is.LessThan(0));
            Assert.That(peter.CompareTo(kate), Is.GreaterThan(0));

            Assert.That(john.CompareTo(tom), Is.LessThan(0));
            Assert.That(tom.CompareTo(john), Is.GreaterThan(0));

            Assert.That(tom.CompareTo(tom), Is.EqualTo(0));

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
    [TestFixture]
    public class DepartmentTests
    {
        Department department;
        Patient[] patients;
        [SetUp]
        public void Setup()
        {
            var peter = new Patient("Peter", "Gabriel", 65000);
            var kate = new Patient("Kate", "Bush", 70000);
            var phil = new Patient("Phil", "Collins", 80000);
            var john = new Patient("John", "Fogerty", 60000);
            var tom = new Patient("Tom", "Fogerty", 60000);

            patients = new Patient[] { peter, kate, phil, john, tom, peter };
            department = new Department("�����������", patients);
        }
        [Test]
        public void ConstructorTest()
        {
            Assert.That(department.DepartmentName, Is.EqualTo("�����������"));

            var patientList = department.ToList();
            foreach (var patient in patients.Distinct())
            {
                Assert.That(patientList.Contains(patient), Is.True);
                Assert.That(patientList.Count(p => p.Equals(patient)), Is.EqualTo(1));
            }
        }
        [Test]
        public void CountTest()
        {
            Assert.That(department.Count, Is.EqualTo(5));
        }
        [Test]
        public void IEnumerableTest()
        {
            var i = 0;
            foreach (var member in department)
                Assert.That(member, Is.SameAs(patients[i++]));
        }
        [TestFixture]
        public class PatientCompareToTests
        {
            [Test]
            [TestCase("����", "������", "����", "������", 1)]
            [TestCase("����", "������", "����", "������", -1)]
            [TestCase("����", "������", "����", "������", -1)]
            [TestCase("����", "������", "����", "������", 1)]
            [TestCase("����", "������", "����", "������", 0)]
            public void CompareTo_VariousCases(string name1, string surname1, string name2, string surname2, int expected)
            {
                var patient1 = new Patient(name1, surname1, 12345);
                var patient2 = new Patient(name2, surname2, 67890);

                int result = patient1.CompareTo(patient2);

                Assert.That(result, Is.EqualTo(expected));
            }

            [Test]
            public void CompareTo_OtherIsNull_ReturnsPositive()
            {
                var patient = new Patient("����", "������", 12345);

                int result = patient.CompareTo(null);

                Assert.That(result, Is.EqualTo(1));
            }

            [TestFixture]
            public class PolicyNumberComparerTests
            {
                private PolicyNumberComparer comparer;

                [SetUp]
                public void Setup()
                {
                    comparer = new PolicyNumberComparer();
                }

                [Test]
                [TestCase(12345, "����", "������", 12345, "����", "��������", 0)]
                [TestCase(12345, "����", "������", 67890, "����", "��������", -1)]
                [TestCase(67890, "����", "������", 12345, "����", "��������", 1)]
                public void Compare_PolicyNumberVariousCases(int policy1, string name1, string surname1, int policy2, string name2, string surname2, int expected)
                {
                    var patient1 = new Patient(name1, surname1, policy1);
                    var patient2 = new Patient(name2, surname2, policy2);

                    int result = comparer.Compare(patient1, patient2);

                    Assert.That(result, Is.EqualTo(expected));
                }
            }
        }

    }
}