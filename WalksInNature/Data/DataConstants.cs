namespace WalksInNature.Data.Models
{
    public class DataConstants
    {
        public class User 
        {
            public const int FullNameMinLength = 5;
            public const int FullNameMaxLength = 40;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Walk 
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;

            public const int StartPointMinLength = 5;
            public const int StartPointMaxLength = 30;

            public const int DescriptionMinLength = 10;
        }
        public class Event
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 50;

            public const int StartPointMinLength = 5;
            public const int StartPointMaxLength = 30;

            public const int DescriptionMinLength = 10;
        }

        public class Level 
        {
            public const int NameMaxLength = 10;
        }
        public class Region
        {
            public const int NameMaxLength = 20;
        }
        public class Guide
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 25;

            public const int PhoneMinLength = 6;
            public const int PhoneMaxLength = 30;
        }
        public class Insurance
        {
            public const int PersonsMinValue = 1;
            public const int PersonsMaxValue = 10;

            public const int LimitMinValue = 2000;
            public const int LimitMaxValue = 10000;

            public const int BeneficiaryMinLength = 20;
            public const int BeneficiaryMaxLength = 800;
                       
        }
    }
}
