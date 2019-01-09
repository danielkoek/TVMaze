using System;

namespace Services.Collectors.Dto
{
    public class CastDto
    {
        public Person Person { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
