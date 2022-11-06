namespace MySportsClubManager.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
            public const int DescriptionMaxLength = 2000;
            public const int AddressMaxLength = 500;
        }

        public static class Sport
        {
            public const int NameMaxLength = 60;
            public const int DescriptionMaxLength = 2000;
        }

        public static class Creator
        {
            public const int NameMaxLength = 100;
        }

        public static class Country
        {
            public const int NameMaxLength = 50;
        }

        public static class Review
        {
            public const int ReviewTextMaxLength = 200;
        }

        public static class Contest
        {
            public const int NameMaxLength = 100;
            public const int AddressMaxLength = 200;
        }

        public static class Training
        {
            public const int TopicMaxLength = 100;
        }
    }
}
