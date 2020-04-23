using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Core.Contracts;
using MovieManager.Core.DataTransferObjects;
using MovieManager.Core.Entities;
using System.Linq;

namespace MovieManager.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork;

    public CategoriesController(IUnitOfWork unitOfWork)
    {
      this._unitOfWork = unitOfWork;
    }


    /// <summary>
    /// Liefert alle Kategorien sortiert nach Namen
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<CategoryDto[]> GetAllCategories()
    {
      return Ok(
        _unitOfWork.CategoryRepository
          .GetAll()
          .Select(c => new CategoryDto() { Id=c.Id, Name=c.CategoryName })
          .OrderBy(c => c.Name));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("{id}")]
    public ActionResult<Category> GetCategoryById(int id)
    {
      Category category = _unitOfWork.CategoryRepository.GetByIdWithMovies(id);
      if(category == null)
      {
        return NotFound(id);
      }

      CategoryWithMoviesDto result = new CategoryWithMoviesDto()
      {
        Id = category.Id,
        Name = category.CategoryName,
        Movies = category
                  .Movies
                  .Select(m => new MovieDto()
                  {
                    Title = m.Title,
                    CategoryId = m.CategoryId,
                    Duration = m.Duration,
                    Year = m.Year
                  })
      };


      return Ok(result);     
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("{id}/movies")]
    public ActionResult<Category> GetMoviesByCategoryId(int id)
    {
      Category category = _unitOfWork.CategoryRepository.GetByIdWithMovies(id);
      if (category == null)
      {
        return NotFound(id);
      }

      CategoryWithMoviesDto result = new CategoryWithMoviesDto()
      {
        Id = category.Id,
        Name = category.CategoryName,
        Movies = category
                  .Movies
                  .Select(m => new MovieDto()
                  {
                    Title = m.Title,
                    CategoryId = m.CategoryId,
                    Duration = m.Duration,
                    Year = m.Year
                  })
      };


      return Ok(result.Movies);
    }


  }
}
