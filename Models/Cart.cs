using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPTBook.Models;

[Table("Cart")]
public partial class Cart
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BookID")]
    public int BookId { get; set; }

    public int Quantity { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("Carts")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Carts")]
    public virtual User User { get; set; } = null!;
}
