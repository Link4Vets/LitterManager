using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LitterManager.Models
{
    public class Invitation
    {
        public Invitation()
        {
            this.Status = InvitationStatus.Draft;
        }

        public int InvitationId { get; set; }
        public string SenderId { get; set; }
        public string OwnerId { get; set; }
        public int? LitterId { get; set; }
        public InvitationStatus Status { get; set; }
    }
}

namespace LitterManager
{
    public enum InvitationStatus
    {
        Draft,
        Sent,
        Inactive,
        Reclaimed,
        Closed
    }
}
