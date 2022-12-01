namespace MySportsClubManager.Data.Common
{
    public static class DataValidation
    {
        public static class ApplicationUser
        {
            public const int FirstNameMaxLength = 50;
            public const int FirstNameMinLength = 2;
            public const int LastNameMaxLength = 50;
            public const int LastNameMinLength = 2;
            public const int UserNameMaxLength = 50;
            public const int UserNameMinLength = 3;
            public const int EmailMaxLength = 50;
            public const int EmailMinLength = 4;
            public const int PasswordMaxLength = 30;
            public const int PasswordMinLength = 6;
        }

        public static class Club
        {
            public const int NameMaxLength = 60;
            public const int NameMinLength = 2;
            public const int DescriptionMaxLength = 2000;
            public const int DescriptionMinLength = 50;
            public const int AddressMaxLength = 500;
            public const int AddressMinLength = 10;
            public const string MaxFee = "9999";
            public const string MinFee = "0.0";
        }

        public static class Sport
        {
            public const int SportNameMaxLength = 60;
            public const int SportNameMinLength = 2;
            public const int SportDescriptionMaxLength = 2000;
            public const int SportDescriptionMinLength = 50;
        }

        public static class Creator
        {
            public const int CreatorNameMaxLength = 100;
            public const int CreatorNameMinLength = 3;
        }

        public static class Country
        {
            public const int CountryNameMaxLength = 50;
            public const int CountryNameMinLength = 2;
        }

        public static class Review
        {
            public const int ReviewTextMaxLength = 200;
            public const int ReviewTextMinLength = 10;
        }

        public static class Contest
        {
            public const int NameMaxLength = 100;
            public const int NameMinLength = 10;
            public const int DescriptionMaxLength = 2000;
            public const int DescriptionMinLength = 50;
            public const int AddressMaxLength = 200;
            public const int AddressMinLength = 20;
        }

        public static class Training
        {
            public const int TopicMaxLength = 100;
            public const int TopicMinLength = 10;
        }

        public static class Trainer
        {
            public const int EmailMinLength = 30;
            public const int EmailMaxLength = 1000;
            public const int TopicMinLength = 3;
            public const int TopicMaxLength = 20;
        }
    }
}
