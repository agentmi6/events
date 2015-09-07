namespace Events.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Infrastructure;
    using Web.Models;
    using Microsoft.AspNet.Identity;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Event> Events { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new IdentityDbInit());
        }        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public class IdentityDbInit
   : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {
            protected override void Seed(ApplicationDbContext context)
            {
                PerformInitialSetup(context);
                base.Seed(context);
            }
            public void PerformInitialSetup(ApplicationDbContext context)
            {
                AppUserManager userMgr = new AppUserManager(new UserStore<ApplicationUser>(context));
                AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));
                string roleName = "Administrators";
                string userName = "Admin";
                string password = "MySecret";
                string email = " admin@example.com ";
                string fullName = "Admin";
                if (!roleMgr.RoleExists(roleName))
                {
                    roleMgr.Create(new AppRole(roleName));
                }
                ApplicationUser user = userMgr.FindByName(userName);
                if (user == null)
                {
                    userMgr.Create(new ApplicationUser { UserName = userName, Email = email, FullName = fullName},
                    password);
                    user = userMgr.FindByName(userName);
                }

                if (!userMgr.IsInRole(user.Id, roleName))
                {
                    userMgr.AddToRole(user.Id, roleName);
                }
            }
        }
    }
}
