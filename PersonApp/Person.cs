using System;

namespace PersonApp.Models
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDate { get; }

        public string IsAdultString => IsAdult ? "Дорослий" : "Не дорослий";
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public string IsBirthdayString => IsBirthday ? "Так" : "Ні";
        public bool IsBirthday { get; private set; }

        private bool IsAdult { get; set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            CalculateProperties();
        }

        public string FormattedBirthDate => BirthDate.ToString("dd/MM/yyyy");

        private void CalculateProperties()
        {
            var today = DateTime.Today;
            IsAdult = today.Year - BirthDate.Year - (today < BirthDate.AddYears(today.Year - BirthDate.Year) ? 1 : 0) >= 18;
            SunSign = GetWesternZodiac(BirthDate);
            ChineseSign = GetChineseZodiac(BirthDate);
            IsBirthday = today.Month == BirthDate.Month && today.Day == BirthDate.Day;
        }

        private string GetWesternZodiac(DateTime birthDate)
        {
            int day = birthDate.Day;
            int month = birthDate.Month;

            return month switch
            {
                1 => (day <= 19) ? "Козеріг" : "Водолій",
                2 => (day <= 18) ? "Водолій" : "Риби",
                3 => (day <= 20) ? "Риби" : "Овен",
                4 => (day <= 19) ? "Овен" : "Телець",
                5 => (day <= 20) ? "Телець" : "Близнюки",
                6 => (day <= 20) ? "Близнюки" : "Рак",
                7 => (day <= 22) ? "Рак" : "Лев",
                8 => (day <= 22) ? "Лев" : "Діва",
                9 => (day <= 22) ? "Діва" : "Терези",
                10 => (day <= 22) ? "Терези" : "Скорпіон",
                11 => (day <= 21) ? "Скорпіон" : "Стрілець",
                12 => (day <= 21) ? "Стрілець" : "Козеріг",
                _ => "Невідомо"
            };
        }

        private string GetChineseZodiac(DateTime birthDate)
        {
            string[] signs = { "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза", "Мавпа", "Півень", "Собака", "Свиня" };
            return signs[(birthDate.Year - 4) % 12];
        }
    }
}