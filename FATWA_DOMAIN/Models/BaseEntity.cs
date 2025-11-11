using FATWA_DOMAIN.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FATWA_DOMAIN.Models
{
    public abstract class BaseEntity<T> : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T ClassificationId { get; set; }

        #region IEntity Members

        [IgnoreDataMember] // OData v8 does not like this property and will break if we don't use [IgnoreDataMember] here.
        public object[] KeyValues => new object[] { ClassificationId };

        #endregion IEntity Members
    }
}
