using CustomRoles.Configs.Roles;
using CustomRoles.Roles;

namespace CustomRoles
{
    using ChargerZombie = CustomRoles.Configs.Roles.ChargerZombie;

    public class RoleConfigs
    {
        public DwarfConfig DwarfCfg { get; set; } = new DwarfConfig();
        public PhantomConfig PhantomCfg { get; set; } = new PhantomConfig();
        public Scp575Config Scp575Cfg { get; set; } = new Scp575Config();
        public MedicConfig MediCfg { get; set; } = new MedicConfig();
        public DemolitionistConfig DemoCfg { get; set; } = new DemolitionistConfig();
        public PlagueZombieConfig PlagueCfg { get; set; } = new PlagueZombieConfig();
        public BallisticZombieConfig BallisticCfg { get; set; } = new BallisticZombieConfig();
        public PDZombieConfig PDZombieCfg { get; set; } = new PDZombieConfig();
        public DwarfZombieConfig DwarfZombieCfg { get; set; } = new DwarfZombieConfig();
        public MedicZombieConfig MedicZombieCfg { get; set; } = new MedicZombieConfig();
        public TankZombieConfig TankZombieCfg { get; set; } = new TankZombieConfig();
        public BerserkZombieConfig BerserkZombieCfg { get; set; } = new BerserkZombieConfig();
        public ChargerZombie ChargerZombieCfg { get; set; } = new ChargerZombie();
    }
}