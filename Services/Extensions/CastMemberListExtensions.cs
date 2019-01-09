using System.Collections.Generic;
using System.Linq;
using Services.Models;

namespace Services.Extensions
{
    public static class CastMemberListExtensions
    {
        public static IEnumerable<CastMember> SortedOnBirthDay(this IEnumerable<CastMember> castMembers)
        {
            return castMembers.OrderByDescending(c => c.BirthDay);
        }
    }
}