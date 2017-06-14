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
        return View["index.cshtml"];
      };
      Get["/categories/new"] = _ => {
        return View["category_form.cshtml"];
      };
      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["index.cshtml"];
      };
      Get["/ingredients/new"] = _ => {
        return View["ingredient_form.cshtml"];
      };
      Post["/ingredients/new"] = _ => {
        Ingredient newIngredient = new Ingredient(Request.Form["ingredient-name"]);
        newIngredient.Save();
        return View["index.cshtml"];
      };
      Get["category/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category SelectedCategories = Category.Find(parameters.id);
        List<Recipe> RecipeCategory = SelectedCategories.GetRecipes();
        List<Recipe> AllRecipes = Recipe.GetAll();
        model.Add("category", SelectedCategories);
        model.Add("recipeCategory", RecipeCategory);
        model.Add("allRecipes", AllRecipes);
        return View["category.cshtml", model];
      };
      Post["category/success"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Recipe recipe = Recipe.Find(Request.Form["recipe-id"]);
        recipe.AddCategory(category);
        return View["index.cshtml"];
      };

      Get["ingredient/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Ingredient SelectedIngredient = Ingredient.Find(parameters.id);
        List<Recipe> RecipeIngredient = SelectedIngredient.GetRecipes();
        List<Recipe> AllRecipes = Recipe.GetAll();
        model.Add("ingredient", SelectedIngredient);
        model.Add("recipeIngredient", RecipeIngredient);
        string ipoopedthebed = "david";
        model.Add("allRecipes", AllRecipes);
        return View["ingredient.cshtml", model];
      };
      Post["ingredient/success"] = _ => {
        Ingredient ingredient = Ingredient.Find(Request.Form["ingredient-id"]);
        Recipe recipe = Recipe.Find(Request.Form["recipe-id"]);
        recipe.AddIngredient(ingredient);
        return View["index.cshtml"];
      };

      Get["recipe/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Recipe SelectedRecipe = Recipe.Find(parameters.id);
        List<Ingredient> RecipeIngredients = SelectedRecipe.GetIngredients();
        List<Ingredient> AllIngredients = Ingredient.GetAll();
        List<Category> RecipeCategories = SelectedRecipe.GetCategories();
        List<Category> AllCategories = Category.GetAll();
        model.Add("recipe", SelectedRecipe);
        model.Add("recipeIngredients", RecipeIngredients);
        model.Add("recipeCategories", RecipeCategories);
        model.Add("allIngredients", AllIngredients);
        model.Add("allCategories", AllCategories);
        return View["recipe.cshtml", model];
      };
      Post["recipe/ingredient/success"] = _ => {
        Ingredient ingredient = Ingredient.Find(Request.Form["ingredient-id"]);
        Recipe recipe = Recipe.Find(Request.Form["recipe-id"]);
        recipe.AddIngredient(ingredient);
        return View["index.cshtml"];
      };
      Post["recipe/category/success"] = _ => {
        Category category = Category.Find(Request.Form["category-id"]);
        Recipe recipe = Recipe.Find(Request.Form["recipe-id"]);
        recipe.AddCategory(category);
        return View["index.cshtml"];
      };

    }
  }
}
