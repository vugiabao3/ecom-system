namespace EcomSystem.Contracts.Enums
{
    public enum UserRole
    {
        User = 0,
        Seller = 1,
        Shipper = 2,
        Admin = 3
    }

    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Preparing = 2,
        ReadyForShipment = 3,
        Shipping = 4,
        Delivered = 5,
        DeliveryFailed = 6,
        Returned = 7,
        Cancelled = 8
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2,
        Cancelled = 3,
        Refunded = 4
    }

    public enum ShipmentStatus
    {
        Created = 0,
        Assigned = 1,
        PickedUp = 2,
        Delivering = 3,
        Delivered = 4,
        Failed = 5,
        Returned = 6
    }

    public enum PaymentMethod
    {
        QR = 0,
        COD = 1
    }
}
