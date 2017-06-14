using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RecipeBox.Objects
{
  public class Category
  {
    private string _categoryName;
    private int _id;

    public Category(string categoryName, int Id = 0)
    {
      _categoryName = categoryName;
      _id = Id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetCategoryName()
    {
      return _categoryName;
    }

    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;
        bool idEquality = (this.GetId() == newCategory.GetId());
        bool categoryNameEquality = (this.GetCategoryName() == newCategory.GetCategoryName());
        return (idEquality && categoryNameEquality);
      }
    }

    public static void DeleteAll()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();
     SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
   }

   public static List<Category> GetAll()
    {
      List<Category> allCategorys = new List<Category>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        allCategorys.Add(newCategory);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCategorys;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (category_name) OUTPUT INSERTED.id VALUES (@CategoryName)", conn);

      SqlParameter categoryNameParameter = new SqlParameter();
      categoryNameParameter.ParameterName = "@CategoryName";
      categoryNameParameter.Value = this.GetCategoryName();

      cmd.Parameters.Add(categoryNameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @CategoryId", conn);
      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = id.ToString();
      cmd.Parameters.Add(categoryIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCategoryId = 0;
      string foundCategoryName = null;

      while(rdr.Read())
      {
        foundCategoryId = rdr.GetInt32(0);
        foundCategoryName = rdr.GetString(1);
      }
      Category foundCategory = new Category(foundCategoryName, foundCategoryId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCategory;
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM recipes_categories WHERE categories_id = @CategoryId;", conn);
      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      cmd.Parameters.Add(categoryIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddRecipe(Recipe newRecipe)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO recipes_categories (categories_id, recipes_id) VALUES (@CategoryId, @RecipeId);", conn);
      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();
      cmd.Parameters.Add(categoryIdParameter);

      SqlParameter recipeIdParameter = new SqlParameter();
      recipeIdParameter.ParameterName = "@RecipeId";
      recipeIdParameter.Value = newRecipe.GetId();
      cmd.Parameters.Add(recipeIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public List<Recipe> GetRecipes()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT recipes.* FROM categories JOIN recipes_categories ON (categories.id = recipes_categories.categories_id) JOIN recipes ON (recipes_categories.recipes_id = recipes.id) WHERE categories.id = @CategoryId;", conn);
      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CategoryId";
      categoryIdParameter.Value = this.GetId();

      cmd.Parameters.Add(categoryIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Recipe> recipes = new List<Recipe> {};

      while(rdr.Read())
        {
          int thisRecipeId = rdr.GetInt32(0);
          string recipeName = rdr.GetString(1);
          int recipeRating = rdr.GetInt32(2);
          string recipeInstructions = rdr.GetString(3);
          Recipe foundRecipe = new Recipe(recipeName, recipeRating, recipeInstructions, thisRecipeId);
          recipes.Add(foundRecipe);
        }
        if (rdr != null)
        {
          rdr.Close();
        }

      if (conn != null)
      {
        conn.Close();
      }
      return recipes;
    }
  }
}
