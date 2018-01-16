using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Models
{
    public class Customer
    {
        IEnumerable<Technology> _technologyList = null;

        [Required]
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [Required]
        [JsonProperty(PropertyName = "ownerId")]
        public string OwnerId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [Required]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty(PropertyName = "registrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty(PropertyName = "technologyList")]
        public IEnumerable<Technology> TechnologyList
        {
            get
            {
                if (_technologyList == null)
                {
                    _technologyList = new Technology[] { };
                }
                return _technologyList;
            }
            set
            {
                _technologyList = value;
            }
        }
    }
}
