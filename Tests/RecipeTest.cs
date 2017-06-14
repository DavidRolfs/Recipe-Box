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
    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
      Ingredient.DeleteAll();
    }
  }
}
