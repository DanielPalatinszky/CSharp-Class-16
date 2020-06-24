using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_Class_16
{
    class Animal
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public Animal(int age, string name)
        {
            Age = age;
            Name = name;
        }
    }
}
