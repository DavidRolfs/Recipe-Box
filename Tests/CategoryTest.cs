using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RecipeBox.Objects;

namespace RecipeBox
{
  [Collection("recipe_box_test")]
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString  = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=recipe_box_test;Integrated Security=SSPI;";
    }


    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Category.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfCategoriesAreTheSame()
    {
      Category firstCategory = new Category("Soup");
      Category secondCategory = new Category("Soup");
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Test_Save_ToCategoryDatabase()
    {
      Category testCategory = new Category("Soup");
      testCategory.Save();

      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};
      Assert.Equal(testList, result);
    }

    [Fact]
     public void Test_Save_AssignsIdToObject()
     {
      Category testCategory = new Category("Soup");
       testCategory.Save();
       int testId = testCategory.GetId();
       int savedCategoryId = Category.GetAll()[0].GetId();
       Assert.Equal(testId, savedCategoryId);
     }

    [Fact]
    public void Test_Find_FindsCategoryInDatabase()
    {
      Category testCategory = new Category("Soup");
      testCategory.Save();
      Category foundCategory = Category.Find(testCategory.GetId());
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void TestRecipe_AddsRecipeToCategory_RecipeList()
    {
      Category testCategory = new Category("Soup");
      testCategory.Save();

      Recipe testRecipe = new Recipe("Spicy Squash Soup", 5, "Put the spice and the squash in the soup.");
      testRecipe.Save();

      testCategory.AddRecipe(testRecipe);

      List<Recipe> result = testCategory.GetRecipes();
      List<Recipe> testList = new List<Recipe>{testRecipe};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_ReturnsAllCategorysRecipes_RecipeList()
    {
      Category testCategory = new Category("Soup");
      testCategory.Save();

      Recipe testRecipe1 = new Recipe("Spicy Squash Soup", 5, "Put the spice and the squash in the soup.");
      testRecipe1.Save();

      Recipe testRecipe2 = new Recipe("White Bean Soup", 3, "Steps 1, 2, 3");
      testRecipe2.Save();

      testCategory.AddRecipe(testRecipe1);
      testCategory.AddRecipe(testRecipe2);
      List<Recipe> result = testCategory.GetRecipes();
      List<Recipe> testList = new List<Recipe> {testRecipe1, testRecipe2};

      Assert.Equal(testList, result);
    }
    [Fact]
    public void Delete_DeletesCategoryAssociationsFromDataBase_CategoryList()
    {
      Recipe testRecipe = new Recipe("Spicy Squash Soup", 5, "Put the spice and the squash in the soup.");
      testRecipe.Save();

      Category testCategory = new Category("Soup");
      testCategory.Save();

      testCategory.AddRecipe(testRecipe);
      testCategory.Delete();

      List<Category> result = testRecipe.GetCategories();
      List<Category> test = new List<Category>{};

      Assert.Equal(test, result);
    }
    public void Dispose()
    {
      Category.DeleteAll();
      Recipe.DeleteAll();
      Ingredient.DeleteAll();
    }
  }
}
