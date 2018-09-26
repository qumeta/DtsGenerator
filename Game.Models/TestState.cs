using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Game.Models
{
    [ComplexType]
    public class Address
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    [Serializable]
    public class TestState
    {
        public long Id { get; set; }
        public bool IsPersisted { get; set; }

        public string Name { get; set; }
        //public Address Data { get; set; }
        //public Dictionary<int, int> items { get; set; }
        //public List<int> attrs { get; set; }
    }
}
