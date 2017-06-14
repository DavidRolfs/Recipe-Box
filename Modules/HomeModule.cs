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
      Get["/ingredients"] = _ => {
        List<Ingredient> AllIngredients = Ingredient.GetAll();
        return View["ingredients.cshtml", AllIngredients];
      };
      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };
      Get["/recipes"] = _ => {
        List<Recipe> AllRecipes = Recipe.GetAll();
        return View["recipes.cshtml", AllRecipes];
      };
      Get["/recipes/new"] = _ => {
        return View["recipe_form.cshtml"];
      };
      Post["/recipes/new"] = _ => {
        Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-rating"], Request.Form["recipe-instructions"]);
        newRecipe.Save();
        return View["success.cshtml"];
      };
      Get["/categories/new"] = _ => {
        return View["category_form.cshtml"];
      };
      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };
      Get["/ingredients/new"] = _ => {
        return View["ingredient_form.cshtml"];
      };
      Post["/ingredients/new"] = _ => {
        Ingredient newIngredient = new Ingredient(Request.Form["ingredient-name"]);
        newIngredient.Save();
        return View["success.cshtml"];
      };
    }
  }
}
