using Microsoft.AspNetCore.Mvc;
using News365.Business.Abstract;
using News365.UI.Models;

namespace News365.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INewsService _newsService;
    private readonly ISliderService _sliderService;
    private readonly IPageService _pageService;
    private readonly ICategoryService _categoryService;

    public HomeController(ILogger<HomeController> logger,
                            INewsService newsService,
                            ISliderService sliderService,
                            ICategoryService categoryService,
                            IPageService pageService)
    {
        _logger = logger;
        _pageService = pageService;
        _categoryService = categoryService;
        _newsService = newsService;
        _sliderService = sliderService;
    }

    [Route("/")]
    [Route("/homepage")]
    public async Task<IActionResult> Index(string _lang = "tr")
    {
        if (HttpContext.Session.GetString("lang") != null)
        {
            ViewBag.Lang = HttpContext.Session.GetString("lang");
        }
        var news = await _newsService.GetNewsListAsync();
        var sliders =  await _sliderService.GetSliderListAsync();
        var pages = await _pageService.GetPageListAsync();

        var pageVM = new MainVM()
        {
            Categories = (await _categoryService.GetCategoryListAsync()).Data.ToList(),
            Pages = pages.Data,
            News = news.Data.Take(6).ToList(),
            Sliders = sliders.Data,
        };
        return View(pageVM);
    }
    // public async Task<PartialViewResult> GetNewsByCategory(int categoryId)
    // {
    //     if (categoryId != null)
    //     {
    //         var news = (await _newsService.GetNewsListAsync()).Data;

    //         NewsPageVM pageVM = new NewsPageVM()
    //         {
    //             Category = news.FirstOrDefault().Category,
    //             News = news.Where(x => x.CategoryId == categoryId == true).ToList()
    //         };
    //         return PartialView(pageVM);
    //     }
    //     return PartialView();
    // } 

    [Route("/newsfiltered")]
    public async Task<PartialViewResult> GetFilter(Guid category)
    {
        try
        { 
            var news = (await _newsService.GetNewsListAsync()).Data;

            if (category !=null)
            {                
                news = news.Where(x => x.CategoryId==category).ToList();
            }
            return PartialView(news);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        } 
    }
}
