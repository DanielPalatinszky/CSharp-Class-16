using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Class_16
{
    static class Extensions
    {
        public static int Age(this Student student)
        {
            return DateTime.Now.Year - student.Birthday.Year; // Nem teljesen pontos számolás, de a példához megfelelő lesz
        }
    }
}
