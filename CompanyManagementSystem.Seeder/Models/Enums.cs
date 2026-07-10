namespace CompanyManagementSystem.Seeder.Models.Enums
{
    public enum UserRoleEnum
    {
        Owner,
        Customer,
        Engineer
    }

    public enum CompanyRankEnum
    {
        Leader,
        Member
    }

    public enum OfferStatusEnum
    {
        Accepted,
        Rejected,
        Pending,
        Canceled
    }

    public enum TaskStateEnum
    {
        Todo,
        InProgress,
        Pending,
        Done,
        Failed
    }
}
