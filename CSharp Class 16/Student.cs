using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Class_16
{
    class Student
    {
        public DateTime Birthday { get; set; }

        public Student(DateTime birthday)
        {
            Birthday = birthday;
        }
    }
}
