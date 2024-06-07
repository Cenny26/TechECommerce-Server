using System.ComponentModel.DataAnnotations.Schema;
using TechECommerceServer.Domain.Entities.Common;

namespace TechECommerceServer.Domain.Entities
{
    public class File : BaseEntity
    {
        public string StorageType { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }

        [NotMapped]
        public override DateTime? ModifiedDate { get => base.ModifiedDate; set => base.ModifiedDate = value; }
    }
}
