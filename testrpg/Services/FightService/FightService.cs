using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using testrpg.Data;
using testrpg.Dtos.Fights;

namespace testrpg.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

      

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var serviceresponse = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Weapon)!.FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                int damage = DoWeaponAttack(attacker, opponent);
                if (opponent.Hitpoints <= 0)
                {
                    serviceresponse.Message = $"{opponent.Name} s'est fait pwn!";

                }
                await _context.SaveChangesAsync();

                serviceresponse.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHp = attacker.Hitpoints,
                    OpponentHp = opponent.Hitpoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = ex.Message;
            }
            return serviceresponse;
        }

        private static int DoWeaponAttack(Character? attacker, Character? opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.Hitpoints -= damage;

            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var serviceresponse = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Skills)!.FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill == null)
                {
                    serviceresponse.Success = false;
                    serviceresponse.Message = $"{attacker.Name} a pas la ref";
                    return serviceresponse;
                }
                int damage = DoSkillAttack(attacker, opponent, skill);
                if (opponent.Hitpoints <= 0)
                {
                    serviceresponse.Message = $"{opponent.Name} s'est fait pwn!";

                }
                await _context.SaveChangesAsync();

                serviceresponse.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHp = attacker.Hitpoints,
                    OpponentHp = opponent.Hitpoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = ex.Message;
            }
            return serviceresponse;
        }

        private static int DoSkillAttack(Character? attacker, Character? opponent, Skill? skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.Hitpoints -= damage;

            }

            return damage;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var serviceresponse = new ServiceResponse<FightResultDto>
            {
                Data = new FightResultDto()
            };

            try
            {
                var characters = await _context.Characters.Include(c => c.Weapon).Include(c => c.Skills).Where(c => request.CharactersId.Contains(c.Id)).ToListAsync();

                bool defeated = false;

                while(!defeated)
                {
                    foreach (var attacker in characters) 
                    {
                        var opponents = characters.Where(c =>c.Id != attacker.Id).ToList(); 
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon)
                        {
                            attackUsed= attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);

                        }
                        serviceresponse.Data.Log.Add($"{attacker.Name}frappe {opponent.Name} avec {attackUsed} et lui inflige {(damage >= 0 ? damage : 0)} dégats.");
                    if (opponent.Hitpoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            serviceresponse.Data.Log.Add($"{opponent.Name} s est fait pwn!");
                            serviceresponse.Data.Log.Add($"{attacker.Name} triomphe avec {attacker.Hitpoints} PV restant");
                            break;

                        }
                    }
                }
                characters.ForEach(c => { c.Fights++; c.Hitpoints = 100; });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceresponse.Success= false;
                serviceresponse.Message = ex.Message;
            }
            return serviceresponse;
        }

        public async Task<ServiceResponse<List<HighscoreDto>>> GetHighscore()
        {
            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();


            var serviceresponse = new ServiceResponse<List<HighscoreDto>>()
            {
                Data = characters.Select(c => _mapper.Map<HighscoreDto>(c)).ToList()
            };
            return serviceresponse;
        }
    }
}
