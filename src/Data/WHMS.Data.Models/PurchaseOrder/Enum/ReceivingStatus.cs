namespace WHMS.Data.Models.PurchaseOrder.Enum
{
    using System.ComponentModel.DataAnnotations;

    public enum ReceivingStatus
    {
        Unreceived = 0,

        [Display(Name = "Partially received")]
        PartiallyReceived = 1,

        Received = 2,
    }
}
