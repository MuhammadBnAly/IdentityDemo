using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IdentityDemo.Models
{
    //My Class inherit IdentityUser :IUser
    public class ApplicationIdentityUser : IdentityUser
    {
        public string Address { get; set; }
       // public virtual Admin admin { get; set; }
       
    }
    //class ApplicationUserManager : UserManager<ApplicationIdentityUser>
    //{

    //}
    #region test
    //public class Admin
    //{
    //    [ForeignKey("app")]
    //    public int Id { get; set; }
    //    public string MyProperty { get; set; }
    //    public virtual ApplicationIdentityUser app { get; set; }
    //}
    //public class StaffUser
    //{
    //    [ForeignKey("app")]
    //    public int Id { get; set; }
    //    public virtual ApplicationIdentityUser app { get; set; }
    //}
    #endregion
    public class Order
    {
        public int Id { get; set; }
       
        [Required]
        public string Date { get; set; }
        
        public int TotalPrice { get; set; }
        public string UserID { get; set; }
       
        [ForeignKey("UserID")]
        public virtual ApplicationIdentityUser ApplicationIdentityUser { get; set; }
    }
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser>
    {
        public ApplicationDbContext() : 
            base("CS")
        {

        }
        public ApplicationDbContext(string name):base(name)
        {

        }
        public DbSet<Order> Orders { get; set; }
    }

}