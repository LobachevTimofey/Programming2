using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientLibrary
{
    public class Department : IEnumerable<Patient>
    {
        public string DepartmentName;
        public int PatientNumber { get => patientList.Count; }
        List<Patient> patientList;

        public Department(string departmentName, IEnumerable<Patient> patients)
        {
            DepartmentName = departmentName;
            patientList = new List<Patient>();

            foreach (var patient in patients) 
                if (!patientList.Contains(patient))
                    patientList.Add(patient);
        }

        public IEnumerator<Patient> GetEnumerator() => patientList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
