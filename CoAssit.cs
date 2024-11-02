using System;
using System.Collections.Generic;

namespace CoAssist
{
    public abstract class Patient
    {
        protected int age;
        protected string name;
        protected decimal weight;
        protected decimal height;
        protected int oxygen;
        protected decimal temperature;
        protected BP bloodPressure;

        public string Name => name;

        public Patient(int age, string name, decimal weight, decimal height, int oxygen, decimal temperature, BP bloodPressure)
        {
            this.age = age;
            this.name = name;
            this.weight = weight;
            this.height = height;
            this.oxygen = oxygen;
            this.temperature = temperature;
            this.bloodPressure = bloodPressure;
        }

        public decimal CalculateBMI()
        {
            return weight / (height * height);
        }

        public virtual void CheckVitals()
        {
            CoAssist.OxyMonitor(oxygen);
            CoAssist.TempMonitor(temperature);
            CoAssist.BPmonitor(bloodPressure);
        }
    }

    public class Family
    {
        public List<Patient> Patients { get; } = new List<Patient>();

        public void AddPatient(Patient patient)
        {
            Patients.Add(patient);
        }

        public void CheckAllPatients()
        {
            foreach (var patient in Patients)
            {
                patient.CheckVitals();
            }
        }

        public void DisplayAllPatients()
        {
            foreach (var patient in Patients)
            {
                Console.WriteLine($"Patient Name: {patient.Name}");
                CoAssist.BMIcalculator(patient);
                Console.WriteLine();
            }
        }
    }

    public class CriticalPatient : Patient
    {
        public CriticalPatient(int age, string name, decimal weight, decimal height, int oxygen, decimal temperature, BP bloodPressure)
            : base(age, name, weight, height, oxygen, temperature, bloodPressure) { }

        public override void CheckVitals()
        {
            base.CheckVitals();
            Console.WriteLine("Critical Patient Monitoring");
        }
    }

    public class StablePatient : Patient
    {
        public StablePatient(int age, string name, decimal weight, decimal height, int oxygen, decimal temperature, BP bloodPressure)
            : base(age, name, weight, height, oxygen, temperature, bloodPressure) { }

        public override void CheckVitals()
        {
            base.CheckVitals();
            Console.WriteLine("Stable Patient Monitoring");
        }
    }

    public class ChronicPatient : Patient
    {
        public ChronicPatient(int age, string name, decimal weight, decimal height, int oxygen, decimal temperature, BP bloodPressure)
            : base(age, name, weight, height, oxygen, temperature, bloodPressure) { }

        public override void CheckVitals()
        {
            base.CheckVitals();
            Console.WriteLine("Chronic Patient Monitoring");
        }
    }

    public abstract class BP
    {
        protected int systolic;
        protected int diastolic;

        public BP(int systolic, int diastolic)
        {
            this.systolic = systolic;
            this.diastolic = diastolic;
        }

        public abstract bool IsOutOfRange();
    }

    public class Dia : BP
    {
        public Dia(int systolic, int diastolic) : base(systolic, diastolic) { }

        public override bool IsOutOfRange()
        {
            return diastolic < 60 || diastolic > 80;
        }
    }

    public class Sys : BP
    {
        public Sys(int systolic, int diastolic) : base(systolic, diastolic) { }

        public override bool IsOutOfRange()
        {
            return systolic < 90 || systolic > 120;
        }
    }

    public static class CoAssist
    {
        public static void BPmonitor(BP pressure)
        {
            if (pressure.IsOutOfRange())
            {
                Console.WriteLine("Warning: Abnormal blood pressure! Seek medical attention.");
            }
        }

        public static void OxyMonitor(int oxyLevel)
        {
            if (oxyLevel < 95)
            {
                Console.WriteLine("Warning: Low oxygen levels! Suggested to use supplemental oxygen.");
            }
        }

        public static void TempMonitor(decimal temperature)
        {
            if (temperature < 36.1m || temperature > 37.2m)
            {
                Console.WriteLine("Warning: Abnormal temperature! Stay hydrated and monitor closely.");
            }
        }

        public static void BMIcalculator(Patient patient)
        {
            decimal bmi = patient.CalculateBMI();
            Console.WriteLine($"BMI for {patient.Name} is {bmi:F2}");
            if (bmi < 18.5m)
                Console.WriteLine("Underweight. Consider consulting a nutritionist.");
            else if (bmi > 24.9m)
                Console.WriteLine("Overweight. Recommended to start a fitness regime.");
            else
                Console.WriteLine("BMI is within the healthy range.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Family family = new Family();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n--- Patient Monitoring Menu ---");
                Console.WriteLine("1. Add a New Patient");
                Console.WriteLine("2. View All Patient Details");
                Console.WriteLine("3. Run Health Monitoring");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddNewPatient(family);
                        break;
                    case 2:
                        family.DisplayAllPatients();
                        break;
                    case 3:
                        family.CheckAllPatients();
                        break;
                    case 4:
                        running = false;
                        Console.WriteLine("Exiting program.");
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        static void AddNewPatient(Family family)
        {
            Console.Write("Enter patient name: ");
            string name = Console.ReadLine();
            Console.Write("Enter patient age: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Enter patient weight (kg): ");
            decimal weight = decimal.Parse(Console.ReadLine());
            Console.Write("Enter patient height (m): ");
            decimal height = decimal.Parse(Console.ReadLine());
            Console.Write("Enter oxygen level (%): ");
            int oxygen = int.Parse(Console.ReadLine());
            Console.Write("Enter temperature (°C): ");
            decimal temperature = decimal.Parse(Console.ReadLine());
            Console.Write("Enter systolic blood pressure: ");
            int systolic = int.Parse(Console.ReadLine());
            Console.Write("Enter diastolic blood pressure: ");
            int diastolic = int.Parse(Console.ReadLine());

            Console.WriteLine("Select Patient Type:");
            Console.WriteLine("1. Critical Patient");
            Console.WriteLine("2. Stable Patient");
            Console.WriteLine("3. Chronic Patient");
            Console.Write("Choose an option: ");
            int type = int.Parse(Console.ReadLine());

            BP bloodPressure = systolic > 120 ? new Sys(systolic, diastolic) : new Dia(systolic, diastolic);

            Patient patient = type switch
            {
                1 => new CriticalPatient(age, name, weight, height, oxygen, temperature, bloodPressure),
                2 => new StablePatient(age, name, weight, height, oxygen, temperature, bloodPressure),
                3 => new ChronicPatient(age, name, weight, height, oxygen, temperature, bloodPressure),
                _ => null
            };

            if (patient != null)
            {
                family.AddPatient(patient);
                Console.WriteLine("Patient added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid patient type selected.");
            }
        }
    }
}
