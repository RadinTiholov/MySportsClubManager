namespace MySportsClubManager.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class DataValidation
    {
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
    }
}
