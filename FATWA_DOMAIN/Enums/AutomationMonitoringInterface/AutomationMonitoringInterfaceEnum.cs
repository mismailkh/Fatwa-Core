using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Enums.AutomationMonitoringInterface
{
    public class AutomationMonitoringInterfaceEnum
    {
        public enum QueueItemsEnum
        {
            [Display(Name = "Pending")]
            Pending = 1,
            [Display(Name = "Locked")]
            Locked = 2,
            [Display(Name = "Completed")]
            Completed = 3,
            [Display(Name = "Exception")]
            Exception = 4, 
            [Display(Name = "Reattempt Exception")]
            ReattemptException = 5, 
            [Display(Name = "Running")]
            Running = 6,
            [Display(Name = "Stop")]
            Stop = 7,
            [Display( Name = "Reinstated")]
            Reinstated = 8,
            [Display(Name = "Closed")]
            Closed = 9,
            [Display(Name = "Disabled")]
            Disabled = 10,
        }
        public enum AMSQueueSessionEnum
        {
            [Display(Name = "Pending")]
            Pending = 1,
            [Display(Name = "Running")]
            Running = 2,
            [Display(Name = "Completed")]
            Completed = 3,
            [Display(Name = "Stop")]
            Stop = 4,
            [Display(Name = "Terminated")]
            Terminated = 5,
        }
        public enum AMSResourceEnum
        {
            [Display(Name = "ResourceIdle")]
            ResourceIdle = 1,
            [Display(Name = "ResourceWorking")]
            ResourceWorking = 2,
            [Display(Name = "ResourceLoggedOut")]
            ResourceLoggedOut = 3,
        }
    }
}
