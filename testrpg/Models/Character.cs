﻿namespace testrpg.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "Momo";
        public int Hitpoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;

        public Weapon Weapon { get; set; }  

        public List<Skill> Skills { get; set; }
        public User? User { get; set; }

        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }


    }
}
