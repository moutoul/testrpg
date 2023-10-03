namespace testrpg
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
        }
    }
}
