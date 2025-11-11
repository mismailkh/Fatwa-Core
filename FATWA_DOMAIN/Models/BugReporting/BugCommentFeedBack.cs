using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.BugReporting
{
    // Cannot restrict the limit of the string "Comment" in the database because it is used to store the multiple  images and text.
    [Table("BUG_COMMENT_FEEDBACK")]
    public class BugCommentFeedBack : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string? Comment { get; set; }
        public int? RemarkType { get; set; }
        public int? Rating { get; set; }
        public Guid? ParentCommentId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public int CommentFeedbackFrom;
        [NotMapped]
        public bool FromFatwa { get; set; } = false;
        [NotMapped]
        public int TicketStatusId { get; set; }
        [NotMapped]
        public Dictionary<string, string> MentionedUser { get; set; } = new Dictionary<string, string>();
        [NotMapped]
        public string MentionedUserTranslatedName { get; set; } = string.Empty;

    }
}
