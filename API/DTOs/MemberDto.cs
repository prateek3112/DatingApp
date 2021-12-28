using System;
using System.Collections.Generic;
using API.Entities;
using API.Extensions;


namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string PhotoUrl { get; set; }
        public int Age { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastActive { get; set; }

        public string Gender { get; set; }

        public string Intro { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<PhotoDto> Photos { get; set; }
    }
}