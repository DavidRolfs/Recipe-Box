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
    public void Dispose()
    {
      Category.DeleteAll();
    }
  }
}
