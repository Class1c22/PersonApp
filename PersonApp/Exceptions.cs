namespace PersonApp.Models
{
    public class FutureBirthDateException : Exception
    {
        public FutureBirthDateException()
            : base("Дата народження не може бути в майбутньому") { }
    }

    public class DistantPastBirthDateException : Exception
    {
        public DistantPastBirthDateException()
            : base("Дата народження занадто давня (максимум 150 років)") { }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
            : base("Невірний формат електронної пошти (приклад: user@example.com)") { }
    }
}