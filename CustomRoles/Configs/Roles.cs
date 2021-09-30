namespace CustomRoles.Configs
{
    using System.Collections.Generic;
    using CustomRoles.Roles;

    public class Roles
    {
        public List<BallisticZombie> BallisticZombies { get; set; } = new List<BallisticZombie>
        {
            new BallisticZombie(),
        };

        public List<BerserkZombie> BerserkZombies { get; set; } = new List<BerserkZombie>
        {
            new BerserkZombie(),
        };

        public List<ChargerZombie> ChargerZombies { get; set; } = new List<ChargerZombie>
        {
            new ChargerZombie(),
        };

        public List<Demolitionist> Demolitionists { get; set; } = new List<Demolitionist>
        {
            new Demolitionist()
        };

        public List<Dwarf> Dwarves { get; set; } = new List<Dwarf>
        {
            new Dwarf()
        };

        public List<DwarfZombie> DwarfZombies { get; set; } = new List<DwarfZombie>
        {
            new DwarfZombie()
        };

        public List<Medic> Medics { get; set; } = new List<Medic>
        {
            new Medic()
        };

        public List<MedicZombie> MedicZombies { get; set; } = new List<MedicZombie>
        {
            new MedicZombie()
        };

        public List<PDZombie> PdZombies { get; set; } = new List<PDZombie>
        {
            new PDZombie()
        };

        public List<Phantom> Phantoms { get; set; } = new List<Phantom>
        {
            new Phantom()
        };

        public List<PlagueZombie> PlagueZombies { get; set; } = new List<PlagueZombie>
        {
            new PlagueZombie()
        };

        public List<Scp575> Scp575s { get; set; } = new List<Scp575>
        {
            new Scp575()
        };

        public List<TankZombie> TankZombies { get; set; } = new List<TankZombie>
        {
            new TankZombie()
        };
    }
}