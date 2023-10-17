using testrpg.Dtos.Skills;
using testrpg.Dtos.Weapon;

namespace testrpg.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Momo";
        public int Hitpoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;

        public GetWeaponDto Weapon { get; set; }

        public List<GetSkillDto> Skills { get; set; }
    }
}
