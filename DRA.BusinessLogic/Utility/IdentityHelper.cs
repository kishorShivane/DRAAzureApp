using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRA.BusinessLogic.Utility
{
    public class IdentityHelper
    {
        public IdentityHelper(string identityNumber)
        {
            Initialize(identityNumber);
        }

        public string IdentityNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public string Gender { get; private set; }

        public int Age { get; private set; }

        public string AgeToLongString { get; private set; }

        public bool IsSouthAfrican { get; private set; }

        public bool IsValid { get; private set; }
        public int Status { get; set; }
        public string ValidationMessage { get; set; }
        public int InValidCount { get; set; }

        public void validateRSAidnumber(string idnumber)
        {

            int invalid = 0;

            // check that value submitted is a number
            if (!IsNumeric(idnumber))
            {
                ValidationMessage = ValidationMessage + "Value supplied is not a valid number.<br />";
                Status = -1;
                invalid++;
                return;
            }

            // check length of 13 digits
            if (idnumber.Length != 13)
            {
                ValidationMessage = ValidationMessage + "Number supplied does not have 13 digits.<br />";
                Status = -2;
                invalid++;
                return;
            }

            // check that YYMMDD group is a valid date
            int yy = Convert.ToInt32(idnumber.Substring(0, 2)) <= 19 ? Convert.ToInt32("20" + idnumber.Substring(0, 2)) : Convert.ToInt32("19" + idnumber.Substring(0, 2));
            int mm = Convert.ToInt32(idnumber.Substring(2, 2));
            int dd = Convert.ToInt32(idnumber.Substring(4, 2));

            DateTime dob = new DateTime();
            try
            {
                dob = new DateTime(yy, (mm), dd);
            }
            catch (Exception)
            {
                ValidationMessage = ValidationMessage + "Date in first 6 digits is invalid.<br />";
                Status = -3;
                invalid++;
                return;
            }


            // check values - add one to month because Date() uses 0-11 for months
            if (!((dob.Year.ToString() == yy.ToString()) && (dob.Month == mm) && (dob.Day == dd)))
            {
                ValidationMessage = ValidationMessage + "Date in first 6 digits is invalid.<br />";
                Status = -3;
                invalid++;
                return;
            }

            // evaluate GSSS group for gender and sequence 
            string gender = Convert.ToInt32(idnumber.Substring(6, 4), 10) > 5000 ? "M" : "F";

            // ensure third to last digit is a 1 or a 0
            if (Convert.ToInt32(idnumber.Substring(10, 1)) > 1)
            {
                ValidationMessage = ValidationMessage + "Third to last digit can only be a 0 or 1 but is a " + idnumber.Substring(10, 11) + ".<br />";
                Status = -4;
                invalid++;
                return;
            }
            else
            {
                // determine citizenship from third to last digit (C)
                string saffer = Convert.ToInt32(idnumber.Substring(10, 1), 10) == 0 ? "C" : "F";
            }

            // ensure second to last digit is a 8 or a 9
            if (Convert.ToInt32(idnumber.Substring(11, 1)) < 8)
            {
                ValidationMessage = ValidationMessage + "Second to last digit can only be a 8 or 9 but is a " + idnumber.Substring(11, 1) + ".<br />";
                Status = -5;
                invalid++;
                return;
            }

            // calculate check bit (Z) using the Luhn algorithm
            int ncheck = 0;
            bool beven = false;

            for (int c = idnumber.Length - 1; c >= 0; c--)
            {
                string cdigit = idnumber.ToCharArray()[c].ToString();
                int ndigit = Convert.ToInt32(cdigit, fromBase: 10);

                if (beven)
                {
                    if ((ndigit *= 2) > 9)
                    {
                        ndigit -= 9;
                    }
                }

                ncheck += ndigit;
                beven = !beven;
            }

            if ((ncheck % 10) != 0)
            {
                ValidationMessage = ValidationMessage + "Checkbit is incorrect.<br />";
                Status = -6;
                invalid++;
                return;
            }
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        private void Initialize(string identityNumber)
        {
            IdentityNumber = (identityNumber ?? string.Empty).Replace(" ", "");
            if (IdentityNumber.Length == 13)
            {
                int[] digits = new int[13];
                for (int i = 0; i < 13; i++)
                {
                    digits[i] = int.Parse(IdentityNumber.Substring(i, 1));
                }
                int control1 = digits.Where((v, i) => i % 2 == 0 && i < 12).Sum();
                string second = string.Empty;
                digits.Where((v, i) => i % 2 != 0 && i < 12).ToList().ForEach(v =>
                      second += v.ToString());
                string string2 = (int.Parse(second) * 2).ToString();
                int control2 = 0;
                for (int i = 0; i < string2.Length; i++)
                {
                    control2 += int.Parse(string2.Substring(i, 1));
                }
                int control = (10 - ((control1 + control2) % 10)) % 10;
                if (digits[12] == control)
                {
                    BirthDate = DateTime.ParseExact(IdentityNumber
                        .Substring(0, 6), "yyMMdd", null);
                    Gender = digits[6] < 5 ? "Female" : "Male";
                    IsSouthAfrican = digits[10] == 0;
                    if (BirthDate > DateTime.Now)
                    {
                        IsValid = false;
                        return;
                    }

                    Age = CalculateAge(BirthDate);
                    AgeToLongString = CalculateAgeToLongString(BirthDate);
                    IsValid = true;
                }
            }
            validateRSAidnumber(identityNumber);
        }

        private int CalculateAge(DateTime birthDay)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDay.Year;
            if (birthDay > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private string CalculateAgeToLongString(DateTime birthDay)
        {
            TimeSpan difference = DateTime.Now.Subtract(birthDay);
            DateTime currentAge = DateTime.MinValue + difference;
            int years = currentAge.Year - 1;
            int months = currentAge.Month - 1;
            int days = currentAge.Day - 1;

            return string.Format("{0} years, {1} months and {2} days.", years, months, days);
        }
    }
}
