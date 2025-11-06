namespace Runtime.Skill
{
    public interface IBuff
    {
        void Apply(WeaponStat stats);
        void Remove(WeaponStat stats);
        void UpdateBuff(WeaponStat stats); 
    }
}