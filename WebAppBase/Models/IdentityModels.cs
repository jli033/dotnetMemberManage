using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAppBase.Models
{
    public enum Authorization
    {
        Deny,
        Allow,
    }

    // ApplicationUser クラスにプロパティを追加することでユーザーのプロファイル データを追加できます。詳細については、http://go.microsoft.com/fwlink/?LinkID=317594 を参照してください。
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType が CookieAuthenticationOptions.AuthenticationType で定義されているものと一致している必要があります
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // ここにカスタム ユーザー クレームを追加します
            return userIdentity;
        }
    }

    public interface IRoleMenuStore : IRoleStore<ApplicationRole, string>
    {
        void CreateMenu(ApplicationMenu menu);
        List<ApplicationRole> GetAllowRolesByMenuId(int MenuId);
        List<ApplicationRole> GetAllowRolesByControllNameActionName(string controllerName, string actionName);
        Task<List<ApplicationMenu>> GetMenusByRoleNameAsync(string RoleName);
        Task AddRoleMenuAsync(string RoleId, int MenuId, int? DisplayNo, bool ShowInMenu, bool SperateMenuFlag);
        Task SetMenuDisplayNoAsync(string RoleId, int MenuId, int DisplayNo);
        Task SetMenuAuthorizationStatusAsync(string RoleId, int MenuId, int AuthorizationStatus);
    }

    public class RoleMenuStore : RoleStore<ApplicationRole>, IRoleMenuStore
    {
        public RoleMenuStore(DbContext context)
            : base(context)
        {

        }

        public void CreateMenu(ApplicationMenu menu)
        {
            var MenuSet = Context.Set<ApplicationMenu>();
            var om = MenuSet.FirstOrDefault(m => m.MenuId.Equals(menu.MenuId));
            if (om != null)
            {
                throw new ArgumentException(string.Format("{0}", menu.MenuId));
            }
            MenuSet.Add(menu);
            Context.SaveChanges();
        }

        public async Task<List<ApplicationMenu>> GetMenusByRoleNameAsync(string RoleName)
        {
            var role = await this.FindByNameAsync(RoleName);
            if (role == null)
            {
                return null;
            }

            var menus = Context.Set<ApplicationMenu>();
            List<ApplicationMenu> list = new List<ApplicationMenu>();
            role.RoleMenus.Sort((x, y) => x.DisplayNo.CompareTo(y.DisplayNo));
            foreach (var rm in role.RoleMenus)
            {                
                var m = menus.Find(rm.MenuId);
                if (m != null)
                {
                    m.ShowInMenu = rm.ShowInMenu;
                    m.SeparateMenuFlag = rm.SeparateMenuFlag;
                    list.Add(m);
                }
            }
            return list;
        }

        public List<ApplicationRole> GetAllowRolesByMenuId(int MenuId)
        {
            var menus = Context.Set<ApplicationMenu>();
            var menu= menus.Find(MenuId);
            if (menu == null)
            {
                throw new KeyNotFoundException();
            }
            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roles = DbEntitySet.Where(m => m.MenuId.Equals(MenuId) && m.AuthorizationStatus == Convert.ToInt32(Authorization.Allow)).Select(m => m.Role).ToList();
            return roles;
        }
        public List<ApplicationRole> GetAllowRolesByControllNameActionName(string controllerName, string actionName)
        {
            var menus = Context.Set<ApplicationMenu>();
            var menu = menus.FirstOrDefault(m => m.ControllerName.ToLower().Equals(controllerName) && m.ActionName.ToLower().Equals(actionName));
            if (menu == null)
            {
                throw new KeyNotFoundException();
            }
            
            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var allowStatus=Convert.ToInt32(Authorization.Allow);
            var roles = DbEntitySet.Where(m => m.MenuId.Equals(menu.MenuId) && m.AuthorizationStatus == allowStatus).Select(m => m.Role).ToList();
            return roles;
       
        }

        public async Task AddRoleMenuAsync(string RoleId, int MenuId, int? DisplayNo, bool ShowInMenu, bool SperateMenuFlag)
        {
            var role = await this.FindByIdAsync(RoleId);
            if (role == null)
            {
                throw new Exception();
            }
            //var roleMenu = await DbEntitySet.FindAsync(RoleId, MenuId);
            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.MenuId.Equals(MenuId));
            if (roleMenu == null)
            {
                ApplicationRoleMenu rm = new ApplicationRoleMenu { MenuId = MenuId, RoleId = RoleId, DisplayNo = DisplayNo ?? MenuId, AuthorizationStatus = Convert.ToInt32(Authorization.Allow),ShowInMenu=ShowInMenu,SeparateMenuFlag=SperateMenuFlag };
                DbEntitySet.Add(rm);
            }
            await Context.SaveChangesAsync();
            return;
        }

        public async Task SetMenuDisplayNoAsync(string RoleId, int MenuId, int DisplayNo)
        {
            var role = await this.FindByIdAsync(RoleId);
            if (role == null)
            {
                throw new Exception();
            }

            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.MenuId.Equals(MenuId));
            if (roleMenu != null)
            {
                roleMenu.DisplayNo = DisplayNo;
                Context.Entry(roleMenu).State = EntityState.Modified;
            }
            await Context.SaveChangesAsync();
            return;

        }

        public async Task SetMenuAuthorizationStatusAsync(string RoleId, int MenuId, int AuthorizationStatus)
        {
            var role = await this.FindByIdAsync(RoleId);
            if (role == null)
            {
                throw new Exception();
            }

            var DbEntitySet = Context.Set<ApplicationRoleMenu>();
            var roleMenu = await DbEntitySet.SingleOrDefaultAsync(m => m.RoleId.Equals(RoleId) && m.MenuId.Equals(MenuId));
            if (roleMenu != null)
            {
                roleMenu.AuthorizationStatus = AuthorizationStatus;
                Context.Entry(roleMenu).State = EntityState.Modified;
            }
            await Context.SaveChangesAsync();
            return;
        }

        
    }

    public class RoleMenuManager : RoleManager<ApplicationRole>
    {
        public RoleMenuManager(IRoleMenuStore store) : base(store) { }

        protected new IRoleMenuStore Store
        {
            get
            {
                return (IRoleMenuStore)base.Store;
            }
        }

    }

    public class ApplicationMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int ParentMenuId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string CssClass { get; set; }
        public string ActionParam { get; set; }
        public bool IsRootMenu
        {
            get
            {
                return this.MenuId == ParentMenuId;
            }
        }
        public bool ShowInMenu { get; set; }
        public bool SeparateMenuFlag { get; set; }

        //public virtual ApplicationMenu ParentMenu { get; set; }

        //public virtual List<ApplicationMenu> SubMenus { get; set; }
        //public virtual List<ApplicationRoleMenu> RoleMenus { get; set; }
    }
    
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name, string description) : base(name) { this.Description = description; }
        public virtual string Description { get; set; }

        public virtual List<ApplicationRoleMenu> RoleMenus { get; set; }
    }

    public class ApplicationRoleMenu
    {
        //public int RoleMenuId { get; set; }
        public int MenuId { get; set; }
        public string RoleId { get; set; }
        public int AuthorizationStatus { get; set; }
        public int DisplayNo { get; set; }
        public bool ShowInMenu { get; set; }
        public bool SeparateMenuFlag { get; set; }
        //public ApplicationMenu Menu { get; set; }
        public ApplicationRole Role { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //static ApplicationDbContext()
        //{         
        //}

        DbSet<ApplicationMenu> Menus { get; set; }
        DbSet<ApplicationRoleMenu> RoleMenus { get; set; }

        public static ApplicationDbContext Create(IdentityFactoryOptions<ApplicationDbContext> options, IOwinContext context)
        {
            // 在第一次启动网站时初始化数据库添加管理员用户凭据和admin 角色到数据库
            var db= new ApplicationDbContext();
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
            return db;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<ApplicationUser>().HasKey(p => p.Id).ToTable("account_m_users");
            modelBuilder.Entity<IdentityRole>().ToTable("account_m_roles");
            modelBuilder.Entity<ApplicationRole>().ToTable("account_m_roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("account_m_userroles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("account_m_userlogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("account_m_userclaims");

            var menu= modelBuilder.Entity<ApplicationMenu>()
                .HasKey(m => m.MenuId)
                .Ignore(m => m.IsRootMenu)
                .Ignore(m => m.ShowInMenu)
                .Ignore(m=>m.SeparateMenuFlag)
                .ToTable("account_m_menus");
            menu.Property(m => m.MenuId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ApplicationRoleMenu>().HasRequired(m => m.Role).WithMany(c => c.RoleMenus).HasForeignKey(c => c.RoleId);
            //modelBuilder.Entity<ApplicationRoleMenu>().HasRequired(m => m.Menu).WithMany().HasForeignKey(c => c.MenuId);

            //modelBuilder.Entity<ApplicationMenu>().HasRequired(m => m.ParentMenu).WithMany(c => c.SubMenus).HasForeignKey(c => c.MenuId);

            var roleMenu = modelBuilder.Entity<ApplicationRoleMenu>()
                .HasKey(m => new { m.RoleId, m.MenuId })
                .ToTable("account_m_rolemenus");

            //roleMenu.Property(m => m.RoleMenuId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //// Keep this:
            //modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

            //// Change TUser to ApplicationUser everywhere else - 
            //// IdentityUser and ApplicationUser essentially 'share' the AspNetUsers Table in the database:
            //EntityTypeConfiguration<ApplicationUser> table =
            //    modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            //table.Property((ApplicationUser u) => u.UserName).IsRequired();

            //// EF won't let us swap out IdentityUserRole for ApplicationUserRole here:
            //modelBuilder.Entity<ApplicationUser>().HasMany<IdentityUserRole>((ApplicationUser u) => u.Roles);
            //modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) =>
            //    new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

            //// Leave this alone:
            //EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
            //    modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
            //        new
            //        {
            //            UserId = l.UserId,
            //            LoginProvider = l.LoginProvider,
            //            ProviderKey
            //                = l.ProviderKey
            //        }).ToTable("AspNetUserLogins");

            //entityTypeConfiguration.HasRequired<IdentityUser>((IdentityUserLogin u) => u.User);
            //EntityTypeConfiguration<IdentityUserClaim> table1 =
            //    modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

            //table1.HasRequired<IdentityUser>((IdentityUserClaim u) => u.User);

            //// Add this, so that IdentityRole can share a table with ApplicationRole:
            //modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            //// Change these from IdentityRole to ApplicationRole:
            //EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 =
            //    modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

            //entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();

        }

    }



}