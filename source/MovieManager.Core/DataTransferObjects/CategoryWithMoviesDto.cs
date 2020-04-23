using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.Core.DataTransferObjects
{
  public class CategoryWithMoviesDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<MovieDto> Movies { get; set; }
  }
}
