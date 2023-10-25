using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPTBook.Models;

[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("BookID")]
    public int BookId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("OrderDetails")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;
}
