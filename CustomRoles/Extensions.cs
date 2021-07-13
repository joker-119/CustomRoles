namespace CustomRoles
{
    public static class Extensions
    {
        public static bool IsGun(this ItemType item)
        {
            switch (item)
            {
                case ItemType.GunCOM15:
                case ItemType.GunE11SR:
                case ItemType.GunLogicer:
                case ItemType.GunMP7:
                case ItemType.GunProject90:
                case ItemType.GunUSP:
                    return true;
                default:
                    return false;
            }
        }
    }
}