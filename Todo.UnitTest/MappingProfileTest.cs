using AutoMapper;
using NUnit.Framework;
using Todo.Api;

namespace Todo.UnitTest
{
    [TestFixture]
    public class MappingProfileTest
    {
        [Test]
        public void MappingProfile_VerifyMappings()
        {
            var mappingProfile = new MappingProfile();

            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            (mapper as IMapper).ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
