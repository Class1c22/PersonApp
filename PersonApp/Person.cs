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
        public string SunSign { get; }
        public string ChineseSign { get; }
        public string IsBirthdayString => IsBirthday ? "Так" : "Ні";
        public bool IsBirthday { get; }
        public bool IsAdult { get; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            ValidateInput(firstName, lastName, email, birthDate);

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate.Date;

            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year - (today < BirthDate.AddYears(today.Year - BirthDate.Year) ? 1 : 0);

            IsAdult = age >= 18;
            SunSign = GetWesternZodiac();
            ChineseSign = GetChineseZodiac();
            IsBirthday = today.Month == BirthDate.Month && today.Day == BirthDate.Day;
        }

        private void ValidateInput(string firstName, string lastName, string email, DateTime birthDate)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ім'я не може бути порожнім");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Прізвище не може бути порожнім");

            ValidateEmail(email);
            ValidateBirthDate(birthDate);
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidEmailException();

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                    throw new InvalidEmailException();
            }
            catch
            {
                throw new InvalidEmailException();
            }
        }

        private void ValidateBirthDate(DateTime birthDate)
        {
            if (birthDate > DateTime.Today)
                throw new FutureBirthDateException();

            if (birthDate < DateTime.Today.AddYears(-150))
                throw new DistantPastBirthDateException();
        }

        private string GetWesternZodiac()
        {
            return BirthDate.Month switch
            {
                1 => BirthDate.Day <= 19 ? "Козеріг" : "Водолій",
                2 => BirthDate.Day <= 18 ? "Водолій" : "Риби",
                3 => BirthDate.Day <= 20 ? "Риби" : "Овен",
                4 => BirthDate.Day <= 19 ? "Овен" : "Телець",
                5 => BirthDate.Day <= 20 ? "Телець" : "Близнюки",
                6 => BirthDate.Day <= 20 ? "Близнюки" : "Рак",
                7 => BirthDate.Day <= 22 ? "Рак" : "Лев",
                8 => BirthDate.Day <= 22 ? "Лев" : "Діва",
                9 => BirthDate.Day <= 22 ? "Діва" : "Терези",
                10 => BirthDate.Day <= 22 ? "Терези" : "Скорпіон",
                11 => BirthDate.Day <= 21 ? "Скорпіон" : "Стрілець",
                12 => BirthDate.Day <= 21 ? "Стрілець" : "Козеріг",
                _ => "Невідомо"
            };
        }

        private string GetChineseZodiac()
        {
            string[] signs = { "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія",
                             "Кінь", "Коза", "Мавпа", "Півень", "Собака", "Свиня" };
            return signs[(BirthDate.Year - 4) % 12];
        }

        public string FormattedBirthDate => BirthDate.ToString("dd/MM/yyyy");
    }
}