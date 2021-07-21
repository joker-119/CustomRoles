using CustomRoles.Configs.Roles;
using CustomRoles.Roles;

namespace CustomRoles
{
    public class RoleConfigs
    {
        public ShotgunnerConfig ShotCfg { get; set; } = new ShotgunnerConfig();
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
        public JuggernautZombieConfig JuggCfg { get; set; } = new JuggernautZombieConfig();
    }
}