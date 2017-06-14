using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RecipeBox.Objects;

namespace RecipeBox
{
  [Collection("recipe_box_test")]
  public class RecipeTest : IDisposable
  {
    public RecipeTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
    }


    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Recipe.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCategoriesAreTheSame()
    {
      Recipe firstRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      Recipe secondRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      Assert.Equal(firstRecipe, secondRecipe);
    }

    [Fact]
    public void Test_Save_ToRecipeDatabase()
    {
      Recipe testRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe.Save();

      List<Recipe> result = Recipe.GetAll();
      List<Recipe> testList = new List<Recipe>{testRecipe};
      Assert.Equal(testList, result);
    }

    [Fact]
     public void Test_Save_AssignsIdToObject()
     {
      Recipe testRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
       testRecipe.Save();
       int testId = testRecipe.GetId();
       int savedRecipeId = Recipe.GetAll()[0].GetId();
       Assert.Equal(testId, savedRecipeId);
     }

    [Fact]
    public void Test_Find_FindsRecipeInDatabase()
    {
      Recipe testRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe.Save();
      Recipe foundRecipe = Recipe.Find(testRecipe.GetId());
      Assert.Equal(testRecipe, foundRecipe);
    }
    [Fact]
    public void TestCategory_AddsCategoryToRecipe_CategoryList()
    {
      Recipe testRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe.Save();

      Category testCategory = new Category("Soup");
      testCategory.Save();

      testRecipe.AddCategory(testCategory);

      List<Category> result = testRecipe.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void TestCategory_ReturnsAllRecipeCategories_CategoryList()
    {
      Recipe testRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe.Save();

      Category testCategory1 = new Category("Soup");
      testCategory1.Save();

      Category testCategory2 = new Category("Dinner Faves");
      testCategory2.Save();

      testRecipe.AddCategory(testCategory1);
      List<Category> result = testRecipe.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void TestIngredient_AddsIngredientToRecipe_IngredientList()
    {
      Recipe testRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe.Save();

      Ingredient testIngredient = new Ingredient("Soup");
      testIngredient.Save();

      testRecipe.AddIngredient(testIngredient);

      List<Ingredient> result = testRecipe.GetIngredients();
      List<Ingredient> testList = new List<Ingredient>{testIngredient};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void TestIngredient_ReturnsAllRecipeIngredients_IngredientList()
    {
      Recipe testRecipe = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe.Save();

      Ingredient testIngredient1 = new Ingredient("Soup");
      testIngredient1.Save();

      Ingredient testIngredient2 = new Ingredient("Dinner Faves");
      testIngredient2.Save();

      testRecipe.AddIngredient(testIngredient1);
      List<Ingredient> result = testRecipe.GetIngredients();
      List<Ingredient> testList = new List<Ingredient> {testIngredient1};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Delete_DeletesRecipeAssociationsFromDataBase_RecipeList()
    {
      Category testCategory = new Category("SO MUCH SOUP");
      testCategory.Save();

      Recipe testRecipe = new Recipe("Spicy Squash Soup", 5, "Put the spice and the squash in the soup.");
      testRecipe.Save();

      testRecipe.AddCategory(testCategory);
      testRecipe.Delete();

      List<Recipe> result = testCategory.GetRecipes();
      List<Recipe> test = new List<Recipe>{};

      Assert.Equal(test, result);
    }
    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
      Ingredient.DeleteAll();
    }
  }
}
