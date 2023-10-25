using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPTBook.Models;

[Table("Order")]
public partial class Order
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal TotalPrice { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
