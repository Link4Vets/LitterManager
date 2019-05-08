using LitterManager.Authorization;
using LitterManager.Data;
using LitterManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LitterManager.Pages.Litters
{
    #region snippet
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Litter> Litters { get; set; }
        public IList<Invitation> Invitations { get; set; }

        public async Task OnGetAsync()
        {
            var litters = from c in Context.Litters
                           select c;

            var isAuthorized = User.IsInRole(Constants.LitterManagersRole) ||
                               User.IsInRole(Constants.LitterAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            var invitations = from i in Context.Invitations
                              select i;

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                invitations = invitations.Where(i => i.SenderId == currentUserId || i.OwnerId == currentUserId);
                litters = from l in litters
                              join i in invitations
                              on l.LitterId equals i.LitterId
                              select l;
            }

            Litters = await litters.ToListAsync();
            Invitations = await invitations.ToListAsync();
        }
    }
    #endregion
}