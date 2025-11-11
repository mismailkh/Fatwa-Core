namespace FATWA_DOMAIN.Enums
{
    public class WorkflowEnums
    {
        public enum WorkflowStatusEnum
        {
            Draft = 1,
            InReview = 2,
            Published = 3,
            Active = 4,
            Suspended = 5,
            Inactive = 6,
            Deleted = 7
        }
        public enum WorkflowInstanceStatusEnum
        {
            InProgress = 1,
            Failed = 2,
            Expired = 3,
            Success = 4,
            Cancelled = 5,
        }
        public enum WorkflowActivityCategory
        {
            GeneralControls = 1,
            Tasks = 2,
            FlowControls = 3
        }
        public enum WorkflowControl
        {
            JumptoStep = 1,
            EndofFlow = 2
        }
        public enum WorkflowBranch
        {
            OptionBranch = 1,
            ConditionalBranch = 2
        }
        public enum SlaAction
        {
            AssignTo = 1,
            SendEmail = 2,
            DoNothing = 3
        }
    }
}
