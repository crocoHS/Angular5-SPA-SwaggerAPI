﻿using System;
using System.Collections.Generic;

namespace Todo.Model
{
    public class CustomerDT
    {
        public int ID { get; set;}
        public string OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public IEnumerable<TechnologyDT> TechnologyList { get; set; }
    }
}