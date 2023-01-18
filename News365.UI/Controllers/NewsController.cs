using Microsoft.AspNetCore.Mvc;
using News365.Business.Abstract;
using News365.UI.Models;

namespace News365.UI.Controllers;

public class NewsController : Controller
{
    private readonly ILogger<NewsController> _logger;
    private readonly INewsService _newsService;
    private readonly IPageService _pageService;
    private readonly ICategoryService _categoryService;

    public NewsController(ILogger<NewsController> logger,
                            INewsService newsService,
                            IPageService pageService, ICategoryService categoryService)
    {
        _logger = logger;
        _pageService = pageService;
        _newsService = newsService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Item(Guid newsid)
    {

        var news = (await _newsService.GetByNewsModelIdAsync(newsid)).Data;
        return View(news);

    }

}
