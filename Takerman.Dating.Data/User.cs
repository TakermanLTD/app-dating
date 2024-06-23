using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Takerman.Dating.Data
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [StringLength(300)]
        public required string FirstName { get; set; }

        [DataType(DataType.Text)]
        [StringLength(300)]
        public required string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(300)]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(300)]
        public required string Password { get; set; }

        [DataType(DataType.Text)]
        [StringLength(200)]
        public string? City { get; set; }

        [DataType(DataType.Text)]
        [StringLength(200)]
        public string? Country { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(100)]
        public string? Phone { get; set; }

        [DataType(DataType.Text)]
        public string? Facebook { get; set; }

        [DataType(DataType.Text)]
        public string? Instagram { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; } = false;

        public required Gender Gender { get; set; }

        public Ethnicity? Ethnicity { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = null;

        public virtual ICollection<UserPicture> Pictures { get; set; } = null;

        public virtual ICollection<DateUserChoice> Choices { get; set; } = null;

        public string? Avatar { get; set; } = null;


    public class Role : IdentityRole<long>
    {
    }

    public class UserClaim : IdentityUserClaim<long> { }

    public class UserRole : IdentityUserRole<long> { }

    public class UserLogin : IdentityUserLogin<long> { }

    public class RoleClaim : IdentityRoleClaim<long> { }

    public class UserToken : IdentityUserToken<long> { }
    }
}