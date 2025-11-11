namespace FATWA_DOMAIN.Enums.Inventory
{
    public class InventoryEnum
    {
        public enum RequestItemStatusEnum
        {
         Submitted =1,
         Approved =2,   
         PartiallyApproved =4,
         Rejected =8,
         InProgress =16,
         Delivered =32,
        }

        public enum PlaceOrderStatusEnum
        {
            Submitted = 1,
            Approved = 2,
            ApprovalInProgress = 4,
           
        }
        public enum InventoryTypeEnum
        {
            GS = 23,
            IT = 24,
        }

        public enum StoreTypeEnum
        {
            FatwaMainStore = 1,
            GSMainStore = 2,
            ITMainStore = 4,
            GSFloorStore = 8,
            ITFloorStore = 16
        }

    }
}
