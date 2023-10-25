using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPTBook.Models;

[Table("User")]
public partial class User
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
