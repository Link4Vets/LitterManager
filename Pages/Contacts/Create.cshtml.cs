using LitterManager.Authorization;
using LitterManager.Data;
using LitterManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LitterManager.Pages.Contacts
{
    #region snippetCtor
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }
        #endregion

        public IActionResult OnGet()
        {
            Contact = new Contact
            {
                Name = "Rick",
                Address = "123 N 456 S",
                City = "GF",
                State = "MT",
                Zip = "59405",
                Email = "rick@rick.com"
            };
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

        #region snippet_Create
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Contact.OwnerID = UserManager.GetUserId(User);

            // requires using LitterManager.Authorization;
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Contact,
                                                        ContactOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            Context.Contact.Add(Contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        #endregion
    }
}