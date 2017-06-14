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
    public void Dispose()
    {
      Ingredient.DeleteAll();
      Recipe.DeleteAll();
      Category.DeleteAll();
    }
  }
}
