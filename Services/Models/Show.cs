using System.Collections.Generic;

namespace Services.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CastMember> Cast { get; set; }
    }
}
