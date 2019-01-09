using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Services.Extensions;
using Xunit;

namespace UnitTests
{
    public class CastMemberListExtensionsTests
    {
        [Fact]
        public void SortedOnBirthDayTest()
        {
            var castMembers = new List<CastMember>()
            {
                new CastMember
                {
                    Id = 1,
                    Name = "last",
                    BirthDay = new DateTime(2010, 01, 01)
                },
                new CastMember
                {
                    Id = 1,
                    Name = "first",
                    BirthDay = new DateTime(2011, 01, 01)
                }
            };
            Assert.Equal("first", castMembers.SortedOnBirthDay().First().Name);
        }
    }
}
