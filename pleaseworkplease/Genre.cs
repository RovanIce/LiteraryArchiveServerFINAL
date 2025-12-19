using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace pleaseworkplease;

[Table("Genre")]
public partial class Genre
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("keyword")]
    [StringLength(50)]
    [Unicode(false)]
    public string Keyword { get; set; } = null!;

    [Column("rating")]
    [StringLength(5)]
    [Unicode(false)]
    public string Rating { get; set; } = null!;

    [Column("Genre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Genre1 { get; set; } = null!;

    [InverseProperty("GenreNavigation")]
    public virtual ICollection<Novel> Novels { get; set; } = new List<Novel>();
}
