using LitterManager.Authorization;
using LitterManager.Data;
using LitterManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LitterManager.Pages.Invitations
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

        public IList<Invitation> Invitations { get; set; }

        public async Task OnGetAsync()
        {
            var isAuthorized = User.IsInRole(Constants.InvitationManagersRole) ||
                               User.IsInRole(Constants.InvitationAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            var invitations = from i in Context.Invitations
                              select i;

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                invitations = invitations.Where(i => i.SenderId == currentUserId || i.OwnerId == currentUserId);
                
            }

            Invitations = await invitations.ToListAsync();
        }
    }
    #endregion
}