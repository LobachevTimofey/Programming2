namespace PatientLibrary.UnitTests
{
    [TestFixture]
    public class PatientUnitTests
    {
        [Test]
        public void ConstructorTest()
        {
            Patient john = CreateTestPatient();
            john.Price = 500;
            Assert.That(john.Name, Is.EqualTo("John"));
            Assert.That(john.Surname, Is.EqualTo("Smith"));
            Assert.That(john.Arrival.ToShortDateString(), Is.EqualTo("15.07.2003"));
            Assert.That(john.Discharge.ToShortDateString(), Is.EqualTo("18.07.2003"));
            Assert.That(john.Type, Is.EqualTo(ServiceType.Paid));
            Assert.That(john.PolicyNumber, Is.EqualTo(123546869));
            Assert.That(john.Price, Is.EqualTo(500));
        }
        [Test]
        public void DatesCheck_ValidDates()
        {
            var patient = new Patient("Иван", "Иванов", 12345);
            DateTime arrival = new DateTime(2025, 4, 10);
            DateTime discharge = new DateTime(2025, 4, 15);

            Assert.DoesNotThrow(() => patient.DatesCheck(arrival, discharge));
            Assert.That(patient.Arrival, Is.EqualTo(arrival));
            Assert.That(patient.Discharge, Is.EqualTo(discharge));
        }

        [Test]
        public void DatesCheck_InvalidDates()
        {
            var patient = new Patient("Иван", "Иванов", 12345);
            DateTime arrival = new DateTime(2025, 4, 15);
            DateTime discharge = new DateTime(2025, 4, 10);

            var exception = Assert.Throws<ArgumentException>(() => patient.DatesCheck(arrival, discharge));
            Assert.That(exception.Message, Is.EqualTo("Дата поступления должна быть раньше даты выписки."));
        }

        [Test]
        public void GetInfoTest()
        {
            Patient patient = CreateTestPatient();
            patient.Price = 500;
            string[] actual = patient.GetInfo();

            Assert.That(actual.Length, Is.EqualTo(3));
            Assert.That(actual[0], Is.EqualTo("Имя: John, Фамилия: Smith, Номер полиса: 123546869"));
            Assert.That(actual[1], Is.EqualTo("Тип обслуживания: Платное, Дата поступления: 15.07.2003, Дата выписки: 18.07.2003"));
            Assert.That(actual[2], Is.EqualTo("Стоимость лечения: 500 руб."));

        }
        private Patient CreateTestPatient()
        {
            Patient patient = new Patient("John", "Smith", 123546869);
            patient.DatesCheck(new DateTime(2003, 7, 15), new DateTime(2003, 7, 18));
            patient.Type = ServiceType.Paid;

            return patient;
        }
    }
}