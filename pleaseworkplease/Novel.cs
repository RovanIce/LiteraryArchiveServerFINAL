using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pleaseworkplease;

public partial class Novel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }
    [Column("ISBN")]
    public double Isbn { get; set; }

    public int Genre { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Author { get; set; } = null!;

    [ForeignKey("Genre")]
    [InverseProperty("Novels")]
    public virtual Genre GenreNavigation { get; set; } = null!;
}
