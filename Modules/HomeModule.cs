using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using RecipeBox.Objects;

namespace RecipeBox
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
    }
  }
}
