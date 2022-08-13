using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Data.Interfaces;

namespace EShop.Data.Entities
{
    public class UserAudit : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; private set; }

        [Column(TypeName = "datetime")]
        public DateTime EventDate { get; private set; } = DateTime.Now;

        [Required]
        public UserAuditEventType AuditEvent { get; set; }

        public string IpAddress { get; private set; }
        
        public static UserAudit CreateAuditEvent(string userId, UserAuditEventType auditEventType, string ipAddress)
        {
            return new UserAudit { UserId = userId, AuditEvent = auditEventType, IpAddress = ipAddress };
        }
    }

    public enum UserAuditEventType
    {
        Login = 1,
        FailedLogin = 2,
        LogOut = 3
    }
}
