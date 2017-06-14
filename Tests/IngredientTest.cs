using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RecipeBox.Objects;

namespace RecipeBox
{
  [Collection("recipe_box_test")]
  public class IngredientTest : IDisposable
  {
    public IngredientTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
    }


    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Ingredient.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCategoriesAreTheSame()
    {
      Ingredient firstIngredient = new Ingredient("Soup");
      Ingredient secondIngredient = new Ingredient("Soup");
      Assert.Equal(firstIngredient, secondIngredient);
    }

    [Fact]
    public void Test_Save_ToIngredientDatabase()
    {
      Ingredient testIngredient = new Ingredient("Soup");
      testIngredient.Save();

      List<Ingredient> result = Ingredient.GetAll();
      List<Ingredient> testList = new List<Ingredient>{testIngredient};
      Assert.Equal(testList, result);
    }

    [Fact]
     public void Test_Save_AssignsIdToObject()
     {
      Ingredient testIngredient = new Ingredient("Soup");
       testIngredient.Save();
       int testId = testIngredient.GetId();
       int savedIngredientId = Ingredient.GetAll()[0].GetId();
       Assert.Equal(testId, savedIngredientId);
     }

    [Fact]
    public void Test_Find_FindsIngredientInDatabase()
    {
      Ingredient testIngredient = new Ingredient("Soup");
      testIngredient.Save();
      Ingredient foundIngredient = Ingredient.Find(testIngredient.GetId());
      Assert.Equal(testIngredient, foundIngredient);
    }
    [Fact]
    public void TestRecipe_AddsRecipeToIngredient_RecipeList()
    {
      Ingredient testIngredient = new Ingredient("Soup");
      testIngredient.Save();

      Recipe testRecipe = new Recipe("Spicy Squash Soup", 5, "Put the spice and the squash in the soup.");
      testRecipe.Save();

      testIngredient.AddRecipe(testRecipe);

      List<Recipe> result = testIngredient.GetRecipes();
      List<Recipe> testList = new List<Recipe>{testRecipe};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_ReturnsAllIngredientsRecipes_RecipeList()
    {
      Ingredient testIngredient = new Ingredient("Soup");
      testIngredient.Save();

      Recipe testRecipe1 = new Recipe("Spicy Squash Soup", 5, "Put the spice and the squash in the soup.");
      testRecipe1.Save();

      Recipe testRecipe2 = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe2.Save();

      testIngredient.AddRecipe(testRecipe1);
      testIngredient.AddRecipe(testRecipe2);
      List<Recipe> result = testIngredient.GetRecipes();
      List<Recipe> testList = new List<Recipe> {testRecipe1, testRecipe2};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Delete_DeletesIngredientAssociationsFromDataBase_IngredientList()
    {
      Recipe testRecipe = new Recipe("Spicy Squash Soup", 5, "Put the spice and the squash in the soup.");
      testRecipe.Save();

      Ingredient testIngredient = new Ingredient("Soup");
      testIngredient.Save();

      testIngredient.AddRecipe(testRecipe);
      testIngredient.Delete();

      List<Ingredient> result = testRecipe.GetIngredients();
      List<Ingredient> test = new List<Ingredient>{};

      Assert.Equal(test, result);
    }
    [Fact]
    public void Test_Search_SearchIngredientByName()
    {
      Ingredient testIngredient1 = new Ingredient("Oregano");
      testIngredient1.Save();

      Ingredient testIngredient2 = new Ingredient("Sea Salt");
      testIngredient2.Save();

      Ingredient testIngredient3 = new Ingredient("Garlic salt");
      testIngredient3.Save();

      List<Ingredient> searchedIngredientInput = Ingredient.SearchIngredients("salt");

      List<Ingredient> Result = new List<Ingredient>{testIngredient2, testIngredient3};

      Assert.Equal(Result, searchedIngredientInput);
    }
    public void Dispose()
    {
      Ingredient.DeleteAll();
      Recipe.DeleteAll();
      Ingredient.DeleteAll();
    }
  }
}
