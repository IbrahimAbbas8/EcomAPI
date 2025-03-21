﻿using System.Text.Json.Serialization;

namespace Ecom.Core.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string AppUserId { get; set; }
       // [JsonIgnore]
        public virtual AppUser AppUser { get; set; }
    }
}